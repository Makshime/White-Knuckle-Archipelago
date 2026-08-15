using System.Collections.Generic;
using System.Linq;

namespace WKRando;

public class APItems
{
    
    
    // Stores flags for facility data to override ingame 
    public static Dictionary<string, Dictionary<string, bool>> FacilityDict { get; set; } =
        new Dictionary<string, Dictionary<string, bool>>()
    {
        ["GLOBAL"] = new() 
        {
            //All other global upgrades are either overridden or inconsequential
            ["UPG_Global_Bazaar_I2"] = false,
            ["UPG_Global_PerkRefresh"] = false
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

    public static void UpdateFromId(long id)
    {


        if (0xAAFFFFF >= id & id >= 0xAA11000 || id == 0xAA10000)
        {

            var vals = APtoFullFacilityUpgrade[id].Split(" ");
            FacilityDict[vals[0]][vals[1]] = true;
            Facility.onPurchaseUpgrade();
        }
        else if (0xA900001 == id)
        {
            Plugin.LoanAmount += 1;
            CL_GameManager.runRoaches += 1;
        }
        else if (0xA900002 == id)
        {
            CL_GameManager.globalRoaches += 10;
        }

    }
    
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
        [0xAA11007] = "UPG_Vendor_T1_01",
        [0xAA11008] = "UPG_Vendor_T1_02",
        
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
        
        [0xAA14000] = "UPG_Recycler",
        [0xAA14001] = "UPG_ItemLocker_01",
        [0xAA14002] = "UPG_ItemLocker_02",
        [0xAA14003] = "UPG_Ration_T1",
        [0xAA14004] = "UPG_Ration_T2",
        [0xAA14005] = "UPG_Wine_T1",
        [0xAA14006] = "UPG_ATM",
        [0xAA14007] = "UPG_Vendor_T2_01"
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
        ["UPG_Vendor_T1_01"] = 0xAA11007,
        ["UPG_Vendor_T1_02"] = 0xAA11008,
        
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

        ["UPG_Recycler"] = 0xAA14000,
        ["UPG_ItemLocker_01"] = 0xAA14001,
        ["UPG_ItemLocker_02"] = 0xAA14002,
        ["UPG_Ration_T1"] = 0xAA14003,
        ["UPG_Ration_T2"] = 0xAA14004,
        ["UPG_Wine_T1"] = 0xAA14005,
        ["UPG_ATM"] = 0xAA14006,
        ["UPG_Vendor_T2_01"] = 0xAA14007
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
        
    };

}