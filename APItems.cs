using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace WKRando;

public class APItems
{
    public static int ProgressiveRegions;
    public static int ProgressivePerkUnlocks;
    public static int TotalBuffs = 0;
    
    // Stores flags for facility data to override ingame 
    public static Dictionary<string, Dictionary<string, bool>> FacilityDict { get; set; } =
        new Dictionary<string, Dictionary<string, bool>>()
    {
        ["GLOBAL"] = new() 
        {
            //All other global upgrades are either overridden or inconsequential
            ["UPG_Global_Bazaar_I2"] = false,
            ["UPG_Global_PerkRefresh"] = false,
        },
        ["CAMPAIGN_INTERLUDE_01"] = new ()
        {
            ["UPG_Recycler"] = false,
            ["UPG_ItemLocker_01"] = false,
            ["UPG_ItemLocker_02"] = false,
            ["UPG_SectorMaintenance"] = false,
            ["UPG_Ration_T1"] = false,
            ["UPG_Ration_T2"] = false,
            ["UPG_ATM"] = false,
            ["UPG_Vendor_T1_01"] = false,
            ["UPG_Vendor_T2_01"] = false
        },
        ["CAMPAIGN_INTERLUDE_02"] = new ()
        {
            ["UPG_Recycler"] = false,
            ["UPG_ItemLocker_01"] = false,
            ["UPG_ItemLocker_02"] = false,
            ["UPG_Ration_T1"] = false,
            ["UPG_Ration_T2"] = false,
            ["UPG_Vendor_T1_01"] = false,
            ["UPG_Vendor_T2_01"] = false,
        },
        ["CAMPAIGN_INTERLUDE_03"] = new ()
        {
            ["UPG_Recycler"] = false,
            ["UPG_ItemLocker_01"] = false,
            ["UPG_ItemLocker_02"] = false,
            ["UPG_ATM"] = false,
            ["UPG_Vendor_T2_01"] = false,
            ["UPG_Rho_Altar_Repair"] = false
        },
        ["CAMPAIGN_INTERLUDE_04"] = new ()
        {
            ["UPG_Recycler"] = false,
            ["UPG_ItemLocker_01"] = false,
            ["UPG_ItemLocker_02"] = false,
            ["UPG_Ration_T1"] = false,
            ["UPG_Ration_T2"] = false,
            ["UPG_Wine_T1"] = false,
            ["UPG_ATM"] = false,
            ["UPG_Vendor_T2_01"] = false,
        }
    };

    public static void ClearCampaignFacilities()
    {
        foreach (var outerKey in FacilityDict.Keys.ToList())
        {
            foreach (var innerKey in FacilityDict[outerKey].Keys.ToList())
            {
                FacilityDict[outerKey][innerKey] = false;
            }
        }
    }

    public static void ClearAllFlags()
    {
        ClearCampaignFacilities();
        foreach (string key in ProgressionUnlocks.Keys.ToList())
        {
            ProgressionUnlocks[key] = false;
        }
        foreach (string key in ModeUnlocks.Keys.ToList())
        {
            ModeUnlocks[key] = false;
        }
        foreach (string key in TrinketUnlocks.Keys.ToList())
        {
            TrinketUnlocks[key] = false;
        }
    }

    public static void UpdateFromId(long id)
    {
        Plugin.Logger.LogInfo("Attempting to update from AP Item: " + id);
        //Facility
        if (0xAAFFFFF >= id & id >= 0xAA11000 || id == 0xAA10000 || id == 0xAA10009)
        {
            var vals = APtoFullFacilityUpgrade[id].Split(" ");
            FacilityDict[vals[0]][vals[1]] = true;
            Facility.onPurchaseUpgrade();
        }
        //Progression Unlock (Rooms/Perks/Vendors)
        else if (0xACFFFFF >= id & id >= 0xAC00000)
        {
            ProgressionUnlocks[APIDtoProgressionUnlock[id]] = true;
        }
        //Mode Unlock (Challenge, maybe endless eventually)
        else if (0xADFFFFF >= id & id >= 0xAD00000)
        {
            ModeUnlocks[APIDtoModeUnlock[id]] = true;
        }
        //Trinket Unlock?
        else if (0xAEFFFFF >= id & id >= 0xAE00000)
        {
            TrinketUnlocks[APIDtoTrinketUnlock[id]] = true;
        }

        switch (id)
        {
            case 0xA900001:
                Plugin.LoanAmount += 1;
                CL_GameManager.runRoaches += 1;
                break;
            case 0xA900002:
                CL_GameManager.globalRoaches += 10;
                break;
            case 0xA900010:
                ProgressiveRegions += 1;
                break;
            case 0xA900020:
                TotalBuffs += 1;
                break;
            case 0xA900030:
                ProgressivePerkUnlocks += 1;
                break;
        }
    }

    public static List<long> CheckFacilities()
    {
        return StatManager.saveData.facilities.SelectMany(facility => facility.upgrades, (facility, upgrade) => FullFacilityUpgradetoAP[$"{facility} {upgrade}"]).Where(id => !SentLocations.Contains((long)id)).ToList();
    }

