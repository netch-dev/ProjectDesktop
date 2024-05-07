using UnityEngine;

public class BuildTask : ITask {
	private Animator animator;

	private BuildingScaffold currentBuilding = null;
	public BuildTask(Animator animator) {
		this.animator = animator;
	}

	public bool IsAvailable(NPC npc) {
		return currentBuilding != null || BuildingManager.Instance.HasBuildingWaitingToBeBuilt();
	}

	public void ExecuteTask(NPC npc) {
		if (currentBuilding == null) {
			currentBuilding = BuildingManager.Instance.GetClosestBuildable(npc.transform.position);
		}

		if (currentBuilding != null) {
			if (Vector3.Distance(npc.transform.position, currentBuilding.transform.position) > 3f) {
				npc.MoveTo(currentBuilding.transform.position);
			} else {
				// Start building
				animator.SetFloat("WalkSpeed", 0f);
				currentBuilding.TryToBuild();
			}
		}
	}
}

