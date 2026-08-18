from .bases import WKTestBase

class TestDefaultLogic(WKTestBase):

    options = {}

    def test_region_locks(self) -> None:

        with self.subTest("Testing region locks"):

            drainage_01 = self.world.get_location("Pipeworks: Drainage 01")
            drainage_02 = self.world.get_location("Pipeworks: Drainage 02")

            shaft_05 = self.world.get_location("Habitation: Service Shaft 05")
            shaft_06 = self.world.get_location("Habitation: Service Shaft 06")

            abyss_01 = self.world.get_location("Abyss: Transit 01")
            abyss_02 = self.world.get_location("Abyss: Transit 02")

            self.collect_by_name("Progressive Buff")

            regions = [item for item in self.multiworld.itempool if item.name == "Progressive Region"]

            self.assertFalse(drainage_01.can_reach(self.multiworld.state))
            self.assertFalse(drainage_02.can_reach(self.multiworld.state))

            self.assertFalse(shaft_05.can_reach(self.multiworld.state))
            self.assertFalse(shaft_06.can_reach(self.multiworld.state))

            self.assertFalse(abyss_01.can_reach(self.multiworld.state))
            self.assertFalse(abyss_02.can_reach(self.multiworld.state))

            self.collect(regions[0])

            self.assertTrue(drainage_01.can_reach(self.multiworld.state))
            self.assertTrue(drainage_02.can_reach(self.multiworld.state))

            self.assertFalse(shaft_05.can_reach(self.multiworld.state))
            self.assertFalse(shaft_06.can_reach(self.multiworld.state))

            self.assertFalse(abyss_01.can_reach(self.multiworld.state))
            self.assertFalse(abyss_02.can_reach(self.multiworld.state))

            self.collect(regions[1])

            self.assertTrue(drainage_01.can_reach(self.multiworld.state))
            self.assertTrue(drainage_02.can_reach(self.multiworld.state))

            self.assertTrue(shaft_05.can_reach(self.multiworld.state))
            self.assertTrue(shaft_06.can_reach(self.multiworld.state))

            self.assertFalse(abyss_01.can_reach(self.multiworld.state))
            self.assertFalse(abyss_02.can_reach(self.multiworld.state))

            self.collect(regions[2])

            self.assertTrue(drainage_01.can_reach(self.multiworld.state))
            self.assertTrue(drainage_02.can_reach(self.multiworld.state))

            self.assertTrue(shaft_05.can_reach(self.multiworld.state))
            self.assertTrue(shaft_06.can_reach(self.multiworld.state))

            self.assertTrue(abyss_01.can_reach(self.multiworld.state))
            self.assertTrue(abyss_02.can_reach(self.multiworld.state))

            self.collect(regions[3])
            self.collect(regions[4])

            self.assertTrue(self.multiworld)

