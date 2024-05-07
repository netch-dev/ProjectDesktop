using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Window_ControlPanel : MonoBehaviour {
	[SerializeField] private GameObject[] uiPages;
	[SerializeField] private TextMeshProUGUI coinsAmountText;
	[SerializeField] private TextMeshProUGUI fuelAmountText;

	private void Awake() {
		for (int i = 1; i < uiPages.Length; i++) {
			uiPages[i].SetActive(false);
		}

		GameResources.OnResourceAmountChanged += GameResourcesOnResourceAmountChanged;
	}

	private void GameResourcesOnResourceAmountChanged(object sender, EventArgs e) {
		fuelAmountText.text = GameResources.GetResourceAmount(GameResources.ResourceType.Fuel).ToString("N0");
		coinsAmountText.text = GameResources.GetResourceAmount(GameResources.ResourceType.Gold).ToString("N0");
	}

	#region UI Buttons
	public void OnClickPage(int pageIndex) {
		for (int i = 0; i < uiPages.Length; i++) {
			uiPages[i].SetActive(i == pageIndex);
		}
	}
	#endregion
}
