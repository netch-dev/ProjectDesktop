using System;
using System.Collections.Generic;
using UnityEngine;

public class HarvestManager : MonoBehaviour {
	public static HarvestManager Instance;

	private List<CropGrower> fullyHarvestedCropList = new List<CropGrower>();

	private void Start() {
		if (Instance != null) Debug.LogError("There are multiple HarvestManager scripts in the scene");
		Instance = this;

		CropGrower.OnCropFullyGrown += CropGrower_OnCropFullyGrown;
	}

	private void CropGrower_OnCropFullyGrown(object sender, EventArgs e) {
		CropGrower cropGrower = sender as CropGrower;
		fullyHarvestedCropList.Add(cropGrower);
	}

	public bool HasCropsWaitingToBeHarvested() {
		return fullyHarvestedCropList.Count > 0;
	}

	public CropGrower GetClosestHarvestable(Vector3 currentPosition) {
		CropGrower closestCrop = null;
		float closestDistance = Mathf.Infinity;

		foreach (CropGrower crop in fullyHarvestedCropList) {
			float distance = Vector3.Distance(currentPosition, crop.transform.position);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestCrop = crop;
			}
		}

		if (closestCrop != null) fullyHarvestedCropList.Remove(closestCrop);
		return closestCrop;
	}
}