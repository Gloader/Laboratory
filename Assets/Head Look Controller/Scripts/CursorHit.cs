using UnityEngine;
using System.Collections;

public class CursorHit : MonoBehaviour {
	
	public HeadLookController headLook;
    public LeftArmController leftArm;
    public RightArmController rightArm;
    public BodyController controller;
	private float offset = 1.5f;
	
	// Update is called once per frame
	void LateUpdate () {
        if (Input.GetKey(KeyCode.PageUp))
			offset += Time.deltaTime;
		if (Input.GetKey(KeyCode.PageDown))
			offset -= Time.deltaTime;
		
		Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(cursorRay, out hit)) {
			transform.position = hit.point + offset * Vector3.up;
		}
		
		headLook.target = transform.position;
        rightArm.target = transform.position;
        leftArm.target = transform.position;

        controller.headTarget = transform.position;
        controller.leftArmTarget = transform.position;
        controller.rightArmTarget = transform.position;
	}
}
