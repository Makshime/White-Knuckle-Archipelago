using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using static CL_GameTracker;
using static SessionEventModule_SendMessageToModules;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace WKRando;

public class ArchipelagoClient
{
    private static ArchipelagoSession _session = ArchipelagoSessionFactory.CreateSession("localhost", 38281);
    public static string Servername = "localhost:38281";
    public static string Username = string.Empty;
    public static string Password = string.Empty;

    public static string DeathMessage = "Died to Archipelago Player";
    public static DeathLinkService Deathlinkservice;

    private static string Seed;
    private static bool _connectedBefore;
    public static bool Connecting;
    public static bool Connected;
    private static int _reconnectAttempts = 0;
    public static int Slot = -1;
    private static bool _goaled;

    private static Dictionary<string, object> _slotData; // might be needed later, who knows? i do, i need it later

    private static Queue<ItemInfo> _items = [];
    private static List<long> _locationsToSend = [];
    

    private static void NewSession(string server)
    {
        _session = ArchipelagoSessionFactory.CreateSession(server);
        Plugin.ClientOptions.Server = server;
    }
    
    //Archipelago connection procedure using Multiclient.net 
    public static async Task<object> Connect(string server = null, string user = null, string pass = null)
    {
        
        
        if (Connected)
        {
            CommandConsole.Log($"Already connected to server {Plugin.ClientOptions.Server} as {Plugin.ClientOptions.User}");
            return null;
        }
        
            
        Plugin.Logger.LogInfo("Connecting to " + server);

        if (server != null && server != Plugin.ClientOptions.Server)
            Plugin.ClientOptions.Server = server;
        if (user != null && user != Plugin.ClientOptions.User) 
            Plugin.ClientOptions.User = user;
        if (pass != null && pass != Plugin.ClientOptions.Password) 
            Plugin.ClientOptions.Password = pass;
        
        NewSession(Plugin.ClientOptions.Server);
        
        _session.Items.ItemReceived += OnItemReceive;
        _session.MessageLog.OnMessageReceived += OnMessageReceive;
        _session.Socket.ErrorReceived += OnError;
        _session.Socket.SocketClosed += OnSocketClosed;
        _session.Locations.CheckedLocationsUpdated += OnLocationReceive;

        APItems.TargetAPDebuffCount = 10;
        APItems.ClearAllFlags();
        APItems.SentLocations.Clear();
        
        LoginResult result;

        
        try
        {
            result = _session.TryConnectAndLogin("White Knuckle", Plugin.ClientOptions.User, ItemsHandlingFlags.AllItems);
        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
        }

        
        if (!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            CommandConsole.Log($"Failed to Connect to {Plugin.ClientOptions.Server} as {Plugin.ClientOptions.User}:");
            foreach (string error in failure.Errors)
            {
                CommandConsole.Log($"    {error}");
            }

            foreach (ConnectionRefusedError error in failure.ErrorCodes)
            {
                CommandConsole.Log($"    {error}");
            }
            
            await Disconnect(true);
            return null;
        }
        
        Connecting = true;
        Connected = true;
        
        var loginSuccess = (LoginSuccessful)result;
        Slot = loginSuccess.Slot;

        APItems.SentLocations.AddRange(_session.Locations.AllLocationsChecked);
        FixSendQueue();
        foreach (var item in _session.Items.AllItemsReceived)
        {
            _items.Enqueue(item);
        }
        CheckReceivedItems();
        foreach (long item in APItems.SentLocations)
        {
            Plugin.Logger.LogInfo($"Logged Sent Location ID: {item}");
        }
        Update();
        
        
        FillOptions(loginSuccess.SlotData);
        if(!server.IsNullOrWhiteSpace())
            Plugin.ClientOptions.Server = server;
        if(!user.IsNullOrWhiteSpace())
            Plugin.ClientOptions.User = user;
        if(!pass.IsNullOrWhiteSpace())
            Plugin.ClientOptions.Password = pass;

        Connecting = false;
        
        CommandConsole.Log($"Successfully connected to {Plugin.ClientOptions.Server} as {Plugin.ClientOptions.User}!");
        CommandConsole.Log($"   Slot Number: {loginSuccess.Slot}");

        Seed = _session.RoomState.Seed;
        if (!File.Exists(Path.Combine(Application.persistentDataPath, $"{Seed}_save.json")))
        {
            File.Create(Path.Combine(Application.persistentDataPath, $"{Seed}_save.json"));
            Plugin.AlterStats.UpdateSaveLocationNames(Seed);
            CommandConsole.hasCheated = true;
            CL_GameManager.SetRoaches(0);
            CL_GameManager.SetRoaches(0, true);
            CL_GameManager.gMan.Restart([]);
        }
        else
        {
            Plugin.AlterStats.UpdateSaveLocationNames(Seed);
        }
        
        Plugin.ClientOptions.SaveOptions();
        
        _connectedBefore = true;
        Deathlinkservice = _session.CreateDeathLinkService();
        Deathlinkservice.OnDeathLinkReceived += DeathLinkHandler.ProcDeathlink;
        if (Plugin.ClientOptions.Deathlink)
        {
            Deathlinkservice.EnableDeathLink();
        }
        
        return null;
    }

    
    
