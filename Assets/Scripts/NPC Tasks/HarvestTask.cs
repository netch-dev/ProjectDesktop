using System.Collections;
using UnityEngine;

public class HarvestTask : ITask {
	private Animator animator;
	private CropGrower currentCrop = null;
	private bool isCurrentlyHarvesting = false;

	public HarvestTask(Animator animator) {
		this.animator = animator;
	}

	public bool IsAvailable() {
		// Task is available if there are crops to harvest or one is already assigned
		return currentCrop != null || CropManager.Instance.HasCropsWaitingToBeHarvested();
	}

	public bool IsComplete(NPC npc) {
		// Task is complete only if no crops are left to harvest
		return currentCrop == null && !CropManager.Instance.HasCropsWaitingToBeHarvested();
	}

	public void OnStart(NPC npc) {
		Debug.Log("Starting HarvestTask...");
	}

	public void ExecuteTask(NPC npc) {
		if (isCurrentlyHarvesting) return;

		if (currentCrop == null) {
			currentCrop = CropManager.Instance.GetClosestHarvestable(npc.transform.position);
		}

		if (currentCrop != null) {
			if (Vector3.Distance(npc.transform.position, currentCrop.transform.position) > 1.5f) {
				npc.MoveTo(currentCrop.transform.position);
			} else {
				npc.StopMoving();
				HarvestCrop(npc);
			}
		}
	}

	private void HarvestCrop(NPC npc) {
		if (isCurrentlyHarvesting) return;

		isCurrentlyHarvesting = true;

		npc.StartCoroutine(HarvestCropCoroutine(npc, currentCrop));
	}

	private IEnumerator HarvestCropCoroutine(NPC npc, CropGrower crop) {
		Debug.Log($"Starting to harvest crop: {crop.name}");

		yield return npc.LookAt(crop.transform.position);

		animator.SetTrigger("Harvest");

		yield return new WaitForSeconds(3f);

		crop.HarvestCrop();
		currentCrop = null;

		isCurrentlyHarvesting = false;
		Debug.Log($"Finished harvesting crop: {crop.name}");
	}

	public void OnFinish(NPC npc) {
		Debug.Log("Finished HarvestTask.");
		currentCrop = null;
		isCurrentlyHarvesting = false;
	}
}
