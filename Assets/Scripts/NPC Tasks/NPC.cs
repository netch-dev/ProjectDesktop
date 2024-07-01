using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {
	[SerializeField] private Animator animator;
	[SerializeField] private NavMeshAgent navMeshAgent;

	[SerializeField] private GameObject wateringCan;
	[SerializeField] private GameObject rightHandEffector;
	[SerializeField] private FullBodyBipedIK fullBodyBipedIK;

	[SerializeField] private TextMeshProUGUI debugTextMesh;

	public Action OnAnimationCompleted;

	private List<ITask> taskList = new List<ITask>();
	private ITask currentTask;

	private void Awake() {
		taskList.Add(new BuildTask(animator));
		taskList.Add(new HarvestTask(animator));
		taskList.Add(new WaterPlantsTask(this, animator, rightHandEffector, fullBodyBipedIK, reduceAmountPerWateringRun: 25));
	}

	private void Update() {
		PerformTask();
	}

	private void PerformTask() {
		if (currentTask != null) {
			if (currentTask.IsComplete(this)) {
				currentTask = null;
			} else {
				debugTextMesh.text = $"Task: {currentTask.ToString()}";
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

	public IEnumerator LookAt(Vector3 targetPosition) {
		Quaternion lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
		lookRotation.x = 0;
		lookRotation.z = 0;

		float speed = 0.6f;
		float time = 0;

		// First check if the rotation is already within a small threshold
		Debug.Log(Quaternion.Angle(transform.rotation, lookRotation));
		if (Quaternion.Angle(transform.rotation, lookRotation) < 21) {
			yield break;
		}

		while (time < 1) {
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
			time += Time.deltaTime * speed;
			yield return null;
		}
	}

	public void MoveTo(Vector3 position) {
		float walkSpeedNormalized = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
		animator.SetFloat("WalkSpeed", walkSpeedNormalized);

		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(position);
	}
	public void Arrived() {
		// Stop the npc instantly
		navMeshAgent.isStopped = true;
		navMeshAgent.velocity = Vector3.zero;

		animator.SetFloat("WalkSpeed", 0f);
	}

	public void OnAnimationComplete() {
		OnAnimationCompleted?.Invoke();
	}
}