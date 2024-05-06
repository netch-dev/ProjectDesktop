using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState {
	private int gameObjectIndex = -1;
	private Grid grid;
	private PreviewSystem previewSystem;
	private BuildingGridData floorData;
	private BuildingGridData furnitureData;
	private BuildingPlacer objectPlacer;

	public RemovingState(Grid grid,
					  PreviewSystem previewSystem,
					  BuildingGridData floorData,
					  BuildingGridData furnitureData,
					  BuildingPlacer objectPlacer) {
		this.grid = grid;
		this.previewSystem = previewSystem;
		this.floorData = floorData;
		this.furnitureData = furnitureData;
		this.objectPlacer = objectPlacer;

		previewSystem.StartShowingRemovePreview();
	}

	public void EndState() {
		previewSystem.StopShowingPreview();
	}

	public void OnAction(Vector3Int gridPosition) {
		BuildingGridData selectedData = null;
		if (!furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one)) {
			selectedData = furnitureData;
		} else if (!floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)) {
			selectedData = floorData;
		}

		if (selectedData == null) {
			// sound
			return;
		} else {
			gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
			if (gameObjectIndex == -1) return;

			selectedData.RemoveObjectAt(gridPosition);
			objectPlacer.RemoveObjectAt(gameObjectIndex);
		}

		Vector3 cellPosition = grid.CellToWorld(gridPosition);
		previewSystem.UpdatePreviewPosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
	}

	private bool CheckIfSelectionIsValid(Vector3Int gridPosition) {
		return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
	}

	public void UpdateState(Vector3Int gridPosition) {
		bool canPlace = CheckIfSelectionIsValid(gridPosition);
		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace);
	}
}
