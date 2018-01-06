using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
	void Start () {
		Anim = GetComponent<Animator> ();
		myBody = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Floor") 
		{
			Anim.SetTrigger ("Grounded");
			Anim.ResetTrigger ("Jump");
			Anim.ResetTrigger ("InAir");
		}
	}
	
	// Update is called once per frame
	void Update () {
		float move = Input.GetAxis ("Vertical");
		Anim.SetFloat ("Speed", move);

		bool jump = Input.GetKeyDown (KeyCode.Space);
		if (jump) 
		{
			Anim.SetTrigger ("Jump");
			Anim.SetTrigger ("InAir");
		}

		bool run = Input.GetKey (KeyCode.LeftShift);
		if (run)
		{
			Anim.SetTrigger ("Run");
		}
		else 
		{
			//Anim.SetTrigger ("Walk");
		}


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

		height = myBody.velocity.y;
		Anim.SetFloat ("Height", height);
		if (Input.GetKeyDown (KeyCode.Space))
		{
			
			Anim.SetTrigger ("Jump");
			Anim.SetTrigger ("InAir");
			myBody.AddForce (this.transform.up * jumpForce, ForceMode.Impulse);
		}

		yaw += horizontalCameraSpeed * Input.GetAxis ("Mouse X");
		pitch -= verticalCameraSpeed * Input.GetAxis ("Mouse Y");

		pitch = Mathf.Clamp (pitch, FOVmin, FOVmax);
		transform.localEulerAngles = new Vector3 (0.0f, yaw, 0.0f);
		mainCamera.transform.localEulerAngles = new Vector3 (pitch, 0.0f, 0.0f);
	//	transform.eulerAngles = new Vector3 (pitch, yaw, 0.0f);
	}
}
