from __future__ import annotations

from typing import TYPE_CHECKING

from BaseClasses import Entrance, Region

if TYPE_CHECKING:
    from .world import WKWorld



def create_and_connect_regions(world: WKWorld) -> None:
    create_all_regions(world)
    connect_regions(world)


def create_all_regions(world: WKWorld) -> None:
    global_shop = Region("Global Shop", world.player, world.multiworld)

    sink = Region("Sink", world.player, world.multiworld)
    chute = Region("Chute", world.player, world.multiworld)
    silos_1 = Region("Silos 1", world.player, world.multiworld)
    silos_2 = Region("Silos 2", world.player, world.multiworld) ## Shattered
    silos_3 = Region("Silos 3", world.player, world.multiworld) ## Air Exchange
    interlude_1 = Region("Interlude 1", world.player, world.multiworld)
    pipeworks_1 = Region("Pipeworks 1", world.player, world.multiworld) ## Drainage
    pipeworks_2 = Region("Pipeworks 2", world.player, world.multiworld) ## Waste Heap
    pipeworks_3 = Region("Pipeworks 3", world.player, world.multiworld) ## Organ
    interlude_2 = Region("Interlude 2", world.player, world.multiworld)
    hab_1 = Region("Habitation 1", world.player, world.multiworld)
    hab_2 = Region("Habitation 2", world.player, world.multiworld)
    hab_3 = Region("Habitation 3", world.player, world.multiworld)
    interlude_3 = Region("Interlude 3", world.player, world.multiworld)
    abyss_1 = Region("Abyss 1", world.player, world.multiworld)
    abyss_2 = Region("Abyss 2", world.player, world.multiworld)
    abyss_3 = Region("Abyss 3", world.player, world.multiworld)
    interlude_4 = Region("Interlude 4", world.player, world.multiworld)
    nest_1 = Region("Nest 1", world.player, world.multiworld) ## Infested Lab
    nest_2 = Region("Nest 2", world.player, world.multiworld) ## Hot Zone
    nest_3 = Region("Nest 3", world.player, world.multiworld)
    core_1 = Region("Core 1", world.player, world.multiworld)

    regions = [sink, chute, global_shop, silos_1, silos_2, silos_3, interlude_1, pipeworks_1, pipeworks_2, pipeworks_3, interlude_2, hab_1, hab_2, hab_3, interlude_3, abyss_1, abyss_2, abyss_3, interlude_4, nest_1, nest_2, nest_3, core_1]

    world.multiworld.regions += regions


def connect_regions(world: WKWorld) -> None:
    global_shop = world.get_region("Global Shop")

    sink = world.get_region("Sink")
    chute = world.get_region("Chute")
    silos_1 = world.get_region("Silos 1")
    silos_2 = world.get_region("Silos 2") ## Shattered
    silos_3 = world.get_region("Silos 3") ## Air Exchange
    interlude_1 = world.get_region("Interlude 1")
    pipeworks_1 = world.get_region("Pipeworks 1") ## Drainage
    pipeworks_2 = world.get_region("Pipeworks 2") ## Waste Heap
    pipeworks_3 = world.get_region("Pipeworks 3") ## Organ
    interlude_2 = world.get_region("Interlude 2")
    hab_1 = world.get_region("Habitation 1")
    hab_2 = world.get_region("Habitation 2")
    hab_3 = world.get_region("Habitation 3")
    interlude_3 = world.get_region("Interlude 3")
    abyss_1 = world.get_region("Abyss 1")
    abyss_2 = world.get_region("Abyss 2")
    abyss_3 = world.get_region("Abyss 3")
    interlude_4 = world.get_region("Interlude 4")
    nest_1 = world.get_region("Nest 1") ## Infested Labs
    nest_2 = world.get_region("Nest 2") ## Hot Zone
    nest_3 = world.get_region("Nest 3")
    core_1 = world.get_region("Core 1")

    silos_1.connect(global_shop, "Global Shop Access")

    silos_1.connect(silos_2, "Silos 1 to Silos 2")
    silos_1.connect(silos_3, "Silos 1 to Silos 3")
    silos_2.connect(interlude_1, "Silos 2 to Interlude 1")
    silos_3.connect(interlude_1, "Silos 3 to Interlude 1")

    silos_1.connect(sink, "Silos 1 to Sink")
    sink.connect(interlude_1, "Sink to Interlude 1")

    interlude_1.connect(pipeworks_1, "Interlude 1 to Pipeworks 1")

    pipeworks_1.connect(chute, "Pipeworks 1 to Chute")
    chute.connect(interlude_2, "Chute to Interlude 2")

    pipeworks_1.connect(pipeworks_2, "Pipeworks 1 to Pipeworks 2")
    pipeworks_2.connect(pipeworks_3, "Pipeworks 2 to Pipeworks 3")
    pipeworks_3.connect(interlude_2, "Pipeworks 3 to Interlude 2")

    interlude_2.connect(hab_1, "Interlude 2 to Habitation 1")
    hab_1.connect(hab_2, "Habitation 1 to Habitation 2")
    hab_2.connect(hab_3, "Habitation 2 to Habitation 3")
    hab_3.connect(interlude_3, "Habitation 3 to Interlude 3")

    interlude_3.connect(abyss_1, "Interlude 3 to Abyss 1")
    abyss_1.connect(abyss_2, "Abyss 1 to Abyss 2")
    abyss_2.connect(abyss_3, "Abyss 2 to Abyss 3")
    abyss_3.connect(interlude_4, "Abyss 3 to Interlude 4")

    interlude_4.connect(nest_1, "Interlude 4 to Nest 1")
    interlude_4.connect(nest_2, "Interlude 4 to Nest 2")
    nest_1.connect(nest_3, "Nest 1 to Nest 3")
    nest_2.connect(nest_3, "Nest 2 to Nest 3")
    nest_3.connect(core_1, "Nest 3 to Core 1")


