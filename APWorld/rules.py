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

    i3_to_a1 = world.get_entrance("Interlude 3 to Abyss 1")

    i4_to_n1 = world.get_entrance("Interlude 4 to Nest 1")

    n3_to_c1 = world.get_entrance("Nest 3 to Core 1")

    world.set_rule(s2_to_i1, Has("Progressive Buff", count=2))
    world.set_rule(s3_to_i1, Has("Progressive Buff", count=2))
    world.set_rule(sink_to_i1, Has("Progressive Buff", count=5))
    world.set_rule(s1_to_sink, Has("Progressive Buff", count=1) & Has("Tangled Sink Access"))

    world.set_rule(p1_to_chute, Has("Progressive Buff", count=5) & Has("Expulsion Chute Access"))
    world.set_rule(chute_to_i2), Has("Progressive Buff", count=7)
    world.set_rule(p3_to_i2, Has("Progressive Buff", count=5))


    world.set_rule(i1_to_p1, Has("Progressive Region", count=1))
    world.set_rule(i2_to_h1, Has("Progressive Region", count=2))
    world.set_rule(i3_to_a1, Has("Progressive Region", count=3))
    world.set_rule(i4_to_n1, Has("Progressive Region", count=4))

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


def set_completion_condition(world: WKWorld) -> None:

    world.set_completion_rule(HasAllCounts("I1: Recycler Upgrade", "I2: Recycler Upgrade", "I3: Recycler Upgrade", "I4: Recycler Upgrade"))
