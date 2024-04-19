using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Window_GameResources : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI cornAmountText;
	[SerializeField] private TextMeshProUGUI woodAmountText;
	[SerializeField] private TextMeshProUGUI goldAmountText;
	private void Awake() {
		GameResources.OnResourceAmountChanged += GameResourceOnWoodAmountChanged;

		UpdateResourceTextObject();
	}

	private void GameResourceOnWoodAmountChanged(object sender, EventArgs e) {
		UpdateResourceTextObject();
	}

	private void UpdateResourceTextObject() {
		if (woodAmountText) woodAmountText.text = GameResources.GetResourceAmount(GameResources.ResourceType.Wood).ToString("N0");
		if (goldAmountText) goldAmountText.text = GameResources.GetResourceAmount(GameResources.ResourceType.Gold).ToString("N0");
	}
}
