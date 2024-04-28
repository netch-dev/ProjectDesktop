using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adding objects and removing them
public class ObjectPlacer : MonoBehaviour {
	[SerializeField] private List<GameObject> placedGameObjectsList = new();

	public int PlaceObject(GameObject prefab, Vector3 position) {
		GameObject newStructureObject = Instantiate(prefab);
		newStructureObject.transform.position = position;
		placedGameObjectsList.Add(newStructureObject);

		return placedGameObjectsList.Count - 1;
	}
}
