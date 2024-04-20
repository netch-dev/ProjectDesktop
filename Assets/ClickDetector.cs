using CodeMonkey;
using System;
using UnityEngine;

public class ClickDetector : MonoBehaviour {
	[SerializeField] private LayerMask clickableLayer;
	[SerializeField] private LayerMask buildableLayer;

	public static event Action<Vector3> OnRightClick;

	private void Update() {
		// Check for mouse click
		if (Input.GetMouseButtonDown(0)) {
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

		if (Input.GetMouseButtonDown(1)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableLayer)) {
				if (hit.collider != null) {
					GameObject clickedObject = hit.collider.gameObject;
					OnRightClick?.Invoke(hit.point);
				}
			} else {
				Debug.Log("No hit found, make sure to set the buildable layer");
			}
		}
	}
}