    public static async Task<object> Disconnect(bool tryReconnect = false)
    {
        
        if (_session != null)
        {
            _session.Items.ItemReceived -= OnItemReceive;
            _session.MessageLog.OnMessageReceived -= OnMessageReceive;
            _session.Socket.ErrorReceived -= OnError;
            _session.Socket.SocketClosed -= OnSocketClosed;
            _session.Locations.CheckedLocationsUpdated -= OnLocationReceive;

            if (Deathlinkservice != null)
            {
                Deathlinkservice.OnDeathLinkReceived -= DeathLinkHandler.ProcDeathlink;
                Deathlinkservice = null;
            }
            
            APItems.TargetAPDebuffCount = 10;
            APItems.TrinketSlots = 3;
            APItems.ProgressiveRegions = 0;
            APItems.ProgressivePerkUnlocks = 0;
            Plugin.LoanAmount = 0;
            APItems.ClearAllFlags();
            APItems.SentLocations.Clear();
        }
        
        Connected = false;
        _goaled = false;
        
        if (_connectedBefore & tryReconnect)
        {
            _reconnectAttempts++;
            if (_reconnectAttempts >= 5)
            {
                await Task.Delay(5000);
                _connectedBefore = false;
            }

            await Connect();
        }

        _slotData = new();

        return null;
    }

    //Main update loop for checking for checks
    public static void Update()
    {
        if (Connected)
        {
            try
            {
                CheckReceivedItems();
                CheckLocationsToSend();
            }
            catch 
            {
                _ = Disconnect();
            }
            
        }
    }

    private static void OnLocationReceive(ReadOnlyCollection<long> newCheckedLocations)
    {
        lock (APItems.SentLocations)
        {
            foreach (var newLoc in newCheckedLocations)
            {
                APItems.SentLocations.Add(newLoc);
            }
        }
    }
    
    private static void OnItemReceive(ReceivedItemsHelper helper)
    {
        while (helper.Any())
        {
            _items.Enqueue(helper.DequeueItem());
        }
        
    }

    private static void OnMessageReceive(LogMessage message)
    {
        CommandConsole.Log($"[{Plugin.ClientOptions.Server}] - {message}"); 
    }
    
    public static void Say(string[] args)
    {
        if(Connected) {_session.Say(string.Join(" ", args));}
        else {CommandConsole.Log("Not currently connected to server");}
    }

    public static void TryQueueLocation(long itemID)
    {
        _locationsToSend.Add(itemID);
    }
    
