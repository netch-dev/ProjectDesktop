using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour {
	[SerializeField] private InputManager inputManager;
	[SerializeField] private Grid grid;

	[SerializeField] private ObjectDatabaseSO objectDatabaseSO;

	[SerializeField] private GameObject gridVisualization;

	private GridData floorData, furnitureData;
	[SerializeField] private Material WhiteMaterial;
	[SerializeField] private Material RedMaterial;

	[SerializeField] private PreviewSystem previewSystem;
	private Vector3Int lastDetectedPosition = Vector3Int.zero;

	[SerializeField] private ObjectPlacer objectPlacer;

	private IBuildingState buildingState;

	private void Start() {
		StopPlacement();

		floorData = new();
		furnitureData = new();
	}

	public void StartPlacement(int id) {
		StopPlacement();
		gridVisualization.SetActive(true);

		buildingState = new PlacementState(id, grid, previewSystem, objectDatabaseSO, floorData, furnitureData, objectPlacer);

		inputManager.OnClicked += PlaceStructure;
		inputManager.OnExit += StopPlacement;
	}

	public void StartRemoving() {
		StopPlacement();
		gridVisualization.SetActive(true);
		buildingState = new RemovingState(grid, previewSystem, floorData, furnitureData, objectPlacer);

		inputManager.OnClicked += PlaceStructure;
		inputManager.OnExit += StopPlacement;
	}

	private void PlaceStructure() {
		if (inputManager.IsPointerOverUI()) return;

		Vector3 mousePosition = inputManager.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		buildingState?.OnAction(gridPosition);
	}

	private void StopPlacement() {
		if (buildingState == null) return;

		gridVisualization.SetActive(false);
		buildingState.EndState();
		inputManager.OnClicked -= PlaceStructure;
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
