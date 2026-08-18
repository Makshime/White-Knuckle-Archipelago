from test.bases import WorldTestBase

from ..world import WKWorld

class WKTestBase(WorldTestBase):
    game = "White Knuckle"
    world: WKWorld