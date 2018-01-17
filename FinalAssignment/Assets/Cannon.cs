using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Cannon : MonoBehaviour {

	public float speed = 5.0f;
	public float angle = 11.0f;
	public float debugRayLength = 10.0f;
	public GameObject wall;
	private Rigidbody rb;
	private float acceleration = -9.8f;
	private float finalVelocity = 0.0f;
	private float time;
	private float displacementAngle;
	private Vector3 direction;
	public bool Shot = true;
	public GameObject cannon;
	public GameObject movingPlatform;
	public Player player;

	void Start () 
	{
		player = ReInput.players.GetPlayer(0);
		rb = GetComponent<Rigidbody> ();

		//rb.velocity = direction * speed;
	}
	void Update () 
	{
		Vector3 targetDir =  wall.transform.position - cannon.transform.position;
		float _angle = Vector3.Angle(targetDir, transform.forward);
//		Debug.Log (_angle);
		direction = Quaternion.Euler (-angle,0.0f, 0.0f) * transform.forward;
		direction.Normalize ();
		if(Input.GetKeyDown(KeyCode.E)|| player.GetButton("Fire"))
		{
			if(Mathf.Abs(rb.velocity.sqrMagnitude) < 0.0000001f)
			{
				if (speed <= 45.0f) {
					rb.velocity = direction * speed;
					Shot = true;
				}
				else if(speed >=45.0f)
				{
					//speed = 37.0f;
					rb.velocity = direction * speed;
					Shot = true;
				}
			}
		}

		if (Shot) 
		{
			this.GetComponent<SkeletonController> ().enabled = true;
		}
	}
}
