from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import Item,ItemClassification

if TYPE_CHECKING:
    from .world import WKWorld



ITEM_NAME_TO_ID = {
    "Interlude Ascent: Bazaar Access": 0xAA10000,
    "Global: Perk Reroll": 0xAA10009,

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

    "Permanent Extra Roach": 0xA900001,
    "10 Facility Credits": 0xA900002,

    "Progressive Region": 0xA900010,
    "Progressive Buff": 0xA900020,
    "Progressive Perk Machine": 0xA900030,

    "Perks: Grub": 0xAC00100,
    "Perks: Tier 1": 0xAC00101,
    "Perks: Tier 2": 0xAC00102,
    "Perks: Tier 3": 0xAC00103,
    "Perks: Tier 4": 0xAC00104,
    "Perks: Tier 5": 0xAC00105,
    "Unstable Perk: Adoption Day": 0xAC00106,
    "Unstable Perks: Delta Perks": 0xAC00107,
    "Unstable Perks: Tier 1": 0xAC00108,
    "Unstable Perks: Tier 2": 0xAC00109,
    "Unstable Perks: Tier 3": 0xAC0010A,


    "Abyss Handle 02 Access": 0xAC00200,
    "Habitation Room Unlocks": 0xAC00201,
    "Pipeworks Room Unlocks: 1": 0xAC00202,
    "Pipeworks Room Unlocks: 2": 0xAC00203,
    "Pipeworks Rho Room Access": 0xAC00204,

    "Expulsion Chute Access": 0xAC00205,
    "Tangled Sink Access": 0xAC00206,

    "Silos Room Unlocks: 1": 0xAC00207,
    "Silos Room Unlocks: 2": 0xAC00208,
    "Silos Room Unlocks: 3": 0xAC00209,
    "Silos Room Unlocks: 4": 0xAC0020A,
    "Deep Storage Room Unlocks": 0xAC0020B,

    "Trinket: The Beta": 0xAD00000,
    "Trinket: Carabiner": 0xAD00001,
    "Trinket: Chalk": 0xAD00002,
    "Trinket: Employee ID": 0xAD00003,
    "Trinket: Gold Nugget": 0xAD00004,
    "Trinket: Mass Damper": 0xAD00005,
    "Trinket: Vial of Moon Rocks": 0xAD00006,
    "Trinket: Photo": 0xAD00007,
    "Trinket: Pouch": 0xAD00008,
    "Trinket: Larger Bag": 0xAD00009,
    "Trinket: Headlamp": 0xAD0000A,
    "Trinket: Climbing Shoes": 0xAD0000B,
    "Trinket: Helmet": 0xAD0000C,
    "Trinket: Calming Buddy": 0xAD0000D,


}

