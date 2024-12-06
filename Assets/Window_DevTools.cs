using UnityEngine;
using UnityEngine.UI;

public class Window_DevTools : MonoBehaviour {
	[SerializeField] private GameObject mainContainer;
	[SerializeField] private Toggle instantBuildToggle;
	[SerializeField] private Toggle instantWaterToggle;
	[SerializeField] private Toggle instantHarvestToggle;

	private void Start() {
		instantBuildToggle.isOn = DeveloperCheats.GetCheat(DeveloperCheats.Cheat.InstantBuild);
		instantBuildToggle.onValueChanged.AddListener((bool value) => {
			DeveloperCheats.ToggleCheat(DeveloperCheats.Cheat.InstantBuild);
		});

		instantWaterToggle.isOn = DeveloperCheats.GetCheat(DeveloperCheats.Cheat.InstantWater);
		instantWaterToggle.onValueChanged.AddListener((bool value) => {
			DeveloperCheats.ToggleCheat(DeveloperCheats.Cheat.InstantWater);
		});

		instantHarvestToggle.isOn = DeveloperCheats.GetCheat(DeveloperCheats.Cheat.InstantHarvest);
		instantHarvestToggle.onValueChanged.AddListener((bool value) => {
			DeveloperCheats.ToggleCheat(DeveloperCheats.Cheat.InstantHarvest);
		});
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F5)) {
			mainContainer.SetActive(!mainContainer.activeSelf);
		}
	}
}
