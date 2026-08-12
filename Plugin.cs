using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;

namespace WKRando;


[BepInPlugin("com.wuckle.concsumer.name", "WhiteKnuckleRando", "1.0.0.0")]
[BepInProcess("White Knuckle.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private static int _loanAmount = 0;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;

        var harmony = new Harmony("com.wuckle.concsumer.name");
        HarmonyFileLog.Enabled = true;
        
        //Creates a fresh save upon initialization for testing purposes
        File.Create(Path.Combine(UnityEngine.Application.persistentDataPath, "modded_save.json"));
        
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Logger.LogInfo($"BasicPlugin Loaded");
    }




    [HarmonyPatch(typeof(FacilityUpgrade), "IsOwned")]
    class AlterCheck
    {
        static bool Prefix(FacilityUpgrade __instance, ref bool __result, object[] __args)
        {
            string facilityId = StatManager.saveData.GetFacility((string)__args[0]).id;
            if (APItems._facilityDict.TryGetValue(facilityId, out Dictionary<string, bool> dict) && dict.ContainsKey(__instance.id))
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
    
    
    //This class replaces the directory that the game saves the game to with its own one, currently it is reset in the Awake function upon game start, however this may be changed later.
    [HarmonyPatch(typeof(StatManager), "Awake")]
    class AlterStats
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions).MatchForward(true,
                new CodeMatch(OpCodes.Ldstr, "wk_save.json")
            ).SetInstruction(new CodeInstruction(OpCodes.Ldstr, "rando_save.json")
            ).InstructionEnumeration();
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
                _loanAmount = int.Parse(args[0]);
            }
        }

        public static void TryConnectCommand(string[] args)
        {
            if (args.Length < 2 || args[0] == "")
            {
                CommandConsole.Log("Incorrect format: Requires server, username, and optional password, separated by spaces");
            }
            else if(args.Length == 2)
            {
                ArchipelagoClient.Connect(args[0], args[1]);
            }
            else
            {
                ArchipelagoClient.Connect(args[0],args[1], args[2]);
            }
        }

        public static void TryReconnectCommand(string[] args)
        {
            ArchipelagoClient.Connect();
        }
        
        //This was a successful attempt at making custom commands into the game through a transpiler, before it was realized
        //that you can just use a prefix instead.
        
        /*
        public static Action<string[]> CreateChangeLoanAction()
        {
            return new Action<string[]>(AddCommands.ChangeLoanCommand);
        }
        public static Action<string[]> CreateTryConnectCommandAction()
        {
            return new Action<string[]>(AddCommands.TryConnectCommand);
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var loanActionFactory = AccessTools.Method(typeof(AddCommands), "CreateChangeLoanAction");
            var connectActionFactory = AccessTools.Method(typeof(AddCommands), "TryConnectCommandAction");
            var commandAdd = AccessTools.Method(typeof(CommandConsole), "AddCommand");

            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldstr, "load"))
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldstr, "changeloan"),
                    new CodeInstruction(OpCodes.Call, loanActionFactory),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, commandAdd)
                )
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldstr, "connect"),
                    new CodeInstruction(OpCodes.Call, connectActionFactory),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, commandAdd)
                )
                .InstructionEnumeration();
        }*/
    }
    
    
    [HarmonyPatch(typeof(CL_GameManager), "LoadIn", MethodType.Enumerator)]
    class PatchCommands
    {
        
        static void Prefix()
        {
            CommandConsole.BuildCommand("setloan", new Action<string[]>(AddCommands.ChangeLoanCommand)).Description("Sets the starting roach loan value to the specified value");
            CommandConsole.BuildCommand("connect", new Action<string[]>(AddCommands.TryConnectCommand)).NotCheat().Description("Attempts to connect to Archipelago Server: Server, Name");
            CommandConsole.BuildCommand("reconnect", new Action<string[]>(AddCommands.TryReconnectCommand)).NotCheat().Description("Reconnects to Archiplago server in case of disconnect");
        }
        
        
        //Alters the roach loan value to refer to the _loanamount value instantiated in this Plugin.cs file
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            var loanInstruction = new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Plugin), nameof(_loanAmount)));

            return new CodeMatcher(instructions)
                .MatchForward(false, 
                    new CodeMatch(OpCodes.Ldc_I4_S))
                .Repeat(matcher => 
                    matcher.SetInstructionAndAdvance(
                    loanInstruction))
                .InstructionEnumeration();
            
            
        }
    }
    
    


        
}