DEFAULT_ITEM_CLASSIFICATIONS = {
    "Interlude Ascent: Bazaar Access": ItemClassification.useful,
    "Global: Biomod Oversupply": ItemClassification.progression,

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

    "I4: Recycler Upgrade": ItemClassification.useful,
    "I4: Locker 1": ItemClassification.useful,
    "I4: Locker 2": ItemClassification.useful,
    "I4: Ration Vendor 1": ItemClassification.useful,
    "I4: Ration Vendor 2": ItemClassification.useful,
    "I4: Wine Vendor": ItemClassification.useful,
    "I4: ATM Install": ItemClassification.useful,
    "I4: Vendor Upgrade": ItemClassification.useful,

    "Permanent Extra Roach": ItemClassification.filler,
    "10 Facility Credits": ItemClassification.filler,

    "Progressive Region": ItemClassification.progression,
    "Progressive Buff": ItemClassification.progression,
    "Progressive Perk Machine": ItemClassification.progression,

    "Perks: Grub": ItemClassification.useful,
    "Perks: Tier 1": ItemClassification.useful,
    "Perks: Tier 2": ItemClassification.useful,
    "Perks: Tier 3": ItemClassification.useful,
    "Perks: Tier 4": ItemClassification.useful,
    "Perks: Tier 5": ItemClassification.useful,
    "Unstable Perk: Adoption Day": ItemClassification.useful,
    "Unstable Perks: Delta Perks": ItemClassification.useful,
    "Unstable Perks: Tier 1": ItemClassification.useful,
    "Unstable Perks: Tier 2": ItemClassification.useful,
    "Unstable Perks: Tier 3": ItemClassification.useful,


    "Abyss Handle 02 Access": ItemClassification.progression,
    "Habitation Room Unlocks": ItemClassification.progression,
    "Pipeworks Room Unlocks: 1": ItemClassification.progression,
    "Pipeworks Room Unlocks: 2": ItemClassification.progression,
    "Pipeworks Rho Room Access": ItemClassification.progression,

    "Expulsion Chute Access": ItemClassification.progression,
    "Tangled Sink Access": ItemClassification.progression,

    "Silos Room Unlocks: 1": ItemClassification.progression,
    "Silos Room Unlocks: 2": ItemClassification.progression,
    "Silos Room Unlocks: 3": ItemClassification.progression,
    "Silos Room Unlocks: 4": ItemClassification.progression,
    "Deep Storage Room Unlocks": ItemClassification.progression,

    "Trinket: The Beta": ItemClassification.useful,
    "Trinket: Carabiner": ItemClassification.useful,
    "Trinket: Chalk": ItemClassification.useful,
    "Trinket: Employee ID": ItemClassification.useful,
    "Trinket: Gold Nugget": ItemClassification.useful,
    "Trinket: Mass Damper": ItemClassification.useful,
    "Trinket: Vial of Moon Rocks": ItemClassification.useful,
    "Trinket: Photo": ItemClassification.useful,
    "Trinket: Pouch": ItemClassification.useful,
    "Trinket: Larger Bag": ItemClassification.useful,
    "Trinket: Headlamp": ItemClassification.useful,
    "Trinket: Climbing Shoes": ItemClassification.useful,
    "Trinket: Helmet": ItemClassification.useful,
    "Trinket: Calming Buddy": ItemClassification.useful,
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

    itempool: list[Item] = []

    itempool += create_interlude_items(world)

    itempool += create_perk_unlocks(world)
    itempool += create_room_unlocks(world)
    itempool += create_trinket_unlocks(world)

    itempool += create_buff_perks(world)
    itempool += create_perk_terminal_unlocks(world)
    itempool += create_progressive_access(world)

    number_of_items = len(itempool)
    number_of_unfilled_locations = len(world.multiworld.get_unfilled_locations(world.player))
    needed_number_of_filler_items = number_of_unfilled_locations - number_of_items

    itempool += [world.create_filler() for _ in range(needed_number_of_filler_items)]

    world.multiworld.itempool += itempool


def create_interlude_items(world:WKWorld) -> list[Item]:
    interludes: list[Item] = []
    interludes += create_global_upgrades(world)
    interludes += create_i1_upgrades(world)
    interludes += create_i2_upgrades(world)
    interludes += create_i3_upgrades(world)
    interludes += create_i4_upgrades(world)
    return interludes

def create_global_upgrades(world:WKWorld) -> list[Item]:
    return [
        world.create_item("Interlude Ascent: Bazaar Access")
    ]

def create_i1_upgrades(world:WKWorld) -> list[Item]:
    return [
        world.create_item("I1: Recycler Upgrade"),
        world.create_item("I1: Sector Maintenance"),
        world.create_item("I1: Locker 1"),
        world.create_item("I1: Locker 2"),
        world.create_item("I1: Ration Vendor 1"),
        world.create_item("I1: Ration Vendor 2"),
        world.create_item("I1: ATM Install"),
        world.create_item("I1: Vendor Upgrade 1"),
        world.create_item("I1: Vendor Upgrade 2")
    ]

def create_i2_upgrades(world:WKWorld) -> list[Item]:
    return [
        world.create_item("I2: Recycler Upgrade"),
        world.create_item("I2: Locker 1"),
        world.create_item("I2: Locker 2"),
        world.create_item("I2: Ration Vendor 1"),
        world.create_item("I2: Ration Vendor 2"),
        world.create_item("I2: Vendor Upgrade 1"),
        world.create_item("I2: Vendor Upgrade 2")
    ]

def create_i3_upgrades(world:WKWorld) -> list[Item]:
    return [
        world.create_item("I3: Recycler Upgrade"),
        world.create_item("I3: Locker 1"),
        world.create_item("I3: Locker 2"),
        world.create_item("I3: ATM Install"),
        world.create_item("I3: Vendor Upgrade"),
        world.create_item("I3: Rho Altar")
    ]

def create_i4_upgrades(world:WKWorld) -> list[Item]:
    return [
        world.create_item("I4: Recycler Upgrade"),
        world.create_item("I4: Locker 1"),
        world.create_item("I4: Locker 2"),
        world.create_item("I4: Ration Vendor 1"),
        world.create_item("I4: Ration Vendor 2"),
        world.create_item("I4: Wine Vendor"),
        world.create_item("I4: ATM Install"),
        world.create_item("I4: Vendor Upgrade")
    ]

def create_perk_unlocks(world:WKWorld) -> list[Item]:
    return [
        world.create_item("Perks: Grub"),
        world.create_item("Perks: Tier 1"),
        world.create_item("Perks: Tier 2"),
        world.create_item("Perks: Tier 3"),
        world.create_item("Perks: Tier 4"),
        world.create_item("Perks: Tier 5"),

        world.create_item("Unstable Perk: Adoption Day"),
        world.create_item("Unstable Perks: Delta Perks"),
        world.create_item("Unstable Perks: Tier 1"),
        world.create_item("Unstable Perks: Tier 2"),
        world.create_item("Unstable Perks: Tier 3"),
    ]

def create_room_unlocks(world:WKWorld) -> list[Item]:
    return [
        world.create_item("Abyss Handle 02 Access"),
        world.create_item("Habitation Room Unlocks"),
        world.create_item("Pipeworks Room Unlocks: 1"),
        world.create_item("Pipeworks Room Unlocks: 2"),
        world.create_item("Pipeworks Rho Room Access"),

        world.create_item("Expulsion Chute Access"),
        world.create_item("Tangled Sink Access"),

        world.create_item("Silos Room Unlocks: 1"),
        world.create_item("Silos Room Unlocks: 2"),
        world.create_item("Silos Room Unlocks: 3"),
        world.create_item("Silos Room Unlocks: 4"),
        world.create_item("Deep Storage Room Unlocks"),
    ]

def create_trinket_unlocks(world:WKWorld) -> list[Item]:
    return [
        world.create_item("Trinket: The Beta"),
        world.create_item("Trinket: Carabiner"),
        world.create_item("Trinket: Chalk"),
        world.create_item("Trinket: Employee ID"),
        world.create_item("Trinket: Gold Nugget"),
        world.create_item("Trinket: Mass Damper"),
        world.create_item("Trinket: Vial of Moon Rocks"),
        world.create_item("Trinket: Photo"),
        world.create_item("Trinket: Pouch"),
        world.create_item("Trinket: Larger Bag"),
        world.create_item("Trinket: Headlamp"),
        world.create_item("Trinket: Climbing Shoes"),
        world.create_item("Trinket: Helmet"),
        world.create_item("Trinket: Calming Buddy"),
    ]

def create_buff_perks(world:WKWorld) -> list[Item]:
    out = []
    for i in range(15):
        out.append(world.create_item("Progressive Buff"))
    return out

def create_perk_terminal_unlocks(world:WKWorld) -> list[Item]:
    out = []
    for i in range(3):
        out.append(world.create_item("Progressive Perk Machine"))
    return out

def create_progressive_access(world:WKWorld) -> list[Item]:
    out = []
    for i in range(5):
        out.append(world.create_item("Progressive Region"))
    return out

