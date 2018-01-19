using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCannon : MonoBehaviour {

	public GameObject bullet;
	public GameObject bulletSpawn;
	private Rigidbody mybody;

	private float initialVelX;
	private float initialVelZ;
	private float finalVel = 0.0f;
	private float Acceleration = -9.8f;
	private float displacementx;
	private float displacementz;


	public AudioSource source;
	public AudioClip clip;

	public float force;



	private bool CanShoot;

	public GameObject player;

	// Use this for initialization
	void Start () {
		mybody = this.GetComponent<Rigidbody> ();
	}


	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") 
		{
			CanShoot = true;
			InvokeRepeating("FireCannon",1.0f, 3.0f);
			Debug.Log ("canshoot");
		}
	}



	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player") 
		{
			CanShoot = true;
			Debug.Log ("cannotshoot");
			CancelInvoke ("FireCannon");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		this.transform.LookAt (player.transform);
	}

	void FireCannon()
	{
		SetDisplacement (player);
		//CalclulateInitalVelX (finalVel, Acceleration, displacementx);
		//CalclulateInitalVelZ (finalVel, Acceleration, displacementz);
		GameObject bulletClone = Instantiate (bullet) as GameObject;
		bulletClone.transform.position = bulletSpawn.transform.position;
		bulletClone.GetComponent<Rigidbody> ().AddForce (this.transform.forward * force, ForceMode.Impulse);
		//bulletClone.GetComponent<Rigidbody>().velocity = CalculateBallistics (bulletSpawn.transform.position, player.transform.position, 0); 
		source.PlayOneShot (clip, 0.7f);
		Debug.Log ("shot");
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

	Vector3 CalculateBallistics(Vector3 startPoint, Vector3 endPoint, float dispY)
	{
		float dispX = Vector3.Distance (startPoint, endPoint);

		float initVelY = Mathf.Sqrt(-2.0f * Physics.gravity.y * dispX);
		//Debug.Log ("init vel y: " + initVelY);

		float time = (0.0f - initVelY) / Physics.gravity.y;
		//Debug.Log ("time: " + time);

		float initVelX = dispX / time;
		//Debug.Log ("init vel x: " + initVelX);

		Vector3 newVelocity = new Vector3(-initVelX, initVelY, 0.0f);
		return newVelocity;
	}
}
