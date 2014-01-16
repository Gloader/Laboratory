using UnityEngine;
using System.Collections;

public class MoveKeyboardControllerB : MonoBehaviour {
    
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

        if (Input.GetKey (KeyboardControls.Singleton.MoveForwardB))
        {
            direction.z += 1f;
        }
        if (Input.GetKey (KeyboardControls.Singleton.MoveBackB))
        {
            direction.z -= 1f;
        }   
        if (Input.GetKey (KeyboardControls.Singleton.TurnLeftB))
        {
            rotation.y -= 1f;
        }
        if (Input.GetKey (KeyboardControls.Singleton.TurnRightB))
        {
            rotation.y += 1f;
        }
        if (Input.GetKey (KeyboardControls.Singleton.JumpB))
        {
            direction.y += 1f;
        }

        
        move.direction = direction.normalized;
        move.rotation = rotation.normalized;
        
    }
}
