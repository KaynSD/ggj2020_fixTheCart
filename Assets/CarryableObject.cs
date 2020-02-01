using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryableObject : MonoBehaviour
{
	public string Name = "";
	public float Weight = 1;

	public void AttachTo(PlayerController playerController)
	{
		transform.position = playerController.transform.position + new Vector3(0, 1.2f, 0);
		transform.parent = playerController.transform;
		GetComponent<Rigidbody>().isKinematic = true;
		
	}

	public void Detach(PlayerController playerController){
		if(transform.parent != playerController.transform);
		transform.parent = null;
		GetComponent<Rigidbody>().isKinematic = false;
	}
}