    public static long? CheckRoom()
    {
        string room = WorldLoader.instance.GetCurrentLevel().GetLevel().levelName;
        if (RoomNameToAP.ContainsKey(room) & !SentLocations.Contains(RoomNameToAP[room]))
        {
            return RoomNameToAP[room];
        }

        return null;
    }

    public static List<long> SentLocations = new List<long>();
    
    //Handles AP IDs of facility upgrades
    public static Dictionary<long, string> APtoFullFacilityUpgrade = new Dictionary<long, string>()
    {
        [0xAA10000] = "GLOBAL UPG_Global_Bazaar_I2",
        [0xAA10001] = "GLOBAL UPG_Global_StartingRoaches_T1",
        [0xAA10002] = "GLOBAL UPG_Global_StartingRoaches_T2",
        [0xAA10003] = "GLOBAL UPG_Global_StartingRoaches_T3",
        [0xAA10004] = "GLOBAL UPG_Global_Buddy_T1",
        [0xAA10005] = "GLOBAL UPG_Global_Pouch_T1",
        [0xAA10006] = "GLOBAL UPG_Global_OrnamentalHammer",
        [0xAA10007] = "GLOBAL UPG_Global_Cosmetic_WorkGloves",
        [0xAA10008] = "GLOBAL UPG_Global_Cosmetic_SpecialtyGloves",
        [0xAA10009] = "GLOBAL UPG_Global_PerkRefresh",
        
        [0xAA11000] = "CAMPAIGN_INTERLUDE_01 UPG_Recycler",
        [0xAA11001] = "CAMPAIGN_INTERLUDE_01 UPG_SectorMaintenance",
        [0xAA11002] = "CAMPAIGN_INTERLUDE_01 UPG_ItemLocker_01",
        [0xAA11003] = "CAMPAIGN_INTERLUDE_01 UPG_ItemLocker_02",
        [0xAA11004] = "CAMPAIGN_INTERLUDE_01 UPG_Ration_T1",
        [0xAA11005] = "CAMPAIGN_INTERLUDE_01 UPG_Ration_T2",
        [0xAA11006] = "CAMPAIGN_INTERLUDE_01 UPG_ATM",
        [0xAA11007] = "CAMPAIGN_INTERLUDE_01 UPG_Vendor_T1_01",
        [0xAA11008] = "CAMPAIGN_INTERLUDE_01 UPG_Vendor_T2_02",
        
        [0xAA12000] = "CAMPAIGN_INTERLUDE_02 UPG_Recycler", 
        [0xAA12001] = "CAMPAIGN_INTERLUDE_02 UPG_ItemLocker_01",
        [0xAA12002] = "CAMPAIGN_INTERLUDE_02 UPG_ItemLocker_02",
        [0xAA12003] = "CAMPAIGN_INTERLUDE_02 UPG_Ration_T1", 
        [0xAA12004] = "CAMPAIGN_INTERLUDE_02 UPG_Ration_T2",
        [0xAA12005] = "CAMPAIGN_INTERLUDE_02 UPG_Vendor_T1_01",
        [0xAA12006] = "CAMPAIGN_INTERLUDE_02 UPG_Vendor_T2_01",
        
        [0xAA13000] = "CAMPAIGN_INTERLUDE_03 UPG_Recycler",
        [0xAA13001] = "CAMPAIGN_INTERLUDE_03 UPG_ItemLocker_01",
        [0xAA13002] = "CAMPAIGN_INTERLUDE_03 UPG_ItemLocker_02",
        [0xAA13003] = "CAMPAIGN_INTERLUDE_03 UPG_ATM",
        [0xAA13004] = "CAMPAIGN_INTERLUDE_03 UPG_Vendor_T2_01",
        [0xAA13005] = "CAMPAIGN_INTERLUDE_03 UPG_Rho_Altar_Repair",
        
        [0xAA14000] = "CAMPAIGN_INTERLUDE_04 UPG_Recycler",
        [0xAA14001] = "CAMPAIGN_INTERLUDE_04 UPG_ItemLocker_01",
        [0xAA14002] = "CAMPAIGN_INTERLUDE_04 UPG_ItemLocker_02",
        [0xAA14003] = "CAMPAIGN_INTERLUDE_04 UPG_Ration_T1",
        [0xAA14004] = "CAMPAIGN_INTERLUDE_04 UPG_Ration_T2",
        [0xAA14005] = "CAMPAIGN_INTERLUDE_04 UPG_Wine_T1",
        [0xAA14006] = "CAMPAIGN_INTERLUDE_04 UPG_ATM",
        [0xAA14007] = "CAMPAIGN_INTERLUDE_04 UPG_Vendor_T2_01"
    };
    
