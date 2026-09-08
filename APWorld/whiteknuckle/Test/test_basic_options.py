from .bases import WKTestBase

class TestBasicOptions(WKTestBase):

    options = {}


    def test_basic_options(self) -> None:

        with self.subTest("Testing Basic Options"):

            drainage_01 = self.world.get_location("Pipeworks: Drainage 01")
            drainage_02 = self.world.get_location("Pipeworks: Drainage 02")
