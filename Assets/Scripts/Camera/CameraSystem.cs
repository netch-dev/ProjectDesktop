using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {
	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

	[SerializeField] private bool useEdgeScrolling = false;
	[SerializeField] private bool useDragPan = false;

	[SerializeField] private float fieldOfViewMin = 10f;
	[SerializeField] private float fieldOfViewMax = 50f;

	[SerializeField] private float followOffsetMin = 5f;
	[SerializeField] private float followOffsetMax = 50f;

	[SerializeField] private float followOffsetMinY = 10f;
	[SerializeField] private float followOffsetMaxY = 50f;

	private bool dragPanMoveActive = false;
	private Vector2 lastMousePosition;

	private float targetFieldOfView = 60f;
	private Vector3 followOffset;

	private void Awake() {
		followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
	}

	private void Update() {
		HandleMovement();
		if (useEdgeScrolling) HandleMovementEdgeScrolling();
		if (useDragPan) HandleMovementDragPan();

		HandleRotation();

		//HandleZoom_FieldOfView();
		//HandleZoom_MoveForward();
		HandleZoom_LowerY();
	}

	private void HandleMovement() {
		Vector3 inputDir = Vector3.zero;

		if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
		if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
		if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
		if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

		Vector3 moveDir = (transform.forward * inputDir.z) + (transform.right * inputDir.x);

		float moveSpeed = 50f;
		transform.position += moveDir * moveSpeed * Time.deltaTime;
	}

	private void HandleMovementEdgeScrolling() {
		Vector3 inputDir = Vector3.zero;

		int edgeScrollSize = 45;
		if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
		if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f;
		if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f;
		if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = +1f;

		Vector3 moveDir = (transform.forward * inputDir.z) + (transform.right * inputDir.x);

		float moveSpeed = 50f;
		transform.position += moveDir * moveSpeed * Time.deltaTime;

	}

	private void HandleMovementDragPan() {
		Vector3 inputDir = Vector3.zero;

		if (Input.GetMouseButtonDown(1)) {
			dragPanMoveActive = true;
			lastMousePosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(1)) {
			dragPanMoveActive = false;
		}

		if (dragPanMoveActive) {
			// Calculate the amount of pixels the mouse has moved since the last frame
			Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

			float dragPanSpeed = 1.1f;
			inputDir.x = mouseMovementDelta.x * dragPanSpeed;
			inputDir.z = mouseMovementDelta.y * dragPanSpeed;

			lastMousePosition = Input.mousePosition;
		}

		Vector3 moveDir = (transform.forward * inputDir.z) + (transform.right * inputDir.x);

		float moveSpeed = 3f;
		transform.position += moveDir * moveSpeed * Time.deltaTime;
	}

	private void HandleRotation() {
		float rotateDir = 0f;
		if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
		if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

		float rotateSpeed = 150f;
		transform.eulerAngles += new Vector3(0f, rotateDir * rotateSpeed * Time.deltaTime, 0f);
	}

	private void HandleZoom_FieldOfView() {
		if (Input.mouseScrollDelta.y > 0) {
			targetFieldOfView -= 5f;
		}

		if (Input.mouseScrollDelta.y < 0) {
			targetFieldOfView += 5f;
		}

		targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

		float zoomSpeed = 5f;
		cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
	}

	private void HandleZoom_MoveForward() {
		Vector3 zoomDir = followOffset.normalized;

		float zoomAmount = 3f;
		if (Input.mouseScrollDelta.y > 0) {
			followOffset -= zoomDir * zoomAmount;
		}

		if (Input.mouseScrollDelta.y < 0) {
			followOffset += zoomDir * zoomAmount;
		}

		if (followOffset.magnitude < followOffsetMin) {
			followOffset = zoomDir * followOffsetMin;
		}

		if (followOffset.magnitude > followOffsetMax) {
			followOffset = zoomDir * followOffsetMax;
		}

		float zoomSpeed = 5f;
		cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
			Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);

	}

	private void HandleZoom_LowerY() {

		float zoomAmount = 3f;
		if (Input.mouseScrollDelta.y > 0) {
			followOffset.y -= zoomAmount;
		}

		if (Input.mouseScrollDelta.y < 0) {
			followOffset.y += zoomAmount;
		}

		followOffset.y = Mathf.Clamp(followOffset.y, followOffsetMinY, followOffsetMaxY);


		float zoomSpeed = 5f;
		cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
			Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);

	}
}