    public static Dictionary<string, long> FullFacilityUpgradetoAP = new Dictionary<string, long>()
    {
        ["GLOBAL UPG_Global_Bazaar_I2"] = 0xAA10000,
        ["GLOBAL UPG_Global_StartingRoaches_T1"] = 0xAA10001,
        ["GLOBAL UPG_Global_StartingRoaches_T2"] = 0xAA10002,
        ["GLOBAL UPG_Global_StartingRoaches_T3"] = 0xAA10003,
        ["GLOBAL UPG_Global_Buddy_T1"] = 0xAA10004,
        ["GLOBAL UPG_Global_Pouch_T1"] = 0xAA10005,
        ["GLOBAL UPG_Global_OrnamentalHammer"] = 0xAA10006,
        ["GLOBAL UPG_Global_Cosmetic_WorkGloves"] = 0xAA10007,
        ["GLOBAL UPG_Global_Cosmetic_SpecialtyGloves"] = 0xAA10008,
        ["GLOBAL UPG_Global_PerkRefresh"]  = 0xAA10009,

        ["CAMPAIGN_INTERLUDE_01 UPG_Recycler"] = 0xAA11000,
        ["CAMPAIGN_INTERLUDE_01 UPG_SectorMaintenance"] = 0xAA11001,
        ["CAMPAIGN_INTERLUDE_01 UPG_ItemLocker_01"] = 0xAA11002,
        ["CAMPAIGN_INTERLUDE_01 UPG_ItemLocker_02"] = 0xAA11003,
        ["CAMPAIGN_INTERLUDE_01 UPG_Ration_T1"] = 0xAA11004,
        ["CAMPAIGN_INTERLUDE_01 UPG_Ration_T2"] = 0xAA11005,
        ["CAMPAIGN_INTERLUDE_01 UPG_ATM"] = 0xAA11006,
        ["CAMPAIGN_INTERLUDE_01 UPG_Vendor_T1_01"] = 0xAA11007,
        ["CAMPAIGN_INTERLUDE_01 UPG_Vendor_T1_02"] = 0xAA11008,
        
        ["CAMPAIGN_INTERLUDE_02 UPG_Recycler"] = 0xAA12000,
        ["CAMPAIGN_INTERLUDE_02 UPG_ItemLocker_01"] = 0xAA12001,
        ["CAMPAIGN_INTERLUDE_02 UPG_ItemLocker_02"] = 0xAA12002,
        ["CAMPAIGN_INTERLUDE_02 UPG_Ration_T1"] = 0xAA12003,
        ["CAMPAIGN_INTERLUDE_02 UPG_Ration_T2"] = 0xAA12004,
        ["CAMPAIGN_INTERLUDE_02 UPG_Vendor_T1_01"] = 0xAA12005,
        ["CAMPAIGN_INTERLUDE_02 UPG_Vendor_T2_01"] = 0xAA12006,

        ["CAMPAIGN_INTERLUDE_03 UPG_Recycler"] = 0xAA13000,
        ["CAMPAIGN_INTERLUDE_03 UPG_ItemLocker_01"] = 0xAA13001,
        ["CAMPAIGN_INTERLUDE_03 UPG_ItemLocker_02"] = 0xAA13002,
        ["CAMPAIGN_INTERLUDE_03 UPG_ATM"] = 0xAA13003,
        ["CAMPAIGN_INTERLUDE_03 UPG_Vendor_T2_01"] = 0xAA13004,
        ["CAMPAIGN_INTERLUDE_03 UPG_Rho_Altar_Repair"] = 0xAA13005,

        ["CAMPAIGN_INTERLUDE_04 UPG_Recycler"] = 0xAA14000,
        ["CAMPAIGN_INTERLUDE_04 UPG_ItemLocker_01"] = 0xAA14001,
        ["CAMPAIGN_INTERLUDE_04 UPG_ItemLocker_02"] = 0xAA14002,
        ["CAMPAIGN_INTERLUDE_04 UPG_Ration_T1"] = 0xAA14003,
        ["CAMPAIGN_INTERLUDE_04 UPG_Ration_T2"] = 0xAA14004,
        ["CAMPAIGN_INTERLUDE_04 UPG_Wine_T1"] = 0xAA14005,
        ["CAMPAIGN_INTERLUDE_04 UPG_ATM"] = 0xAA14006,
        ["CAMPAIGN_INTERLUDE_04 UPG_Vendor_T2_01"] = 0xAA14007
    };

