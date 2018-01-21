using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour {
	struct HandCoords{
		public Vector3 position;
		public Vector3 up;
		public Vector3 forward;
	}
	public Transform camerAnchor;
	public Transform cam;
	public Transform aimpoint;
	public Transform gunAimpoint;
	private Animator animator;
	private Transform head;
	private Transform chest;
	private Transform spine;
	private Transform rightHand;
	private Transform leftHand;
	private HandCoords localRightHandCoords;
	private HandCoords localLeftHandCoords;
	private HandCoords targetRightHandCoords;
	private HandCoords targetLeftHandCoords;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		head = animator.GetBoneTransform (HumanBodyBones.Head);
		chest = animator.GetBoneTransform (HumanBodyBones.Chest);
		spine = animator.GetBoneTransform (HumanBodyBones.Spine);
		rightHand = animator.GetBoneTransform (HumanBodyBones.RightHand);
		leftHand = animator.GetBoneTransform (HumanBodyBones.LeftHand);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		chest.rotation *= Quaternion.Slerp (Quaternion.identity, cam.localRotation, 0.25f);
		spine.rotation *= Quaternion.Slerp (Quaternion.identity, cam.localRotation, 0.25f);
		head.rotation = cam.rotation;
		cam.position = camerAnchor.position;

		//Saving hands coords to gun aimpoint space
		localRightHandCoords.position = gunAimpoint.InverseTransformPoint (rightHand.position);
		localRightHandCoords.up = gunAimpoint.InverseTransformDirection (rightHand.up);
		localRightHandCoords.forward = gunAimpoint.InverseTransformDirection (rightHand.forward);

		localLeftHandCoords.position = gunAimpoint.InverseTransformPoint (leftHand.position);
		localLeftHandCoords.up = gunAimpoint.InverseTransformDirection (leftHand.up);
		localLeftHandCoords.forward = gunAimpoint.InverseTransformDirection (leftHand.forward);
		//

		//Saving original gun aimpoints coords
		Vector3 originalAimPointPos = gunAimpoint.position;
		Quaternion originalAimPointRot = gunAimpoint.rotation;
		//

		//Setting target gun aimpoint coords
		gunAimpoint.position = aimpoint.position;
		gunAimpoint.rotation = aimpoint.rotation;
		//

		//Restoring saved hands coords from aimpoint space to world space
		targetRightHandCoords.position = gunAimpoint.TransformPoint (localRightHandCoords.position);
		targetRightHandCoords.up = gunAimpoint.TransformDirection (localRightHandCoords.up);
		targetRightHandCoords.forward = gunAimpoint.TransformDirection (localRightHandCoords.forward);

		targetLeftHandCoords.position = gunAimpoint.TransformPoint (localLeftHandCoords.position);
		targetLeftHandCoords.up = gunAimpoint.TransformDirection (localLeftHandCoords.up);
		targetLeftHandCoords.forward = gunAimpoint.TransformDirection (localLeftHandCoords.forward);
		//

		//Restoring original gun aimpoint coords
		gunAimpoint.position = originalAimPointPos;
		gunAimpoint.rotation = originalAimPointRot;
		//
		//Using IK to set hands in to target coords
		IK.ik (rightHand, targetRightHandCoords.position, Quaternion.LookRotation (targetRightHandCoords.forward, targetRightHandCoords.up), 1, 1);
		IK.ik (leftHand, targetLeftHandCoords.position, Quaternion.LookRotation (targetLeftHandCoords.forward, targetLeftHandCoords.up), 1, 1);
	    //
	}
}
