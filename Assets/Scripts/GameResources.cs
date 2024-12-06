using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResources {

	public static event EventHandler OnResourceAmountChanged;

	public enum ResourceType {
		Gold,
		Fuel,
	}

	private static Dictionary<ResourceType, int> resourceAmountDictionary;

	public static void Init() {
		resourceAmountDictionary = new Dictionary<ResourceType, int>();
		foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType))) {
			resourceAmountDictionary[resourceType] = 999;
		}
	}

	public static void AddResourceAmount(ResourceType resourceType, int amount) {
		resourceAmountDictionary[resourceType] += amount;
		OnResourceAmountChanged?.Invoke(null, EventArgs.Empty);
	}

	public static void RemoveResourceAmount(ResourceType resourceType, int amount) {
		resourceAmountDictionary[resourceType] -= amount;
		OnResourceAmountChanged?.Invoke(null, EventArgs.Empty);
	}

	public static int GetResourceAmount(ResourceType resourceType) {
		return resourceAmountDictionary[resourceType];
	}

}
