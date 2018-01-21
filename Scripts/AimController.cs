using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour {
	public Transform adsPoint;
	public Transform notAdsPoint;
	public Transform gunRig;
	public float adsTime = 0.25f;
	private Vector3 targetPos;
	private Vector3 adsVelocity = Vector3.zero;
	private bool ads = false;
	// Use this for initialization
	void Start () {
		targetPos = notAdsPoint.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire2")) {
			ads = !ads;
		}
		if (ads) {
			targetPos = adsPoint.position;
		} else {
			targetPos = notAdsPoint.position;
		}
			
		gunRig.position = Vector3.SmoothDamp (gunRig.position, targetPos, ref adsVelocity, adsTime);
	}
}
