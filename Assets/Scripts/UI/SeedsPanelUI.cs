using TMPro;
using UnityEngine;

public class SeedsPanelUI : MonoBehaviour {
	[SerializeField] private GameObject contentPanel;
	[SerializeField] private GameObject seedEntryPrefab;
	[SerializeField] private ObjectDatabaseSO seedDatabase;
	[SerializeField] private PlacementSystem placementSystem;

	private void Start() {
		foreach (Transform child in contentPanel.transform) {
			child.gameObject.SetActive(false);
		}

		for (int i = 0; i < seedDatabase.objectDataList.Count; i++) {
			GameObject prefab = seedDatabase.objectDataList[i].Prefab;
			CropSO cropData = prefab.GetComponent<CropGrower>().cropData;

			GameObject uiEntry = Instantiate(seedEntryPrefab, contentPanel.transform);

			UnityEngine.UI.Image seedIcon = uiEntry.transform.Find("SeedIcon").GetComponent<UnityEngine.UI.Image>();
			if (seedIcon) seedIcon.sprite = cropData.cropIcon;
			else Debug.LogError("Seed icon not found in seed entry prefab");

			TextMeshProUGUI costText = uiEntry.transform.Find("CostTxt").GetComponent<TextMeshProUGUI>();
			if (costText) costText.SetText($"{cropData.seedPrice.ToString("N0")}");
			else Debug.LogError("Cost text not found in seed entry prefab");

			TextMeshProUGUI nameText = uiEntry.transform.Find("Panel")?.Find("NameTxt").GetComponent<TextMeshProUGUI>();
			if (nameText) nameText.SetText(cropData.cropName);
			else Debug.LogError("Name text not found in seed entry prefab");

			UnityEngine.UI.Button button = uiEntry.GetComponent<UnityEngine.UI.Button>();
			if (button) {
				int id = seedDatabase.objectDataList[i].ID;
				button.onClick.AddListener(() => {
					placementSystem.StartCropPlacement(id);
				});
				Debug.Log($"Added button listener for {cropData.cropName} | #{seedDatabase.objectDataList[i].ID}");
			} else {
				Debug.LogError("Button not found in seed entry prefab");
			}
		}
	}
}
