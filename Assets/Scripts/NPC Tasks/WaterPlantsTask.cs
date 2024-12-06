using System.Collections;
using UnityEngine;

public class WaterPlantsTask : ITask {
	private Animator animator;
	private CropGrower currentCrop = null;
	private bool isCurrentlyWatering = false;

	private int currentWaterLevel = 0;
	private readonly int reduceAmountPerWateringRun = 25;

	public WaterPlantsTask(Animator animator) {
		this.animator = animator;
	}

	public bool IsAvailable() {
		// Task is available if there are crops waiting to be watered or one is already assigned
		return currentCrop != null || CropManager.Instance.HasCropsWaitingToBeWatered();
	}

	public bool IsComplete(NPC npc) {
		// Task is complete only if there are no crops to water
		return currentCrop == null && !CropManager.Instance.HasCropsWaitingToBeWatered();
	}

	public void OnStart(NPC npc) {
		Debug.Log("Starting WaterPlantsTask...");
	}

	public void ExecuteTask(NPC npc) {
		if (isCurrentlyWatering) return;

		if (currentCrop == null) {
			currentCrop = CropManager.Instance.GetClosestCropThatNeedsWater(npc.transform.position);
		}

		if (currentCrop != null) {
			if (Vector3.Distance(npc.transform.position, currentCrop.transform.position) > 1.5f) {
				npc.MoveTo(currentCrop.transform.position);
			} else {
				npc.StopMoving();
				WaterCrop(npc);
			}
		} else if (currentWaterLevel <= 0) {
			RefillWater(npc);
		}
	}

	private void WaterCrop(NPC npc) {
		if (isCurrentlyWatering) return;

		isCurrentlyWatering = true;

		npc.StartCoroutine(WaterCropCoroutine(npc, currentCrop));
	}

	private IEnumerator WaterCropCoroutine(NPC npc, CropGrower crop) {
		Debug.Log($"Starting to water crop: {crop.name}");

		yield return npc.LookAt(crop.transform.position);

		if (!DeveloperCheats.GetCheat(DeveloperCheats.Cheat.InstantWater)) {
			animator.SetTrigger("Watering");
			yield return new WaitForSeconds(6f);
		}

		crop.cropArea.WaterCropArea();
		currentCrop = null;
		currentWaterLevel -= reduceAmountPerWateringRun;

		isCurrentlyWatering = false;
		Debug.Log($"Finished watering crop: {crop.name}");
	}

	private void RefillWater(NPC npc) {
		Transform waterNode = GameHandler.GetClosestWaterNode_Static(npc.transform.position);

		if (Vector3.Distance(npc.transform.position, waterNode.position) > 1.5f) {
			npc.MoveTo(waterNode.position);
		} else {
			npc.StopMoving();
			currentWaterLevel = 100;
			Debug.Log("Refilled water.");
		}
	}

	public void OnFinish(NPC npc) {
		Debug.Log("Finished WaterPlantsTask.");
		currentCrop = null;
		isCurrentlyWatering = false;
	}
}
