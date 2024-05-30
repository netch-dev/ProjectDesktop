using System;
using UnityEngine;

// Add this script to new buildins that are placed in the scene by the player
// It will automatically add itself to the building queue in the BuildingManager
// Then an NPC can pick it up in its task system
public class BuildingScaffold : MonoBehaviour {
	public static event EventHandler OnBuildingPlaced;

	public event EventHandler<ProgressChangedEventArgs> OnProgressChanged;
	public class ProgressChangedEventArgs : EventArgs {
		public float progressNormalized { get; set; }
	}

	[SerializeField] private GameObject finalBuildingPrefab;
	[SerializeField] private float secondsToBuildMax;

	private bool isCurrentlyBuilding;
	private float secondsLeftToBuild;

	private void Start() {
		OnBuildingPlaced?.Invoke(this, EventArgs.Empty);
	}

	// Returns true if the building is finished building
	public bool TryToBuild(bool instantBuild) {
		if (instantBuild) {
			FinishedBuilding();
			return true;
		}

		if (!isCurrentlyBuilding) {
			StartBuilding();
		}

		if (secondsLeftToBuild < secondsToBuildMax) {
			secondsLeftToBuild += Time.deltaTime;
			OnProgressChanged?.Invoke(this, new ProgressChangedEventArgs { progressNormalized = secondsLeftToBuild / secondsToBuildMax });
		} else {
			FinishedBuilding();
			return true;
		}

		return false;
	}

	private void StartBuilding() {
		secondsLeftToBuild = 0;
		OnProgressChanged?.Invoke(this, new ProgressChangedEventArgs { progressNormalized = 0 });
		isCurrentlyBuilding = true;
	}

	private void FinishedBuilding() {
		GameObject buildingGameObject = Instantiate(finalBuildingPrefab, transform.position, transform.rotation);

		// replace the building so it can be removed or moved later
		BuildingPlacer.Instance.ReplaceBuilding(gameObject, buildingGameObject);

		Destroy(gameObject);
	}
}