    public static Dictionary<string, long> RoomNameToAP = new Dictionary<string, long>()
    {
        //Default Silos rooms
        ["M1_Silos_Storage_01"] = 0xAB10000,
        ["M1_Silos_Storage_03"] = 0xAB10001,
        ["M1_Silos_Storage_06"] = 0xAB10002,
        ["M1_Silos_Storage_09"] = 0xAB10003,
        ["M1_Silos_Storage_10"] = 0xAB10004,
        ["M1_Silos_Storage_15"] = 0xAB10005,

        ["M1_Silos_Air_02"] = 0xAB10006,
        ["M1_Silos_Air_03"] = 0xAB10007,
        ["M1_Silos_Air_04"] = 0xAB10008,
        ["M1_Silos_Air_08"] = 0xAB10009,
        
        ["M1_Silos_Broken_01"] = 0xAB1000A,
        ["M1_Silos_Broken_04"] = 0xAB1000B,
        ["M1_Silos_Broken_08"] = 0xAB1000C,
        ["M1_Silos_Broken_09"] = 0xAB1000D,
        ["M1_Silos_Broken_10"] = 0xAB1000E,
        
        //Deep Storage Tier 1 Unlocks
        ["M1_Silos_Storage_04"] = 0xAB10100,
        ["M1_Silos_Storage_11"] = 0xAB10101,
        ["M1_Silos_Storage_12"] = 0xAB10102,
        
        //Silos Tier 1 Unlocks
        ["M1_Silos_Storage_02"] = 0xAB10200,
        ["M1_Silos_Storage_08"] = 0xAB10201,
        ["M1_Silos_Storage_16"] = 0xAB10202,
        ["M1_Silos_Air_05"] = 0xAB10203,
        ["M1_Silos_Air_06"] = 0xAB10204,
        ["M1_Silos_Broken_03"] = 0xAB10205,
        ["M1_Silos_Broken_05"] = 0xAB10206,
        
        //Silos Tier 2 Unlocks
        ["M1_Silos_Storage_05"] = 0xAB10300,
        ["M1_Silos_Storage_07"] = 0xAB10301,
        ["M1_Silos_Air_01"] = 0xAB10302,
        ["M1_Silos_Air_09"] = 0xAB10303,
        ["M1_Silos_Broken_02"] = 0xAB10304,
        ["M1_Silos_Broken_06"] = 0xAB10305,
        
        //Silos Tier 3 Unlocks
        ["M1_Silos_Storage_13"] = 0xAB10400,
        ["M1_Silos_Storage_14"] = 0xAB10401,
        ["M1_Silos_Air_07"] = 0xAB10402,
        ["M1_Silos_Air_10"] = 0xAB10403,
        ["M1_Silos_Broken_07"] = 0xAB10404,
        
        //Silos Tier 4 Unlocks
        ["M1_Silos_Storage_17"] = 0xAB10500,
        ["M1_Silos_Air_11"] = 0xAB10501,
        ["M1_Silos_Broken_11"] = 0xAB10502,
        
        //Pipeworks Rooms
        ["M2_Pipeworks_Drainage_01"] = 0xAB20000,
        ["M2_Pipeworks_Drainage_02"] = 0xAB20001,
        ["M2_Pipeworks_Drainage_05"] = 0xAB20002,
        ["M2_Pipeworks_Drainage_07"] = 0xAB20003,
        ["M2_Pipeworks_Drainage_08"] = 0xAB20004,
        ["M2_Pipeworks_Drainage_09"] = 0xAB20005,
        ["M2_Pipeworks_Drainage_10"] = 0xAB20006,
        
        ["M2_Pipeworks_Waste_02"] = 0xAB20100,
        ["M2_Pipeworks_Waste_04"] = 0xAB20101,
        ["M2_Pipeworks_Waste_05"] = 0xAB20102,
        
        ["M2_Pipeworks_Organ_02"] = 0xAB20200,
        ["M2_Pipeworks_Organ_05"] = 0xAB20201,
        ["M2_Pipeworks_Organ_06"] = 0xAB20202,
        ["M2_Pipeworks_Organ_07"] = 0xAB20203,
        
        
        //Pipeworks Tier 1 Unlocks
        ["M2_Pipeworks_Drainage_03"] = 0xAB21000,
        ["M2_Pipeworks_Drainage_04"] = 0xAB21001,
        
        ["M2_Pipeworks_Waste_01"] = 0xAB21100,
        
        ["M2_Pipeworks_Organ_01"] = 0xAB21200,
        ["M2_Pipeworks_Organ_03"] = 0xAB21201,
        
        
        //Pipeworks Tier 2 Unlocks
        ["M2_Pipeworks_Drainage_06"] = 0xAB22000,
        
        ["M2_Pipeworks_Waste_03"] = 0xAB22100,
        
        ["M2_Pipeworks_Organ_04"] = 0xAB22200,
        ["M2_Pipeworks_Organ_08"] = 0xAB22201,
        
        //Pipeworks Tier 3 Unlock
        ["M2_Pipeworks_Organ_09"] = 0xAB23200,
        
        //Habitation Rooms
        ["M3_Habitation_Shaft_01"] = 0xAB30000,
        ["M3_Habitation_Shaft_02"] = 0xAB30001,
        ["M3_Habitation_Shaft_03"] = 0xAB30002,
        ["M3_Habitation_Shaft_04"] = 0xAB30003,
        ["M3_Habitation_Shaft_05"] = 0xAB30004,
        ["M3_Habitation_Shaft_06"] = 0xAB30005,
        ["M3_Habitation_Shaft_To_Pier"] = 0xAB30006,
        
        ["M3_Habitation_Pier_01"] = 0xAB30100,
        ["M3_Habitation_Pier_02"] = 0xAB30101,
        
        ["M3_Habitation_Lab_Lobby"] = 0xAB30200,
        ["M3_Habitation_Lab_01"] =  0xAB30201,
        ["M3_Habitation_Lab_02"] = 0xAB30202,
        ["M3_Habitation_Lab_03"] = 0xAB30203,
        ["M3_Habitation_Lab_04"] = 0xAB30204,
        ["M3_Habitation_Lab_Ending"] = 0xAB30205,
        
        //Habitation Tier 1 Unlocks
        ["M3_Habitation_Shaft_07"] = 0xAB31000,
        
        ["M3_Habitation_Pier_03"] = 0xAB31100,
        ["M3_Habitation_Pier_04"] = 0xAB31101,
        
        ["M3_Habitation_Lab_05"] = 0xAB31200,
        ["M3_Habitation_Lab_06"] = 0xAB31201,
        ["M3_Habitation_Lab_07"] = 0xAB31202,
        ["M3_Habitation_Lab_08"] = 0xAB31203,
        
        //Abyss Rooms
        ["M4_Abyss_Intro_01"] =  0xAB40000,
        ["M4_Abyss_Outro_01"] = 0xAB40001,
        
        ["M4_Abyss_Transit_01"] = 0xAB40101,
        ["M4_Abyss_Transit_02"] = 0xAB40102,
        ["M4_Abyss_Transit_03"] = 0xAB40103,
        ["M4_Abyss_Transit_04"] = 0xAB40104,
        ["M4_Abyss_Transit_05"] = 0xAB40105,
        ["M4_Abyss_Transit_06"] = 0xAB40106,
        
        ["M4_Abyss_Handle_01"] = 0xAB40200,
        
        ["M4_Abyss_Garden_01"] = 0xAB40300,
        ["M4_Abyss_Garden_02"] = 0xAB40301,
        ["M4_Abyss_Garden_03"] = 0xAB40302,
        ["M4_Abyss_Garden_04"] = 0xAB40303,
        
        
        //Abyss Tier 1 Unlocks
        ["M4_Abyss_Handle_02"] = 0xAB40400,
        
        //Nest Rooms
        ["M5_Nest_Infested_01"] = 0xAB50000,
        ["M5_Nest_Infested_02"] = 0xAB50001,
        ["M5_Nest_Infested_03"] = 0xAB50002,
        ["M5_Nest_Infested_04"] = 0xAB50003,
        ["M5_Nest_Infested_05"] = 0xAB50004,
        ["M5_Nest_Infested_HunterIntro_01"] = 0xAB50005,
        ["M5_Nest_Infested_Chase_01"] = 0xAB50006,
        ["M5_Nest_Infested_Chase_02"] = 0xAB50007,
        
        ["M5_Nest_Trough_01"] = 0xAB50100,
        ["M5_Nest_Trough_02"] = 0xAB50101,
        ["M5_Nest_Trough_03"] = 0xAB50102,
        ["M5_Nest_Trough_04"] = 0xAB50103,
        ["M5_Nest_Trough_05"] = 0xAB50104,
        ["M5_Nest_Trough_06"] = 0xAB50105,
        ["M5_Nest_Trough_07"] = 0xAB50106,
        ["M5_Nest_Trough_08"] = 0xAB50107,
        ["M5_Nest_Trough_Ending_01"] = 0xAB50108,
        
        ["M5_Nest_HotZone_Intro_01"] = 0xAB50200,
        ["M5_Nest_HotZone_Outro_01"] = 0xAB50201,
        ["M5_Nest_HotZone_01"] = 0xAB50202,
        ["M5_Nest_HotZone_02"] = 0xAB50203,
        ["M5_Nest_HotZone_03"] = 0xAB50204,
        ["M5_Nest_HotZone_04"] = 0xAB50205,
        
        //Tangled Sink Rooms
        ["S1_TangledSink_Intro_01"] = 0xAB60000,
        ["S1_TangledSink_01"] = 0xAB60001,
        ["S1_TangledSink_02"] = 0xAB60002,
        ["S1_TangledSink_03"] = 0xAB60003,
        ["S1_TangledSink_04"] = 0xAB60004,
        
        //Expulsion Chute Rooms
        ["S1_GarbageChute_Start_01"] = 0xAB70000,
        ["S1_GarbageChute_01"] = 0xAB70001,
        ["S1_GarbageChute_02"] = 0xAB70002,
        ["S1_GarbageChute_03"] = 0xAB70003,
        ["S1_GarbageChute_04"] = 0xAB70004,
        ["S1_GarbageChute_05"] = 0xAB70005,
        ["S1_GarbageChute_06"] = 0xAB70006,
        ["S1_GarbageChute_End_01"] = 0xAB70007,
        
        //Training Sector Rooms
        ["T1_Foundations_Items_01"] = 0xAB80000,
        ["T1_Foundations_Items_02"] = 0xAB80001,
        ["T1_Foundations_Items_03"] = 0xAB80002,
        ["T1_Foundations_Items_04"] = 0xAB80003,
        ["T1_Foundations_Items_05"] = 0xAB80004,
        ["T1_Foundations_Low_01"] = 0xAB80005,
        ["T1_Foundations_Low_02"] = 0xAB80006,
        ["T1_Foundations_Low_03"] = 0xAB80007,
        ["T1_Foundations_Low_04"] = 0xAB80008,
        ["T1_Foundations_Low_05"] = 0xAB80009,
        ["T1_Foundations_Low_06"] = 0xAB8000A,
        ["T1_Foundations_Mastery_01"] = 0xAB8000B,
        ["T1_Foundations_Mastery_02"] = 0xAB8000C,
        
    };
    
