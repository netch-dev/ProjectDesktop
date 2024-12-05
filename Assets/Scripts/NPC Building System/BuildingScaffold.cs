using System;
using UnityEngine;

public class BuildingScaffold : MonoBehaviour {
	public static event EventHandler OnBuildingPlaced;

	public event EventHandler<ProgressChangedEventArgs> OnProgressChanged;
	public class ProgressChangedEventArgs : EventArgs {
		public float progressNormalized { get; set; }
	}

	[SerializeField] private GameObject finalBuildingPrefab;
	[SerializeField] private float secondsToBuildMax;

	private bool isCurrentlyBuilding = false;
	private float secondsLeftToBuild;

	private void Start() {
		OnBuildingPlaced?.Invoke(this, EventArgs.Empty);
	}

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
			OnProgressChanged?.Invoke(this, new ProgressChangedEventArgs {
				progressNormalized = secondsLeftToBuild / secondsToBuildMax
			});
		} else {
			FinishedBuilding();
			return true;
		}

		return false;
	}

	private void StartBuilding() {
		secondsLeftToBuild = 0;
		isCurrentlyBuilding = true;
		OnProgressChanged?.Invoke(this, new ProgressChangedEventArgs { progressNormalized = 0 });
	}

	private void FinishedBuilding() {
		GameObject buildingGameObject = Instantiate(finalBuildingPrefab, transform.position, transform.rotation);

		// Replace the building scaffold with the final building
		BuildingPlacer.Instance.ReplaceBuilding(gameObject, buildingGameObject);

		Destroy(gameObject);
	}

	public bool IsBuilt() {
		return secondsLeftToBuild >= secondsToBuildMax;
	}
}
