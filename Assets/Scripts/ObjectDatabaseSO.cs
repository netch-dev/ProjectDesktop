using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject {
	public List<ObjectData> objectDataList;
}

[Serializable]
public class ObjectData {
	[field: SerializeField]
	public string Name { get; private set; }

	[field: SerializeField]
	public int ID { get; private set; }

	[field: SerializeField]
	public Vector2Int Size { get; private set; } = Vector2Int.one;

	// This is a reference to the object that will be spawned in the world
	[field: SerializeField]
	public GameObject Prefab { get; private set; }

	// This is a reference to the ghost object that will be shown when the player is placing the object
	[field: SerializeField]
	public GameObject PlacementGhostPrefab { get; private set; }
}
