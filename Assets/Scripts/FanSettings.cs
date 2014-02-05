using UnityEngine;
using System.Collections;

public class FanSettings : MonoBehaviour {

    public float maxForce = 15f;
    public float minForce = 1f;
    public float force = 0f;
    public float maxSpeed = 10f;
    public float posXHide = -40f;
    public bool On = true;
    GameObject fan;
    GameObject fanForce;
    Transform fanTransform;

    // Use this for initialization
	void Start () {
        fan = GameObject.Find("prop_fan_large");
        fanForce = GameObject.Find("fan_force");
        fanTransform = fanForce.transform;
        //fanTransform.Translate(new Vector3(posXHide,0f,0f));
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyboardControls.Singleton.Fan))
        {
            foreach (AnimationState state in fan.animation)
            {
                if (state.speed < 1.1f)
                {
                    state.speed = maxSpeed;
                    force = maxForce;
                    //fanTransform.Translate(new Vector3(-posXHide,0f,0f));

                } 
                else
                {
                    state.speed = 1f;
                    force = minForce;
                    //fanTransform.Translate(new Vector3(posXHide,0f,0f));
                    //GameObject.Find("fan_force").GetComponent<BoxCollider>().enabled = false;
                }
                
            }
        }
	}
}
