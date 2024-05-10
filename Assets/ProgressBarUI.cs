using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
	[SerializeField] private BuildingScaffold buildingScaffold;
	[SerializeField] private Image barImage;

	private void Start() {
		barImage.fillAmount = 0;
		buildingScaffold.OnProgressChanged += BuildingScaffold_UpdateProgress;
	}

	private void BuildingScaffold_UpdateProgress(object sender, BuildingScaffold.ProgressChangedEventArgs e) {
		barImage.fillAmount = e.progressNormalized;
		Debug.Log("Progress: " + e.progressNormalized);
	}
}
