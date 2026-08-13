from collections.abc import Mapping
from typing import Any

from World.AutoWorld import World

from . import items, locations, regions, rules
from . import options as WKOptions

class WKWorld(World):
    """
    White Knuckle is a funny and silly climbing game about escaping da facility
    Class 7 fluid detected. Escape.
    """

    game = "White Knuckle"

    location_name_to_id = locations.LOCATION_NAME_TO_ID
    item_name_to_id = items.ITEM_NAME_TO_ID

    origin_region_name = "Silos 1"

    def create_regions(self) -> None:
        regions.create_and_connect_regions(self)
        locations.create_all_locations(self)


    def set_rules(self) -> None:
        rules.set_all_rules(self)

    def create_items(self) -> None:
        items.create_all_items(self)

    def create_item(self, name: str) -> items.WKItem:
        return items.create_item_with_correct_classification(self, name)

    def get_filler_item_name(self) -> str:
        return items.get_random_filler_item_name(self)

    def fill_slot_data(self) -> Mapping[str, Any]:
        return self.options.as_dict(
            "Mode"
        )