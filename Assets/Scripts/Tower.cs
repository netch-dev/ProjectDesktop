using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower {
	private Transform towerTransform;
	private Action onTowerConstructionAction;
	private Tower(Transform towerTransform, Action onTowerConstructionAction) {
		this.towerTransform = towerTransform;
		this.onTowerConstructionAction = onTowerConstructionAction;
	}

	public static bool TrySpendResourcesCost() {
		if (GameResources.GetResourceAmount(GameResources.ResourceType.Fuel) >= 3 && GameResources.GetResourceAmount(GameResources.ResourceType.Gold) >= 3) {

			GameResources.RemoveResourceAmount(GameResources.ResourceType.Fuel, 3);
			GameResources.RemoveResourceAmount(GameResources.ResourceType.Gold, 3);
			return true;
		} else {
			return false;
		}
	}

	public static Tower Create(Transform towerPrefab, Vector3 position, Action onTowerConstructedAction) {
		Transform towerTransform = UnityEngine.Object.Instantiate(towerPrefab, position, Quaternion.identity);
		Tower tower = new Tower(towerTransform, onTowerConstructedAction);

		// Run the action after the tower has been constructed
		Animator animator = towerTransform.GetComponent<Animator>();
		FunctionTimer.Create(onTowerConstructedAction, animator.GetCurrentAnimatorClipInfo(0).Length);

		return tower;
	}
}
