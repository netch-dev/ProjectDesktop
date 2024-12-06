using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropArea : MonoBehaviour {
	public static event EventHandler OnCropAreaSpawned;
	public static event EventHandler OnCropAreaRemoved;

	[SerializeField] private Vector2Int size;
	[SerializeField] private int gridSize = 1;
	[SerializeField] private Vector3 plantPositionOffset;

	private List<Vector3> cropSlots = new List<Vector3>();

	private Dictionary<Vector3, CropGrower> plantedCrops = new Dictionary<Vector3, CropGrower>();

	private bool inUseFlag = false;

	private void Awake() {
		PlacementSystem.OnStartCropPlacement += PlacementSystemOnStartCropPlacement;
		PlacementSystem.OnStopCropPlacement += PlacementSystemOnStopCropPlacement;
	}

	private void Start() {
		OnCropAreaSpawned?.Invoke(this, EventArgs.Empty);

		// Generate the crop slots using the xSize and zSize and the position of the grid area
		for (int x = 0; x < size.x; x++) {
			for (int z = 0; z < size.y; z++) {
				Vector3 cropSlot = new Vector3(transform.position.x + (gridSize * x), transform.position.y, transform.position.z + (gridSize * z));
				cropSlots.Add(cropSlot);
				Debug.Log($"Crop slot at {cropSlot}");
			}
		}
	}

	private void PlacementSystemOnStopCropPlacement() {
		// Toggle visual objects
	}
	private void PlacementSystemOnStartCropPlacement() {
		// Toggle visual objects
	}

	public bool ContainsPosition(Vector3 position) {
		return cropSlots.Contains(position);
	}

	public bool ContainsCropAtPosition(Vector3 position) {
		return plantedCrops.ContainsKey(position);
	}

	public void PlaceCrop(GameObject cropPrefab, Vector3 position) {
		GameObject newCrop = Instantiate(cropPrefab, position + plantPositionOffset, Quaternion.identity);
		CropGrower cropGrower = newCrop.GetComponent<CropGrower>();
		cropGrower.InitCrop(this);
		plantedCrops.Add(position, cropGrower);

		cropGrower.OnCropHarvested += () => {
			plantedCrops.Remove(position);
		};

		// todo subscribe to the water action here after moving the water levels to the area

		Debug.Log($"Planted crop at {position} ({plantedCrops.Count} total)");
	}

	public void WaterCropArea() {
		foreach (KeyValuePair<Vector3, CropGrower> crop in plantedCrops) {
			if (crop.Value.CanWaterCrop()) {
				crop.Value.WaterCrop();
			}
		}
	}

	public Vector3 GetCropPositionOffset() {
		return plantPositionOffset;
	}

	public void SetInUseFlag(bool value) {
		inUseFlag = value;
	}

	public bool IsInUse() {
		return inUseFlag;
	}
}
