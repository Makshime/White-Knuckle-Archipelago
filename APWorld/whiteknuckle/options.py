from dataclasses import dataclass

from Options import Choice, OptionGroup, PerGameCommonOptions, NamedRange, Range, Toggle

class Deathlink(Choice):
    """
    Whether you want to play with deathlink on
    can always be changed in the client
    unchanged means it wont change the setting in the client upon first connection
    """

    display_name = "Deathlink"

    option_unchanged = -1
    option_off = 0
    option_on = 1
    default = -1

class Deathlink_Amnesty(NamedRange):
    """
    How many deathlinks you need to RECIEVE before you actually die
    unchanged means it wont change the setting in the client upon first connection
    """

    display_name = "Deathlink Amnesty"
    special_range_names = {
    "unchanged": 0,
    "range_start": 0,
    "range_end": 20,
    "default": 2,
    }

class TotalBuffs(Range):
    """
    The amount of "Progressive Stats" will be in the pool
    One buff is worth the opposite of a debuff
    """

    display_name = "Total Buffs"

    range_start = 0
    range_end = 25
    default = 15

class StartingDebuffs(Range):
    """
    The amount of "Debuff" perks you start with
    """

    display_name = "Starting Debuffs"
    range_start = 0
    range_end = 12
    default = 10
    

@dataclass
class WKOptions(PerGameCommonOptions):
    Starting_Debuffs: StartingDebuffs
    Total_Buffs: TotalBuffs
    deathlink: Deathlink
    deathlink_amnesty: Deathlink_Amnesty

option_groups = [
    OptionGroup(
        "Difficulty",
        [StartingDebuffs, TotalBuffs],
    ),
    OptionGroup(
        "Client_Side",
        [Deathlink, Deathlink_Amnesty],
    ),
]
