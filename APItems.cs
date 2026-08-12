using System.Collections.Generic;

namespace WKRando;

public class APItems
{
    
    
    // Stores flags for facility data to override ingame 
    public static Dictionary<string, Dictionary<string, bool>> _facilityDict { get; set; } =
        new Dictionary<string, Dictionary<string, bool>>()
    {
        ["GLOBAL"] = new() 
        {
            //All other global upgrades are either overriden or inconsequential
            ["UPG_Global_Bazaar_I2"] = false,
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
    
    
    //Handles AP IDs of the 
    public static Dictionary<long, string> APtoFullFacilityUpgrade = new Dictionary<long, string>()
    {
        [0xAA10000] = "GLOBAL UPG_Global_Bazaar_I2",
        
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
    
    public static Dictionary<string, long> fullFacilityUpgradetoAP = new Dictionary<string, long>()
    {
        ["GLOBAL UPG_Global_Bazaar_I2"] = 0xAA10000,

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

}