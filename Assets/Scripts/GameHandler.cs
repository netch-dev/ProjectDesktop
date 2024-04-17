using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
	private static GameHandler Instance;

	[SerializeField] private GathererAI gathererAI;

	[SerializeField] private Transform[] woodNodeTransformArray;
	[SerializeField] private Transform storageTransform;

	private List<ResourceNode> resourceNodeList;
	private void Awake() {
		Instance = this;

		resourceNodeList = new List<ResourceNode>();
		foreach (Transform woodNodeTransform in woodNodeTransformArray) {
			resourceNodeList.Add(new ResourceNode(woodNodeTransform.transform));
		}

		ResourceNode.OnResourceNodeClicked += ResourceNode_OnResourceNodeClicked;
	}

	private void ResourceNode_OnResourceNodeClicked(object sender, EventArgs e) {
		if (gathererAI == null) {
			Debug.LogError("GathererAI is not set in the GameHandler script.");
			return;
		}

		ResourceNode resourceNode = sender as ResourceNode;
		if (resourceNode != null) gathererAI.SetResouceNode(resourceNode);
	}

	private ResourceNode GetResourceNode() {
		List<ResourceNode> tempResourceNodeList = new List<ResourceNode>(resourceNodeList);
		foreach (ResourceNode resourceNode in resourceNodeList) {
			if (!resourceNode.HasResources()) {
				tempResourceNodeList.Remove(resourceNode);
			}
		}
		if (tempResourceNodeList.Count > 0) {
			return tempResourceNodeList[UnityEngine.Random.Range(0, tempResourceNodeList.Count)];
		} else {
			return null;
		}
	}
	public static ResourceNode GetResourceNode_Static() {
		return Instance.GetResourceNode();
	}

	private ResourceNode GetResourceNodeNearPosition(Vector3 position) {
		float maxDistance = 9f;

		List<ResourceNode> tempResourceNodeList = new List<ResourceNode>(resourceNodeList);
		foreach (ResourceNode resourceNode in resourceNodeList) {
			if (!resourceNode.HasResources() || Vector3.Distance(position, resourceNode.GetPosition()) > maxDistance) {
				tempResourceNodeList.Remove(resourceNode);
			}
		}
		if (tempResourceNodeList.Count > 0) {
			return tempResourceNodeList[UnityEngine.Random.Range(0, tempResourceNodeList.Count)];
		} else {
			return null;
		}
	}
	public static ResourceNode GetResourceNodeNearPosition_Static(Vector3 position) {
		return Instance.GetResourceNodeNearPosition(position);
	}

	private Transform GetStorage() {
		return storageTransform;
	}
	public static Transform GetStorage_Static() {
		return Instance.GetStorage();
	}
}
