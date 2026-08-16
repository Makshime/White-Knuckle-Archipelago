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

    public static void UpdateFromId(long id)
    {


        if (0xAAFFFFF >= id & id >= 0xAA11000 || id == 0xAA10000 || id == 0xAA10009)
        {

            var vals = APtoFullFacilityUpgrade[id].Split(" ");
            FacilityDict[vals[0]][vals[1]] = true;
            Facility.onPurchaseUpgrade();
        }
        else if (0xABFFFFF >= id & id  >= 0xAB00000)
        {
            
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

    public static List<long> CheckFacilities()
    {
        return StatManager.saveData.facilities.SelectMany(facility => facility.upgrades, (facility, upgrade) => FullFacilityUpgradetoAP[$"{facility} {upgrade}"]).Where(id => !SentItems.Contains((long)id)).ToList();
    }

    public static long? CheckRoom()
    {
        string room = WorldLoader.instance.GetCurrentLevel().GetLevel().levelName;
        if (RoomNameToAP.ContainsKey(room) & !SentItems.Contains(RoomNameToAP[room]))
        {
            return RoomNameToAP[room];
        }

        return null;
    }

    public static List<long> SentItems = new List<long>();
    
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
        [0xAA11008] = "CAMPAIGN_INTERLUDE_01 UPG_Vendor_T1_02",
        
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
        
        
        //Pipeworks Tier 1 Unlocks
        
        
        //Pipeworks Tier 2 Unlocks
        
        
        //Pipeworks Tier 3 Unlocks
        
        
        //Habitation Rooms
        
        //Habitation Tier 1 Unlocks
        
        
        //Abyss Rooms
        
        //Abyss Tier 1 Unlocks
        
        
        //Nest Rooms
        
        
        
        //Tangled Sink Rooms
        
        //Expulsion Chute Rooms
        
        //Training Sector Rooms
        
    };

    public static Dictionary<string, bool> ProgressionUnlocks = new Dictionary<string, bool>()
    {
        ["binding_abyss"] = false,
        ["binding_core"] = false,
        ["binding_habitation"] = false,
        ["binding_nest"] = false,
        ["binding_roach"] = false,
            
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
        
        ["r_hardmode"] = false,
        ["r_10kendless"] = false,
        ["r_competitive"] = false,
        
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
        ["r_shortcut_expulsionchute"] = false,
        ["r_shortcut_tangledsink"] = false,
        ["r_silos_t1"] = false,
        ["r_silos_t2"] = false,
        ["r_silos_t3"] = false,
        ["r_silos_t4"] = false,
        
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
        
        //Challenge course unlocks
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
        
        //For now trinkets are unlocked in groups
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
        ["Mode Selection Button - Abyss"] = false,
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

}