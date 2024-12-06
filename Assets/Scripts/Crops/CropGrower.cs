using System;
using UnityEngine;
using UnityEngine.UI;

public class CropGrower : MonoBehaviour {
	public static event EventHandler OnCropFullyGrown;
	public static event EventHandler OnEmptyWater;
	public Action OnCropHarvested;
	//public Action OnCropWatered;

	[SerializeField] private GameObject[] cropStages;
	[SerializeField] private float secondsToFullyGrow;
	[SerializeField] private int goldAmountForHarvesting;
	[SerializeField] private float waterRequirementPerSecond;
	[SerializeField] private Image waterImage;

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

		secondsPerStage = secondsToFullyGrow / cropStages.Length;
		nextTimeToChange = UnityEngine.Time.timeSinceLevelLoad + secondsToFullyGrow;

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

		currentWaterLevel -= waterRequirementPerSecond;

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

		GameResources.AddResourceAmount(GameResources.ResourceType.Gold, goldAmountForHarvesting);

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
