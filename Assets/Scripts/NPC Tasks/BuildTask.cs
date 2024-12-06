using System.Collections;
using UnityEngine;

public class BuildTask : ITask {
	private Animator animator;
	private BuildingScaffold currentBuilding = null;
	private bool isBuilding = false;

	public BuildTask(Animator animator) {
		this.animator = animator;
	}

	public bool IsAvailable() {
		// Task is available if there are buildings waiting to be built
		return currentBuilding != null || BuildingManager.Instance.HasBuildingWaitingToBeBuilt();
	}

	public bool IsComplete(NPC npc) {
		// Task is complete only if no buildings are left to build
		return currentBuilding == null && !BuildingManager.Instance.HasBuildingWaitingToBeBuilt();
	}

	public void OnStart(NPC npc) {
		Debug.Log("Starting BuildTask...");
	}

	public void ExecuteTask(NPC npc) {
		if (currentBuilding == null) {
			// Assign the next available building
			currentBuilding = BuildingManager.Instance.GetClosestBuildable(npc.transform.position);
		}

		if (currentBuilding != null) {
			if (Vector3.Distance(npc.transform.position, currentBuilding.transform.position) > 3f) {
				// Move closer to the building
				npc.MoveTo(currentBuilding.transform.position);
			} else {
				// Stop moving and build
				npc.StopMoving();
				BuildStructure(npc);
			}
		} else {
			Debug.Log("No building to build.");
		}
	}

	private void BuildStructure(NPC npc) {
		if (isBuilding) return;

		isBuilding = true;

		npc.StartCoroutine(BuildCoroutine(npc));
	}

	private IEnumerator BuildCoroutine(NPC npc) {
		while (!currentBuilding.TryToBuild(DeveloperCheats.GetCheat(DeveloperCheats.Cheat.InstantBuild))) {
			Debug.Log($"Building in progress: {currentBuilding.name}");
			yield return null;
		}

		Debug.Log($"Finished building: {currentBuilding.name}");
		currentBuilding = null;
		isBuilding = false;
	}

	public void OnFinish(NPC npc) {
		Debug.Log("Finished BuildTask.");
		currentBuilding = null;
		isBuilding = false;
	}
}
