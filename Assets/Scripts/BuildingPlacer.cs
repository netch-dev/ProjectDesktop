using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour {
	public static BuildingPlacer Instance;

	private void Awake() {
		if (Instance != null) Debug.LogError("More than one BuildingPlacer in the scene");
		Instance = this;
	}

	[SerializeField] private List<GameObject> placedGameObjectsList = new();

	public int PlaceObject(GameObject prefab, Vector3 position) {
		GameObject newStructureObject = Instantiate(prefab, position, Quaternion.identity);

		// Get the building object to enable all of the scripts after its placed down
		BuildingScriptEnabler building = newStructureObject.GetComponent<BuildingScriptEnabler>();
		if (building) building.OnBuilt();

		placedGameObjectsList.Add(newStructureObject);

		return placedGameObjectsList.Count - 1;
	}

	public void RemoveObjectAt(int gameObjectIndex) {
		if (placedGameObjectsList.Count <= gameObjectIndex || placedGameObjectsList[gameObjectIndex] == null) return;

		Destroy(placedGameObjectsList[gameObjectIndex]);
		placedGameObjectsList[gameObjectIndex] = null;
	}

	public void ReplaceBuilding(GameObject oldBuilding, GameObject newBuilding) {
		int index = placedGameObjectsList.IndexOf(oldBuilding);
		if (index == -1) return;

		placedGameObjectsList[index] = newBuilding;
		Debug.Log("BuildingScriptEnabler replaced");
	}
}
