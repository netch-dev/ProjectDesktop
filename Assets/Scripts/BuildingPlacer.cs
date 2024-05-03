using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour {
	[SerializeField] private List<GameObject> placedGameObjectsList = new();

	public int PlaceObject(GameObject prefab, Vector3 position) {
		GameObject newStructureObject = Instantiate(prefab);
		newStructureObject.transform.position = position;
		placedGameObjectsList.Add(newStructureObject);

		return placedGameObjectsList.Count - 1;
	}

	public void RemoveObjectAt(int gameObjectIndex) {
		if (placedGameObjectsList.Count <= gameObjectIndex || placedGameObjectsList[gameObjectIndex] == null) return;

		Destroy(placedGameObjectsList[gameObjectIndex]);
		placedGameObjectsList[gameObjectIndex] = null;
	}
}
