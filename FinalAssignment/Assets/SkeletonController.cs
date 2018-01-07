using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkeletonController : MonoBehaviour {

	Animator Anim;
	Rigidbody myBody;
	private float height;

	public float speed;
	public float jumpForce;
	public float verticalCameraSpeed;
	public float horizontalCameraSpeed;
	public Camera mainCamera;
	public float FOVmin;
	public float FOVmax;

	private float yaw = 0.0f;
	private float pitch = 0.0f;

	public bool checkpoint1;
	public GameObject Checkpoint;
	public bool checkpoint2;
	public bool checkpoint3;


	// Use this for initialization
	void Start () {
		Anim = GetComponent<Animator> ();
		myBody = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Floor" || col.gameObject.tag == "Falling tile" ) 
		{
			Anim.SetTrigger ("Grounded");
			Anim.ResetTrigger ("Jump");
			Anim.ResetTrigger ("InAir");
		}

	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Cp1") 
		{
			checkpoint1 = true;
			Checkpoint = col.gameObject;
			Debug.Log ("here1");
		}

		if (col.gameObject.tag == "Cp2") 
		{
			checkpoint1 = false;
			checkpoint2 = true;
			Checkpoint = col.gameObject;
			Debug.Log ("here2");
			//this.GetComponent<MovingPlatform> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();

		if (Input.GetKey(KeyCode.R)) 
		{
			Restart ();
		}
	}

	void Restart()
	{
		if (checkpoint1 || checkpoint2 || checkpoint3)
		{
			this.transform.position = Checkpoint.gameObject.transform.position;
		}
		else
		{
		SceneManager.LoadScene ("Game Scene");
		}
	}

	void Movement()
	{
		//Movement
		float move = Input.GetAxis ("Vertical");
		Anim.SetFloat ("Speed", move);
		if (Input.GetKey(KeyCode.W)) 
		{

			myBody.AddForce (this.transform.forward * speed, ForceMode.Force);
			Anim.SetTrigger ("Walk");
		}

		if (Input.GetKey(KeyCode.S)) 
		{

			myBody.AddForce (-this.transform.forward * speed, ForceMode.Force);
			Anim.SetTrigger ("Walk");
		}

		if (Input.GetKey(KeyCode.D)) 
		{

			myBody.AddForce (this.transform.right * speed, ForceMode.Force);
			Anim.SetTrigger ("Walk");
		}

		if (Input.GetKey(KeyCode.A)) 
		{

			myBody.AddForce (-this.transform.right * speed, ForceMode.Force);
			Anim.SetTrigger ("Walk");
		}

		//Run
		bool run = Input.GetKey (KeyCode.LeftShift);
		if (run)
		{
			Anim.SetTrigger ("Run");
			speed = 120;
		}
		else 
		{
			speed = 100.0f;
			//Anim.SetTrigger ("Walk");
		}


		//Jump
		height = myBody.velocity.y;
		Anim.SetFloat ("Height", height);
		if (Input.GetKeyDown (KeyCode.Space))
		{

			Anim.SetTrigger ("Jump");
			Anim.SetTrigger ("InAir");
			myBody.AddForce (this.transform.up * jumpForce, ForceMode.Impulse);
		}

		//Camera and rotation
		yaw += horizontalCameraSpeed * Input.GetAxis ("Mouse X");
		pitch -= verticalCameraSpeed * Input.GetAxis ("Mouse Y");

		pitch = Mathf.Clamp (pitch, FOVmin, FOVmax);
		myBody.AddTorque (0, Input.GetAxis ("Mouse X") * horizontalCameraSpeed, 0);
		transform.localEulerAngles = new Vector3 (0.0f, yaw, 0.0f);
		mainCamera.transform.localEulerAngles = new Vector3 (pitch, 0.0f, 0.0f);

	}
}
