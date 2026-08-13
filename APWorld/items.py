from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import Item,ItemClassification

if TYPE_CHECKING:
    from .world import WKWorld



ITEM_NAME_TO_ID = {
    "Interlude Ascent: Bazaar Access": 0xAA10000,

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

DEFAULT_ITEM_CLASSIFICATIONS = {
    "Interlude Ascent: Bazaar Access": ItemClassification.useful,

    "I1: Recycler Upgrade": ItemClassification.progression,
    "I1: Sector Maintenance": ItemClassification.useful,
    "I1: Locker 1": ItemClassification.useful,
    "I1: Locker 2": ItemClassification.useful,
    "I1: Ration Vendor 1": ItemClassification.useful,
    "I1: Ration Vendor 2": ItemClassification.useful,
    "I1: ATM Install": ItemClassification.useful,
    "I1: Vendor Upgrade 1": ItemClassification.useful,
    "I1: Vendor Upgrade 2": ItemClassification.useful,

    "I2: Recycler Upgrade": ItemClassification.progression,
    "I2: Locker 1": ItemClassification.useful,
    "I2: Locker 2": ItemClassification.useful,
    "I2: Ration Vendor 1": ItemClassification.useful,
    "I2: Ration Vendor 2": ItemClassification.useful,
    "I2: Vendor Upgrade 1": ItemClassification.useful,
    "I2: Vendor Upgrade 2": ItemClassification.useful,

    "I3: Recycler Upgrade": ItemClassification.progression,
    "I3: Locker 1": ItemClassification.useful,
    "I3: Locker 2": ItemClassification.useful,
    "I3: ATM Install": ItemClassification.useful,
    "I3: Vendor Upgrade": ItemClassification.useful,
    "I3: Rho Altar": ItemClassification.progression,

    "I4: Recycler Upgrade": ItemClassification.progression,
    "I4: Locker 1": ItemClassification.useful,
    "I4: Locker 2": ItemClassification.useful,
    "I4: Ration Vendor 1": ItemClassification.useful,
    "I4: Ration Vendor 2": ItemClassification.useful,
    "I4: Wine Vendor": ItemClassification.useful,
    "I4: ATM Install": ItemClassification.useful,
    "I4: Vendor Upgrade": ItemClassification.useful,

    "Permanent Extra Roach": ItemClassification.filler,
    "10 Facility Credits": ItemClassification.filler
}

class WKItem(Item):
    game = "White Knuckle"

def get_random_filler_item_name(world: WKWorld) -> str:

    if world.random.randint(0,99) < 50:
        return "Permanent Extra Roach"
    return "10 Facility Credits"

def create_item_with_correct_classification(world: WKWorld, name: str) -> WKItem:

    classification = DEFAULT_ITEM_CLASSIFICATIONS[name]
    return WKItem(name, classification, ITEM_NAME_TO_ID[name], world.player)

def create_all_items(world: WKWorld) -> None:

    itempool: list[Item] = [

        world.create_item("Interlude Ascent: Bazaar Access"),

        world.create_item("I1 Upgrade: Recycler"),
        world.create_item("I1 Upgrade: Sector Maintenance"),
        world.create_item("I1 Upgrade: Locker 1"),
        world.create_item("I1 Upgrade: Locker 2"),
        world.create_item("I1 Upgrade: Ration Vendor 1"),
        world.create_item("I1 Upgrade: Ration Vendor 2"),
        world.create_item("I1 Upgrade: ATM Install"),
        world.create_item("I1 Upgrade: Vendor Upgrade 1"),
        world.create_item("I1 Upgrade: Vendor Upgrade 2"),

        world.create_item("I2 Upgrade: Recycler"),
        world.create_item("I2 Upgrade: Locker 1"),
        world.create_item("I2 Upgrade: Locker 2"),
        world.create_item("I2 Upgrade: Ration Vendor 1"),
        world.create_item("I2 Upgrade: Ration Vendor 2"),
        world.create_item("I2 Upgrade: Vendor Upgrade 1"),
        world.create_item("I2 Upgrade: Vendor Upgrade 2"),

        world.create_item("I3 Upgrade: Recycler"),
        world.create_item("I3 Upgrade: Locker 1"),
        world.create_item("I3 Upgrade: Locker 2"),
        world.create_item("I3 Upgrade: ATM Install"),
        world.create_item("I3 Upgrade: Vendor Upgrade"),
        world.create_item("I3 Upgrade: Rho Altar"),

        world.create_item("I4 Upgrade: Recycler"),
        world.create_item("I4 Upgrade: Locker 1"),
        world.create_item("I4 Upgrade: Locker 2"),
        world.create_item("I4 Upgrade: Ration Vendor 1"),
        world.create_item("I4 Upgrade: Ration Vendor 2"),
        world.create_item("I4 Upgrade: Wine vendor"),
        world.create_item("I4 Upgrade: ATM Install"),
        world.create_item("I4 Upgrade: Vendor Upgrade"),
    ]

    number_of_items = len(itempool)
    number_of_unfilled_locations = len(world.multiworld.get_unfilled_locations(world.player))
    needed_number_of_filler_items = number_of_unfilled_locations - number_of_items

    itempool += [world.create_filler() for _ in range(needed_number_of_filler_items)]

    world.multiworld.itempool += itempool