using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {
	private Animator animator;
	private CharacterController cc;
	private Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		cc = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		velocity = transform.InverseTransformDirection(cc.velocity);
		animator.SetFloat ("Forward", velocity.z);
		animator.SetFloat ("Right", velocity.x);
	}
}
