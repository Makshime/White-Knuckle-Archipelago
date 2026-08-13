from __future__ import annotations
from typing import TYPE_CHECKING

from BaseClasses import ItemClassification, Location
from . import items

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

    "Permanent Extra Roach": 0xA90001,
    "10 Facility Credits": 0xA90002

}

class WKLocation(Location):
    game = "White Knuckle"

def get_location_names_with_ids(location_names: list[str]) -> dict[str, int | None]:
    return {location_name: LOCATION_NAME_TO_ID[location_name] for location_name in location_names}

def create_all_locations(world: WKWorld) -> None:
    create_regular_locations(world)
    create_events(world)

def create_regular_locations(world: WKWorld) -> None:

    global_shop = world.get_region("Global Shop")

    silos_1 = world.get_region("Silos 1")

    pipeworks_1 = world.get_region("Pipeworks 1")

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
             "Global: New Gloves Purchase"]
        )
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
        )
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
        )
    )

def create_events(world : WKWorld) -> None:

    core = world.get_region("Core")
    core.add_event(
        "Reached_Win", "Victory", location_type=WKLocation, item_type=items.WKItem
    )

