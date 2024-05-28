using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {
	[SerializeField] private Animator animator;
	[SerializeField] private NavMeshAgent navMeshAgent;
	[SerializeField] private GameObject wateringCan;

	[SerializeField] private TextMeshProUGUI debugTextMesh;

	public Action OnAnimationCompleted;

	private List<ITask> taskList = new List<ITask>();
	private ITask currentTask;

	private void Awake() {
		taskList.Add(new BuildTask(animator));
		taskList.Add(new HarvestTask(animator));
		taskList.Add(new WaterPlantsTask(this, animator));
	}

	private void Update() {
		PerformTask();
	}

	private void PerformTask() {
		if (currentTask != null) {
			if (currentTask.IsComplete(this)) {
				currentTask = null;
			} else {
				currentTask.ExecuteTask(this);
				return;
			}
		}

		foreach (ITask task in taskList) {
			if (task.IsAvailable(this)) {
				task.ExecuteTask(this);
				currentTask = task;
				debugTextMesh.text = $"Task: {task.ToString()}";
				break;
			}
		}
	}

	public void MoveTo(Vector3 position) {
		float walkSpeedNormalized = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
		animator.SetFloat("WalkSpeed", walkSpeedNormalized);

		navMeshAgent.SetDestination(position);
	}

	public void OnAnimationComplete() {
		OnAnimationCompleted?.Invoke();
	}
}