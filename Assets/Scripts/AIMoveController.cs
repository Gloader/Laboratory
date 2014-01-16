using UnityEngine;
using System.Collections;

public class AIMoveController : MonoBehaviour {

	public GameObject target;
	public string targetTag = "prop_floorBot_male";
	//public string targetTag = "Player";
	private Move move;
	public bool activateMagnet = false;

	
	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
		//target = GameObject.FindWithTag (targetTag);
		target = GameObject.Find(targetTag);
		//target = GameObject.FindGameObjectWithTag (targetTag);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = new Vector3 (0,0,0);

		if (Input.GetKeyDown (KeyboardControls.Singleton.Magnet))
		{
			activateMagnet = !activateMagnet;
		}

		if (activateMagnet)
		{	
			if (target!=null)
			{
				direction = target.transform.position - this.transform.position;
			}
		}
			move.direction = direction.normalized;
		
	}
}
