using RootMotion.FinalIK;
using System.Collections;
using UnityEngine;

public class WaterPlantsTask : ITask {
	private Animator animator;

	private CropGrower currentCrop = null;
	private bool isCurrentlyWatering = false;

	private int currentWaterLevel = 0;

	public WaterPlantsTask(NPC npc, Animator animator, int reduceAmountPerWateringRun) {
		npc.OnAnimationCompleted += () => {
			currentCrop.WaterCrop();
			currentCrop = null;
			isCurrentlyWatering = false;
			currentWaterLevel -= reduceAmountPerWateringRun;
		};

		this.animator = animator;
	}
	public bool IsAvailable(NPC npc) {
		return currentCrop != null || CropManager.Instance.HasCropsWaitingToBeWatered();
	}

	public bool IsComplete(NPC npc) {
		return currentCrop == null || !currentCrop.CanWaterCrop();
	}

	public void ExecuteTask(NPC npc) {
		if (isCurrentlyWatering) {
			Debug.Log("Already watering");
			return;
		}

		if (currentCrop == null) {
			currentCrop = CropManager.Instance.GetClosestCropThatNeedsWater(npc.transform.position);
		}

		if (currentWaterLevel <= 0) {
			Transform waterNode = GameHandler.GetClosestWaterNode_Static(npc.transform.position);
			if (Vector3.Distance(npc.transform.position, waterNode.position) > 1.5f) {
				npc.MoveTo(waterNode.position);
			} else {
				npc.Arrived();
				currentWaterLevel = 100;
			}
			return;
		}

		if (currentCrop != null) {
			if (Vector3.Distance(npc.transform.position, currentCrop.transform.position) > 1.5f) {
				npc.MoveTo(currentCrop.transform.position);
			} else {
				npc.Arrived();
				WaterPlant(npc, currentCrop.transform.position);
			}
		}
	}

	private void WaterPlant(NPC npc, Vector3 cropPosition) {
		isCurrentlyWatering = true;

		npc.transform.LookAt(cropPosition);

		animator.SetTrigger("Watering");
	}

	public override string ToString() {
		return $"WaterPlantsTask\nCrop at {currentCrop?.transform.position}\nWater level: {currentWaterLevel}";
	}
}

