using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GridData {
	private Dictionary<Vector3Int, PlacementData> placedObjects = new();


	public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex) {
		List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
		PlacementData data = new PlacementData(ID, placedObjectIndex);

		foreach (Vector3Int position in positionToOccupy) {
			if (placedObjects.ContainsKey(position)) {
				throw new Exception($"Dictionary already contains this cell position {position}");
			}

			placedObjects[position] = data;
		}
	}

	private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize) {
		List<Vector3Int> returnValues = new();
		// Always assuming we're placing objects from the bottom left corner
		for (int x = 0; x < objectSize.x; x++) {

			for (int y = 0; y < objectSize.y; y++) {
				returnValues.Add(gridPosition + new Vector3Int(x, 0, y));
			}
		}

		return returnValues;
	}

	public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize) {
		List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

		foreach (Vector3Int position in positionToOccupy) {
			if (placedObjects.ContainsKey(position)) {
				return false;
			}
		}

		return true;
	}
}

public class PlacementData {
	public PlacementData(int id, int placedObjectIndex) {
		ID = id;
		PlacedObjectIndex = placedObjectIndex;
	}

	public List<Vector3Int> occupiedPositions;
	public int ID { get; private set; }
	public int PlacedObjectIndex { get; private set; }
}