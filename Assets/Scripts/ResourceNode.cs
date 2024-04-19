using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode {
	public static event EventHandler OnResourceNodeClicked;

	private Transform resourceNodeTransform;
	private GameResources.ResourceType resourceType;

	private int resourceAmount;

	public ResourceNode(Transform resourceNodeTransform, GameResources.ResourceType resourceType) {
		resourceNodeTransform.GetComponent<ClickableObject>().OnClick += ClickableObjectOnClick;

		this.resourceNodeTransform = resourceNodeTransform;
		this.resourceType = resourceType;

		this.resourceAmount = 3;
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
			ToggleVisuals(false);
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

	private void RespawnResource() {
		resourceAmount = 3;
		ToggleVisuals(true);
	}

	public bool HasResources() {
		return resourceAmount > 0;
	}

}
