using UnityEngine;

public class HarvestTask : ITask {
	private Animator animator;
	private CropGrower currentCrop = null;
	public HarvestTask(Animator animator) {
		this.animator = animator;
	}
	public bool IsAvailable(NPC npc) {
		return currentCrop != null || HarvestManager.Instance.HasCropsWaitingToBeHarvested();
	}

	public void ExecuteTask(NPC npc) {
		if (currentCrop == null) {
			currentCrop = HarvestManager.Instance.GetClosestHarvestable(npc.transform.position);
		}

		if (currentCrop != null) {
			if (Vector3.Distance(npc.transform.position, currentCrop.transform.position) > 3f) {
				npc.MoveTo(currentCrop.transform.position);
			} else {
				// Start harvesting
				animator.SetFloat("WalkSpeed", 0f);
				currentCrop.HarvestCrop();
			}
		}
	}
}

