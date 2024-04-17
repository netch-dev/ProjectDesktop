using System;
using UnityEngine;

public class ClickDetector : MonoBehaviour {
	public LayerMask clickableLayer; // Specify which layers should be clickable

	private void Update() {
		// Check for mouse click
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Mouse clicked");
			// Cast a ray from the mouse position into the scene
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer)) {

				if (hit.collider != null) {
					GameObject clickedObject = hit.collider.gameObject;

					ClickableObject clickableObject = clickedObject.GetComponentInParent<ClickableObject>();
					if (clickableObject != null) {
						clickableObject.Clicked();
					}
				}
			}
		}
	}
}