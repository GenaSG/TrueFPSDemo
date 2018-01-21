using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IK {
	public static void ik(Transform End,Vector3 TargetPosition, Quaternion TargetRotation,float positionsWeight,float rotationWeight){
		Transform Middle = End.parent;
		Transform Start = Middle.parent;

		TargetPosition = Vector3.Lerp (End.position,TargetPosition,positionsWeight);
		TargetRotation = Quaternion.Slerp (End.rotation,TargetRotation,rotationWeight);

		ActualIK(End,Middle,Start,TargetPosition,TargetRotation);

		End.rotation = TargetRotation;
	}

	static void ActualIK(Transform End,Transform Middle,Transform Start, Vector3 TargetPosition, Quaternion TargetRotation){

		float a = End.localPosition.magnitude;
		float b = Start.InverseTransformPoint(TargetPosition).magnitude;
		float c = Middle.localPosition.magnitude;
		b = Mathf.Clamp(b,0.0001f,(a + c) - 0.0001f);
		float middleAngle = Mathf.Acos(Mathf.Clamp((Mathf.Pow(a,2)+Mathf.Pow(c,2)-Mathf.Pow(b,2))/(2*a*c),-1,1)) * Mathf.Rad2Deg;
		Vector3 middleToStartDir = Middle.InverseTransformPoint (Start.position);
		Vector3 middleToEndDir = Middle.InverseTransformPoint (End.position);
		Vector3 middleAxis = Vector3.Cross (-middleToStartDir,middleToEndDir);
		Middle.localRotation *= Quaternion.AngleAxis (Vector3.Angle (middleToStartDir, middleToEndDir) - middleAngle,middleAxis);

		Quaternion deltaRotation = Quaternion.Inverse (End.rotation) * TargetRotation;
		Vector3 middleDelta;
		middleDelta.x = Mathf.LerpAngle (0, deltaRotation.eulerAngles.x, End.localPosition.normalized.x * 0.33f);
		middleDelta.y = Mathf.LerpAngle (0, deltaRotation.eulerAngles.y, End.localPosition.normalized.y * 0.33f);
		middleDelta.z = Mathf.LerpAngle (0, deltaRotation.eulerAngles.z, End.localPosition.normalized.z * 0.33f);
		Middle.localRotation *= Quaternion.Euler (middleDelta);
		Vector3 endPos = Start.InverseTransformPoint (End.position);
		Vector3 startDelta;
		startDelta.x = Mathf.LerpAngle (0, deltaRotation.eulerAngles.x, endPos.normalized.x * 0.33f);
		startDelta.y = Mathf.LerpAngle (0, deltaRotation.eulerAngles.y, endPos.normalized.y * 0.33f);
		startDelta.z = Mathf.LerpAngle (0, deltaRotation.eulerAngles.z, endPos.normalized.z * 0.33f);
		Start.localRotation *= Quaternion.Euler (startDelta);


		Vector3 startToEndDir = Start.InverseTransformPoint (End.position);
		Vector3 startToTargetDir = Start.InverseTransformPoint (TargetPosition);
		Vector3 startAxis = Vector3.Cross (startToEndDir,startToTargetDir);
		Start.localRotation *= Quaternion.AngleAxis (Vector3.Angle (startToEndDir, startToTargetDir), startAxis);

	}
}
