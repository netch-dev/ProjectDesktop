using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode {
	private Transform resourceNodeTransform;

	private int resourceAmount;

	public ResourceNode(Transform resourceNodeTransform) {
		this.resourceNodeTransform = resourceNodeTransform;
		resourceAmount = 3;
	}

	public Vector3 GetPosition() {
		return resourceNodeTransform.position;
	}

	public void GrabResource() {
		resourceAmount -= 1;

		if (resourceAmount <= 0) {
			ToggleVisuals(false);
		}
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
