from dataclasses import dataclass
from worlds.AutoWorld import WebWorld
from Options import Choice, OptionGroup, PerGameCommonOptions, NamedRange, Range, Toggle, Visibility

class DeathLink(Choice):
    """
    Whether you want to play with deathlink on
    Can always be changed in the client
    'Unchanged' means it won't change the setting in the client upon first connection
    """

    display_name = "Deathlink"

    option_unchanged = -1
    option_off = 0
    option_on = 1
    default = option_unchanged

class DeathlinkAmnesty(NamedRange):
    """
    How many deathlinks you need to RECEIVE before you actually die
    'Unchanged' means it won't change the setting in the client upon first connection
    """

    display_name = "Deathlink Amnesty"
    special_range_names = {
    "unchanged": 0,
    }
    range_start = 0
    range_end = 20
    default = 2


class FreeDiskVendors(Toggle):
    """
    Makes all disk vendors free if enabled.
    Can always be changed in the client
    """
    display_name = "Free Disk Vendors"


class GoalRegion(Choice):
    """
    What region you have to reach in order to send the goal
    """
    display_name = "Goal Region"
    option_Pipeworks = 1
    option_Habitation = 2
    option_Abyss = 3
    option_Nest = 4
    option_Core = 5
    default = option_Nest

class TotalBuffs(Range):
    """
    The amount of "Progressive Buffs" that will be in the pool
    One buff is worth the opposite of a debuff
    ***WARNING*** If this is set too low for the logic to generate, it will throw an error!
    """

    display_name = "Total Buffs"

    range_start = 5
    range_end = 40
    default = 20

class StartingDebuffs(Range):
    """
    The amount of "Debuff" perks you start with
    """

    display_name = "Starting Debuffs"
    range_start = 0
    range_end = 12
    default = 10

class TotalTrinketSlots(Range):
    """
    The total amount of progressive trinket slot items that will be in the pool
    """
    display_name = "Total Extra Trinket Slots"
    range_start = 0
    range_end = 10
    default = 5

class StartingTrinketSlots(Range):
    """
    The amount of trinket slots that you start with
    """
    display_name = "Starting Trinket Slots"
    range_start = 0
    range_end = 10
    default = 3

class IncludeTrainingSector(Toggle):
    """
    Adds training sector rooms to the location pool as well as an item check to allow you to enter it
    """
    display_name = "Include Training Sector"


class IncludeChallengeModes(Choice):
    """
    Makes the individual challenge modes and their medals checks to be completed, 4 per challenge, for 24 total checks
    Off: Turns off the challenge modes entirely
    Open: Makes all challenges available from the start
    Locked: Makes each challenge require a check to be accessible
    """

    display_name = "Include Challenge Modes"
    option_Off = 0
    option_Open = 1
    option_Locked = 2
    default = option_Locked

class HiddenClampTotalBuffs(Toggle):
    """
    Hidden option specifically for large sync/async creators to prevent gen errors
    """
    visibility = Visibility.none
    display_name = "Hidden Clamp Total Buffs"



@dataclass
class WKOptions(PerGameCommonOptions):
    Deathlink: DeathLink
    Deathlink_Amnesty: DeathlinkAmnesty
    Free_Disk_Vendors: FreeDiskVendors

    Starting_Debuffs: StartingDebuffs
    Total_Buffs: TotalBuffs
    Starting_Trinket_Slots: StartingTrinketSlots
    Total_Trinket_Slots: TotalTrinketSlots

    Goal_Region: GoalRegion
    Include_Training_Sector: IncludeTrainingSector
    Include_Challenge_Modes: IncludeChallengeModes

    Clamp: HiddenClampTotalBuffs

    def __post_init__(self):
        if self.Goal_Region == 3 and self.Total_Buffs < 6:
            if self.Clamp:
                self.Total_Buffs.value = 6
            else:
                raise ValueError("Total Buff count too low for generation! Need at least 6")
        elif self.Goal_Region == 4 and self.Total_Buffs < 8:
            if self.Clamp:
                self.Total_Buffs.value = 8
            else:
                raise ValueError("Total Buff count too low for generation! Need at least 8")
        elif self.Goal_Region == 5 and self.Total_Buffs < 11:
            if self.Clamp:
                self.Total_Buffs.value = 11
            else:
                raise ValueError("Total Buff count too low for generation! Need at least 11")
        if self.Include_Challenge_Modes != 0 and self.Total_Buffs < 12:
            if self.Clamp:
                self.Total_Buffs.value = 12
            else:
                raise ValueError("Total Buff count too low for generation! Need at least 12")


##needed for options creator to recognize groups
class WKWebWorld(WebWorld):
    game = "White Knuckle"

    option_groups = [
        OptionGroup(
            "Difficulty",
            [StartingDebuffs, TotalBuffs, GoalRegion, IncludeChallengeModes, IncludeTrainingSector, StartingTrinketSlots, TotalTrinketSlots]
        ),
        OptionGroup(
            "Client Side",
            [DeathLink, DeathlinkAmnesty, FreeDiskVendors]
        ),
    ]
