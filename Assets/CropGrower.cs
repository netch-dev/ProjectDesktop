using UnityEngine;

public class CropGrower {
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
	}

	public void TryGrowCrop() {
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
		}
	}

	public void HarvestCrop() {
		// Give the player some coins
		GameResources.AddResourceAmount(GameResources.ResourceType.Gold, goldAmountForHarvesting);

		// todo create a pooling system for the crops
		// Destroy the gameobject this script is on
		UnityEngine.Object.Destroy(parentObject);
	}

	public bool CanHarvestCrop() {
		return canHarvestCrop;
	}
}
