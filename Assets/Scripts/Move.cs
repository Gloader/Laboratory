using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{

    public float rigidbodyTranslationSpeed = 15f;
    public float rigidbodyRotationSpeed = 300f;
    public float TranslationSpeed = 10f;
    public float RotationSpeed = 100f;
    public Vector3 direction;
    public Vector3 rotation;
    private bool moveRotation = false;

    // Use this for initialization
    void Start()
    {
        direction = new Vector3(0, 0, 0);
        rotation = new Vector3(0, 0, 0);
    }
    
    // Update is called once per frame
    void Update()
    {

        if (rigidbody == null)
        {
            if (direction != Vector3.zero)
            {
                transform.Translate(direction.normalized * TranslationSpeed * Time.deltaTime);
            }
            transform.Rotate(rotation.normalized * RotationSpeed * Time.deltaTime);
        } else
        {
            //Translation
            if (direction != Vector3.zero)
            {
                if (!moveRotation)
                    rigidbody.velocity += direction.normalized * rigidbodyTranslationSpeed * Time.deltaTime;
                else
                    rigidbody.velocity += transform.TransformDirection(direction.normalized * rigidbodyTranslationSpeed * Time.deltaTime);
            }

            //Rotation
            if (rotation != Vector3.zero)
            {
                moveRotation = true;
                Quaternion deltarotation = Quaternion.Euler(rotation * rigidbodyRotationSpeed * Time.deltaTime);
                rigidbody.MoveRotation(rigidbody.rotation * deltarotation);
            }

            /*if (Input.GetKey (KeyboardControls.Singleton.Reset))
            {
                Application.LoadLevel("Bouncing ball");
            }*/
        }
    }
}
