using UnityEngine;

[CreateAssetMenu(fileName = "NewCrop", menuName = "Farming/Crop")]
public class CropScriptableObject : ScriptableObject {
	public GameObject[] cropStages;
	public float secondsToFullyGrow;
	public int goldAmountForHarvesting;
	public float waterRequirementPerSecond;
}
