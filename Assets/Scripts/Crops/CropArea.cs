using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropArea : MonoBehaviour {
	public static event EventHandler OnCropAreaSpawned;
	public static event EventHandler OnCropAreaRemoved;

	[SerializeField] private Vector2Int size;
	[SerializeField] private int gridSize = 1;
	[SerializeField] private GameObject testVisualObjectPrefab;

	private List<Vector3> cropSlots = new List<Vector3>();

	private Dictionary<Vector3, CropGrower> plantedCrops = new Dictionary<Vector3, CropGrower>();

	private List<GameObject> visualGameObjects = new List<GameObject>();

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

		/*		foreach (Vector3 cropSlot in cropSlots) {
					GameObject visual = Instantiate(testVisualObjectPrefab, cropSlot + Vector3.up, Quaternion.identity);
					visual.transform.SetParent(transform);
					visual.SetActive(false);
					visualGameObjects.Add(visual);
				}*/
	}

	private void PlacementSystemOnStopCropPlacement() {
		foreach (GameObject visualObject in visualGameObjects) {
			if (visualObject == null) continue;
			visualObject.SetActive(false);
		}
	}
	private void PlacementSystemOnStartCropPlacement() {
		foreach (GameObject visualObject in visualGameObjects) {
			if (visualObject == null) continue;
			visualObject.SetActive(true);
		}
	}

	public bool ContainsPosition(Vector3 position) {
		return cropSlots.Contains(position);
	}

	public bool ContainsCropAtPosition(Vector3 position) {
		return plantedCrops.ContainsKey(position);
	}

	public void PlaceCrop(GameObject cropPrefab, Vector3 position) {
		GameObject newCrop = Instantiate(cropPrefab, position, Quaternion.identity);
		CropGrower cropGrower = newCrop.GetComponent<CropGrower>();
		cropGrower.InitCrop();
		plantedCrops.Add(position, cropGrower);

		cropGrower.OnCropHarvested += () => {
			plantedCrops.Remove(position);
		};

		Debug.Log($"Planted crop at {position} ({plantedCrops.Count} total)");
	}
}
