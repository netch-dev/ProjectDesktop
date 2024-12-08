using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CropSO))]
public class CreateCropPrefabs_Editor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		CropSO cropData = (CropSO)target;

		if (GUILayout.Button("Create Prefabs and Update Crop Database")) {
			CreatePrefabs(cropData);
		}
	}

	private void CreatePrefabs(CropSO cropData) {
		if (cropData.cropPrefabVisuals == null) {
			Debug.LogError("Crop Prefab is not assigned in the CropData.");
			return;
		}

		string folderPath = "Assets/Prefabs/Crops";
		if (!AssetDatabase.IsValidFolder(folderPath)) {
			AssetDatabase.CreateFolder("Assets/Prefabs", "Crops");
		}

		GameObject cropPrefab = CreateCropGrowerVersionPrefab(cropData);
		GameObject transparentPrefab = CreateTransparentVersionPrefab(cropData);

		// Update the crop database to include the new prefabs
		ObjectDatabaseSO objectDatabase = AssetDatabase.LoadAssetAtPath<ObjectDatabaseSO>("Assets/Scriptable Objects/Object Database/Crop Object Database.asset");
		if (objectDatabase == null) {
			Debug.LogError("Object Database not found at Assets/Scriptable Objects/Object Database/Crop Object Database.asset");
			return;
		}

		int previousID = -1;
		for (int i = 0; i < objectDatabase.objectDataList.Count; i++) {
			ObjectData objectData = objectDatabase.objectDataList[i];
			if (objectData.Name == cropData.name) {
				previousID = objectData.ID;
				objectDatabase.objectDataList.RemoveAt(i);
				break;
			}
		}

		int cropID = previousID == -1 ? objectDatabase.objectDataList.Count : previousID;
		ObjectData cropObjectData = new ObjectData(cropData.name, cropID, new Vector2Int(1, 1), cropPrefab, transparentPrefab);
		objectDatabase.objectDataList.Add(cropObjectData);

		objectDatabase.objectDataList = objectDatabase.objectDataList.OrderBy(x => x.ID).ToList();

		AssetDatabase.Refresh();
	}

	private GameObject CreateCropGrowerVersionPrefab(CropSO cropData) {
		GameObject parentGameObject = new GameObject($"{cropData.cropName}_Seed");

		GameObject modelOffset = new GameObject("Model Offset");
		modelOffset.transform.SetParent(parentGameObject.transform);
		modelOffset.transform.localPosition = new Vector3(0.5f, 0.0f, 0.5f);

		List<GameObject> cropVisuals = new List<GameObject>();
		foreach (GameObject item in cropData.cropPrefabVisuals) {
			GameObject cropVisual = PrefabUtility.InstantiatePrefab(item) as GameObject;
			cropVisual.transform.SetParent(modelOffset.transform);
			cropVisual.transform.localPosition = Vector3.zero;
			cropVisuals.Add(cropVisual);
		}

		for (int i = 0; i < cropVisuals.Count; i++) {
			if (i == 0) continue;
			cropVisuals[i].gameObject.SetActive(false);
		}

		GameObject waterIconCanvas = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/Seed_WorldSpaceCanvas.prefab")) as GameObject;
		waterIconCanvas.transform.SetParent(parentGameObject.transform);

		CropGrower cropGrower = parentGameObject.AddComponent<CropGrower>();

		cropGrower.cropStages = cropVisuals.ToArray();
		cropGrower.cropData = cropData;
		cropGrower.waterImage = waterIconCanvas.GetComponentInChildren<UnityEngine.UI.Image>();
		cropGrower.waterImage.gameObject.SetActive(false);

		string folderPath = "Assets/Prefabs/Crops";
		string prefabPath = $"{folderPath}/{parentGameObject.name}";
		if (AssetDatabase.LoadAssetAtPath<GameObject>($"{prefabPath}.prefab") != null) {
			Debug.LogWarning($"Prefab already exists at {prefabPath}.prefab");
			AssetDatabase.DeleteAsset($"{prefabPath}.prefab. Replaced it");
		}

		GameObject prefab = PrefabUtility.SaveAsPrefabAsset(parentGameObject, $"{prefabPath}.prefab");
		DestroyImmediate(parentGameObject);
		return prefab;
	}

	private GameObject CreateTransparentVersionPrefab(CropSO cropData) {
		Material transparentMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Shaders/TransparentMaterial.mat");
		if (transparentMaterial == null) {
			Debug.LogError("Transparent Material not found at Assets/Shaders/TransparentMaterial.mat");
			return null;
		}

		GameObject parentGameObject = new GameObject($"Placement_Ghost_{cropData.cropName}_Seed");

		GameObject modelOffset = new GameObject("Model Offset");
		modelOffset.transform.SetParent(parentGameObject.transform);
		modelOffset.transform.localPosition = new Vector3(0.5f, 0.0f, 0.5f);

		GameObject visual = cropData.cropPrefabVisuals[0];
		GameObject cropVisual = PrefabUtility.InstantiatePrefab(visual) as GameObject;
		cropVisual.transform.SetParent(modelOffset.transform);
		cropVisual.transform.localPosition = Vector3.zero;

		Renderer renderer = cropVisual.GetComponentInChildren<Renderer>();
		renderer.material = transparentMaterial;
		renderer.sharedMaterial = transparentMaterial;

		string folderPath = "Assets/Prefabs/Crops";
		string prefabPath = $"{folderPath}/{parentGameObject.name}";
		if (AssetDatabase.LoadAssetAtPath<GameObject>($"{prefabPath}.prefab") != null) {
			Debug.LogWarning($"Prefab already exists at {prefabPath}.prefab. Replaced it");
			AssetDatabase.DeleteAsset($"{prefabPath}.prefab");
		}

		GameObject prefab = PrefabUtility.SaveAsPrefabAsset(parentGameObject, $"{prefabPath}.prefab");
		DestroyImmediate(parentGameObject);
		return prefab;
	}
}
