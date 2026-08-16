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
        }
    }




[HarmonyPatch(typeof(FacilityUpgrade), "IsOwned")]
    class AlterUpgradeCheck
    {
        static bool Prefix(FacilityUpgrade __instance, ref bool __result, object[] __args)
        {
            
            string facilityId = StatManager.saveData.GetFacility((string)__args[0]).id;
            if (APItems.FacilityDict.TryGetValue(facilityId, out Dictionary<string, bool> dict) && dict.ContainsKey(__instance.id))
            {
                 __result = dict[__instance.id];
                 return false;
            }
            return true;
        }
    }
    
    //Patch I was making before realizing that I can just do a prefix instead
    
    /*
    [HarmonyPatch(typeof(UT_FacilityUpgrade_Activator), "Check")]
    class AlterCheck2
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            //var jump = generator.DefineLabel();
            
            return new CodeMatcher(instructions).MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldfld),
                new CodeMatch(OpCodes.Brfalse_S),
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldc_I4_0),
                new CodeMatch(OpCodes.Ceq)
            ).InsertAndAdvance(
                //new CodeInstruction(OpCodes.Ldstr, "UPG_Recycler"),
                //new CodeInstruction(OpCodes.Ldfld, typeof(UT_FacilityUpgrade_Activator).GetField("invert", BindingFlags.Public)),
                //new CodeInstruction(OpCodes.Brfalse_S, jump),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Stloc_0)
                ).InstructionEnumeration();

        }
    }*/
    
    
    //This class replaces the directory that the game saves the game to with its own one
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

        public static void SendFacilityDebugCommand(string[] args)
        {
            if (args.Length > 0)
                if (APItems.FullFacilityUpgradetoAP.ContainsKey(args[0]))
                    ArchipelagoClient.SendItem(APItems.FullFacilityUpgradetoAP[args[0]]);
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
    //
    [HarmonyPatch(typeof(App_PerkPage), "CheckIronKnuckle")]
    class PatchPerksPage
    {

        static bool Prefix(App_PerkPage __instance)
        {
            string level = WorldLoader.instance.GetCurrentLevel().GetLevel().levelName;
            switch (level)
            {
                case "Campaign_Interlude_Silo_To_Pipeworks_01": 
                case "M3_Habitation_Shaft_Intro": 
                case "Campaign_Interlude_Habitation_To_Abyss_01": 
                case "Campaign_Interlude_Abyss_To_Nest_01_SafeArea":
                    __instance.window.os.messageManager.CreateMessage(new Message_Manager.Message_Packet()
                    {
                        type = "default",
                        closeText = "Quit",
                        closeFunction = new Action(__instance.window.CloseApp),
                        message = "Perks for this sector are disabled by Archipelago!",
                        screenPos = new Vector2(0.0f, 0.0f)
                    });
                    return false;
            }

            return true;
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

            if (currLvl == "Campaign_Interlude_Silo_To_Pipeworks_01" & __instance.name == "Button.002")
            {
                return false;
            }
            if (currLvl == "M3_Habitation_Shaft_Intro" & __instance.name == "Prop_Button_03_Door")
            {
                return false;
            }
            if (currLvl == "Campaign_Interlude_Habitation_To_Abyss_01" & __instance.name == "Prop_Button_03.01")
            {
                return false;
            }
            return !(currLvl == "Campaign_Interlude_Abyss_To_Nest_01_SafeArea" & __instance.name == "Prop_Button_03");
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
            CommandConsole.BuildCommand("say", ArchipelagoClient.Say).NotCheat()
                .Description("Sends a message to the archipelago client.");
            CommandConsole.BuildCommand("cheatroaches", AddCommands.CheatRoachesInCommand)
                .NotCheat().Description("Cheats in roaches without activating cheat mode for debug purposes");
            CommandConsole.BuildCommand("senditem", AddCommands.SendFacilityDebugCommand).NotCheat();
        }
    }
    
    [HarmonyPatch(typeof(CL_GameManager), "LoadIn", MethodType.Enumerator)]
    class PatchCommands
    {
        
        
        //Alters the roach loan value to refer to the LoanAmount value instantiated in this Plugin.cs file
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            var loanInstruction = new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Plugin), nameof(LoanAmount)));

            return new CodeMatcher(instructions)
                .MatchForward(false, 
                    new CodeMatch(OpCodes.Ldc_I4_S))
                .Repeat(matcher => 
                    matcher.SetInstructionAndAdvance(
                    loanInstruction))
                .InstructionEnumeration();
            
            
        }
    }
    
    [HarmonyPatch(typeof(FacilityUpgrade), "Purchase", typeof(StatManager.SaveData.Facility))]
    class PatchPurchase
    {
        //Sends the check from the facility upgrade purchase to the client
        static void Prefix(FacilityUpgrade __instance, object[] __args)
        {
            long APID = APItems.FullFacilityUpgradetoAP[$"{((StatManager.SaveData.Facility)__args[0]).id} {__instance.id}"];
            ArchipelagoClient.SendItem(APID);
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