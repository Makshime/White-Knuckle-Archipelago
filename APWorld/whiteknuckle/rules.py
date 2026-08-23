from __future__ import annotations
from typing import TYPE_CHECKING

from rule_builder.options import OptionFilter
from rule_builder.rules import Has, HasAll, Rule, HasAllCounts

if TYPE_CHECKING:
    from .world import WKWorld

def set_all_rules(world: WKWorld) -> None:

    set_all_entrance_rules(world)
    set_all_location_rules(world)
    set_completion_condition(world)



def set_all_entrance_rules(world: WKWorld) -> None:

    s1_to_sink = world.get_entrance("Silos 1 to Sink")
    sink_to_i1 = world.get_entrance("Sink to Interlude 1")

    s1_to_s2 = world.get_entrance("Silos 1 to Silos 2")
    s1_to_s3 = world.get_entrance("Silos 1 to Silos 3")
    s2_to_i1 = world.get_entrance("Silos 2 to Interlude 1")
    s3_to_i1 = world.get_entrance("Silos 3 to Interlude 1")

    p1_to_chute = world.get_entrance("Pipeworks 1 to Chute")
    chute_to_i2 = world.get_entrance("Chute to Interlude 2")

    i1_to_p1 = world.get_entrance("Interlude 1 to Pipeworks 1")
    p1_to_p2 = world.get_entrance("Pipeworks 1 to Pipeworks 2")
    p2_to_p3 = world.get_entrance("Pipeworks 2 to Pipeworks 3")
    p3_to_i2 = world.get_entrance("Pipeworks 3 to Interlude 2")


    i2_to_h1 = world.get_entrance("Interlude 2 to Habitation 1")
    h1_to_h2 = world.get_entrance("Habitation 1 to Habitation 2")
    h2_to_h3 = world.get_entrance("Habitation 2 to Habitation 3")
    h3_to_i3 = world.get_entrance("Habitation 3 to Interlude 3")

    i3_to_a1 = world.get_entrance("Interlude 3 to Abyss 1")
    a1_to_a2 = world.get_entrance("Abyss 1 to Abyss 2")
    a2_to_a3 = world.get_entrance("Abyss 2 to Abyss 3")
    a3_to_i4 = world.get_entrance("Abyss 3 to Interlude 4")

    i4_to_n1 = world.get_entrance("Interlude 4 to Nest 1")
    i4_to_n2 = world.get_entrance("Interlude 4 to Nest 2")
    n1_to_n3 = world.get_entrance("Nest 1 to Nest 3")
    n2_to_n3 = world.get_entrance("Nest 2 to Nest 3")

    n3_to_c1 = world.get_entrance("Nest 3 to Core 1")

    #i1 logic rules
    world.set_rule(s2_to_i1, Has("Progressive Buff", count=world.options.Starting_Debuffs - 8))
    world.set_rule(s3_to_i1, Has("Progressive Buff", count=world.options.Starting_Debuffs - 8))
    world.set_rule(sink_to_i1, Has("Progressive Buff", count=world.options.Starting_Debuffs - 5))
    world.set_rule(s1_to_sink, HasAllCounts({"Progressive Buff": world.options.Starting_Debuffs - 8, "Tangled Sink Access": 1}))

    world.set_rule(p1_to_chute, HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 5), "Expulsion Chute Access": 1}) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 7), "Progressive Perk Machine": 1, "Expulsion Chute Access":1}))
    world.set_rule(chute_to_i2, Has("Progressive Buff", count=(world.options.Starting_Debuffs - 3)) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 5), "Progressive Perk Machine": 1}))
    world.set_rule(p3_to_i2, Has("Progressive Buff", count=(world.options.Starting_Debuffs - 5)) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 7), "Progressive Perk Machine": 1}))

    world.set_rule(h3_to_i3, Has("Progressive Buff", count=(world.options.Starting_Debuffs - 3)) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 5), "Progressive Perk Machine": 1}) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 6), "Progressive Perk Machine": 2}))

    world.set_rule(a3_to_i4, Has("Progressive Buff", count=(world.options.Starting_Debuffs + 0)) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 2), "Progressive Perk Machine": 1}) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 4), "Progressive Perk Machine": 2}))


    world.set_rule(n2_to_n3, Has("Progressive Buff", count=(world.options.Starting_Debuffs + 5)) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs + 2), "Progressive Perk Machine": 1}) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs + 0), "Progressive Perk Machine": 2}) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 2), "Progressive Perk Machine": 3}))
    world.set_rule(n2_to_n3, Has("Progressive Buff", count=(world.options.Starting_Debuffs + 3)) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs + 1), "Progressive Perk Machine":1}) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs + 0), "Progressive Perk Machine": 2}) | HasAllCounts({"Progressive Buff": (world.options.Starting_Debuffs - 2), "Progressive Perk Machine": 3}))

    world.set_rule(i1_to_p1, Has("Progressive Region", 1))
    world.set_rule(i2_to_h1, Has("Progressive Region", 2))
    world.set_rule(i3_to_a1, Has("Progressive Region", 3))
    world.set_rule(i4_to_n1, Has("Progressive Region", 4))
    world.set_rule(i4_to_n2, Has("Progressive Region", 4))

