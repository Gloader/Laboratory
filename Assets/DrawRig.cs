using UnityEngine;
using System.Collections;

public class DrawRig : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        DrawSkel(this.transform);
    }

    void DrawSkel(Transform t)
    {
        int nbChildren = t.childCount;
        for (int i=0; i<nbChildren; ++i)
        {
            Gizmos.DrawLine(t.position, t.GetChild(i).transform.position);
            DrawSkel(t.GetChild(i));
        }

        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