    private static void CheckReceivedItems()
    {
        
        while(_items.Any() && Connected)
        {
            ItemInfo item = _items.Dequeue();
            
            APItems.UpdateFromItem(item);
            CommandConsole.Log($"Received item: {item.ItemDisplayName} from {item.LocationName} in game {item.LocationGame}");
            
        }
        
    }

    public static void Goal()
    {
        if (_goaled)
            return;
        _session.SetGoalAchieved();
        _goaled = true;
    }

    private static void CheckLocationsToSend()
    {
        if (_locationsToSend.Any() & Connected)
        {
            try
            {
                _session.Locations.CompleteLocationChecksAsync(_locationsToSend.ToArray());
                List<long> locationsToSend = _locationsToSend;
                _locationsToSend.Clear();
                foreach (long l in locationsToSend)
                {
                    Plugin.Logger.LogInfo("Sent item ID: " + l);
                    if (!APItems.SentLocations.Contains(l))
                    {
                        APItems.SentLocations.Add(l);
                    }
                }
                ShowSentItemPopup(locationsToSend);
                
            }
            catch (ArchipelagoSocketClosedException e)
            {
                Plugin.Logger.LogError(e.ToString());
                Disconnect();
            }
            
        }
    }

    private static async Task ShowSentItemPopup(List<long> locationsToSend)
    {
        Dictionary<long, ScoutedItemInfo> infos = await _session.Locations.ScoutLocationsAsync(_locationsToSend.ToArray());
        foreach (long l in locationsToSend)
        {
            if (infos[l].Player.Name != Plugin.ClientOptions.User && !Connecting)
                CL_ProgressionManager.ShowUnlockPopup(
                    APItems.SpriteFromPath($"WKRando/Assets/Archipelago_{
                        infos[l].Flags switch {
                            ItemFlags.None => "Filler_Icon",
                            ItemFlags.Advancement => "Progression_Icon",
                            ItemFlags.NeverExclude => "Icon",
                            ItemFlags.Trap => "Trap_Icon",
                            _ => throw new ArgumentOutOfRangeException() }
                    }.png"), 
                    "Sent Item:", 
                    $"{infos[l].ItemDisplayName} for {infos[l].Player} from {infos[l].LocationDisplayName}", 
                    infos[l].Flags switch
                    {
                        ItemFlags.None => new Color(0.2f,0.2f,0.2f),
                        ItemFlags.Advancement => new Color(0.5f,0.5f,0f),
                        ItemFlags.NeverExclude => new Color(0.3f,0f,0.5f),
                        ItemFlags.Trap => new Color(0.5f,0f,0f),
                        _ => throw new ArgumentOutOfRangeException()
                    });
        }
    }

    private static void OnError(Exception exception, string message)
    {
        Plugin.Logger.LogError(exception.ToString());
        CommandConsole.Log("AP " + message);
        
        Disconnect();
    }

    private static void OnSocketClosed(string message)
    {
        Plugin.Logger.LogError("AP " + message);
        CommandConsole.Log("AP " + message);
        
        Disconnect();
    }

    private static void FixSendQueue()
    {
        for (int i = 0; i < _locationsToSend.Count; i++) 
        {
            if (APItems.SentLocations.Contains(_locationsToSend[i]))
            {
                _locationsToSend.RemoveAt(i);
            }
        }
    }
    
