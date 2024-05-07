using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {
	[SerializeField] private Animator animator;
	[SerializeField] private NavMeshAgent navMeshAgent;

	private List<ITask> taskList = new List<ITask>();

	private void Awake() {
		taskList.Add(new BuildTask(animator));
		taskList.Add(new WaterPlantsTask());
	}

	private void Update() {
		PerformTask();
	}

	private void PerformTask() {
		foreach (ITask task in taskList) {
			if (task.IsAvailable(this)) {
				Debug.Log("Performing task: " + task.GetType().Name);
				task.ExecuteTask(this);
				break;
			}
		}
	}

	public void MoveTo(Vector3 position) {
		Debug.Log("Moving to: " + position);
		navMeshAgent.SetDestination(position);
	}
}