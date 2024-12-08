using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farming/Crop")]
public class CropSO : ScriptableObject {
	[Header("General Info")]
	public string cropName;
	public Sprite cropIcon;
	public int seedPrice;
	public GameObject[] cropPrefabVisuals;

	[Header("Growth Settings")]
	public float secondsToFullyGrow;
	public float waterUsagePerSecond;

	[Header("Harvest")]
	public int harvestCropYield;
	public int harvestGoldYield;
	public int sellPrice;

	[Header("Seasons")]
	public bool growsInSpring;
	public bool growsInSummer;
	public bool growsInFall;
	public bool growsInWinter;

	[Header("Other Properties")]
	[TextArea] public string description;
}
