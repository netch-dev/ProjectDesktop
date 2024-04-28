using UnityEngine;

public class PreviewSystem : MonoBehaviour {
	[SerializeField] private float previewYOffset = 0.06f;

	[SerializeField] private GameObject cellIndicator;
	private GameObject previewObject;

	[SerializeField] private Material previewMaterialPrefab;
	private Material previewMaterialInstance;

	private Renderer cellIndicatorRenderer;

	private void Start() {
		previewMaterialInstance = new Material(previewMaterialPrefab);
		cellIndicator.SetActive(false);
		cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
	}

	public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size) {
		previewObject = Instantiate(prefab);
		PreparePreview(previewObject);
		PrepareCursor(size);
		cellIndicator.SetActive(true);
	}

	private void PrepareCursor(Vector2Int size) {
		if (size.x > 0 && size.y > 0) {
			cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
			cellIndicatorRenderer.material.mainTextureScale = size;
		}
	}

	private void PreparePreview(GameObject previewObject) {
		Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			Material[] materials = renderer.materials;
			for (int i = 0; i < materials.Length; i++) {
				materials[i] = previewMaterialInstance;
			}

			renderer.materials = materials;
		}
	}

	public void StopShowingPreview() {
		cellIndicator.SetActive(false);
		if (previewObject != null) Destroy(previewObject);
	}

	public void UpdatePreviewPosition(Vector3 position, bool canPlace) {
		if (previewObject != null) {
			MovePreview(position);
			ApplyFeedbackToPreview(canPlace);
		}
		MoveCursor(position);
		ApplyFeedbackToCursor(canPlace);
	}

	private void ApplyFeedbackToPreview(bool canPlace) {
		Color color = canPlace ? Color.white : Color.red;
		color.a = 0.5f;
		previewMaterialInstance.color = color;
	}

	private void ApplyFeedbackToCursor(bool canPlace) {
		Color color = canPlace ? Color.white : Color.red;
		color.a = 0.5f;
		cellIndicatorRenderer.material.color = color;
	}

	private void MoveCursor(Vector3 position) {
		cellIndicator.transform.position = position;
	}

	private void MovePreview(Vector3 position) {
		previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
	}

	public void StartShowingRemovePreview() {
		cellIndicator.SetActive(true);
		PrepareCursor(Vector2Int.one);
		ApplyFeedbackToCursor(false);
	}
}
