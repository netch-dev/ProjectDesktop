using UnityEngine;

public class CropPlacementState : IBuildingState {
	private int selectedObjectIndex = -1;
	private int ID;
	private Grid grid;
	private PreviewSystem previewSystem;
	private ObjectDatabaseSO objectDatabaseSO;
	private GridData cropPositionData;
	private ObjectPlacer objectPlacer;

	public CropPlacementState(
		int ID,
		Grid grid,
		PreviewSystem previewSystem,
		ObjectDatabaseSO objectDatabaseSO,
		GridData cropPositionData,
		ObjectPlacer objectPlacer) {
		this.ID = ID;
		this.grid = grid;
		this.previewSystem = previewSystem;
		this.objectDatabaseSO = objectDatabaseSO;
		this.cropPositionData = cropPositionData;
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

		// Todo stop showing all the crop positions
	}

	public void OnAction(Vector3Int gridPosition) {
		bool canPlace = CanPlace(gridPosition, selectedObjectIndex);
		if (!canPlace) {
			// todo play a sound
			return;
		}

		int index = objectPlacer.PlaceObject(objectDatabaseSO.objectDataList[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

		cropPositionData.AddObjectAt(
			gridPosition,
			objectDatabaseSO.objectDataList[selectedObjectIndex].Size,
			objectDatabaseSO.objectDataList[selectedObjectIndex].ID,
			index);

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace: false); // Position is now invalid after placing here
	}

	private bool CanPlace(Vector3Int gridPosition, int selectedObjectIndex) {
		GridData selectedData = cropPositionData;

		return selectedData.CanPlaceObjectAt(gridPosition, objectDatabaseSO.objectDataList[selectedObjectIndex].Size);
	}

	public void UpdateState(Vector3Int gridPosition) {
		bool canPlace = CanPlace(gridPosition, selectedObjectIndex);

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace);
	}
}