    public static Dictionary<string, bool> ProgressionUnlocks = new Dictionary<string, bool>()
    {
        //overridden here to prevent binding unlock popups
        ["binding_abyss"] = false,
        ["binding_core"] = false,
        ["binding_habitation"] = false,
        ["binding_nest"] = false,
        ["binding_roach"] = false,
            
        //Overrides popups
        ["challenge_advancedcourse"] = false,
        ["challenge_boostcourse"] = false,
        ["challenge_commsarray"] = false,
        ["challenge_fracturedterritory"] = false,
        ["challenge_roachrun"] = false,
        ["challenge_shutteredrift"] = false,
        
        //Set some of these to be off by default (specifically to prevent any popups)
        ["cosmetic_bloodied"] = false,
        ["cosmetic_crowbar"] = false,
        ["cosmetic_denizen"] = false,
        ["cosmetic_experienced"] = false,
        ["cosmetic_glove_leather"] = false,
        ["cosmetic_glove_winter"] = false,
        ["cosmetic_glove_wraps"] = false,
        ["cosmetic_hammer_alternate"] = false,
        ["cosmetic_hammer_mallet"] = false,
        ["cosmetic_hammer_mallet_christmas"] = false,
        ["cosmetic_hammer_pipe"] = false,
        ["cosmetic_hammer_wrench"] = false,
        ["cosmetic_hand_celestial"] = false,
        ["cosmetic_hand_stone"] = false,
        ["cosmetic_hand_sunspot"] = false,
        ["cosmetic_infested"] = false,
        ["cosmetic_inverted"] = false,
        ["cosmetic_maintenanceglove_new"] = false,
        ["cosmetic_maintenanceglove_worn"] = false,
        ["cosmetic_marionette"] = false,
        ["cosmetic_hammer_ornamental"] = false,
        ["cosmetic_outline"] = false,
        ["cosmetic_roach"] = false,
        ["cosmetic_scribble"] = false,
        ["cosmetic_skeletontattoo"] = false,
        ["cosmetic_streaked"] = false,
        ["cosmetic_xray"] = false,
        
        //Locks these modes to be off
        ["r_hardmode"] = false,
        ["r_10kendless"] = false,
        ["r_competitive"] = false,
        
        //Woah the real unlocks!
        ["perk_mother"] = false,
        ["perk_t1"] = false,
        ["perk_t2"] = false,
        ["perk_t3"] = false,
        ["perk_t4"] = false,
        ["perk_t5"] = false,
        ["perk_u_adoption"] = false,
        ["perk_u_delta"] = false,
        ["perk_u_t1"] = false,
        ["perk_u_t2"] = false,
        ["perk_u_t3"] = false,
        
        ["r_abyss_t1"] = false,
        ["r_deepstorage_t1"] = false,
        ["r_habitation_t1"] = false,
        ["r_pipeworks_t1"] = false,
        ["r_pipeworks_t2"] = false,
        ["r_pipeworks_t3"] = false,
        ["r_shortcut_expulsionchute"] = true,
        ["r_shortcut_tangledsink"] = true,
        ["r_silos_t1"] = false,
        ["r_silos_t2"] = false,
        ["r_silos_t3"] = false,
        ["r_silos_t4"] = false,
        
        //Also to disable the popups
        ["trinket_calmingbuddy"] = false,
        ["trinket_deltalabs"] = false,
        ["trinket_helmet"] = false,
        ["trinket_interlude01"] = false,
        ["trinket_interlude02"] = false,
        ["trinket_interlude03"] = false,
        ["trinket_interlude04"] = false,
        ["trinket_pouch"] = false,
        ["trinket_recycler"] = false,
        ["trinket_deepstorage"] = false,
        
        //These do keep popups...
        ["vendor_item_autopiton"] = false,
        ["vendor_item_blinkeye"] = false,
        ["vendor_item_explosiverebar"] = false,
        ["vendor_item_flaregun"] = false,
        ["vendor_item_flares"] = false,
        ["vendor_item_foodbar"] = false,
        ["vendor_item_10mm_ammo"] = false,
        ["vendor_item_injector"] = false,
        ["vendor_item_pills"] = false,
        ["vendor_item_grub"] = false
    };

