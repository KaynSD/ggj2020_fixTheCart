using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	public int Device;
	public float speed = 5;
	public float accel = 0.1f;
	public float decel = 0.9f;
	public bool moveLeft = false;
	public bool moveRight = false;
	public bool moveUp = false;
	public bool moveDown = false;
	public ComponentGrabber leftHand;
	public ComponentGrabber rightHand;
	
	protected float accelX = 0;
	protected float accelZ = 0;
	 
	
	public bool isGrabbing = false;

	private CharacterController controller;
	public CarryableObject carriedObject;

	void Start(){
		controller = GetComponent<CharacterController>();
	}

	public void HandleMessage(JToken data)
	{
		switch(data.Value<string>("element")){
			case "move" :
				string direction = data.Value<JToken>("data").Value<string>("key");
				bool pressed = data.Value<JToken>("data").Value<bool>("pressed");

				Debug.Log($"{Device}: {direction} -> {pressed}");
				switch(direction) {
					case "up" : moveUp = pressed; break;
					case "down" : moveDown = pressed; break;
					case "left" : moveLeft = pressed; break;
					case "right" : moveRight = pressed; break;
				}
				break;
			case "hit" :
				bool hitting = data.Value<JToken>("data").Value<bool>("pressed");
				if(hitting) {
					Debug.Log($"{Device}: HIT!");
				}
				break;
			case "grab" :
				bool grabbed = data.Value<JToken>("data").Value<bool>("pressed");
				Debug.Log($"{Device}: Grab {grabbed}");
				isGrabbing = grabbed;

				if(isGrabbing){
					Grab();
				} else {
					Ungrab();
				}

				break;
		}
	}

	public void Grab(){
		carriedObject = null;
		leftHand.isGripped = true;
		rightHand.isGripped = true;

		CarryableObject objectCarried = null;

		if(leftHand.objectToTarget == null && rightHand.objectToTarget != null) {
			objectCarried = rightHand.objectToTarget;
		} else if (rightHand.objectToTarget == null && leftHand.objectToTarget != null) {
			objectCarried = leftHand.objectToTarget;
		} else if (leftHand.objectToTarget != null && leftHand.objectToTarget == rightHand.objectToTarget) {
			objectCarried = leftHand.objectToTarget;
		}

		if(objectCarried != null) {
			carriedObject = objectCarried;
			carriedObject.AttachTo(this);
		}
	}

	public void Ungrab(){
		leftHand.isGripped = false;
		rightHand.isGripped = false;

		if(carriedObject != null) {
			carriedObject.Detach(this);
			carriedObject = null;
		}
	}

	void Update(){
		Vector3 move = new Vector3(
			moveRight == moveLeft ? 0 : moveLeft ? -1 : 1,
			0,
			moveUp == moveDown ? 0 : moveUp ? 1 : -1 );

		accelX *= decel;
		accelZ *= decel;
		

		accelX += move.x * accel;
		accelZ += move.z * accel;
		
		if(accelX < -1) accelX = -1;
		if(accelX > 1) accelX = 1;
		if(accelZ < -1) accelZ = -1;
		if(accelZ > 1) accelZ = 1;

		float modSpeed = speed;
		if(carriedObject != null) {
			modSpeed /= carriedObject.Weight;
		}

		controller.SimpleMove(new Vector3(accelX, 0, accelZ) * modSpeed);

		Vector3 target = transform.position + move;
		transform.LookAt(target);
	}


}
