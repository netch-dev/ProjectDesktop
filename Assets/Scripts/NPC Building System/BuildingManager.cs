using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
	public static BuildingManager Instance;

	private List<BuildingScaffold> pendingBuildingList = new List<BuildingScaffold>();

	private void Awake() {
		if (Instance != null) Debug.LogError("There are multiple BuildingManager scripts in the scene");
		Instance = this;

		BuildingScaffold.OnBuildingPlaced += BuildingScaffold_OnBuildingPlaced;
	}

	private void BuildingScaffold_OnBuildingPlaced(object sender, EventArgs e) {
		BuildingScaffold buildingScaffold = sender as BuildingScaffold;
		pendingBuildingList.Add(buildingScaffold);
	}

	public bool HasBuildingWaitingToBeBuilt() {
		return pendingBuildingList.Count > 0;
	}

	public BuildingScaffold GetClosestBuildable(Vector3 currentPosition) {
		BuildingScaffold closestBuilding = null;
		float closestDistance = Mathf.Infinity;

		foreach (BuildingScaffold building in pendingBuildingList) {
			float distance = Vector3.Distance(currentPosition, building.transform.position);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestBuilding = building;
			}
		}

		pendingBuildingList.Remove(closestBuilding);
		return closestBuilding;
	}
}