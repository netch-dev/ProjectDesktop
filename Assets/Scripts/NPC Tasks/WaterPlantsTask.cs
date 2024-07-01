using RootMotion.FinalIK;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WaterPlantsTask : ITask {
	private Animator animator;

	private GameObject rightHandEffector;
	private Vector3 initialRightHandLocalPosition;
	private Vector3 wateringRightHandLocalPosition = new Vector3(0.43f, 1.47f, 1.12f);

	private FullBodyBipedIK fullBodyBipedIK;

	private CropGrower currentCrop = null;
	private bool isCurrentlyWatering = false;

	private int currentWaterLevel = 0;

	public WaterPlantsTask(NPC npc, Animator animator, GameObject rightHandEffector, FullBodyBipedIK fullBodyBipedIK, int reduceAmountPerWateringRun) {
		npc.OnAnimationCompleted += () => {
			//currentCrop.WaterCrop();
			currentCrop.cropArea.WaterCropArea();
			currentCrop = null;
			currentWaterLevel -= reduceAmountPerWateringRun;

			isCurrentlyWatering = false;
		};

		this.animator = animator;

		this.rightHandEffector = rightHandEffector;
		this.initialRightHandLocalPosition = rightHandEffector.transform.localPosition;

		this.fullBodyBipedIK = fullBodyBipedIK;
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
				npc.StartCoroutine(WaterPlantCoroutine(npc, currentCrop.transform.position));
				//WaterPlant(npc, currentCrop.transform.position);
			}
		}
	}

	private IEnumerator WaterPlantCoroutine(NPC npc, Vector3 cropPosition) {
		isCurrentlyWatering = true;

		yield return npc.LookAt(cropPosition);

		animator.SetTrigger("Watering");
	}


	private void WaterPlant(NPC npc, Vector3 cropPosition) {
		isCurrentlyWatering = true;

		npc.transform.LookAt(cropPosition);

		animator.SetTrigger("Watering");
		rightHandEffector.transform.localPosition = wateringRightHandLocalPosition;
	}

	public override string ToString() {
		return $"WaterPlantsTask\nCrop at {currentCrop?.transform.position}\nWater level: {currentWaterLevel}";
	}
}

