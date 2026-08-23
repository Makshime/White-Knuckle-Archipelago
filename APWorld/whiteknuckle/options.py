from dataclasses import dataclass

from Options import Choice, OptionGroup, PerGameCommonOptions, Range, Toggle

class StartingDebuffs(Range):
    """
    The amount of "Debuff" perks you start with
    """

    display_name = "Starting Debuffs"

    range_start = 0
    range_end = 20
    default = 10

class TotalBuffs(Range):
    """
    The amount of "Progressive Stats" will be in the pool
    One buff is worth the opposite of a debuff
    """

    display_name = "Total Buffs"

    range_start = 0
    range_end = 25
    default = 15


@dataclass
class WKOptions(PerGameCommonOptions):
    Starting_Debuffs: StartingDebuffs
    Total_Buffs: TotalBuffs

option_groups = [
    OptionGroup(
        "Difficulty",
        [StartingDebuffs, TotalBuffs],
    ),
]