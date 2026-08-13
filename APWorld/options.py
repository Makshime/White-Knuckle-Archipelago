from dataclasses import dataclass

from Options import Choice, OptionGroup, PerGameCommonOptions, Range, Toggle

class TestOption(Toggle):
    """
    A test option meant to check if the options menu works
    """

    display_name = "Test Option"

class WKOptions(PerGameCommonOptions):
    test_option = TestOption

    option_groups = [
        OptionGroup(
            "Gameplay Options",
            [TestOption],
        )
    ]
