using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public float position;
	public GameObject platform;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float newposition = Mathf.PingPong (5 * Time.time, position);
		this.transform.position = new Vector3 (newposition, transform.position.y,transform.position.z);
	}
}
