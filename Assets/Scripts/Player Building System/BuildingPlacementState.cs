using System;
using UnityEngine;

public class BuildingPlacementState : IBuildingState {
	private int selectedObjectIndex = -1;
	private int ID;
	private Grid grid;
	private PreviewSystem previewSystem;
	private ObjectDatabaseSO objectDatabaseSO;
	private BuildingGridData floorData;
	private BuildingGridData furnitureData;
	private BuildingPlacer objectPlacer;

	public BuildingPlacementState(int ID,
						Grid grid,
						PreviewSystem previewSystem,
						ObjectDatabaseSO objectDatabaseSO,
						BuildingGridData floorData,
						BuildingGridData furnitureData,
						BuildingPlacer objectPlacer) {

		this.ID = ID;
		this.grid = grid;
		this.previewSystem = previewSystem;
		this.objectDatabaseSO = objectDatabaseSO;
		this.floorData = floorData;
		this.furnitureData = furnitureData;
		this.objectPlacer = objectPlacer;

		selectedObjectIndex = objectDatabaseSO.objectDataList.FindIndex(x => x.ID == ID);
		if (selectedObjectIndex > -1) {
			previewSystem.StartShowingPlacementPreview(
				objectDatabaseSO.objectDataList[selectedObjectIndex].Prefab,
				objectDatabaseSO.objectDataList[selectedObjectIndex].Size);
		} else {
			throw new System.Exception($"Object ID not found in the database - {ID}");
		}
	}

	public void EndState() {
		previewSystem.StopShowingPreview();
	}

	public void OnAction(Vector3Int gridPosition) {
		bool canPlace = CanPlace(gridPosition, selectedObjectIndex);
		if (!canPlace) {
			// todo play a sound
			return;
		}

		int newObjectIndex = objectPlacer.PlaceObject(objectDatabaseSO.objectDataList[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

		// removed floor check because we arent using floors at this time
		BuildingGridData gridData = furnitureData;

		gridData.AddObjectAt(
			gridPosition,
			objectDatabaseSO.objectDataList[selectedObjectIndex].Size,
			objectDatabaseSO.objectDataList[selectedObjectIndex].ID,
			newObjectIndex);

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace: false); // Position is now invalid after placing here
	}

	private bool CanPlace(Vector3Int gridPosition, int selectedObjectIndex) {
		//bool isFloorObjectType = buildingObjectDatabase.objectDataList[selectedObjectIndex].ID == 0;
		BuildingGridData gridData = furnitureData;

		return gridData.CanPlaceObjectAt(gridPosition, objectDatabaseSO.objectDataList[selectedObjectIndex].Size);
	}

	public void UpdateState(Vector3Int gridPosition) {
		bool canPlace = CanPlace(gridPosition, selectedObjectIndex);

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace);
	}
}
