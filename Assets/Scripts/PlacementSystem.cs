using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour {
	[SerializeField] private GameObject mouseIndicator;
	[SerializeField] private InputManager inputManager;
	[SerializeField] private Grid grid;

	[SerializeField] private ObjectDatabaseSO objectDatabaseSO;
	private int selectedObjectIndex = -1;

	[SerializeField] private GameObject gridVisualization;

	private GridData floorData, furnitureData;
	[SerializeField] private Material WhiteMaterial;
	[SerializeField] private Material RedMaterial;

	private List<GameObject> placedGameObjectsList = new();

	[SerializeField] private PreviewSystem previewSystem;
	private Vector3Int lastDetectedPosition = Vector3Int.zero;

	private void Start() {
		StopPlacement();

		floorData = new();
		furnitureData = new();
	}

	public void StartPlacement(int id) {
		StopPlacement();
		// Passed the object id from the UI
		selectedObjectIndex = objectDatabaseSO.objectDataList.FindIndex(x => x.ID == id);
		if (selectedObjectIndex < 0) {
			Debug.LogError($"Object ID not found in the database - {id}");
			return;
		}

		gridVisualization.SetActive(true);
		previewSystem.StartShowingPlacementPreview(objectDatabaseSO.objectDataList[selectedObjectIndex].Prefab,
													objectDatabaseSO.objectDataList[selectedObjectIndex].Size);
		inputManager.OnClicked += PlaceStructure;
		inputManager.OnExit += StopPlacement;
		lastDetectedPosition = Vector3Int.zero;
	}

	private void PlaceStructure() {
		if (inputManager.IsPointerOverUI()) return;

		Vector3 mousePosition = inputManager.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		bool canPlace = CanPlace(gridPosition, selectedObjectIndex);
		if (!canPlace) {
			return;
		}

		GameObject newStructureObject = Instantiate(objectDatabaseSO.objectDataList[selectedObjectIndex].Prefab);
		newStructureObject.transform.position = grid.CellToWorld(gridPosition);
		placedGameObjectsList.Add(newStructureObject);

		bool isFloor = objectDatabaseSO.objectDataList[selectedObjectIndex].ID == 0;
		GridData selectedData = isFloor ? floorData : furnitureData;

		selectedData.AddObjectAt(gridPosition,
								objectDatabaseSO.objectDataList[selectedObjectIndex].Size,
								objectDatabaseSO.objectDataList[selectedObjectIndex].ID,
								placedGameObjectsList.Count - 1);

		previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace: false); // Position is now invalid after placing here
	}

	private bool CanPlace(Vector3Int gridPosition, int selectedObjectIndex) {
		bool isFloor = objectDatabaseSO.objectDataList[selectedObjectIndex].ID == 0;
		GridData selectedData = isFloor ? floorData : furnitureData;

		return selectedData.CanPlaceObjectAt(gridPosition, objectDatabaseSO.objectDataList[selectedObjectIndex].Size);
	}

	private void StopPlacement() {
		selectedObjectIndex = -1;
		gridVisualization.SetActive(false);
		previewSystem.StopShowingPreview();
		inputManager.OnClicked -= PlaceStructure;
		inputManager.OnExit -= StopPlacement;
	}

	private void Update() {
		if (selectedObjectIndex < 0) return; // Dont move anything without anything selected

		Vector3 mousePosition = inputManager.GetSelectedMapPosition();
		Vector3Int gridPosition = grid.WorldToCell(mousePosition);

		if (lastDetectedPosition != gridPosition) { // Only calculate when the last position has changed
			bool canPlace = CanPlace(gridPosition, selectedObjectIndex);

			mouseIndicator.transform.position = mousePosition;
			previewSystem.UpdatePreviewPosition(grid.CellToWorld(gridPosition), canPlace);

			lastDetectedPosition = gridPosition;
		}


	}
}
