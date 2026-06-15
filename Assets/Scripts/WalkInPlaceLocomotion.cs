using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkInPlaceLocomotion : MonoBehaviour {
	[SerializeField] CharacterController characterController;
	[SerializeField] GameObject leftHand, rightHand;

	Vector3 previousLeftPosition, previousRightPosition, direction;
	Vector3 gravity = new Vector3(0, -9.81f, 0);

	[SerializeField] float speed = 4;

	void Start() {
		SetPreviousPosition();
	}

	void Update() {
		// Calculate  the velocity of the player hand movement
		Vector3 leftHandVelocity = leftHand.transform.position - previousLeftPosition;
		Vector3 rightHandVelocity = rightHand.transform.position - previousRightPosition;
		float totalVelocity =+ leftHandVelocity.magnitude * 0.5f + rightHandVelocity.magnitude * 0.5f;

		if(totalVelocity >= 0.02f) { // Player has swung their head if true
			// Getting the direction which the player is facing
			direction = Camera.main.transform.forward;

			// Move the player using character controller
			characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up));
		}

		// Applying gravity
		characterController.Move(gravity * Time.deltaTime);
		SetPreviousPosition();

	}

	void SetPreviousPosition() {
		previousLeftPosition = leftHand.transform.position;
		previousRightPosition = rightHand.transform.position;
	}
}