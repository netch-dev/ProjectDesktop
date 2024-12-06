using System;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
	public static GameHandler Instance;

	[SerializeField] private Transform[] waterNodeTransformArray;
	[SerializeField] private Transform storageTransform;

	private void Awake() {
		Instance = this;
		GameResources.Init();
	}

	public Transform GetClosestWaterNode(Vector3 position) {
		Transform closestWaterNode = null;
		float closestDistance = Mathf.Infinity;

		foreach (Transform waterNodeTransform in waterNodeTransformArray) {
			float distance = Vector3.Distance(position, waterNodeTransform.position);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestWaterNode = waterNodeTransform;
			}
		}

		return closestWaterNode;
	}
}
