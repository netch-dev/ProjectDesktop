using CodeMonkey;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode {
	public static event EventHandler OnResourceNodeClicked;

	private Transform resourceNodeTransform;
	private GameResources.ResourceType resourceType;

	private int resourceAmount;
	private int resourceAmountMax;

	public ResourceNode(Transform resourceNodeTransform, GameResources.ResourceType resourceType) {
		resourceNodeTransform.GetComponent<ClickableObject>().OnClick += ClickableObjectOnClick;

		this.resourceNodeTransform = resourceNodeTransform;
		this.resourceType = resourceType;

		this.resourceAmountMax = 3;
		this.resourceAmount = resourceAmountMax;

		FunctionPeriodic.Create(RegenerateSingleAmount, 6f);
		CMDebug.TextUpdater(() => resourceAmount.ToString(), resourceNodeTransform.position, resourceNodeTransform);
	}

	private void ClickableObjectOnClick(object sender, EventArgs e) {
		OnResourceNodeClicked?.Invoke(this, EventArgs.Empty);
	}

	public Vector3 GetPosition() {
		return resourceNodeTransform.position;
	}
	public GameResources.ResourceType GetResourceType() {
		return resourceType;
	}

	public GameResources.ResourceType GrabResource() {
		resourceAmount -= 1;

		if (resourceAmount <= 0) {
			// Node is depleted
			ToggleVisuals(false);
			/*			FunctionTimer.Create(() => {
							ResetResourceAmount();
						}, 5f);*/
		}

		return resourceType;
	}
	private void ToggleVisuals(bool isEnabled) {
		if (isEnabled) {
			resourceNodeTransform.Find("Enabled").gameObject.SetActive(true);
			resourceNodeTransform.Find("Disabled").gameObject.SetActive(false);
		} else {
			resourceNodeTransform.Find("Enabled").gameObject.SetActive(false);
			resourceNodeTransform.Find("Disabled").gameObject.SetActive(true);
		}
	}

	private void ResetResourceAmount() {
		if (resourceAmount == 0) ToggleVisuals(true);
		resourceAmount = resourceAmountMax;
	}

	private void RegenerateSingleAmount() {
		if (resourceAmount == 0) ToggleVisuals(true);

		resourceAmount += 1;
		if (resourceAmount > resourceAmountMax) {
			resourceAmount = resourceAmountMax;
		}
	}

	public bool HasResources() {
		return resourceAmount > 0;
	}

}
