using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
	public static BuildingManager Instance;

	private Queue<BuildingScaffold> buildingQueue = new Queue<BuildingScaffold>();

	private void Awake() {
		Instance = this;

		BuildingScaffold.OnBuildingPlaced += BuildingScaffold_OnBuildingPlaced;
	}

	private void BuildingScaffold_OnBuildingPlaced(object sender, EventArgs e) {
		BuildingScaffold buildingScaffold = sender as BuildingScaffold;
		AddBuildingToQueue(buildingScaffold);
	}
	private void AddBuildingToQueue(BuildingScaffold building) {
		buildingQueue.Enqueue(building);
	}

	public bool HasBuildingWaitingToBeBuilt() {
		return buildingQueue.Count > 0;
	}

	public BuildingScaffold GetNextBuildingToBuild() {
		return buildingQueue.Dequeue();
	}
}