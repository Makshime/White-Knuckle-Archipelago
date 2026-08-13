using System;
using System.Collections.Generic;
using System.Threading;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace WKRando;

public class ArchipelagoClient
{
    private static ArchipelagoSession _session = ArchipelagoSessionFactory.CreateSession("localhost", 38281);
    private static string _servername = "localhost:38281";
    private static string _username = string.Empty;
    private static string _password = string.Empty;

    private static bool _connecting;
    private static bool _connectedBefore;
    private static bool _connected;
    private static int _reconnectAttempts = 0;

    private static void NewSession(string host, int port)
    {
        _session = ArchipelagoSessionFactory.CreateSession(host, port);
        _servername = $"{host}:{port}";
    }
    private static void NewSession(string server)
    {
        _session = ArchipelagoSessionFactory.CreateSession(server);
        _servername = server;
    }
    
    //Standard connection procedure for Archipelago.MultiClient.Net
    public static async Task<object> Connect(string server = null, string user = null, string pass = null)
    {

        if (_connecting)
        {
            return null;
        }
        
        _connecting = true;
        
        if (_connected)
        {
            CommandConsole.Log($"Already connected to server {_servername} as {user}");
        }
        
        Plugin.Logger.LogInfo("Connecting to " + server);

        if (server != null && server != _servername)
        {
            NewSession(server);
        }
        if (user != null && user != _username) 
            _username = user;
        if (pass != null && pass != _password) 
            _password = pass;
        
        
        LoginResult result;

        try
        {
            result = await Task.Run(() => _session.TryConnectAndLogin("White Knuckle", user, ItemsHandlingFlags.AllItems));
        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
        }

        if (!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            CommandConsole.Log($"Failed to Connect to {_servername} as {user}:");
            foreach (string error in failure.Errors)
            {
                CommandConsole.Log($"    {error}");
            }

            foreach (ConnectionRefusedError error in failure.ErrorCodes)
            {
                CommandConsole.Log($"    {error}");
            }
            
            _connecting = false;
            await Disconnect();
            return null;
        }
        
        var loginSuccess = (LoginSuccessful)result;

        _session.Items.ItemReceived += OnItemReceive;
        _session.MessageLog.OnMessageReceived += OnMessageReceive;

        CommandConsole.Log($"Successfully connected to {_servername} as {user}!");
        CommandConsole.Log($"   Slot Number: {loginSuccess.Slot}");

        _connected = true;
        _connectedBefore = true;
        _connecting = false;
        
        return null;
    }

    public static async Task<object> Disconnect()
    {
        _connected = false;


        if (_session != null)
        {
            _session.Items.ItemReceived -= OnItemReceive;
        }

        if (false)
        {
            _reconnectAttempts++;
            if (_reconnectAttempts >= 5)
            {
                await Task.Delay(5000);
                _connectedBefore = false;
            }

            await Connect();
        }

        return null;
    }

    //Main update loop for checking for checks
    public void Update()
    {
        if (_connected)
        {
            try
            {
                return;
            }
            catch 
            {
                _ = Disconnect();
            }
            
        }
    }
    
    private static void OnItemReceive(ReceivedItemsHelper helper)
    {
        ItemInfo item = helper.DequeueItem();
        
        APItems.UpdateFromId(item.ItemId);
        
    }

    private static void OnMessageReceive(LogMessage message)
    {
        CommandConsole.Log($"    {message}"); 
    }

    private static bool _saying;
    public static void Say(string[] args)
    {
        if (_saying) { return;}
        _saying = true;
        if(_connected) {_session.Say(args[0]);}
        else {CommandConsole.Log("Not currently connected to server");}
        _saying = false;
    }

    public static void SendItem(long itemID)
    {
        try
        {
            _session.Locations.CompleteLocationChecksAsync(itemID);
        }
        catch (ArchipelagoSocketClosedException)
        {
            Disconnect();
        }
    }

}