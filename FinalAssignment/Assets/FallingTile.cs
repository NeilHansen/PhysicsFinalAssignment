using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTile : MonoBehaviour {

	public float angle;
	public float period;
	private float time = 1.0f;
	private bool startDrop;
	public float dropTime;
	public GameObject tile;

	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Player") {
			time = Time.time;
			float phase = Mathf.Sin (time / period);
			tile.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, phase * angle));
			startDrop = true;
			Debug.Log ("here");
		}
		else
		{
			tile.transform.rotation = Quaternion.identity;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (startDrop) 
		{
			dropTime -=1 * Time.deltaTime;

			if (dropTime <= 0)
			{
				tile.GetComponent<Rigidbody> ().useGravity = true;
				tile.transform.rotation = Quaternion.identity;

			}
		}
	}
}
