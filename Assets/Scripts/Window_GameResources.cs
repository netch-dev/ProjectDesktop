using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_GameResources : MonoBehaviour {
	private Text woodAmountText;
	private void Awake() {
		GameResources.OnWoodAmountChanged += GameResourceOnWoodAmountChanged;

		woodAmountText = transform.Find("woodAmount").GetComponent<Text>();
		UpdateResourceTextObject();
	}

	private void GameResourceOnWoodAmountChanged(object sender, EventArgs e) {
		UpdateResourceTextObject();
	}

	private void UpdateResourceTextObject() {
		if (woodAmountText) woodAmountText.text = "WOOD: " + GameResources.GetResourceAmount(GameResources.ResourceType.Wood);
	}
}
