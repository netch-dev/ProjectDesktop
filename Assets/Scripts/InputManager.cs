using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
	[SerializeField] private Camera sceneCamera;
	private Vector3 lastPosition;

	[SerializeField] private LayerMask buildableLayer;

	public event Action OnClicked, OnExit;

	private void Update() {
		// Check for mouse click
		if (Input.GetMouseButton(0)) {
			Debug.Log("Mouse clicked");
			OnClicked?.Invoke();
		}

		if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) {
			OnExit?.Invoke();
		}
	}

	public bool IsPointerOverUI() {
		return EventSystem.current.IsPointerOverGameObject();
	}

	public Vector3 GetSelectedMapPosition() {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = sceneCamera.nearClipPlane;
		Ray ray = sceneCamera.ScreenPointToRay(mousePos);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f, buildableLayer)) {
			lastPosition = hit.point;
		}

		return lastPosition;
	}
}