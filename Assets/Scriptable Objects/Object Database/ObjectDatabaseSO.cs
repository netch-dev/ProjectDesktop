using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject {
	public List<ObjectData> objectDataList;
}

[Serializable]
public class ObjectData {
	public ObjectData(string name, int id, Vector2Int size, GameObject prefab, GameObject placementGhostPrefab) {
		this.Name = name;
		this.ID = id;
		this.Size = size;
		this.Prefab = prefab;
		this.PlacementGhostPrefab = placementGhostPrefab;
	}

	[field: SerializeField] public string Name { get; private set; }

	[field: SerializeField] public int ID { get; private set; }

	[field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;

	[field: SerializeField] public GameObject Prefab { get; private set; }

	[field: SerializeField] public GameObject PlacementGhostPrefab { get; private set; }
}
