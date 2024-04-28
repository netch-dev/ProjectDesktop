using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
	[SerializeField] private Camera sceneCamera;
	private Vector3 lastPosition;

	[SerializeField] private LayerMask clickableLayer;
	[SerializeField] private LayerMask buildableLayer;

	public event Action OnClicked, OnExit;

	private void Update() {
		// Check for mouse click
		if (Input.GetMouseButtonDown(0)) {
			OnClicked?.Invoke();

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer)) {
				if (hit.collider != null) {
					GameObject clickedObject = hit.collider.gameObject;

					ClickableObject clickableObject = clickedObject.GetComponentInParent<ClickableObject>();
					if (clickableObject != null) { // Trees get from the parent
						clickableObject.Clicked();
					} else { // players get from the object itself. should probably be changed to a parent or something idk
						clickableObject = clickedObject.GetComponent<ClickableObject>();
						if (clickableObject != null) {
							clickableObject.Clicked();
						}
					}
				}
			} else {
				Debug.Log("No hit found, make sure to set the clickable layer");
			}
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