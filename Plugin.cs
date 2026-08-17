using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Logger = UnityEngine.Logger;
using Object = System.Object;

namespace WKRando;


[BepInPlugin("com.wuckle.concsumer.name", "WhiteKnuckleRando", "1.0.0.0")]
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


        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Logger.LogInfo($"BasicPlugin Loaded");
    }

    //Handles all Update functions
    [HarmonyPatch(typeof(ENT_Player), "Update")]
    class update
    {
        static void Prefix()
        {
            ArchipelagoClient.Update();
            long roomID = APItems.RoomNameToAP[WorldLoader.instance.GetCurrentLevel().GetLevel().levelName];
            if (!APItems.SentLocations.Contains(roomID))
            {
                APItems.SentLocations.Add(roomID);
                ArchipelagoClient.TryQueueLocation(roomID);
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


    
    [HarmonyPatch(typeof(CL_GameManager), "Start")]
    class AddCommands
    {
        public static void ChangeLoanCommand(string[] args)
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

        public static async void TryConnectCommand(string[] args)
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

        public static async void TryReconnectCommand(string[] args)
        {
           await ArchipelagoClient.Connect();
        }

        public static void ResetAPSaveData(string[] args)
        {
            //Creates a fresh save
            CommandConsole.Log("Resetting Save...");
            File.Create(Path.Combine(UnityEngine.Application.persistentDataPath, "rando_save.json"));
        }

        public static async void DisconnectCommand(string[] args)
        {
            await ArchipelagoClient.Disconnect(false);
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

            if (APItems.ModeUnlocks.ContainsKey(__instance.name))
            {
                bool flag = APItems.ModeUnlocks[__instance.name];
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

            if (level is "Campaign_Interlude_Silo_To_Pipeworks_01" or "Campaign_Interlude_Sink_To_Pipeworks_01" & unlock > 0 || 
                level is "M3_Habitation_Shaft_Intro" or "Campaign_Interlude_Chute_To_Habitation" & unlock > 1 ||
                level is "Campaign_Interlude_Abyss_To_Nest_01_SafeArea" & unlock > 2)
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
            //TODO: Find the ID that must be added to gamemode exclude to prevent bindings from rendering
            if (__instance.name.Split("_")[0] == "binding")
            {
                __instance.icon = null;
                __instance.lockIcon = null;
                return true;
            }

            if (APItems.TrinketUnlocks.TryGetValue(__instance.name, out var unlock))
            {
                __result = unlock;
                return false;
            }
            return true;
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
            Logger.LogInfo(__instance.name);
            string currLvl = WorldLoader.instance.GetCurrentLevel().GetLevel().levelName;
            int reg = APItems.ProgressiveRegions;

            if (reg > 0 & (currLvl == "Campaign_Interlude_Silo_To_Pipeworks_01" & __instance.name == "Button.002" ||
                currLvl == "Campaign_Interlude_Sink_To_Pipeworks_01" & __instance.name == "Prop_Button_02_Switch.01"))
            {
                return false;
            }
            if (reg > 1 & (currLvl == "M3_Habitation_Shaft_Intro" & __instance.name == "Prop_Button_03_Door" ||
                currLvl == "Campaign_Interlude_Chute_To_Habitation" & __instance.name == "Prop_Button_04"))
            {
                return false;
            }
            if (reg > 2 & currLvl == "Campaign_Interlude_Habitation_To_Abyss_01" & __instance.name == "Prop_Button_03.01")
            {
                return false;
            }
            return !(reg > 3 & currLvl == "Campaign_Interlude_Abyss_To_Nest_01_SafeArea" & __instance.name == "Prop_Button_03");
        }
        
        
    }

    [HarmonyPatch(typeof(CL_GameManager), "Awake")]
    class PatchCommandConsole
    {

        static void Prefix()
        {
            selectablenames.ForEach(name => Logger.LogInfo(name));
            CommandConsole.BuildCommand("setloan", AddCommands.ChangeLoanCommand).Description("Sets the starting roach loan value to the specified value");
            CommandConsole.BuildCommand("connect", AddCommands.TryConnectCommand).NotCheat().Description("Attempts to connect to Archipelago Server: Server, Name");
            CommandConsole.BuildCommand("reconnect", AddCommands.TryReconnectCommand).NotCheat().Description("Reconnects to Archipelago server in case of disconnect");
            CommandConsole.BuildCommand("resetapsave", AddCommands.ResetAPSaveData).NotCheat().Description("Deletes the current APSave's data for starting a new archipelago game");
            CommandConsole.BuildCommand("say", ArchipelagoClient.Say).NotCheat().Description("Sends a message to the archipelago client.");
            CommandConsole.BuildCommand("disconnect", AddCommands.DisconnectCommand).NotCheat();
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
    
    
    [HarmonyPatch(typeof(App_Facility_Card), "CheckLock")]
    class PatchPerkRefresh
    {
        static bool Prefix(App_Facility_Card __instance)
        {
            if (!ArchipelagoClient.Connected)
            {
                __instance.lockedObject.SetActive(true);
                __instance.tooltip.tip = $"<color=red>LOCKED\nDisconnected from Archipelago!</color>\nConnect to purchase upgrades!";
                __instance.locked = true;
            }
            else
            {
                __instance.lockedObject.SetActive(false);
                __instance.tooltip.tip = __instance.upgrade.description;
                __instance.locked = false;
            }
            return false;
        }
    }
    

        
}