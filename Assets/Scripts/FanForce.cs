using UnityEngine;
using System.Collections;

public class FanForce : MonoBehaviour
{
    GameObject fan;
    FanSettings fanSettings;

    // Use this for initialization
    void Start()
    {
        GameObject fan = GameObject.Find("fan_force");
        fanSettings = fan.GetComponent<FanSettings>();
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider otherCollider)
    {

        Vector3 direction = otherCollider.transform.forward.normalized;
        this.rigidbody.velocity += direction.normalized * fanSettings.force * Time.deltaTime;

    }

    void OnTriggerEnter(Collider otherCollider)
    {
        
        Vector3 direction = otherCollider.transform.forward.normalized;
        this.rigidbody.velocity += direction.normalized * fanSettings.force * Time.deltaTime;
        
    }
   
}
