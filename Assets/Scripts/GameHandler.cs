using CodeMonkey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameHandler : MonoBehaviour {
	private static GameHandler Instance;

	[SerializeField] private GathererAI[] gathererAIArray;
	private GathererAI selectedGathererAI;

	[SerializeField] private Transform[] woodNodeTransformArray;
	[SerializeField] private Transform[] goldNodeTransformArray;
	[SerializeField] private Transform storageTransform;

	private List<ResourceNode> resourceNodeList;
	private void Awake() {
		Instance = this;

		GameResources.Init();

		resourceNodeList = new List<ResourceNode>();
		foreach (Transform woodNodeTransform in woodNodeTransformArray) {
			resourceNodeList.Add(new ResourceNode(woodNodeTransform.transform, GameResources.ResourceType.Wood));
		}
		foreach (Transform goldNodeTransform in goldNodeTransformArray) {
			resourceNodeList.Add(new ResourceNode(goldNodeTransform.transform, GameResources.ResourceType.Gold));
		}

		ResourceNode.OnResourceNodeClicked += ResourceNode_OnResourceNodeClicked;

		GathererAI.OnGathererClicked += GathererAI_OnGathererClicked;

		ClickDetector.OnRightClick += ClickDetector_OnRightClick;
	}

	private void ClickDetector_OnRightClick(Vector3 rightClickPosition) {
		if (GameResources.GetResourceAmount(GameResources.ResourceType.Wood) >= 10 &&
			GameResources.GetResourceAmount(GameResources.ResourceType.Gold) >= 10) {
			// We do have resources
			Debug.Log("We have resources");

			// Spawn a tower
			GameResources.RemoveResourceAmount(GameResources.ResourceType.Wood, 10);
			GameResources.RemoveResourceAmount(GameResources.ResourceType.Gold, 10);
		} else {
			// Not enough resources
			Debug.Log("Not enough resources");
		}
	}

	private void GathererAI_OnGathererClicked(object sender, EventArgs e) {
		if (selectedGathererAI != null) selectedGathererAI.HideSelectedObject();

		GathererAI gathererAI = sender as GathererAI;
		selectedGathererAI = gathererAI;
		gathererAI.ShowSelectedObject();
	}

	private void ResourceNode_OnResourceNodeClicked(object sender, EventArgs e) {
		if (selectedGathererAI == null) return;

		ResourceNode resourceNode = sender as ResourceNode;
		if (resourceNode != null) selectedGathererAI.SetResouceNode(resourceNode);
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

	private ResourceNode GetResourceNodeNearPosition(Vector3 position, GameResources.ResourceType resourceType) {
		float maxDistance = 7.5f;

		List<ResourceNode> tempResourceNodeList = new List<ResourceNode>(resourceNodeList);
		foreach (ResourceNode resourceNode in resourceNodeList) {
			if (!resourceNode.HasResources()
				|| Vector3.Distance(position, resourceNode.GetPosition()) > maxDistance
				|| resourceType != resourceNode.GetResourceType()) {
				tempResourceNodeList.Remove(resourceNode);
			}
		}
		if (tempResourceNodeList.Count > 0) {
			return tempResourceNodeList[UnityEngine.Random.Range(0, tempResourceNodeList.Count)];
		} else {
			return null;
		}
	}
	public static ResourceNode GetResourceNodeNearPosition_Static(Vector3 position, GameResources.ResourceType resourceType) {
		return Instance.GetResourceNodeNearPosition(position, resourceType);
	}

	private Transform GetStorage() {
		return storageTransform;
	}
	public static Transform GetStorage_Static() {
		return Instance.GetStorage();
	}
}
