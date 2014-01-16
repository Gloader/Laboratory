using UnityEngine;
using System.Collections;

public class MoveKeyboardController : MonoBehaviour {

	private Move move;

	// Use this for initialization
	void Start () {
		move =  GetComponent<Move>();
		if (move==null)
		{
			Debug.Log("move==null");
			throw new UnityException("move==null");
		}
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = new Vector3 (0,0,0);
		Vector3 rotation = new Vector3 (0,0,0);

		if (this.tag == "MainCamera")
		{
			if (Input.GetKey (KeyboardControls.Singleton.CameraForward))
			{
				direction.z += 1f;
			}
			if (Input.GetKey (KeyboardControls.Singleton.CameraBack))
			{
				direction.z -= 1f;
			}	
			if (Input.GetKey (KeyboardControls.Singleton.CameraLeft))
			{
				rotation.y -= 1f;
			}
			if (Input.GetKey (KeyboardControls.Singleton.CameraRight))
			{
				rotation.y += 1f;
			}
			if (Input.GetKey (KeyboardControls.Singleton.CameraUp))
			{
				rotation.x -= 1f;
			}
			if (Input.GetKey (KeyboardControls.Singleton.CameraDown))
			{
				rotation.x += 1f;
			}
		}
		else
		{
			if (Input.GetKey (KeyboardControls.Singleton.MoveForward))
			{
				direction.z += 1f;
			}
			if (Input.GetKey (KeyboardControls.Singleton.MoveBack))
			{
				direction.z -= 1f;
			}	
			if (Input.GetKey (KeyboardControls.Singleton.TurnLeft))
			{
				rotation.y -= 1f;
			}
			if (Input.GetKey (KeyboardControls.Singleton.TurnRight))
			{
				rotation.y += 1f;
			}
			if (Input.GetKey (KeyboardControls.Singleton.Jump))
			{
				direction.y += 1f;
			}
			//if (Input.GetKey (KeyCode.E))
			//{
			//	direction.y -= 1f;
			//}
		}
		
		move.direction = direction.normalized;
		move.rotation = rotation.normalized;

	}
}
