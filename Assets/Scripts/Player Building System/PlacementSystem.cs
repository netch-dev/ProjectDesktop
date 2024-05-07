using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlacementSystem : MonoBehaviour {
	public static Action OnStartCropPlacement;
	public static Action OnStopCropPlacement;

	[SerializeField] private InputManager inputManager;
	[SerializeField] private Grid grid;

	[SerializeField] private ObjectDatabaseSO buildingObjectDatabase;
	[SerializeField] private ObjectDatabaseSO cropObjectDatabase;

	[SerializeField] private GameObject gridVisualization;

	private BuildingGridData floorGridData, buildingGridData;
	private List<CropArea> cropAreas;

	[SerializeField] private Material WhiteMaterial;
	[SerializeField] private Material RedMaterial;

	[SerializeField] private PreviewSystem previewSystem;
	private Vector3Int lastDetectedPosition = Vector3Int.zero;

	[SerializeField] private BuildingPlacer buildingPlacer;

	private IBuildingState buildingState;

	private void Start() {
		StopPlacement();

		floorGridData = new();
		buildingGridData = new();

		cropAreas = new List<CropArea>();
		CropArea.OnCropAreaSpawned += (sender, e) => {
			Debug.Log("CropArea spawned");
			CropArea cropArea = sender as CropArea;
			cropAreas.Add(cropArea);
		};
		CropArea.OnCropAreaRemoved += (sender, e) => {
			CropArea cropArea = sender as CropArea;
			cropAreas.Remove(cropArea);
		};
	}

	#region Crops
	public void StartCropPlacement(int id) {
		StopPlacement();

		OnStartCropPlacement?.Invoke();

		buildingState = new CropPlacementState(id, grid, previewSystem, cropObjectDatabase, cropAreas);

		inputManager.OnClicked += PlaceObject;
		inputManager.OnExit += StopCropPlacement;
	}

	public void RemoveCropObject() {

	}
	#endregion

	#region Buildings
	public void StartBuildingPlacement(int id) {
		StopPlacement();
		gridVisualization.SetActive(true);

		buildingState = new BuildingPlacementState(id, grid, previewSystem, buildingObjectDatabase, floorGridData, buildingGridData, buildingPlacer);

		inputManager.OnClicked += PlaceObject;
		inputManager.OnExit += StopPlacement;
	}

	public void RemoveBuildingObject() {
		StopPlacement();
		gridVisualization.SetActive(true);

		buildingState = new RemovingState(grid, previewSystem, floorGridData, buildingGridData, buildingPlacer);

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
	private void StopCropPlacement() {
		StopPlacement();
		inputManager.OnExit -= StopCropPlacement;
		OnStopCropPlacement?.Invoke();
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
