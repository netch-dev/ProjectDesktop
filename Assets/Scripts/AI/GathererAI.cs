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
	private ResourceNode resourceNode;
	private Transform storageTransform;

	private int woodInventoryAmount = 0;

	private void Awake() {
		unit = gameObject.GetComponent<IUnit>();
		state = State.Idle;
	}

	private void Update() {
		switch (state) {
			case State.Idle:
				resourceNode = GameHandler.GetResourceNode_Static();
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
						state = State.MovingToStorage;
					} else {
						unit.PlayAnimationMine(resourceNode.GetPosition(), () => {
							resourceNode.GrabResource();
							woodInventoryAmount++;
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
						state = State.Idle;
					});
				}
				break;
		}
	}
}
