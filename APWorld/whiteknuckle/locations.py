from __future__ import annotations
from typing import TYPE_CHECKING

from BaseClasses import ItemClassification, Location

if TYPE_CHECKING:
    from .world import WKWorld

LOCATION_NAME_TO_ID = {
    "Global: Bazaar Access": 0xAA10000,
    "Global: Starting Roaches T1": 0xAA10001,
    "Global: Starting Roaches T2": 0xAA10002,
    "Global: Starting Roaches T3": 0xAA10003,
    "Global: Calming Buddy Purchase": 0xAA10004,
    "Global: Pouch Purchase": 0xAA10005,
    "Global: Ornamental Hammer Purchase": 0xAA10006,
    "Global: Work Gloves Purchase": 0xAA10007,
    "Global: New Gloves Purchase": 0xAA10008,
    "Global: Biomod Oversupply": 0xAA10009,

    "I1: Recycler Upgrade": 0xAA11000,
    "I1: Sector Maintenance": 0xAA11001,
    "I1: Locker 1": 0xAA11002,
    "I1: Locker 2": 0xAA11003,
    "I1: Ration Vendor 1": 0xAA11004,
    "I1: Ration Vendor 2": 0xAA11005,
    "I1: ATM Install": 0xAA11006,
    "I1: Vendor Upgrade 1": 0xAA11007,
    "I1: Vendor Upgrade 2": 0xAA11008,

    "I2: Recycler Upgrade": 0xAA12000,
    "I2: Locker 1": 0xAA12001,
    "I2: Locker 2": 0xAA12002,
    "I2: Ration Vendor 1": 0xAA12003,
    "I2: Ration Vendor 2": 0xAA12004,
    "I2: Vendor Upgrade 1": 0xAA12005,
    "I2: Vendor Upgrade 2": 0xAA12006,

    "I3: Recycler Upgrade": 0xAA13000,
    "I3: Locker 1": 0xAA13001,
    "I3: Locker 2": 0xAA13002,
    "I3: ATM Install": 0xAA13003,
    "I3: Vendor Upgrade": 0xAA13004,
    "I3: Rho Altar": 0xAA13005,

    "I4: Recycler Upgrade": 0xAA14000,
    "I4: Locker 1": 0xAA14001,
    "I4: Locker 2": 0xAA14002,
    "I4: Ration Vendor 1": 0xAA14003,
    "I4: Ration Vendor 2": 0xAA14004,
    "I4: Wine Vendor": 0xAA14005,
    "I4: ATM Install": 0xAA14006,
    "I4: Vendor Upgrade": 0xAA14007,
    
    #Room Checks
    "Silos: Deep Storage 01": 0xAB10000,
    "Silos: Deep Storage 03": 0xAB10001,
    "Silos: Deep Storage 06": 0xAB10002,
    "Silos: Deep Storage 09": 0xAB10003,
    "Silos: Deep Storage 10": 0xAB10004,
    "Silos: Deep Storage 15": 0xAB10005,
    "Silos: Air Exchange 02": 0xAB10006,
    "Silos: Air Exchange 03": 0xAB10007,
    "Silos: Air Exchange 04": 0xAB10008,
    "Silos: Air Exchange 08": 0xAB10009,
    "Silos: Shattered Chambers 01": 0xAB1000A,
    "Silos: Shattered Chambers 04": 0xAB1000B,
    "Silos: Shattered Chambers 08": 0xAB1000C,
    "Silos: Shattered Chambers 09": 0xAB1000D,
    "Silos: Shattered Chambers 10": 0xAB1000E,
    #yes these have wrong formatting for their IDs TODO: Fix the formatting both here and in the APItems.cs file
    #Deep Storage 1
    "Silos: Deep Storage 04": 0xAB10100,
    "Silos: Deep Storage 11": 0xAB10101,
    "Silos: Deep Storage 12": 0xAB10102,
    #Silos 1
    "Silos: Deep Storage 02": 0xAB10200,
    "Silos: Deep Storage 08": 0xAB10201,
    "Silos: Deep Storage 16": 0xAB10202,
    "Silos: Air Exchange 05": 0xAB10203,
    "Silos: Air Exchange 06": 0xAB10204,
    "Silos: Shattered Chambers 03": 0xAB10205,
    "Silos: Shattered Chambers 05": 0xAB10206,
    #Silos 2
    "Silos: Deep Storage 05": 0xAB10300,
    "Silos: Deep Storage 07": 0xAB10301,
    "Silos: Air Exchange 01": 0xAB10302,
    "Silos: Air Exchange 09": 0xAB10303,
    "Silos: Shattered Chambers 02": 0xAB10304,
    "Silos: Shattered Chambers 06": 0xAB10305,
    #Silos 3
    "Silos: Deep Storage 13": 0xAB10400,
    "Silos: Deep Storage 14": 0xAB10401,
    "Silos: Air Exchange 07": 0xAB10402,
    "Silos: Air Exchange 10": 0xAB10403,
    "Silos: Shattered Chambers 07": 0xAB10404,
    #Silos 4
    "Silos: Deep Storage 17": 0xAB10500,
    "Silos: Air Exchange 11": 0xAB10501,
    "Silos: Shattered Chambers 11": 0xAB10502,

    #Pipeworks
    "Pipeworks: Drainage 01": 0xAB20000,
    "Pipeworks: Drainage 02": 0xAB20001,
    "Pipeworks: Drainage 05": 0xAB20002,
    "Pipeworks: Drainage 07": 0xAB20003,
    "Pipeworks: Drainage 08": 0xAB20004,
    "Pipeworks: Drainage 09": 0xAB20005,
    "Pipeworks: Drainage 10": 0xAB20006,
    "Pipeworks: Waste Heap 02": 0xAB20100,
    "Pipeworks: Waste Heap 04": 0xAB20101,
    "Pipeworks: Waste Heap 05": 0xAB20102,
    "Pipeworks: Pipe Organ 02": 0xAB20200,
    "Pipeworks: Pipe Organ 05": 0xAB20201,
    "Pipeworks: Pipe Organ 06": 0xAB20202,
    "Pipeworks: Pipe Organ 07": 0xAB20203,
    #Pipeworks 1
    "Pipeworks: Drainage 03": 0xAB21000,
    "Pipeworks: Drainage 04": 0xAB21001,
    "Pipeworks: Waste Heap 01": 0xAB21100,
    "Pipeworks: Pipe Organ 01": 0xAB21200,
    "Pipeworks: Pipe Organ 03": 0xAB21201,
    #Pipeworks 2
    "Pipeworks: Drainage 06": 0xAB22000,
    "Pipeworks: Waste Heap 03": 0xAB22100,
    "Pipeworks: Pipe Organ 04": 0xAB22200,
    "Pipeworks: Pipe Organ 08": 0xAB22201,
    #Pipeworks 3
    "Pipeworks: Pipe Organ 09": 0xAB23200,

    #Habitation
    "Habitation: Service Shaft 01": 0xAB30000,
    "Habitation: Service Shaft 02": 0xAB30001,
    "Habitation: Service Shaft 03": 0xAB30002,
    "Habitation: Service Shaft 04": 0xAB30003,
    "Habitation: Service Shaft 05": 0xAB30004,
    "Habitation: Service Shaft 06": 0xAB30005,
    "Habitation: Service Shaft Exit": 0xAB30006,
    "Habitation: Haunted Pier 01": 0xAB30100,
    "Habitation: Haunted Pier 02": 0xAB30101,
    "Habitation: Delta Labs Lobby": 0xAB30200,
    "Habitation: Delta Labs 01": 0xAB30201,
    "Habitation: Delta Labs 02": 0xAB30202,
    "Habitation: Delta Labs 03": 0xAB30203,
    "Habitation: Delta Labs 04": 0xAB30204,
    "Habitation: Delta Labs Exit": 0xAB30205,
    #Habitation 1
    "Habitation: Service Shaft 07": 0xAB31000,
    "Habitation: Haunted Pier 03": 0xAB31100,
    "Habitation: Haunted Pier 04": 0xAB31101,
    "Habitation: Delta Labs 05": 0xAB31200,
    "Habitation: Delta Labs 06": 0xAB31201,
    "Habitation: Delta Labs 07": 0xAB31202,
    "Habitation: Delta Labs 08": 0xAB31203,


    "Abyss: Entrance": 0xAB40000,
    "Abyss: Exit": 0xAB40001,
    "Abyss: Transit 01": 0xAB40101,
    "Abyss: Transit 02": 0xAB40102,
    "Abyss: Transit 03": 0xAB40103,
    "Abyss: Transit 04": 0xAB40104,
    "Abyss: Transit 05": 0xAB40105,
    "Abyss: Transit 06": 0xAB40106,
    "Abyss: Handle 01": 0xAB40200,
    "Abyss: Hanging Gardens 01": 0xAB40300,
    "Abyss: Hanging Gardens 02": 0xAB40301,
    "Abyss: Hanging Gardens 03": 0xAB40302,
    "Abyss: Hanging Gardens 04": 0xAB40303,
    #The single Abyss unlock
    "Abyss: Handle 02": 0xAB40400,

    "Nest: Infested Labs 01": 0xAB50000,
    "Nest: Infested Labs 02": 0xAB50001,
    "Nest: Infested Labs 03": 0xAB50002,
    "Nest: Infested Labs 04": 0xAB50003,
    "Nest: Infested Labs 05": 0xAB50004,
    "Nest: Infested Labs Hunter Intro": 0xAB50005,
    "Nest: Infested Labs Chase 01": 0xAB50006,
    "Nest: Infested Labs Chase 02": 0xAB50007,
    "Nest: Feeding Trough 01": 0xAB50100,
    "Nest: Feeding Trough 02": 0xAB50101,
    "Nest: Feeding Trough 03": 0xAB50102,
    "Nest: Feeding Trough 04": 0xAB50103,
    "Nest: Feeding Trough 05": 0xAB50104,
    "Nest: Feeding Trough 06": 0xAB50105,
    "Nest: Feeding Trough 07": 0xAB50106,
    "Nest: Feeding Trough 08": 0xAB50107,
    "Nest: Hot Zone Intro": 0xAB50200,
    "Nest: Hot Zone Outro": 0xAB50201,
    "Nest: Hot Zone 01": 0xAB50202,
    "Nest: Hot Zone 02": 0xAB50203,
    "Nest: Hot Zone 03": 0xAB50204,
    "Nest: Hot Zone 04": 0xAB50205,

    "Tangled Sink Intro": 0xAB60000,
    "Tangled Sink 01": 0xAB60001,
    "Tangled Sink 02": 0xAB60002,
    "Tangled Sink 03": 0xAB60003,
    "Tangled Sink 04": 0xAB60004,

    "Expulsion Chute Start": 0xAB70000,
    "Expulsion Chute 01": 0xAB70001,
    "Expulsion Chute 02": 0xAB70002,
    "Expulsion Chute 03": 0xAB70003,
    "Expulsion Chute 04": 0xAB70004,
    "Expulsion Chute 05": 0xAB70005,
    "Expulsion Chute 06": 0xAB70006,
    "Expulsion Chute End": 0xAB70007,

    "Training Sector: Foundations 01": 0xAB80000,
    "Training Sector: Foundations 02": 0xAB80001,
    "Training Sector: Foundations 03": 0xAB80002,
    "Training Sector: Foundations 04": 0xAB80003,
    "Training Sector: Foundations 05": 0xAB80004,
    "Training Sector: Basic Climbing 01": 0xAB80005,
    "Training Sector: Basic Climbing 02": 0xAB80006,
    "Training Sector: Basic Climbing 03": 0xAB80007,
    "Training Sector: Basic Climbing 04": 0xAB80008,
    "Training Sector: Basic Climbing 05": 0xAB80009,
    "Training Sector: Basic Climbing 06": 0xAB8000A,
    "Training Sector: Sector Mastery 01": 0xAB8000B,
    "Training Sector: Sector Mastery 02": 0xAB8000C,

}

