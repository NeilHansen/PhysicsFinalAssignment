using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public bool canSwing = true;
	public GameObject cannon;
	public GameObject movingPlatform;

	void Start () 
	{
		
		rb = GetComponent<Rigidbody> ();

		//rb.velocity = direction * speed;
	}
	void Update () 
	{
		Vector3 targetDir =  wall.transform.position - cannon.transform.position;
		float _angle = Vector3.Angle(targetDir, transform.forward);
//		Debug.Log (_angle);
		direction = Quaternion.Euler (-angle, 0.0f, 0.0f) * transform.forward;
		direction.Normalize ();
		if(Input.GetKeyDown(KeyCode.E))
		{
			if(Mathf.Abs(rb.velocity.sqrMagnitude) < 0.0000001f)
			{
				if (speed <= 45.0f) {
					rb.velocity = direction * speed;
					canSwing = false;
				}
				else if(speed >=45.0f)
				{
					//speed = 37.0f;
					rb.velocity = direction * speed;
					canSwing = false;
				}
			}
		}
		if(rb.velocity.y < 0.001f)
		{
			
		}

		if (this.transform.position.z >= wall.transform.position.z) 
		{
	
		}
	}
}