    private static void FillOptions(Dictionary<string, object> slotData)
    {
        string slotDataLogger = "";
        foreach (string I in slotData.Keys)
        {
            slotDataLogger += $"{I}: {slotData[I]}\n"; 
        }
        Plugin.Logger.LogInfo(slotDataLogger);
        Plugin.Logger.LogInfo(Convert.ToString((bool)_session.DataStorage[Scope.Slot, "ConnectedOnce"]));

        _session.DataStorage[Scope.Slot, "ConnectedOnce"].Initialize(false); // sets it to be SOMETHING
        // done first as otherwise itd not log if there's any error
        // defaults to the value on the right of the conditional if no key is present
        
        // Standard options to handle the general stuff
        Plugin.APOptions.StartingDebuffs = slotData.TryGetValue("Starting_Debuffs", out object value) ? Convert.ToInt32(value) : 10;
        APItems.TargetAPDebuffCount -= 10 - Plugin.APOptions.StartingDebuffs;
        Plugin.APOptions.StartingTrinketSlots = slotData.TryGetValue("Starting_Trinket_Slots", out object value1) ? Convert.ToInt32(value1) : 1;
        APItems.TrinketSlots += 3-Plugin.APOptions.StartingTrinketSlots;
        //Currently unimplemented option to enable all trinkets
        Plugin.APOptions.EnableAllTrinkets = slotData.TryGetValue("Enable_Trinket_Randomization", out object value2) && Convert.ToBoolean(value2);
        Plugin.APOptions.SetGoalArea(slotData.TryGetValue("Goal_Region", out object value3) ? Convert.ToInt32(value3) : 3);
        if(slotData.TryGetValue("Include_Challenge_Modes", out object value4) && (int) value4 == 1)
            Plugin.APOptions.UnlockChallenges();
        

        Plugin.ClientOptions.LoadOptions();

        if (!_session.DataStorage[Scope.Slot, "ConnectedOnce"])
        {
            Plugin.Logger.LogInfo("First connection detecting, setting any client data");
            SetClientOptions(slotData); 
        }

        _session.DataStorage[Scope.Slot, "ConnectedOnce"] = true; //causes client settings to not be changed upon future connections
        
        
    }
    


    public static async Task<List<ScoutedItemInfo>> ScoutItemDescriptionFromID(long[] ids)
    {
        Dictionary<long, ScoutedItemInfo> scouted = await _session.Locations.ScoutLocationsAsync(false, ids);
        return scouted.Values.ToList();
    }
    

    // writes all options from the ap server into variables accessible here
    private static void FillOptions()
    {

    }

    public static void SetClientOptions(string[] args)
    {
        SetClientOptions(_slotData);
    }
    
    public static void SetClientOptions(Dictionary<string, object> slotData)
    {
        //Deathlink
        try
        {
            // why isnt this a switch block?? fuck if i know but it stopped working when i tried it
            // did it have a default block oh well it shouldn't matter too too much 
            if (Convert.ToInt32(slotData["Deathlink"]) == 0)
            {
                Plugin.ClientOptions.EnableDeathlink();
            }
            else if (Convert.ToInt32(slotData["Deathlink"]) == 1)
            {
                Plugin.ClientOptions.DisableDeathlink();
            }
        } catch { Plugin.Logger.LogInfo("Deathlink not found, skipping"); }
        
        //Deathlink Amnesty
        try
        {
            if (Convert.ToInt32(slotData["Deathlink_Amnesty"]) != 0)
            {
                Plugin.ClientOptions.DeathlinkAmnesty = Convert.ToInt32(slotData["deathlink_amnesty"]);
            }
        } catch { Plugin.Logger.LogInfo("Deathlink amnesty not found, skipping"); }
        
        //Free Disk Vendors
        try
        {
            if (Convert.ToBoolean(slotData["Include_Challenge_Modes"]))
            {
                Plugin.ClientOptions.FreeDiskVendors = true;
            }
            else
            {
                Plugin.ClientOptions.FreeDiskVendors = false;
            }
        } catch { Plugin.Logger.LogInfo("Disk Vendor Cost not found, skipping"); }
        
        //Sets Seed value for the save
        Plugin.ClientOptions.ServerSeed = _session.RoomState.Seed;
        
        Plugin.ClientOptions.SaveOptions();
        if (Plugin.ClientOptions.Deathlink) {Deathlinkservice.EnableDeathLink();}
    }

    public event DeathLinkService.DeathLinkReceivedHandler OnDeathLinkReceived { add { } remove { } }
}