using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResources {

	public static event EventHandler OnWoodAmountChanged;

	private static int woodAmount;
	public static void AddWoodAmount(int amount) {
		woodAmount += amount;
		OnWoodAmountChanged?.Invoke(null, EventArgs.Empty);
	}

	public static int GetWoodAmount() {
		return woodAmount;
	}

}
