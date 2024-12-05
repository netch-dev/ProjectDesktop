using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {
	[SerializeField] private Animator animator;
	[SerializeField] private NavMeshAgent navMeshAgent;
	[SerializeField] private TextMeshProUGUI debugText;

	private INPCState currentState;
	private TaskQueue taskQueue;

	private void Awake() {
		taskQueue = new TaskQueue();
		taskQueue.AddTask(new WaterPlantsTask(animator));
		taskQueue.AddTask(new HarvestTask(animator));
		taskQueue.AddTask(new BuildTask(animator));
		SetState(new IdleState());
	}

	private void Update() {
		currentState?.OnUpdate(this);
	}

	public void SetState(INPCState newState) {
		currentState?.OnExit(this);
		currentState = newState;
		currentState?.OnEnter(this);
		debugText.SetText($"State: {currentState.GetType().Name}");
	}

	public TaskQueue GetTaskQueue() {
		return taskQueue;
	}

	public void MoveTo(Vector3 position) {
		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(position);
		animator.SetFloat("WalkSpeed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
	}

	public void StopMoving() {
		navMeshAgent.isStopped = true;
		navMeshAgent.velocity = Vector3.zero;
		animator.SetFloat("WalkSpeed", 0f);
	}

	public IEnumerator LookAt(Vector3 targetPosition) {
		Quaternion lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
		lookRotation.x = 0;
		lookRotation.z = 0;

		float time = 0f;
		float duration = 0.6f;

		while (time < 1f) {
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time / duration);
			time += Time.deltaTime;
			yield return null;
		}
	}
}
