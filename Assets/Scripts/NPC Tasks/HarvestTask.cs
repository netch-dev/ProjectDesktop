using UnityEngine;

public class HarvestTask : ITask {
	private Animator animator;
	private CropGrower currentCrop = null;
	public HarvestTask(Animator animator) {
		this.animator = animator;
	}
	public bool IsAvailable(NPC npc) {
		return currentCrop != null || CropManager.Instance.HasCropsWaitingToBeHarvested();
	}

	public bool IsComplete(NPC npc) {
		return currentCrop == null || !currentCrop.CanHarvestCrop();
	}

	public void ExecuteTask(NPC npc) {
		if (currentCrop == null) {
			currentCrop = CropManager.Instance.GetClosestHarvestable(npc.transform.position);
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
	public override string ToString() {
		return $"Harvest Task\nCrop: {currentCrop?.gameObject.name}";
	}
}

