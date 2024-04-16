using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
	private static GameHandler Instance;

	[SerializeField] private Transform woodNodeTransform;
	[SerializeField] private Transform woodNodeTransform1;
	[SerializeField] private Transform woodNodeTransform2;
	[SerializeField] private Transform storageTransform;

	private List<ResourceNode> resourceNodeList;
	private void Awake() {
		Instance = this;

		resourceNodeList = new List<ResourceNode>();
		resourceNodeList.Add(new ResourceNode(woodNodeTransform));
		resourceNodeList.Add(new ResourceNode(woodNodeTransform1));
		resourceNodeList.Add(new ResourceNode(woodNodeTransform2));
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

	private Transform GetStorage() {
		return storageTransform;
	}
	public static Transform GetStorage_Static() {
		return Instance.GetStorage();
	}
}
