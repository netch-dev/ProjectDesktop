using UnityEngine;

public class BuildTask : ITask {
	private Animator animator;

	private BuildingScaffold currentBuilding = null;
	public BuildTask(Animator animator) {
		this.animator = animator;
	}

	public bool IsAvailable(NPC npc) {
		// Check the game handler for bulid that need to be built
		Debug.Log("Has current building ? " + currentBuilding != null);
		Debug.Log($"Has building manager instance? {BuildingManager.Instance != null}");
		return currentBuilding != null || BuildingManager.Instance.HasBuildingWaitingToBeBuilt();
	}

	public void ExecuteTask(NPC npc) {
		if (currentBuilding == null) {
			currentBuilding = BuildingManager.Instance.GetNextBuildingToBuild();
		}

		if (currentBuilding != null) {
			if (Vector3.Distance(npc.transform.position, currentBuilding.transform.position) > 3f) {
				npc.MoveTo(currentBuilding.transform.position);
			} else {
				// Start building
				currentBuilding.TryToBuild();
			}
		}
	}
}

