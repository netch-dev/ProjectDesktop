using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Window_ControlPanel : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI coinsAmountText;
	[SerializeField] private TextMeshProUGUI fuelAmountText;

	private void Awake() {
		GameResources.OnResourceAmountChanged += GameResourcesOnResourceAmountChanged;
	}

	private void GameResourcesOnResourceAmountChanged(object sender, EventArgs e) {
		fuelAmountText.text = GameResources.GetResourceAmount(GameResources.ResourceType.Fuel).ToString("N0");
		coinsAmountText.text = GameResources.GetResourceAmount(GameResources.ResourceType.Gold).ToString("N0");
	}
}