def set_all_location_rules(world: WKWorld) -> None:

    can_be_rich: Rule = HasAll("I1: Recycler Upgrade", "Trinket: Gold Nugget") | (HasAll("I1: Recycler Upgrade", "I2: Recycler Upgrade", "I3: Recycler Upgrade") & Has("Progressive Region", count = 2) & Has("Progressive Buff", count = 8))

    expensive_items = [
        world.get_location("Global: Ornamental Hammer Purchase"),
        world.get_location("Global: New Gloves Purchase"),
        world.get_location("Global: Bazaar Access"),
        world.get_location("Global: Pouch Purchase"),
        world.get_location("Global: Calming Buddy Purchase"),
        world.get_location("Global: Work Gloves Purchase"),
        world.get_location("I4: Wine Vendor"),
        world.get_location("I3: Rho Altar")
    ]

    for item in expensive_items:
        world.set_rule(item, can_be_rich)

    set_silos_unlock_rules(world)
    set_pipeworks_unlock_rules(world)
    set_habitation_abyss_unlock_rules(world)


def set_completion_condition(world: WKWorld) -> None:

    world.set_completion_rule(HasAllCounts({"Progressive Region":4, "Progressive Perk Machine": 1, "Progressive Buff": (world.options.Starting_Debuffs + 2)})) #this is like not rules builder... but i cant figure out how the fromoption works, soooooo


def set_silos_unlock_rules(world: WKWorld) -> None:

    deep_1 = [
        "Silos: Deep Storage 04",
        "Silos: Deep Storage 11",
        "Silos: Deep Storage 12"
        ]
    silos_1 = [
        "Silos: Deep Storage 02",
        "Silos: Deep Storage 08",
        "Silos: Deep Storage 16",
        "Silos: Air Exchange 05",
        "Silos: Air Exchange 06",
        "Silos: Shattered Chambers 03",
        "Silos: Shattered Chambers 05"
        ]
    silos_2 = [
        "Silos: Deep Storage 05",
        "Silos: Deep Storage 07",
        "Silos: Air Exchange 01",
        "Silos: Air Exchange 09",
        "Silos: Shattered Chambers 02",
        "Silos: Shattered Chambers 06"
        ]
    silos_3 = [
        "Silos: Deep Storage 13",
        "Silos: Deep Storage 14",
        "Silos: Air Exchange 07",
        "Silos: Air Exchange 10",
        "Silos: Shattered Chambers 07"
        ]
    silos_4 = [
        "Silos: Deep Storage 17",
        "Silos: Air Exchange 11",
        "Silos: Shattered Chambers 11"
    ]

    for name in deep_1:
        world.set_rule(world.get_location(name), Has("Deep Storage Room Unlocks"))
    for name in silos_1:
        world.set_rule(world.get_location(name), Has("Silos Room Unlocks: 1"))
    for name in silos_2:
        world.set_rule(world.get_location(name), Has("Silos Room Unlocks: 2"))
    for name in silos_3:
        world.set_rule(world.get_location(name), Has("Silos Room Unlocks: 3"))
    for name in silos_4:
        world.set_rule(world.get_location(name), Has("Silos Room Unlocks: 4"))


def set_pipeworks_unlock_rules(world:WKWorld) -> None:
    pipe_1 = [
        "Pipeworks: Drainage 03",
        "Pipeworks: Drainage 04",
        "Pipeworks: Waste Heap 01",
        "Pipeworks: Pipe Organ 01",
        "Pipeworks: Pipe Organ 03"
    ]
    pipe_2 = [
        "Pipeworks: Drainage 06",
        "Pipeworks: Waste Heap 03",
        "Pipeworks: Pipe Organ 04",
        "Pipeworks: Pipe Organ 08"
    ]
    world.set_rule(world.get_location("Pipeworks: Pipe Organ 09"), Has("Pipeworks Rho Room Access"))
    for name in pipe_1:
        world.set_rule(world.get_location(name), Has("Pipeworks Room Unlocks: 1"))
    for name in pipe_1:
        world.set_rule(world.get_location(name), Has("Pipeworks Room Unlocks: 2"))

def set_habitation_abyss_unlock_rules(world:WKWorld) -> None:
    hab_1 = [
        "Habitation: Service Shaft 07",
        "Habitation: Haunted Pier 03",
        "Habitation: Haunted Pier 04",
        "Habitation: Delta Labs 05",
        "Habitation: Delta Labs 06",
        "Habitation: Delta Labs 07",
        "Habitation: Delta Labs 08"
    ]
    for name in hab_1:
        world.set_rule(world.get_location(name), Has("Habitation Room Unlocks"))

    world.set_rule(world.get_location("Abyss: Handle 02"), Has("Abyss Handle 02 Access"))