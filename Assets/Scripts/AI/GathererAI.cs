using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GathererAI : MonoBehaviour {
	public static event EventHandler OnGathererClicked;
	private enum State {
		Idle,
		MovingToResourceNode,
		GatheringResources,
		MovingToStorage
	}
	private IUnit unit;
	private State state;

	private Dictionary<GameResources.ResourceType, int> inventoryAmountDictionary;

	private ResourceNode resourceNode = null;
	private Transform storageTransform;
	private TextMesh inventoryTextMesh;

	private void Awake() {
		unit = gameObject.GetComponent<IUnit>();
		state = State.Idle;

		inventoryAmountDictionary = new Dictionary<GameResources.ResourceType, int>();
		foreach (GameResources.ResourceType resourceType in System.Enum.GetValues(typeof(GameResources.ResourceType))) {
			inventoryAmountDictionary[resourceType] = 0;
		}

		inventoryTextMesh = transform.Find("inventoryTextMesh").GetComponent<TextMesh>();
		UpdateInventoryText();

		ClickableObject clickable = gameObject.GetComponent<ClickableObject>();
		if (clickable == null) Debug.LogError("ClickableObject not found");

		gameObject.GetComponent<ClickableObject>().OnClick += GathererAI_OnClick;
	}

	private void GathererAI_OnClick(object sender, EventArgs e) {
		OnGathererClicked?.Invoke(this, EventArgs.Empty);
	}

	private void DropInventoryIntoGameResources() {
		foreach (GameResources.ResourceType resourceType in System.Enum.GetValues(typeof(GameResources.ResourceType))) {
			GameResources.AddResourceAmount(resourceType, inventoryAmountDictionary[resourceType]);
			inventoryAmountDictionary[resourceType] = 0;
		}
	}

	private int GetTotalInventoryAmount() {
		int total = 0;
		foreach (int amount in inventoryAmountDictionary.Values) {
			total += amount;
		}
		return total;
	}

	private bool IsInventoryFull() {
		return GetTotalInventoryAmount() >= 3;
	}

	private void UpdateInventoryText() {
		int inventoryAmount = GetTotalInventoryAmount();
		if (inventoryAmount > 0) {
			inventoryTextMesh.text = "" + inventoryAmount;
		} else {
			inventoryTextMesh.text = "";
		}
	}

	private void Update() {
		switch (state) {
			case State.Idle:
				//resourceNode = GameHandler.GetResourceNode_Static();
				if (resourceNode != null) state = State.MovingToResourceNode;
				break;

			case State.MovingToResourceNode:
				if (unit.isIdle()) {
					unit.MoveTo(resourceNode.GetPosition(), 3.7f, () => {
						state = State.GatheringResources;
					});
				}
				break;

			case State.GatheringResources:
				if (unit.isIdle()) {
					if (IsInventoryFull() || !resourceNode.HasResources()) {
						storageTransform = GameHandler.GetStorage_Static();
						resourceNode = GameHandler.GetResourceNodeNearPosition_Static(resourceNode.GetPosition(), resourceNode.GetResourceType());
						state = State.MovingToStorage;
					} else {
						switch (resourceNode.GetResourceType()) {
							case GameResources.ResourceType.Wood:
								unit.PlayAnimationChop(resourceNode.GetPosition(), GrabResourceFromNode);
								break;

							case GameResources.ResourceType.Gold:
								unit.PlayAnimationChop(resourceNode.GetPosition(), GrabResourceFromNode);
								break;
						}
					}
				}
				break;

			case State.MovingToStorage:
				if (unit.isIdle()) {
					unit.MoveTo(storageTransform.position, 2.5f, () => {
						DropInventoryIntoGameResources();
						UpdateInventoryText();
						state = State.Idle;
					});
				}
				break;
		}
	}

	public void SetResouceNode(ResourceNode resourceNode) {
		if (this.resourceNode == null) this.resourceNode = resourceNode;
	}

	public void ShowSelectedObject() {
		transform.Find("SelectedCircle").gameObject.SetActive(true);
	}

	public void HideSelectedObject() {
		transform.Find("SelectedCircle").gameObject.SetActive(false);
	}

	private void GrabResourceFromNode() {
		if (resourceNode.HasResources()) {
			GameResources.ResourceType resourceType = resourceNode.GrabResource();
			inventoryAmountDictionary[resourceType]++;
			UpdateInventoryText();
		}
	}
}
