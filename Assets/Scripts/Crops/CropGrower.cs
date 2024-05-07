using System;
using UnityEngine;

public class CropGrower : MonoBehaviour {
	public Action OnCropHarvested;

	[SerializeField] private GameObject parentObject;
	[SerializeField] private GameObject[] cropStages;
	[SerializeField] private float secondsToFullyGrow;
	[SerializeField] private int goldAmountForHarvesting;

	private float secondsPerStage;

	private float nextTimeToChange;
	private int currentStage = 0;
	private bool canHarvestCrop = false;

	public void InitCrop() {
		for (int i = 1; i < cropStages.Length; i++) {
			cropStages[i].SetActive(false);
		}

		secondsPerStage = secondsToFullyGrow / cropStages.Length;
		nextTimeToChange = UnityEngine.Time.timeSinceLevelLoad + nextTimeToChange;

		// Invoke repeating method to grow the crop
		InvokeRepeating(nameof(TryGrowCrop), 0, 1);
	}

	private void TryGrowCrop() {
		if (canHarvestCrop) return;

		bool canGrow = UnityEngine.Time.timeSinceLevelLoad >= nextTimeToChange;
		if (!canGrow) return;

		if (currentStage < cropStages.Length - 1) {
			// Disable the current stage game object
			cropStages[currentStage].SetActive(false);

			// Enable the next stage game object
			currentStage++;
			cropStages[currentStage].SetActive(true);

			nextTimeToChange = UnityEngine.Time.timeSinceLevelLoad + secondsPerStage;
		} else {
			canHarvestCrop = true;
			Debug.Log($"Crop is fully grown");
			if (IsInvoking(nameof(TryGrowCrop))) {
				CancelInvoke(nameof(TryGrowCrop));
			}

			// todo remove
			Invoke(nameof(HarvestCrop), 3f);
		}
	}

	public void HarvestCrop() {
		OnCropHarvested?.Invoke();

		GameResources.AddResourceAmount(GameResources.ResourceType.Gold, goldAmountForHarvesting);

		if (IsInvoking(nameof(TryGrowCrop))) {
			CancelInvoke(nameof(TryGrowCrop));
		}
		UnityEngine.Object.Destroy(parentObject);
	}

	public bool CanHarvestCrop() {
		return canHarvestCrop;
	}
}
