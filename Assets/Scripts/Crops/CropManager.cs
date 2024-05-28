using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CropManager : MonoBehaviour {
	public static CropManager Instance;

	private List<CropGrower> fullyGrownCropList = new List<CropGrower>();
	private List<CropGrower> needsWaterCropList = new List<CropGrower>();

	private void Start() {
		if (Instance != null) Debug.LogError("There are multiple CropManager scripts in the scene");
		Instance = this;

		CropGrower.OnCropFullyGrown += CropGrower_OnCropFullyGrown;
		CropGrower.OnEmptyWater += CropGrower_OnEmptyWater;
	}

	private void CropGrower_OnCropFullyGrown(object sender, EventArgs e) {
		CropGrower cropGrower = sender as CropGrower;
		fullyGrownCropList.Add(cropGrower);
	}
	private void CropGrower_OnEmptyWater(object sender, EventArgs e) {
		CropGrower cropGrower = sender as CropGrower;
		needsWaterCropList.Add(cropGrower);
	}

	public bool HasCropsWaitingToBeHarvested() {
		return fullyGrownCropList.Count > 0;
	}
	public bool HasCropsWaitingToBeWatered() {
		return needsWaterCropList.Count > 0;
	}

	public CropGrower GetClosestHarvestable(Vector3 currentPosition) {
		CropGrower closestCrop = null;
		float closestDistance = Mathf.Infinity;

		foreach (CropGrower crop in fullyGrownCropList) {
			float distance = Vector3.Distance(currentPosition, crop.transform.position);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestCrop = crop;
			}
		}

		if (closestCrop != null) fullyGrownCropList.Remove(closestCrop);
		return closestCrop;
	}

	public CropGrower GetClosestCropThatNeedsWater(Vector3 currentPosition) {
		CropGrower closestCrop = null;
		float closestDistance = Mathf.Infinity;

		// Remove crops that don't need water anymore - In case the area is watered
		foreach (CropGrower crop in needsWaterCropList.ToList()) {
			if (!crop.CanWaterCrop()) {
				needsWaterCropList.Remove(crop);
			}
		}

		foreach (CropGrower crop in needsWaterCropList) {
			float distance = Vector3.Distance(currentPosition, crop.transform.position);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestCrop = crop;
			}
		}

		if (closestCrop != null) needsWaterCropList.Remove(closestCrop);

		return closestCrop;
	}
}