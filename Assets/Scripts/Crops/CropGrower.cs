using System;
using UnityEngine;
using UnityEngine.UI;

public class CropGrower : MonoBehaviour {
	public static event EventHandler OnCropFullyGrown;
	public static event EventHandler OnEmptyWater;
	public Action OnCropHarvested;
	//public Action OnCropWatered;

	public GameObject[] cropStages;
	public Image waterImage;
	public CropSO cropData;

	public CropArea cropArea;

	private float secondsPerStage;
	private float nextTimeToChange;
	private int currentStage = 0;

	private bool canHarvestCrop = false;

	private float currentWaterLevel = 0;
	private bool canWaterCrop = false;

	public void InitCrop(CropArea cropArea) {
		this.cropArea = cropArea;

		for (int i = 1; i < cropStages.Length; i++) {
			cropStages[i].SetActive(false);
		}

		secondsPerStage = cropData.secondsToFullyGrow / cropStages.Length;
		nextTimeToChange = UnityEngine.Time.timeSinceLevelLoad + cropData.secondsToFullyGrow;

		InvokeRepeating(nameof(TryGrowCrop), 0, 1);
	}

	private void TryGrowCrop() {
		if (canHarvestCrop || canWaterCrop) return;

		if (currentWaterLevel <= 0) {
			OnEmptyWater?.Invoke(this, EventArgs.Empty);
			canWaterCrop = true;

			waterImage.gameObject.SetActive(true);
			return;
		}

		currentWaterLevel -= cropData.waterUsagePerSecond;

		bool canGrow = UnityEngine.Time.timeSinceLevelLoad >= nextTimeToChange;
		if (!canGrow) return;

		if (currentStage < cropStages.Length - 1) {
			cropStages[currentStage].SetActive(false);
			cropStages[++currentStage].SetActive(true);

			nextTimeToChange = UnityEngine.Time.timeSinceLevelLoad + secondsPerStage;
		}

		if (currentStage == cropStages.Length - 1) {
			canHarvestCrop = true;
			if (IsInvoking(nameof(TryGrowCrop))) {
				CancelInvoke(nameof(TryGrowCrop));
			}
			OnCropFullyGrown?.Invoke(this, EventArgs.Empty);
		}
	}

	public void HarvestCrop() {
		OnCropHarvested?.Invoke();

		GameResources.AddResourceAmount(GameResources.ResourceType.Gold, cropData.harvestGoldYield);

		UnityEngine.Object.Destroy(this.gameObject);
	}

	public bool CanHarvestCrop() {
		return canHarvestCrop;
	}

	public void WaterCrop() {
		//OnCropWatered?.Invoke();
		currentWaterLevel = 100;
		canWaterCrop = false;
		waterImage.gameObject.SetActive(false);
	}

	public bool CanWaterCrop() {
		return currentWaterLevel <= 0;
	}
}