class WKLocation(Location):
    game = "White Knuckle"

def get_location_names_with_ids(location_names: list[str]) -> dict[str, int | None]:
    return {location_name: LOCATION_NAME_TO_ID[location_name] for location_name in location_names}

def get_ids_with_location_names(location_ids: list[int]) -> dict[str, int | None]:
    keys = [k for k, v in LOCATION_NAME_TO_ID.items() if v in location_ids]
    return {k: v for k, v in zip(keys, location_ids)}

def create_all_locations(world: WKWorld) -> None:
    create_regular_locations(world)
    create_events(world)

def create_regular_locations(world: WKWorld) -> None:

    global_shop = world.get_region("Global Shop")

    sink = world.get_region("Sink")
    chute = world.get_region("Chute")

    deep_storage = world.get_region("Silos 1")
    shattered_chambers = world.get_region("Silos 2")
    air_exchange = world.get_region("Silos 3")

    drainage_system = world.get_region("Pipeworks 1")
    waste_heap = world.get_region("Pipeworks 2")
    pipe_organ = world.get_region("Pipeworks 3")

    shaft = world.get_region("Habitation 1")
    pier = world.get_region("Habitation 2")
    delta_labs = world.get_region("Habitation 3")

    transit = world.get_region("Abyss 1")
    handle = world.get_region("Abyss 2")
    gardens = world.get_region("Abyss 3")

    lambda_labs = world.get_region("Nest 1")
    hot_zone = world.get_region("Nest 2")
    feeding_trough = world.get_region("Nest 3")

    interlude_1 = world.get_region("Interlude 1")
    interlude_2 = world.get_region("Interlude 2")
    interlude_3 = world.get_region("Interlude 3")
    interlude_4 = world.get_region("Interlude 4")

    global_shop.add_locations(
        get_location_names_with_ids(
            ["Global: Bazaar Access",
             "Global: Starting Roaches T1",
             "Global: Starting Roaches T2",
             "Global: Starting Roaches T3",
             "Global: Calming Buddy Purchase",
             "Global: Pouch Purchase",
             "Global: Ornamental Hammer Purchase",
             "Global: Work Gloves Purchase",
             "Global: New Gloves Purchase",
             "Global: Biomod Oversupply"]
        ), WKLocation
    )

    interlude_1_locations = get_location_names_with_ids(
        ["I1: Recycler Upgrade",
         "I1: Sector Maintenance",
         "I1: Locker 1",
         "I1: Locker 2",
         "I1: Ration Vendor 1",
         "I1: Ration Vendor 2",
         "I1: ATM Install",
         "I1: Vendor Upgrade 1",
         "I1: Vendor Upgrade 2"]
    )

    interlude_1.add_locations(interlude_1_locations, WKLocation)

    interlude_2.add_locations(
        get_location_names_with_ids(
            ["I2: Recycler Upgrade",
             "I2: Locker 1",
             "I2: Locker 2",
             "I2: Ration Vendor 1",
             "I2: Ration Vendor 2",
             "I2: Vendor Upgrade 1",
             "I2: Vendor Upgrade 2"
             ]), WKLocation
    )

    interlude_3.add_locations(
        get_location_names_with_ids(
            ["I3: Recycler Upgrade",
             "I3: Locker 1",
             "I3: Locker 2",
             "I3: ATM Install",
             "I3: Vendor Upgrade",
             "I3: Rho Altar"]
        ), WKLocation
    )

    interlude_4.add_locations(
        get_location_names_with_ids(
            ["I4: Recycler Upgrade",
             "I4: Locker 1",
             "I4: Locker 2",
             "I4: Ration Vendor 1",
             "I4: Ration Vendor 2",
             "I4: Wine Vendor",
             "I4: ATM Install",
             "I4: Vendor Upgrade"]
        ), WKLocation
    )

    sink.add_locations(get_ids_with_location_names(list(range(0xAB60000, 0xAB60005))), WKLocation)
    chute.add_locations(get_ids_with_location_names(list(range(0xAB70000, 0xAB70008))), WKLocation)

    deep_storage.add_locations(get_location_names_with_ids([f"Silos: Deep Storage {i:02d}" for i in range(1,18)]), WKLocation)
    shattered_chambers.add_locations(get_location_names_with_ids([f"Silos: Shattered Chambers {i:02d}" for i in range(1,12)]), WKLocation)
    air_exchange.add_locations(get_location_names_with_ids([f"Silos: Air Exchange {i:02d}" for i in range(1,12)]), WKLocation)

    drainage_system.add_locations(get_location_names_with_ids([f"Pipeworks: Drainage {i:02d}" for i in range(1,11)]), WKLocation)
    waste_heap.add_locations(get_location_names_with_ids([f"Pipeworks: Waste Heap {i:02d}" for i in range(1,6)]), WKLocation)
    pipe_organ.add_locations(get_location_names_with_ids([f"Pipeworks: Pipe Organ {i:02d}" for i in range(1,10)]), WKLocation)

    s = [f"Habitation: Service Shaft {i:02d}" for i in range(1,8)]
    s.append("Habitation: Service Shaft Exit")
    shaft.add_locations(get_location_names_with_ids(s), WKLocation)
    pier.add_locations(get_location_names_with_ids([f"Habitation: Haunted Pier {i:02d}" for i in range(1,5)]), WKLocation)
    s = [f"Habitation: Delta Labs {i:02d}" for i in range(1,9)]
    s.extend(["Habitation: Delta Labs Lobby", "Habitation: Delta Labs Exit"])
    delta_labs.add_locations(get_location_names_with_ids(s), WKLocation)

    s = list(range(0xAB40101, 0xAB40107))
    s.append(0xAB40000)
    transit.add_locations(get_ids_with_location_names(s), WKLocation)
    handle.add_locations(get_ids_with_location_names([0xAB40200,0xAB40400]), WKLocation)
    gardens.add_locations(get_ids_with_location_names([0xAB40300,0xAB40301,0xAB40302,0xAB40303,0xAB40001]), WKLocation)

    lambda_labs.add_locations(get_ids_with_location_names(list(range(0xAB50000,0xAB50008))), WKLocation)
    feeding_trough.add_locations(get_ids_with_location_names(list(range(0xAB50100,0xAB50108))), WKLocation)
    hot_zone.add_locations(get_ids_with_location_names(list(range(0xAB50200,0xAB50206))), WKLocation)



def create_events(world : WKWorld) -> None:
    return

