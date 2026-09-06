using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Yoga;
using Logger = UnityEngine.Logger;
using Object = System.Object;

namespace WKRando;


[BepInPlugin("com.wuckle.concsumer.name", "WKRando", "1.0.0")]
[BepInProcess("White Knuckle.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    
    public static int LoanAmount = 0;
    public static bool Loaded;
    

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;

        var harmony = new Harmony("com.wuckle.concsumer.name");
        HarmonyFileLog.Enabled = true;

        if (!Directory.Exists($"{Application.persistentDataPath}\\Archipelago"))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Archipelago"));
        }

        if (!Directory.Exists($"{Application.persistentDataPath}\\Archipelago"))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Archipelago"));
        }

        if (!File.Exists($"{Application.persistentDataPath}\\Archipelago\\ClientOptions.json"))
        {
            File.Create($"{Application.persistentDataPath}\\Archipelago\\ClientOptions.json");
        }

        harmony.PatchAll(Assembly.GetExecutingAssembly());

        Logger.LogInfo("Archipelago plugin Loaded");
    }

    //Handles all Update functions
    [HarmonyPatch(typeof(ENT_Player), "Update")]
    class PlayerUpdate
    {
        static void Prefix(ENT_Player __instance)
        {   
            ArchipelagoClient.Update();
            try
            {
                if (WorldLoader.instance != null &&
                    APItems.RoomNameToAP.TryGetValue(WorldLoader.instance.GetCurrentLevel().GetLevel().levelName, out var roomID))
                {
                    if (!APItems.SentLocations.Contains(roomID))
                    {
                        APItems.SentLocations.Add(roomID);
                        ArchipelagoClient.TryQueueLocation(roomID);
                    }
                }
            }
            catch
            {
                // ignored
            }

            int perkCount = __instance?.GetPerk("archipelago_debuff")?.stackAmount ?? -__instance?.GetPerk("archipelago_buff")?.stackAmount ?? 0;
                 
            lock(__instance) {
                if (perkCount != APItems.TargetAPDebuffCount && __instance != null)
                {
                    Logger.LogInfo("Amount, Target: " + perkCount + ", " + APItems.TargetAPDebuffCount);

                    if (perkCount < APItems.TargetAPDebuffCount)
                    {
                        __instance.RemovePerk("archipelago_buff");
                        __instance.RemovePerk("archipelago_debuff");
                        if (perkCount >= 0) 
                        {
                            __instance.AddPerk(__instance.GetPerk("archipelago_debuff") ?? CL_AssetManager.GetPerkAsset("archipelago_debuff"), APItems.TargetAPDebuffCount);
                        }
                        else if (APItems.TargetAPDebuffCount != 0)
                        {
                            __instance.AddPerk(__instance.GetPerk("archipelago_debuff") ?? CL_AssetManager.GetPerkAsset("archipelago_debuff"), APItems.TargetAPDebuffCount);
                        }
                    }
                    else
                    {
                        __instance.RemovePerk("archipelago_buff");
                        __instance.RemovePerk("archipelago_debuff");
                        if (perkCount <= 0)
                        {
                            __instance.AddPerk(__instance.GetPerk("archipelago_buff") ?? CL_AssetManager.GetPerkAsset("archipelago_buff"), APItems.TargetAPDebuffCount);
                        }
                        else // alternate way to do it cause why not
                        {
                            if (APItems.TargetAPDebuffCount != 0)
                                __instance.AddPerkCommand([
                                    "archipelago_buff", $"{-APItems.TargetAPDebuffCount}"
                                ]);
                        }
                    }
                }
            } 
            
        }
        
    }



    
    [HarmonyPatch(typeof(FacilityUpgrade), "IsOwned")]
    class AlterUpgradeCheck
    {
        static bool Prefix(FacilityUpgrade __instance, ref bool __result, object[] __args)
        {
            //Replaces the check in the save data for if a facility upgrade is available with the mod's info
            string facilityId = StatManager.saveData.GetFacility((string)__args[0]).id;
            if (APItems.FacilityDict.TryGetValue(facilityId, out Dictionary<string, bool> dict) && dict.TryGetValue(__instance.id, out var value))
            {
                 __result = value;
                 return false;
            }
            return true;
        }
    }
    
    
    //This class replaces the directories that the game saves the game to with its own ones
    [HarmonyPatch(typeof(StatManager), "Awake")]
    class AlterStats
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions).MatchForward(true, new CodeMatch(OpCodes.Ldstr, "wk_save.json"))
                    .Repeat(matcher => matcher.SetInstruction(new CodeInstruction(OpCodes.Ldstr, "rando_save.json")))
                .Start().MatchForward(true, new CodeMatch(OpCodes.Ldstr, "wk_save-backup.json"))
                    .Repeat(matcher => matcher.SetInstruction(new CodeInstruction(OpCodes.Ldstr, "rando_save-backup.json")))
                .Start().MatchForward(true, new CodeMatch(OpCodes.Ldstr, "save-error-backup.json"))
                    .Repeat(matcher => matcher.SetInstruction(new CodeInstruction(OpCodes.Ldstr, "rando_save-error-backup.json")))
                .Start().MatchForward(true, new CodeMatch(OpCodes.Ldstr, "save-backup-crash.json"))
                    .Repeat(matcher => matcher.SetInstruction(new CodeInstruction(OpCodes.Ldstr, "save-backup-crash.json")))
                .Start().MatchForward(true, new CodeMatch(OpCodes.Ldstr, "save-backup-quit.json"))
                    .Repeat(matcher => matcher.SetInstruction(new CodeInstruction(OpCodes.Ldstr, "rando_save-backup-quit.json")))
                .InstructionEnumeration();
        }
    }


    
    [HarmonyPatch(typeof(CommandConsole), "Awake")]
    class AddCommands
    {
        private static void ChangeLoanCommand(string[] args)
        {
            if (args.Length == 0 || args[0] == "")
            {
                CommandConsole.Log("Incorrect format: Uses a single integer");
            }
            else
            {
                LoanAmount = int.Parse(args[0]);
            }
        }

        public static void CheatRoachesInCommand(string[] args)
        {
            try
            {
                CL_GameManager.runRoaches = int.Parse(args[0]);
            }
            catch 
            {
                CommandConsole.Log("Requires a single integer");
            }

        }

        private static async void TryConnectCommand(string[] args)
        {
            if (args.Length < 2 || args[0] == "")
            {
                CommandConsole.Log("Incorrect format: Requires server, username, and optional password, separated by spaces");
            }
            else if(args.Length == 2)
            {
                await ArchipelagoClient.Connect(args[0], args[1]);
            }
            else
            {
                await ArchipelagoClient.Connect(args[0],args[1], args[2]);
            }
            
        }

        private static async void TryReconnectCommand(string[] args)
        {
           await ArchipelagoClient.Connect();
        }

        private static void ResetAPSaveData(string[] args)
        {
            //Creates a fresh save
            CommandConsole.Log("Resetting Save...");
            File.Create(Path.Combine(UnityEngine.Application.persistentDataPath, "rando_save.json"));
        }

        private static async void DisconnectCommand(string[] args)
        {
            await ArchipelagoClient.Disconnect(false);
        }

        private static void SetDebuffs(string[] args)
        {
            if (args.Length < 1 || !int.TryParse(args[0], out int result))
            {
                CommandConsole.Log("Requires a single integer");
                return;
            }
            APItems.TargetAPDebuffCount = result;
        }

        private static void TestPopup(string[] args)
        {
            string text = "Test message";
            if (args.Length >= 1)
            {
                text = args[0];
            }
            CL_ProgressionManager.ShowUnlockPopup(APItems.SpriteFromPath("WKRando/Assets/Archipelago_Icon.png"), "Test Popup", text, new Color(0f, 1f, 0f));

        }

        private static void ChangeClientSettings(string[] args)
        {
            for (int i  = 0; i < args.Length - 1; i++)
            {
                try
                {
                    switch (args[i].ToLower().Replace('-','_')) //hehe, lil faces
                    {
                        case "deathlink":
                            Logger.LogInfo($"deathlink...");
                            if (args[i + 1] == "amnesty")
                            {
                                ClientOptions.deathlink_amnesty = Convert.ToInt32(args[i + 2]);
                                Logger.LogInfo($"setting deathlink amnesty to {args[i + 2]}");
                                CommandConsole.Log($"setting deathlink amnesty to {args[i + 2]}");
                            }
                            else
                            {
                                if (Convert.ToBoolean(args[i + 1])) { ClientOptions.EnableDeathlink(); }
                                else if (!Convert.ToBoolean(args[i + 1])) { ClientOptions.DisableDeathlink(); }
                                Logger.LogInfo($"setting deathlink to {args[i + 1]}");
                                CommandConsole.Log($"setting deathlink to {args[i + 1]}");
                            }
                            break;
                            
                        case "deathlink_amnesty":
                            ClientOptions.deathlink_amnesty = Convert.ToInt32(args[i + 1]);
                            Logger.LogInfo($"setting deathlink amnesty to {args[i + 1]}");
                            CommandConsole.Log($"setting deathlink amnesty to {args[i + 1]}");
                            break;
                    }
                } catch { }
            }
            ClientOptions.SaveOptions();
        }
        
        private static void TestArchipelagoDeath(string[] args)
        {
            if (args.Length >= 1)
            {
                ArchipelagoClient.DeathMessage = string.Join(" ", args);
            }
            ENT_Player.playerObject.Kill("deathlink");
        }

        static void Postfix()
        {
            CommandConsole.BuildCommand("setdebuff", SetDebuffs).NotCheat().Description(
                "Sets the number of archipelago debuffs to the specified integer. Using a negative integer will apply buffs instead.");
            CommandConsole.BuildCommand("setloan", ChangeLoanCommand).Description("Sets the starting roach loan value to the specified value");
            CommandConsole.BuildCommand("connect", TryConnectCommand).NotCheat().Description("Attempts to connect to Archipelago Server \n")
                .OverValue((Func<object>) (() => (object) $"\n<color=yellow>Servername:</color> {ArchipelagoClient.Servername} " +
                                                 $"\n<color=yellow>Username:</color> {(ArchipelagoClient.Username.IsNullOrWhiteSpace() ? "none" : ArchipelagoClient.Username)} " +
                                                 $"\n<color=yellow>Password:</color> {(ArchipelagoClient.Password.IsNullOrWhiteSpace() ? "none" : new string('*',ArchipelagoClient.Password.Length))}\n"));
            CommandConsole.BuildCommand("reconnect", TryReconnectCommand).NotCheat().Description("Reconnects to Archipelago server in case of disconnect");
            CommandConsole.BuildCommand("resetapsave", ResetAPSaveData).NotCheat().Description("Deletes the current APSave's data for starting a new archipelago game");
            CommandConsole.BuildCommand("say", ArchipelagoClient.Say).NotCheat().Description("Sends a message to the archipelago client.");
            CommandConsole.BuildCommand("disconnect", DisconnectCommand).NotCheat();
            CommandConsole.BuildCommand("testpopup", TestPopup).NotCheat();
            CommandConsole.BuildCommand("ResetClientOptions", ArchipelagoClient.SetClientOptions).NotCheat().Description("Resets your client options back to what is listed in the yaml");
            CommandConsole.BuildCommand("ListClientOptions", ClientOptions.ListClientSettings).NotCheat().Description("Lists all options in the client settings, that can be set by the player in the client");
            CommandConsole.BuildCommand("ChangeClientOptions", ChangeClientSettings).NotCheat().Description("Changes options, use ListOptions to get a list of the ones you can change, spaces and both dashes work");
            CommandConsole.BuildCommand("TestArchipelagoDeath", TestArchipelagoDeath).Description("Kills the player with the specified death message");
        }
    }

    private static List<string> selectablenames = [];
    [HarmonyPatch(typeof(UI_CapsuleButton), "CheckAchievement")]
    class PatchModeUIButtons
    {

        static bool Prefix(UI_CapsuleButton __instance)
        {
            if (!selectablenames.Contains(__instance.name) & !APItems.ModeUnlocks.ContainsKey(__instance.name))
            {
                selectablenames.Add(__instance.name);
                Logger.LogInfo(__instance.name);
            }
            
            
            
            if (APItems.ModeUnlocks.TryGetValue(__instance.name, out var flag))
            {
                if ((Object) __instance.unlockIcon != (Object) null)
                    __instance.unlockIcon.gameObject.SetActive(!flag);
                if ((Object) __instance.group != (Object) null)
                {
                    __instance.group.interactable = flag;
                    __instance.group.alpha = flag ? 1f : 0.5f;
                }
                __instance.button.interactable = flag;
                return false;
            }

            return true;
        }
        
    }

    [HarmonyPatch(typeof(Cosmetic_Base), "IsUnlocked")]
    class PatchCosmeticBaseIsUnlocked
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(UI_FacilityMenu_Button), "Refresh")]
    class PatchMainMenuFacilityButtons
    {
        static void Postfix(UI_FacilityMenu_Button __instance)
        {
            Logger.LogInfo(__instance.upgrade.name);
            if (!ArchipelagoClient.Connected)
            {
                __instance.tooltip.tip = $"<color=red>DISCONNECTED\n</color>Connect to the archipelago server to purchase facility upgrades";
                __instance.lockRoot.SetActive(true);
                __instance.isLocked = true;
            }
            else
            {
                __instance.lockRoot.SetActive(false);
                __instance.isLocked = false;
                __instance.icon.gameObject.SetActive(true);
                __instance.icon.sprite = APItems.SpriteFromPath("WKRando/Assets/Archipelago_Icon.png");
                
            }
        }
        

    }
    

    [HarmonyPatch(typeof(SteamManager), "Update")]
    class DisableSteamLeaderboardUploads
    {
        static bool Prefix()
        {
            return false;
        }
    }
    
    [HarmonyPatch(typeof(ProgressionUnlock), "CheckUnlock")]
    class PatchUnlocks
    {
        static bool Prefix(ProgressionUnlock __instance, ref bool __result)
        {
            bool state = APItems.ProgressionUnlocks[__instance.id];
            __instance.state = state;
            if ((UnityEngine.Object) CL_GameManager.gMan != (UnityEngine.Object) null)
                CL_GameManager.SetGameFlag("unlock_" + __instance.name, state);
            __result = state;
            return false;
        }
        
    }
    
    [HarmonyPatch(typeof(App_PerkPage), "CheckIronKnuckle")]
    class PatchPerksPage
    {

        static bool Prefix(App_PerkPage __instance)
        {
            string level = WorldLoader.instance.GetCurrentLevel().GetLevel().levelName;
            int unlock = APItems.ProgressivePerkUnlocks;

            if (level is "Campaign_Interlude_Silo_To_Pipeworks_01" or "Campaign_Interlude_Sink_To_Pipeworks_01" & unlock < 1 || 
                level is "M3_Habitation_Shaft_Intro" or "Campaign_Interlude_Chute_To_Habitation" & unlock < 2 ||
                level is "Campaign_Interlude_Abyss_To_Nest_01_SafeArea" & unlock < 3)
            {
                __instance.window.os.messageManager.CreateMessage(new Message_Manager.Message_Packet()
                {
                    type = "default",
                    closeText = "Quit",
                    closeFunction = __instance.window.CloseApp,
                    message = "Perks for this sector are disabled by Archipelago!",
                    screenPos = new Vector2(0.0f, 0.0f)
                });
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(App_Unlocker))]
    class PatchShortcutUnlock
    {
        [HarmonyPatch("Start")]
        static void Postfix(App_Unlocker __instance)
        {
            
            /*Logger.LogInfo(__instance.authorizationTitle);
            Logger.LogInfo(__instance.flagOnUnlock);
            Logger.LogInfo(__instance.saveFlagOnUnlock);
            Logger.LogInfo(__instance.window.file.fileInfo.data);*/
            //Above commented out loggers are for testing, maybe they'll add another one in the future 

            __instance.password = "impossible passcode uwu you can't unlock this no matter what you do!";
            if (__instance.saveFlagOnUnlock == "unlockedsink" & APItems.ProgressionUnlocks["r_shortcut_tangledsink"] ||
                __instance.window.file.fileInfo.data.Split(",")[0].Split(":")[1] == "chutecode" & APItems.ProgressionUnlocks["r_shortcut_expulsionchute"])
            {
                __instance.authorizationCost = 0;
                __instance.CheckAuthorize();
            }
        }
        
    }
    
    [HarmonyPatch(typeof(Trinket), "IsUnlocked")]
    class PatchTrinketUnlock
    {
        static bool Prefix(Trinket __instance, ref bool __result)
        {
            //TODO: block the bindings from rendering in the first place
            if (APItems.TrinketUnlocks.TryGetValue(__instance.name, out var unlock))
            {
                __result = APOptions.EnableAllTrinkets || unlock;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(UI_TrinketPicker))]
    class PatchPips
    {
        //Changes the logic for if you can use the trinket to use the value of TrinketSlots instead
        [HarmonyPatch("UpdateTrinketActivation")]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions).MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_I4_0),
                    new CodeMatch(OpCodes.Stloc_0),
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Stloc_1))
                .Advance(2)
                .SetInstruction(new CodeInstruction(OpCodes.Ldsfld, 
                    AccessTools.Field(typeof(APOptions),nameof(APOptions.TrinketSlots))))
                .InstructionEnumeration();
        }
        //Extra failsafe 
        [HarmonyPatch("UpdatePips")]
        static void Prefix(object[] __args)
        {
            __args[0] = APOptions.TrinketSlots;
        }
    }

    [HarmonyPatch(typeof(Item))]
    class RemoveUnlockPopup
    {
        [HarmonyPatch("SendUnlockMessage")]
        static bool Prefix()
        {
            return false;
        }
    }
    
    //Blocks the buttons of these regions from being interacted with if their region is not found
    [HarmonyPatch(typeof(CL_Button), "Interact", [])]
    class PatchFacilityButtons
    {
        

        static bool Prefix(CL_Button __instance)
        {
            Logger.LogInfo(__instance.name); ;
            string currLvl = WorldLoader.instance.GetCurrentLevel().GetLevel().levelName;
            int reg = APItems.ProgressiveRegions;
            Logger.LogInfo("Current Progressive Region Count: " + reg);

            if (reg < 1 && (currLvl == "Campaign_Interlude_Silo_To_Pipeworks_01" && __instance.name == "Button.002" ||
                            currLvl == "Campaign_Interlude_Sink_To_Pipeworks_01" && __instance.name == "Prop_Button_02_Switch.01"))
            {
                return false;
            }
            if (reg < 2 && (currLvl == "M3_Habitation_Shaft_Intro" && __instance.name == "Prop_Button_03_Door" ||
                            currLvl == "Campaign_Interlude_Chute_To_Habitation" && __instance.name == "Prop_Button_04"))
            {
                return false;
            }
            if (reg < 3 && currLvl == "Campaign_Interlude_Habitation_To_Abyss_01" && __instance.name == "Prop_Button_03.01")
            {
                return false;
            }
            return !(reg < 4 & currLvl == "Campaign_Interlude_Abyss_To_Nest_01_SafeArea" && __instance.name == "Prop_Button_03");
        }
        
        
    }

    [HarmonyPatch(typeof(CL_GameManager), "Awake")]
    class PatchCommandConsole
    {

        static void Prefix()
        {

        }
    }
    
    [HarmonyPatch(typeof(CL_GameManager), "LoadIn", MethodType.Enumerator)]
    class PatchCommands
    {
        
        static void Postfix()
        {
            Loaded = true;
        }
        
        //Alters the roach loan value to refer to the LoanAmount value instantiated in this Plugin.cs file
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(OpCodes.Ldc_I4_S))
                .SetInstructionAndAdvance(new CodeInstruction(OpCodes.Ldsfld,
                    AccessTools.Field(typeof(Plugin), nameof(LoanAmount))))
                .InstructionEnumeration();
        }
    }

    [HarmonyPatch(typeof(StatManager.SaveData.Facility), "HasUpgrade")]
    class Force20RoachConditional
    {
        static bool Prefix(ref bool __result, object[] __args)
        {
            if ((string)__args[0] == "UPG_Global_StartingRoaches_T3")
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
    
    [HarmonyPatch(typeof(CL_GameManager), "OnDestroy")]
    class UnloadgMan
    {
        static void Prefix()
        {
            Loaded = false;
        }
    }
    
    [HarmonyPatch(typeof(FacilityUpgrade), "Purchase", typeof(StatManager.SaveData.Facility))] 
    class PatchPurchase
    {
        //Sends the check from the facility upgrade purchase to the client
        static void Prefix(FacilityUpgrade __instance, object[] __args)
        {
            long APID = APItems.FullFacilityUpgradetoAP[$"{((StatManager.SaveData.Facility)__args[0]).id} {__instance.id}"];
            ArchipelagoClient.TryQueueLocation(APID);
        }
    }
    
    //Initializes the custom perks
    [HarmonyPatch(typeof(CL_AssetManager), "Initialize")]
    class PatchPerks
    {
        static void Postfix()
        {
            CL_AssetManager.baseDatabase.perkAssets.AddRange([CustomPerks.ApBuff(), CustomPerks.ApDebuff()]);
        }
    }

    private static Dictionary<string, string> _nameToDescription;
    [HarmonyPatch(typeof(App_Facility_Card), "CheckLock")]
    class PatchPerkRefresh
    {
        static bool Prefix(App_Facility_Card __instance)
        {
            if (!ArchipelagoClient.Connected)
            {
                __instance.lockedObject.SetActive(true);
                __instance.tooltip.tip = "<color=red>LOCKED\nDisconnected from Archipelago!</color>\nConnect to purchase upgrades!";
                __instance.locked = true;
            }
            else
            {
                __instance.lockedObject.SetActive(false);
                __instance.locked = false;
                if (_nameToDescription != null && _nameToDescription.TryGetValue($"{__instance.facility.id} {__instance.upgrade.id}", out string description))
                {
                    __instance.tooltip.tip = description.Split("__")[1];
                    __instance.titleText.SetText(description.Split("__")[0]);
                    __instance.art.gameObject.SetActive(true);
                    __instance.art.sprite = APItems.SpriteFromPath("WKRando/Assets/Archipelago_Icon.png");
                    
                }
                else
                {
                    __instance.tooltip.tip = "Some archipelago item probably";
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(App_FacilitySlotHolder), "Start")]
    class PatchSlotHolder
    {
        //Makes the archipelago client request all 
        static void Prefix(App_FacilitySlotHolder __instance)
        {
            if (!ArchipelagoClient.Connected && __instance.window == null)
                return;
            Logger.LogInfo("Attempting to scout facility upgrades");
            Facility facilityAsset = CL_AssetManager.GetFacilityAsset(__instance.window.os.worldInterface.facilityID);
            List<long> locationsToScout = [];
            List<string> upgradeNames = [];
            foreach (FacilityUpgrade n in facilityAsset.GetUpgrades())
            {
                if (APItems.FullFacilityUpgradetoAP.TryGetValue($"{facilityAsset.id} {n.id}", out long location))
                {
                    locationsToScout.Add(location);
                    upgradeNames.Add($"{facilityAsset.id} {n.id}");
                }
            }
            List<string> descriptions = Task.Run(async () => await ArchipelagoClient.ScoutItemDescriptionFromID(locationsToScout.ToArray())).GetAwaiter().GetResult();
            _nameToDescription = upgradeNames.Zip(descriptions, (k,v) => new {k,v}).ToDictionary(x => x.k, x => x.v);
        }
    }

    [HarmonyPatch(typeof(MenuManager), "Start")]
    class PatchChallengeChecks
    {
        static void Prefix()
        {
            ChallengeQueue();
        }

        static void ChallengeQueue()
        {
            foreach (var t1 in CL_AssetManager.GetFullCombinedAssetDatabase().gamemodeAssets)
            {
                if (t1.gamemodeModule.GetType() == typeof(GamemodeModule_Challenge))
                {
                    foreach (var t in ((GamemodeModule_Challenge)t1.gamemodeModule).medals)
                    {
                        if (t.scoreRequirement < 1f)
                        {
                            t.scoreRequirement = 1f;
                        }
                    }
                }
            }

            foreach (M_Gamemode mode in CL_AssetManager.GetFullCombinedAssetDatabase().gamemodeAssets)
            {
                int rank = ChallengeMode.GetMedalRankFromGamemode(mode);
                for (int i = 0; i < rank; rank--)
                {
                    Logger.LogInfo($"Detected medal: {mode.gamemodeName} rank {rank}");
                    if (APItems.ChallengeMedaltoAPID.TryGetValue($"{mode.gamemodeName} {rank}", out long id) && !APItems.SentLocations.Contains(id))
                    {
                        ArchipelagoClient.TryQueueLocation(id);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(CL_LocalizationManager), "GetLocalization")]
    class PatchDeathmessage
    {

        static bool Prefix(ref string __result, object[] __args)
        {
            if ((string)__args[0] == "deathmessages" && (string)__args[1] == "deathlink")
            {
                __result = ArchipelagoClient.DeathMessage;
                return false;
            }
            return true;
        }
    }
    
    
    
    

    public class APOptions
    {
        public static int StartingDebuffs = 10;
        public static int TrinketSlots = 1;
        public static bool EnableAllTrinkets;
        public static string GoalArea = "M5_Nest_Trough_Ending_01";
        
        public static void UnlockChallenges()
        {
            foreach (string mode in APItems.ModeUnlocks.Keys.ToList())
            {
                if (mode.Contains("Challenge"))
                    APItems.ModeUnlocks[mode] = true;
            }
        }
        
    }

    

    // contains all information that will be written to in the client
    // stored in the datastorage path in a json format
    // C:\Users\X\AppData\LocalLow\Dark Machine\White Knuckle\Archipelago\ClientOptions.json
    // file only appears if its detected to be missing during awake

    public static CLoptions ClientOptions = new();
    public class CLoptions
    {
        public bool deathlink; // need to be simple fields for jsonutility to work
        public int deathlink_amnesty; // if you need to change that they have get/set youd need to move to a different json library

        public void EnableDeathlink()
        {
            deathlink = true;
            ArchipelagoClient.Deathlinkservice.EnableDeathLink();
        }
        public void DisableDeathlink()
        {
            deathlink = false;
            ArchipelagoClient.Deathlinkservice.DisableDeathLink();
        }        

        public void SaveOptions()
        {
            File.WriteAllText($"{Application.persistentDataPath}\\Archipelago\\ClientOptions.json", JsonUtility.ToJson(ClientOptions));
            Logger.LogInfo($"saving client options as {JsonUtility.ToJson(ClientOptions)}");
        }
        public void LoadOptions()
        {
            string jsonString = File.ReadAllText($"{Application.persistentDataPath}\\Archipelago\\ClientOptions.json");
            Logger.LogInfo($"loading client options as: {jsonString}");
            ClientOptions = JsonUtility.FromJson<CLoptions>(jsonString);
        }
        // left here so people dont forget to update it
        public void ListClientSettings(string[] args)
        {
            CommandConsole.Log($"Deathlink: {deathlink} \nDeathlink Amnesty: {deathlink_amnesty}");
        }
    }   
}