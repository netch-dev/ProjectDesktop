using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGridData {
	private Dictionary<Vector3Int, PlacementData> placedObjects = new();

	public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex) {
		List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
		PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, gridPosition, objectSize);

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

	private List<Vector3Int> CalculateOuterPositions(Vector3Int gridPosition, Vector2Int objectSize) {
		List<Vector3Int> outerPositions = new();

		// Bottom row (excluding corners)
		for (int x = -1; x <= objectSize.x; x++) {
			outerPositions.Add(gridPosition + new Vector3Int(x, 0, -1));
		}

		// Top row (excluding corners)
		for (int x = -1; x <= objectSize.x; x++) {
			outerPositions.Add(gridPosition + new Vector3Int(x, 0, objectSize.y));
		}

		// Left column (excluding corners)
		for (int y = 0; y < objectSize.y; y++) {
			outerPositions.Add(gridPosition + new Vector3Int(-1, 0, y));
		}

		// Right column (excluding corners)
		for (int y = 0; y < objectSize.y; y++) {
			outerPositions.Add(gridPosition + new Vector3Int(objectSize.x, 0, y));
		}

		// debug draw line for 1.5f
		foreach (Vector3Int position in outerPositions) {
			if (placedObjects.ContainsKey(position)) Debug.DrawLine(position, position + (Vector3.up * 1.5f), Color.red, 1.5f);
			else Debug.DrawLine(position, position + (Vector3.up * 1.5f), Color.green, 1.5f);
		}

		return outerPositions;
	}

	public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize) {
		List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

		foreach (Vector3Int position in positionToOccupy) {
			if (placedObjects.ContainsKey(position)) {
				return false;
			}
		}

		List<Vector3Int> outerPositions = CalculateOuterPositions(gridPosition, objectSize);
		foreach (Vector3Int outerPosition in outerPositions) {
			// now we need to check if the outer positions contain an object
			// if they dont contain an object, generate the outer positions for that object, and make sure that object has an open spot for pathing
			if (!placedObjects.ContainsKey(outerPosition)) continue;

			PlacementData outerObject = placedObjects[outerPosition];
			List<Vector3Int> outerObjectOuterPositions = CalculateOuterPositions(outerObject.gridOriginPosition, outerObject.objectSize);

			// Now we need to make sure the outer object has an open spot for pathing
			bool hasSpot = false;
			foreach (Vector3Int outerObjectOuterPosition in outerObjectOuterPositions) {
				if (!placedObjects.ContainsKey(outerObjectOuterPosition) && !positionToOccupy.Contains(outerObjectOuterPosition)) {
					hasSpot = true;
					break;
				}
			}

			if (!hasSpot) return false;
		}

		return true;
	}

	public int GetRepresentationIndex(Vector3Int gridPosition) {
		if (!placedObjects.ContainsKey(gridPosition)) {
			return -1;
		} else {
			return placedObjects[gridPosition].PlacedObjectIndex;
		}
	}

	public void RemoveObjectAt(Vector3Int gridPosition) {
		foreach (Vector3Int position in placedObjects[gridPosition].occupiedPositions) {
			placedObjects.Remove(position);
		}
	}
}

public class PlacementData {
	public PlacementData(List<Vector3Int> occupiedPositions, int id, int placedObjectIndex, Vector3Int gridOriginPosition, Vector2Int objectSize) {
		this.occupiedPositions = occupiedPositions;
		this.ID = id;
		this.PlacedObjectIndex = placedObjectIndex;
		this.gridOriginPosition = gridOriginPosition;
		this.objectSize = objectSize;

	}

	public List<Vector3Int> occupiedPositions;
	public int ID { get; private set; }
	public int PlacedObjectIndex { get; private set; }

	public Vector3Int gridOriginPosition { get; private set; }
	public Vector2Int objectSize { get; private set; }
}
