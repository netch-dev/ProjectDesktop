using System;
using UnityEngine;

// This disables all of the building functionality until the building is placed
public class BuildingScriptEnabler : MonoBehaviour {
	private void Awake() {
		foreach (MonoBehaviour script in GetComponents<MonoBehaviour>()) {
			if (script.GetType() == typeof(BuildingScriptEnabler)) continue;

			script.enabled = false;
		}
	}

	public void OnBuilt() {
		// Enable all of the scrips on the game object this script is attached to 
		foreach (MonoBehaviour script in GetComponents<MonoBehaviour>()) {
			script.enabled = true;
		}
	}
}