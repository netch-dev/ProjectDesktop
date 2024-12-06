using System.Collections.Generic;

public static class DeveloperCheats {
	public enum Cheat {
		InstantBuild,
		InstantWater,
		InstantHarvest
	}

	private static Dictionary<Cheat, bool> cheats = new Dictionary<Cheat, bool> {
		{ Cheat.InstantBuild, false },
		{ Cheat.InstantWater, false },
		{ Cheat.InstantHarvest, false }
	};

	public static bool GetCheat(Cheat cheat) {
		return cheats[cheat];
	}

	public static void ToggleCheat(Cheat cheat) {
		cheats[cheat] = !cheats[cheat];
	}
}
