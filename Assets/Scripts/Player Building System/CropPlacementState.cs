﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CropPlacementState : IBuildingState {
	private int selectedObjectIndex = -1;
	private int ID;
	private Grid grid;
	private PreviewSystem previewSystem;
	private ObjectDatabaseSO objectDatabaseSO;
	private List<CropArea> cropAreaList;

	public CropPlacementState(
		int ID,
		Grid grid,
		PreviewSystem previewSystem,
		ObjectDatabaseSO objectDatabaseSO,
		List<CropArea> cropAreaList) {
		this.ID = ID;
		this.grid = grid;
		this.previewSystem = previewSystem;
		this.objectDatabaseSO = objectDatabaseSO;
		this.cropAreaList = cropAreaList;

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
		bool canPlace = CanPlace(gridPosition);
		if (!canPlace) return;

		CropArea cropArea = GetCropAreaAtPosition(gridPosition);
		ObjectData objectData = objectDatabaseSO.objectDataList[selectedObjectIndex];

		cropArea.PlaceCrop(objectData.Prefab, grid.CellToWorld(gridPosition));

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace: false); // Position is now invalid after placing here
	}

	public void UpdateState(Vector3Int gridPosition) {
		bool canPlace = CanPlace(gridPosition);

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace);
	}
	private bool CanPlace(Vector3Int gridPosition) {
		CropArea cropArea = GetCropAreaAtPosition(gridPosition);
		if (cropArea == null) return false;

		return !cropArea.ContainsCropAtPosition(gridPosition);
	}
	private CropArea GetCropAreaAtPosition(Vector3Int position) {
		return cropAreaList.Find(x => x.ContainsPosition(position));
	}
}