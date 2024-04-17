using UnityEngine;

public class GathererAI : MonoBehaviour {
	private enum State {
		Idle,
		MovingToResourceNode,
		GatheringResources,
		MovingToStorage
	}
	private IUnit unit;
	private State state;
	private ResourceNode resourceNode = null;
	private Transform storageTransform;
	private TextMesh inventoryTextMesh;

	private int woodInventoryAmount = 0;

	private void Awake() {
		unit = gameObject.GetComponent<IUnit>();
		state = State.Idle;

		inventoryTextMesh = transform.Find("inventoryTextMesh").GetComponent<TextMesh>();
		UpdateInventoryText();
	}

	private void UpdateInventoryText() {
		if (woodInventoryAmount > 0) {
			inventoryTextMesh.text = "" + woodInventoryAmount;
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
					unit.MoveTo(resourceNode.GetPosition(), 2.3f, () => {
						state = State.GatheringResources;
					});
				}
				break;

			case State.GatheringResources:
				if (unit.isIdle()) {
					if (woodInventoryAmount >= 3) {
						storageTransform = GameHandler.GetStorage_Static();
						resourceNode = GameHandler.GetResourceNodeNearPosition_Static(resourceNode.GetPosition());
						state = State.MovingToStorage;
					} else {
						unit.PlayAnimationMine(resourceNode.GetPosition(), () => {
							resourceNode.GrabResource();
							woodInventoryAmount++;
							UpdateInventoryText();
						});
					}
				}
				break;

			case State.MovingToStorage:
				if (unit.isIdle()) {
					unit.MoveTo(storageTransform.position, 2.3f, () => {
						GameResources.AddWoodAmount(woodInventoryAmount);
						Debug.Log("Gathered " + GameResources.GetWoodAmount() + " wood");
						woodInventoryAmount = 0;
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
}
