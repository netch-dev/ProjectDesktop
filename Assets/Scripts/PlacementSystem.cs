using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour {
	[SerializeField] private InputManager inputManager;
	[SerializeField] private Grid grid;

	[SerializeField] private ObjectDatabaseSO buildingObjectDatabase;
	[SerializeField] private ObjectDatabaseSO cropObjectDatabase;

	[SerializeField] private GameObject gridVisualization;

	private GridData floorData, buildingData, cropData;
	[SerializeField] private Material WhiteMaterial;
	[SerializeField] private Material RedMaterial;

	[SerializeField] private PreviewSystem previewSystem;
	private Vector3Int lastDetectedPosition = Vector3Int.zero;

	[SerializeField] private ObjectPlacer objectPlacer;

	private IBuildingState buildingState;

	private void Start() {
		StopPlacement();

		floorData = new();
		buildingData = new();
		cropData = new();
	}

	#region Crops
	public void StartCropPlacement(int id) {
		StopPlacement();
		gridVisualization.SetActive(true);
		// Instead of enabling the entire grid, only enable the grid for the available crop positions
		// The best way to do this would be to have a separate grid for crops
		// Or to have a list of available positions for crops and only enable those cells

		buildingState = new CropPlacementState(id, grid, previewSystem, cropObjectDatabase, cropData, objectPlacer);

		inputManager.OnClicked += PlaceObject;
		inputManager.OnExit += StopPlacement;
	}

	public void RemoveCropObject() {

	}
	#endregion

	#region Buildings
	public void StartBuildingPlacement(int id) {
		StopPlacement();
		gridVisualization.SetActive(true);

		buildingState = new BuildingPlacementState(id, grid, previewSystem, buildingObjectDatabase, floorData, buildingData, objectPlacer);

		inputManager.OnClicked += PlaceObject;
		inputManager.OnExit += StopPlacement;
	}

	public void RemoveBuildingObject() {
		StopPlacement();
		gridVisualization.SetActive(true);

		buildingState = new RemovingState(grid, previewSystem, floorData, buildingData, objectPlacer);

		inputManager.OnClicked += PlaceObject;
		inputManager.OnExit += StopPlacement;
	}
	#endregion

	private void PlaceObject() {
		if (inputManager.IsPointerOverUI()) return;

		Vector3 mousePosition = inputManager.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		buildingState?.OnAction(gridPosition);
	}

	private void StopPlacement() {
		if (buildingState == null) return;

		gridVisualization.SetActive(false);
		buildingState.EndState();
		inputManager.OnClicked -= PlaceObject;
		inputManager.OnExit -= StopPlacement;
		buildingState = null;
	}

	private void Update() {
		if (buildingState == null) return;

		Vector3 mousePosition = inputManager.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		if (lastDetectedPosition != gridPosition) { // Only calculate when the last position has changed
			buildingState.UpdateState(gridPosition);

			lastDetectedPosition = gridPosition;
		}
	}
}
