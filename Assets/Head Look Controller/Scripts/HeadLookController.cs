using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class BendingSegment {
	public Transform firstTransform;
	public Transform lastTransform;
	public float thresholdAngleDifference = 0;
	public float bendingMultiplier = 0.6f;
	public float maxAngleDifference = 30;
	public float maxBendingAngle = 80;
	public float responsiveness = 5;
	internal float angleH;
	internal float angleV;
	internal Vector3 dirUp;
	internal Vector3 referenceLookDir;
	internal Vector3 referenceUpDir;
	internal int chainLength;
	internal Quaternion[] origRotations;
}

[System.Serializable]
public class NonAffectedJoints {
	public Transform joint;
	public float effect = 0;
}

public class HeadLookController : MonoBehaviour {
	
	public Transform rootNode;
	public BendingSegment[] segments;
	public NonAffectedJoints[] nonAffectedJoints;
	public Vector3 headLookVector = Vector3.forward;
	public Vector3 headUpVector = Vector3.up;
	public Vector3 target = Vector3.zero;
	public float effect = 1;
	public bool overrideAnimation = false;

    // Desired look direction in world space
    Vector3 lookDirWorld = Vector3.zero;  
    // Desired look directions in neck parent space
    Vector3 lookDirGoal = Vector3.zero;

    Vector3 rightOfTarget = Vector3.zero;   
    Vector3 lookDirGoalinHPlane = Vector3.zero;

    Vector3 referenceRightDir = Vector3.zero;
	
    Vector3 lookDir = Vector3.zero;

	void Start () {
		if (rootNode == null) {
			rootNode = transform;
		}
		
		// Setup segments
		foreach (BendingSegment segment in segments) { //Premier segment S1: Neck-Head
			Quaternion parentRot = segment.firstTransform.parent.rotation; //S1: Rotation du buste
			Quaternion parentRotInv = Quaternion.Inverse(parentRot); //S1: Rotation inverse du buste
			segment.referenceLookDir =
                parentRotInv * rootNode.rotation * headLookVector.normalized; //S1:referenceLookDir prend en compte la rotation du rootNode et du buste 
			segment.referenceUpDir =
                parentRotInv * rootNode.rotation * headUpVector.normalized; //S1:referenceUpDir prend en compte la rotation du rootNode et du buste 
			segment.angleH = 0;
			segment.angleV = 0;
			segment.dirUp = segment.referenceUpDir;
			
			segment.chainLength = 1;
			Transform t = segment.lastTransform;
			while (t != segment.firstTransform && t != t.root) {
				segment.chainLength++;
				t = t.parent;
			}
			
			segment.origRotations = new Quaternion[segment.chainLength];
			t = segment.lastTransform;
			for (int i=segment.chainLength-1; i>=0; i--) {
				segment.origRotations[i] = t.localRotation;
				t = t.parent;
			}
		}
	}
	
    void OnDrawGizmos()
    {
        Vector3 p;
        Transform t;

        t = this.transform;
        p = this.transform.position;

        DrawVector(p, p + t.TransformDirection(headLookVector),Color.black,"headLookVector");
        DrawVector(p, p + t.TransformDirection(headUpVector),Color.black,"headUpVector");



        foreach (BendingSegment segment in segments) 
        {
            //Desired look direction in world space (LastTransform)
            t = segment.lastTransform.transform;
            p = segment.lastTransform.transform.position;
            DrawVector(p, p + lookDirWorld,Color.green,"lookDirWorld");

            //Neck parent space
            t = segment.firstTransform.parent.transform;
            p = segment.firstTransform.parent.transform.position;
            DrawVector(p, p + t.TransformDirection(segment.referenceUpDir),Color.blue,"referenceUpDir");
            DrawVector(p, p + t.TransformDirection(lookDirGoal),Color.cyan,"lookDirGoal");
            DrawVector(p, p + t.TransformDirection(segment.referenceLookDir),Color.blue,"referenceLookDir");

            DrawVector(p, p + t.TransformDirection(rightOfTarget),Color.gray,"rightOfTarget");
            DrawVector(p, p + t.TransformDirection(lookDirGoalinHPlane),Color.green,"lookDirGoalinHPlane");

            DrawVector(p, p + t.TransformDirection(referenceRightDir),Color.magenta,"referenceRightDir");

            //DrawVector(p, p + t.TransformDirection(lookDir),Color.red,"lookDir"); //lookDir == lookDirGoal

            t = segment.firstTransform;
            p = segment.firstTransform.position;                             
        }

        //joints not affected
        Gizmos.color = Color.white;
        for (int i=0; i<nonAffectedJoints.Length; i++) {
            foreach (Transform child in nonAffectedJoints[i].joint) {
                Gizmos.DrawLine(child.position, nonAffectedJoints[i].joint.position);
                break;
            }
        }
    }