    public static Dictionary<string, long> ProgressionUnlocksToAPID = new Dictionary<string, long>()
    {
        /*["binding_abyss"]
        ["binding_core"]
        ["binding_habitation"]
        ["binding_nest"]
        ["binding_roach"]*/
        //Disabled
        
        //Deprecated will remove later i guess
        ["challenge_advancedcourse"] = 0xAC00000,
        ["challenge_boostcourse"] =  0xAC00001,
        ["challenge_commsarray"] = 0xAC00002,
        ["challenge_fracturedterritory"]  = 0xAC00003,
        ["challenge_roachrun"]  = 0xAC00004,
        ["challenge_shutteredrift"]  = 0xAC00005,
        
        /*["cosmetic_bloodied"]
        ["cosmetic_crowbar"]
        ["cosmetic_denizen"]
        ["cosmetic_glove_leather"]
        ["cosmetic_glove_winter"]
        ["cosmetic_glove_wraps"]
        ["cosmetic_hammer_mallet"]
        ["cosmetic_hammer_mallet_christmas"]
        ["cosmetic_hammer_pipe"]
        ["cosmetic_hammer_wrench"]
        ["cosmetic_hand_celestial"]
        ["cosmetic_hand_stone"]
        ["cosmetic_hand_sunspot"]
        ["cosmetic_infested"]
        ["cosmetic_inverted"]
        ["cosmetic_maintenanceglove_new"]
        ["cosmetic_marionette"]
        ["cosmetic_hammer_ornamental"]
        ["cosmetic_outline"]
        ["cosmetic_roach"]
        ["cosmetic_scribble"]
        ["cosmetic_skeletontattoo"]
        ["cosmetic_streaked"]
        ["cosmetic_xray"]
        ["r_hardmode"]
        ["r_10kendless"]
        */ // also disabled... for now
        
        //Perk group unlocks
        ["perk_mother"] = 0xAC00100,
        ["perk_t1"] = 0xAC00101,
        ["perk_t2"] = 0xAC00102,
        ["perk_t3"] = 0xAC00103,
        ["perk_t4"] = 0xAC00104,
        ["perk_t5"] = 0xAC00105,
        ["perk_u_adoption"] = 0xAC00106,
        ["perk_u_delta"] = 0xAC00107,
        ["perk_u_t1"] = 0xAC00108,
        ["perk_u_t2"] = 0xAC00109,
        ["perk_u_t3"] = 0xAC0010A,
        
        //Level unlocks
        ["r_abyss_t1"] = 0xAC00200,
        ["r_habitation_t1"] = 0xAC00201,
        ["r_pipeworks_t1"] = 0xAC00202,
        ["r_pipeworks_t2"] = 0xAC00203,
        ["r_pipeworks_t3"] = 0xAC00204,
        ["r_shortcut_expulsionchute"] = 0xAC00205,
        ["r_shortcut_tangledsink"] = 0xAC00206,
        ["r_silos_t1"] = 0xAC00207,
        ["r_silos_t2"] = 0xAC00208,
        ["r_silos_t3"] = 0xAC00209,
        ["r_silos_t4"] = 0xAC0020A,
        ["r_deepstorage_t1"] = 0xAC0020B,
        
        //Remove these later ig
        ["trinket_deltalabs"] = 0xAC00300,
        ["trinket_helmet"] = 0xAC00301,
        ["trinket_interlude02"] = 0xAC00302,
        ["trinket_interlude03"] = 0xAC00303,
        ["trinket_interlude04"] = 0xAC00304,
        ["trinket_recycler"] = 0xAC00305,
        
        //Vendor item unlocks
        ["vendor_item_autopiton"] = 0xAC00400,
        ["vendor_item_blinkeye"] = 0xAC00401,
        ["vendor_item_explosiverebar"] = 0xAC00402,
        ["vendor_item_flaregun"] = 0xAC00403,
        ["vendor_item_flares"] = 0xAC00404,
        ["vendor_item_foodbar"] = 0xAC00405,
        ["vendor_item_10mm_ammo"] = 0xAC00406,
        ["vendor_item_injector"] = 0xAC00407,
        ["vendor_item_pills"] = 0xAC00408,
        ["vendor_item_grubs"] = 0xAC00409
    };

