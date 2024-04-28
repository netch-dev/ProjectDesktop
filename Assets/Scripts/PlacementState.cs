using UnityEngine;

public class PlacementState : IBuildingState {

	private int selectedObjectIndex = -1;
	private int ID;
	private Grid grid;
	private PreviewSystem previewSystem;
	private ObjectDatabaseSO objectDatabaseSO;
	private GridData floorData;
	private GridData furnitureData;
	private ObjectPlacer objectPlacer;

	public PlacementState(int iD,
					   Grid grid,
					   PreviewSystem previewSystem,
					   ObjectDatabaseSO objectDatabaseSO,
					   GridData floorData,
					   GridData furnitureData,
					   ObjectPlacer objectPlacer) {
		ID = iD;
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

		int index = objectPlacer.PlaceObject(objectDatabaseSO.objectDataList[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

		bool isFloor = objectDatabaseSO.objectDataList[selectedObjectIndex].ID == 0;
		GridData selectedData = isFloor ? floorData : furnitureData;

		selectedData.AddObjectAt(
			gridPosition,
			objectDatabaseSO.objectDataList[selectedObjectIndex].Size,
			objectDatabaseSO.objectDataList[selectedObjectIndex].ID,
			index);

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace: false); // Position is now invalid after placing here
	}

	private bool CanPlace(Vector3Int gridPosition, int selectedObjectIndex) {
		bool isFloorObjectType = objectDatabaseSO.objectDataList[selectedObjectIndex].ID == 0;
		GridData selectedData = isFloorObjectType ? floorData : furnitureData;

		return selectedData.CanPlaceObjectAt(gridPosition, objectDatabaseSO.objectDataList[selectedObjectIndex].Size);
	}

	public void UpdateState(Vector3Int gridPosition) {
		bool canPlace = CanPlace(gridPosition, selectedObjectIndex);

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace);
	}
}