    void DrawVector(Vector3 from, Vector3 to, Color color, string text = null)
    {
        Vector3 v = to - from;

        Gizmos.color = color;
        //DrawVector(from,to,text);

        GUIStyle style = new GUIStyle();
        style.fontSize = 6;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = color;
        
        Gizmos.DrawLine(from, to);
        
        if (text != null)
        {
            Handles.Label(to,/*v.ToString() + "\n" + */text, style);
        }
        
    }

    void DrawVector(Vector3 from, Vector3 to, string text = null)
    {
        Vector3 v = to - from;

        GUIStyle style = new GUIStyle();
        style.fontSize = 6;
        style.fontStyle = FontStyle.Bold;

        Gizmos.DrawLine(from, to);
        
        if (text != null)
        {
            Handles.Label(to,/*v.ToString() + "\n" + */text, style);
        }
    }

    void LateUpdate () {
		if (Time.deltaTime == 0)
			return;
		
		// Remember initial directions of joints that should not be affected
		Vector3[] jointDirections = new Vector3[nonAffectedJoints.Length];
		for (int i=0; i<nonAffectedJoints.Length; i++) {
			foreach (Transform child in nonAffectedJoints[i].joint) {
				jointDirections[i] = child.position - nonAffectedJoints[i].joint.position;
				break;
			}
		}
		
		// Handle each segment
		foreach (BendingSegment segment in segments) 
        {
			Transform t = segment.lastTransform;
			if (overrideAnimation) {
				for (int i=segment.chainLength-1; i>=0; i--) {
					t.localRotation = segment.origRotations[i];
					t = t.parent;
				}
			}
			
			Quaternion parentRot = segment.firstTransform.parent.rotation;
			Quaternion parentRotInv = Quaternion.Inverse(parentRot);
			
			// Desired look direction in world space
			/*Vector3*/ lookDirWorld = (target - segment.lastTransform.position).normalized;
			
			// Desired look directions in neck parent space
			/*Vector3*/ lookDirGoal = (parentRotInv * lookDirWorld);
			
			// Get the horizontal and vertical rotation angle to look at the target
			float hAngle = AngleAroundAxis(
				segment.referenceLookDir, lookDirGoal, segment.referenceUpDir
			);
			
			/*Vector3*/ rightOfTarget = Vector3.Cross(segment.referenceUpDir, lookDirGoal);
			
			/*Vector3 */lookDirGoalinHPlane =
				lookDirGoal - Vector3.Project(lookDirGoal, segment.referenceUpDir);
			
			float vAngle = AngleAroundAxis(
				lookDirGoalinHPlane, lookDirGoal, rightOfTarget
			);
			
			// Handle threshold angle difference, bending multiplier,
			// and max angle difference here
			float hAngleThr = Mathf.Max(
				0, Mathf.Abs(hAngle) - segment.thresholdAngleDifference
			) * Mathf.Sign(hAngle);
			
			float vAngleThr = Mathf.Max(
				0, Mathf.Abs(vAngle) - segment.thresholdAngleDifference
			) * Mathf.Sign(vAngle);
			
			hAngle = Mathf.Max(
				Mathf.Abs(hAngleThr) * Mathf.Abs(segment.bendingMultiplier),
				Mathf.Abs(hAngle) - segment.maxAngleDifference
			) * Mathf.Sign(hAngle) * Mathf.Sign(segment.bendingMultiplier);
			
			vAngle = Mathf.Max(
				Mathf.Abs(vAngleThr) * Mathf.Abs(segment.bendingMultiplier),
				Mathf.Abs(vAngle) - segment.maxAngleDifference
			) * Mathf.Sign(vAngle) * Mathf.Sign(segment.bendingMultiplier);
			
			// Handle max bending angle here
			hAngle = Mathf.Clamp(hAngle, -segment.maxBendingAngle, segment.maxBendingAngle);
			vAngle = Mathf.Clamp(vAngle, -segment.maxBendingAngle, segment.maxBendingAngle);
			
			/*Vector3 */referenceRightDir =
				Vector3.Cross(segment.referenceUpDir, segment.referenceLookDir);
			
			// Lerp angles
			segment.angleH = Mathf.Lerp(
				segment.angleH, hAngle, Time.deltaTime * segment.responsiveness
			);
			segment.angleV = Mathf.Lerp(
				segment.angleV, vAngle, Time.deltaTime * segment.responsiveness
			);
			
			// Get direction
			lookDirGoal = Quaternion.AngleAxis(segment.angleH, segment.referenceUpDir)
				* Quaternion.AngleAxis(segment.angleV, referenceRightDir)
				* segment.referenceLookDir;
			
			// Make look and up perpendicular
			Vector3 upDirGoal = segment.referenceUpDir;
			Vector3.OrthoNormalize(ref lookDirGoal, ref upDirGoal);
			
			// Interpolated look and up directions in neck parent space
			/*Vector3 */lookDir = lookDirGoal;
			segment.dirUp = Vector3.Slerp(segment.dirUp, upDirGoal, Time.deltaTime*5);
			Vector3.OrthoNormalize(ref lookDir, ref segment.dirUp);
			
			// Look rotation in world space
			Quaternion lookRot = (
				(parentRot * Quaternion.LookRotation(lookDir, segment.dirUp))
				* Quaternion.Inverse(
					parentRot * Quaternion.LookRotation(
						segment.referenceLookDir, segment.referenceUpDir
					)
				)
			);
			
			// Distribute rotation over all joints in segment
			Quaternion dividedRotation =
				Quaternion.Slerp(Quaternion.identity, lookRot, effect / segment.chainLength);
			t = segment.lastTransform;
			for (int i=0; i<segment.chainLength; i++) {
				t.rotation = dividedRotation * t.rotation;
				t = t.parent;
			}
		}
		
		// Handle non affected joints
		for (int i=0; i<nonAffectedJoints.Length; i++) {
			Vector3 newJointDirection = Vector3.zero;
			
			foreach (Transform child in nonAffectedJoints[i].joint) {
				newJointDirection = child.position - nonAffectedJoints[i].joint.position;
				break;
			}
			
			Vector3 combinedJointDirection = Vector3.Slerp(
				jointDirections[i], newJointDirection, nonAffectedJoints[i].effect
			);
			
			nonAffectedJoints[i].joint.rotation = Quaternion.FromToRotation(
				newJointDirection, combinedJointDirection
			) * nonAffectedJoints[i].joint.rotation;
		}
	}
	
	// The angle between dirA and dirB around axis
	public static float AngleAroundAxis (Vector3 dirA, Vector3 dirB, Vector3 axis) {
		// Project A and B onto the plane orthogonal target axis
		dirA = dirA - Vector3.Project(dirA, axis);
		dirB = dirB - Vector3.Project(dirB, axis);
		
		// Find (positive) angle between A and B
		float angle = Vector3.Angle(dirA, dirB);
		
		// Return angle multiplied with 1 or -1
		return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
	}
}
