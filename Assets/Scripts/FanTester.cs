using UnityEngine;
using System.Collections;

public class FanTester : MonoBehaviour
{
    public float rigidbodyTranslationSpeed = 10f;

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider otherCollider)
    {

        Vector3 direction = otherCollider.transform.forward.normalized;
        Debug.Log("Fan Stay");
        this.rigidbody.velocity += direction.normalized * rigidbodyTranslationSpeed * Time.deltaTime;

    }
}