    public static Dictionary<long, string> APIDtoProgressionUnlock = new Dictionary<long, string>()
    {
        [0xAC00100] = "perk_mother",
        [0xAC00101] = "perk_t1",
        [0xAC00102] = "perk_t2",
        [0xAC00103] = "perk_t3",
        [0xAC00104] = "perk_t4",
        [0xAC00105] = "perk_t5",
        [0xAC00106] = "perk_u_adoption",
        [0xAC00107] = "perk_u_delta",
        [0xAC00108] = "perk_u_t1",
        [0xAC00109] = "perk_u_t2",
        [0xAC0010A] = "perk_u_t3",

        [0xAC00200] = "r_abyss_t1",
        [0xAC00201] = "r_habitation_t1",
        [0xAC00202] = "r_pipeworks_t1",
        [0xAC00203] = "r_pipeworks_t2",
        [0xAC00204] = "r_pipeworks_t3",
        [0xAC00205] = "r_shortcut_expulsionchute",
        [0xAC00206] = "r_shortcut_tangledsink",
        [0xAC00207] = "r_silos_t1",
        [0xAC00208] = "r_silos_t2",
        [0xAC00209] = "r_silos_t3",
        [0xAC0020A] = "r_silos_t4",
        [0xAC0020B] = "r_deepstorage_t1",
    };
    

