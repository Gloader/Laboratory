using UnityEngine;
using System.Collections;

public class KeyboardControls : MonoBehaviour
{

    /// <summary>
    /// The move forward.
    /// </summary>
    public KeyCode MoveForward = KeyCode.Z;
    /// <summary>
    /// The move back.
    /// </summary>
    public KeyCode MoveBack = KeyCode.S;
    /// <summary>
    /// The turn left.
    /// </summary>
    public KeyCode TurnLeft = KeyCode.Q;
    /// <summary>
    /// The turn right.
    /// </summary>
    public KeyCode TurnRight = KeyCode.D;
    /// <summary>
    /// The move forward.
    /// </summary>
    public KeyCode MoveForwardB = KeyCode.Keypad8;
    /// <summary>
    /// The move back.
    /// </summary>
    public KeyCode MoveBackB = KeyCode.Keypad5;
    /// <summary>
    /// The turn left.
    /// </summary>
    public KeyCode TurnLeftB = KeyCode.Keypad4;
    /// <summary>
    /// The turn right.
    /// </summary>
    public KeyCode TurnRightB = KeyCode.Keypad6;
    /// <summary>
    /// The camera forward.
    /// </summary>
    public KeyCode CameraForward = KeyCode.UpArrow;
    /// <summary>
    /// The camera back.
    /// </summary>
    public KeyCode CameraBack = KeyCode.DownArrow;
    /// <summary>
    /// The camera left.
    /// </summary>
    public KeyCode CameraLeft = KeyCode.LeftArrow;
    /// <summary>
    /// The camera right.
    /// </summary>
    public KeyCode CameraRight = KeyCode.RightArrow;
    /// <summary>
    /// The camera up.
    /// </summary>
    public KeyCode CameraUp = KeyCode.PageUp;
    /// <summary>
    /// The camera down.
    /// </summary>
    public KeyCode CameraDown = KeyCode.PageDown;
    /// <summary>
    /// The jump.
    /// </summary>
    public KeyCode Jump = KeyCode.Space;
    /// <summary>
    /// The jump.
    /// </summary>
    public KeyCode JumpB = KeyCode.Keypad0;
    /// <summary>
    /// The reset.
    /// </summary>
    public KeyCode Reset = KeyCode.R;
    public KeyCode Exit = KeyCode.Escape;
    /// <summary>
    /// The switch camera.
    /// </summary>
    public KeyCode SwitchCamera = KeyCode.C;
    /// <summary>
    /// The magnet.
    /// </summary>
    public KeyCode Magnet = KeyCode.M;
    /// <summary>
    /// The fan.
    /// </summary>
    public KeyCode Fan = KeyCode.F;
    GameObject[] cameras;

    //private static KeyboardControls singleton = null;
    
    /// <summary>
    /// Gets the singleton.
    /// </summary>
    /// <value>The singleton.</value>
    public static KeyboardControls Singleton
    { 
        get
        {
            //return KeyboardControls.singleton == null ? 
            //  new KeyboardControls() : KeyboardControls.singleton;
            return GameObject.FindWithTag("GameController").GetComponent<KeyboardControls>();
        } 
    }

    // Use this for initialization
    void Start()
    {
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        
        foreach (GameObject camera in cameras)
        {
            camera.camera.enabled = false;
            camera.GetComponent<AudioListener>().enabled = false;
        }

        cameras [1].camera.enabled = true;
        cameras [1].GetComponent<AudioListener>().enabled = true;
    
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyboardControls.Singleton.Reset))
        {
            Application.LoadLevel("Robot");
        }

        if (Input.GetKeyDown(KeyboardControls.Singleton.Exit))
        {
            Application.Quit();
        }
 
        if (Input.GetKeyDown(KeyboardControls.Singleton.SwitchCamera))
        {
            foreach (GameObject camera in cameras)
            {
                camera.camera.enabled = !camera.camera.enabled;
            }
        }
    }
}
