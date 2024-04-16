using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ResourceGathererUnit : MonoBehaviour, IUnit {
	[SerializeField] private Animator animator;
	private NavMeshAgent navMeshAgent;

	private bool isGathering;
	private Vector3 currentDestination = Vector3.zero;
	private float stopDistance;
	private System.Action finishedMovingCallback;

	private void Awake() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.isStopped = true;
	}

	private void Update() {
		float walkSpeedNormalized = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
		animator.SetFloat("WalkSpeed", walkSpeedNormalized);

		if (navMeshAgent.remainingDistance <= stopDistance && currentDestination != Vector3.zero && !navMeshAgent.pathPending) {
			navMeshAgent.isStopped = true;
			if (finishedMovingCallback != null) {
				finishedMovingCallback.Invoke();
				finishedMovingCallback = null;
				currentDestination = Vector3.zero;
			}
		}
	}

	public void MoveTo(Vector3 position, float stopDistance, System.Action callback) {
		this.currentDestination = position;
		this.stopDistance = stopDistance;
		this.finishedMovingCallback = callback;

		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(position);
	}

	public void PlayAnimationMine(Vector3 lookAtPosition, System.Action callback) {
		StartCoroutine(PlayAnimation("Mine", callback));
	}

	private IEnumerator PlayAnimation(string animationName, System.Action callback) {
		isGathering = true;

		animator.SetTrigger("Attack_OneHand");

		Debug.Log("Playing animation: " + animator.GetCurrentAnimatorClipInfo(0).Length + "s");
		yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
		/*animator.SetInteger("WeaponType_int", 12);
		animator.SetInteger("MeleeType_int", 1);*/

		//yield return new WaitForSeconds(1.8f);

		/*		animator.SetInteger("WeaponType_int", 0);
				animator.SetInteger("MeleeType_int", 0);*/
		isGathering = false;
		if (callback != null) callback.Invoke();
	}

	public bool isIdle() {
		return navMeshAgent.isStopped && !isGathering;
	}
}
