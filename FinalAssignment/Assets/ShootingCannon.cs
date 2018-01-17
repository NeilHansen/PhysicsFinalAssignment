using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCannon : MonoBehaviour {

	private Rigidbody mybody;

	private float initialVelX;
	private float initialVelZ;
	private float finalVel = 0.0f;
	private float Acceleration = -9.8f;
	private float displacementx;
	private float displacementz;


	private bool CanShoot;

	public GameObject player;

	// Use this for initialization
	void Start () {
		mybody = this.GetComponent<Rigidbody> ();
	}


	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "CannonRange") 
		{
			CanShoot = true;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (CanShoot)
		{
			StartCoroutine ("FireCannon");
		}
	}

	IEnumerator FireCannon()
	{
		yield return new WaitForSeconds (3.0f);
		SetDisplacement (player);
		CalclulateInitalVelX (finalVel, Acceleration, displacementx);
		CalclulateInitalVelZ (finalVel, Acceleration, displacementz);
		mybody.velocity = new Vector3 (initialVelX, 0, initialVelZ);
		yield return null;
	}

	void SetDisplacement(GameObject player)
	{
		displacementx = player.transform.localPosition.x;
		displacementz = player.transform.localPosition.z;
	}

	void CalclulateInitalVelX(float finalvel, float accel, float dis)
	{
		initialVelX = Mathf.Sqrt (-2 * accel * dis);
		Debug.Log (initialVelX);
	}
		
	void CalclulateInitalVelZ(float finalvel, float accel, float dis)
	{
		initialVelZ = Mathf.Sqrt (-2 * accel * dis);
		Debug.Log (initialVelZ);
	}
}