    public static Dictionary<string, bool> ModeUnlocks = new Dictionary<string, bool>()
    {
        ["Mode Selection Button - Campaign Variant"] = true,
        ["Mode Selection Button - Tutorial"] = true,
        ["Mode Selection Button - Training Sector"] = false,
        ["Mode Selection Button - Endless"] = false,
        ["Mode Selection Button - Endless Underworks"] = false,
        ["Mode Selection Button - Endless Superstructure"] = false,
        ["Mode Selection Button - Silos"] = false,
        ["Mode Selection Button - Pipeworks"] = false,
        ["Mode Selection Button - Habitation"] = false,
        ["Mode Selection Button - Abyss"] = true,
        ["Mode Selection Button - Nest"] = false,
        ["Mode Selection Button - Challenge 01 - Advanced Course"] = false,
        ["Mode Selection Button - Challenge 02 - Shattered"] = false,
        ["Mode Selection Button - Challenge 03 - Roach Run"] = false,
        ["Mode Selection Button - Challenge 04 - Comms"] = false,
        ["Mode Selection Button - Challenge 05 - Shutter"] = false,
        ["Mode Selection Button - Challenge 06 - Boost"] = false,
        ["Mode Selection Button - Chimney"] = false,
        ["Mode Selection Button - Parasite.01"] = false,
    };

    public static Dictionary<long, string> APIDtoModeUnlock = new Dictionary<long, string>()
    {
        [0xAD00000] = "Mode Selection Button - Campaign Variant",
        [0xAD00001] = "Mode Selection Button - Training Sector",
        [0xAD00002] = "Mode Selection Button - Endless",
        [0xAD00003] = "Mode Selection Button - Endless Underworks",
        [0xAD00004] = "Mode Selection Button - Endless Superstructure",
        [0xAD00005] = "Mode Selection Button - Silos",
        [0xAD00006] = "Mode Selection Button - Pipeworks",
        [0xAD00007] = "Mode Selection Button - Habitation",
        [0xAD00008] = "Mode Selection Button - Abyss",
        [0xAD00009] = "Mode Selection Button - Nest",
        
        [0xAD0000A] = "Mode Selection Button - Challenge 01 - Advanced Course",
        [0xAD0000B] = "Mode Selection Button - Challenge 02 - Shattered",
        [0xAD0000C] = "Mode Selection Button - Challenge 03 - Roach Run",
        [0xAD0000E] = "Mode Selection Button - Challenge 04 - Comms",
        [0xAD0000F] = "Mode Selection Button - Challenge 05 - Shutter",
        [0xAD00010] = "Mode Selection Button - Challenge 06 - Boost",
        
        [0xAD00011] = "Mode Selection Button - Chimney",
        [0xAD00012] = "Mode Selection Button - Parasite.01"
    };
    
    public static Dictionary<string, bool> TrinketUnlocks = new Dictionary<string, bool>()
    {
        ["Trinket_Beta"] = false,
        ["Trinket_Carabiner"] = false,
        ["Trinket_Chalk"] = false,
        ["Trinket_EmployeeID"] = false,
        ["Trinket_GoldNugget"] = false,
        ["Trinket_MassDamper"] = false,
        ["Trinket_MoonRock"] = false,
        ["Trinket_PhotoOfHome"] = false,
        ["Trinket_Pouch"] = false,
        ["Trinket_BagExpander"] = false,
        ["Trinket_Headlamp"] = false,
        ["Trinket_ClimbingShoes"] = false,
        ["Trinket_Helmet"] = false,
        ["Trinket_CalmingBuddy"] = false,
    };

    public static Dictionary<long, string> APIDtoTrinketUnlock = new Dictionary<long, string>()
    {
        [0xAD00000] = "Trinket_Beta",
        [0xAD00001] = "Trinket_Carabiner",
        [0xAD00002] = "Trinket_Chalk",
        [0xAD00003] = "Trinket_EmployeeID",
        [0xAD00004] = "Trinket_GoldNugget",
        [0xAD00005] = "Trinket_MassDamper",
        [0xAD00006] = "Trinket_MoonRock",
        [0xAD00007] = "Trinket_PhotoOfHome",
        [0xAD00008] = "Trinket_Pouch",
        [0xAD00009] = "Trinket_BagExpander",
        [0xAD0000A] = "Trinket_Headlamp",
        [0xAD0000B] = "Trinket_ClimbingShoes",
        [0xAD0000C] = "Trinket_Helmet",
        [0xAD0000D] = "Trinket_CalmingBuddy"
    };



}