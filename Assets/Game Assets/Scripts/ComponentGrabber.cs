using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentGrabber : MonoBehaviour
{

	public CarryableObject objectToTarget;
	public bool isGripped;

	void OnTriggerEnter(Collider collision) {

		if(isGripped) return;
		if(objectToTarget != null) return;

		if(collision.gameObject.tag == "Carryable") {

			CarryableObject co = collision.gameObject.GetComponentInParent<CarryableObject>();
			Debug.Log($"{gameObject.name} Can carry a {co.name}");
			objectToTarget = co;
		}
	}

	void OnTriggerExit(Collider collision) {

		if(isGripped) return;

		if(collision.gameObject.tag == "Carryable") {

			CarryableObject co = collision.gameObject.GetComponentInParent<CarryableObject>();
			Debug.Log($"{gameObject.name} is out of range of {co.name}");

			if(objectToTarget == co) {
				objectToTarget = null;
			}
		}
	}
}
