using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

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
	private bool canJump;
	public GameObject skeleton;
	public Camera cannonCamera;
	public GameObject cannonUI;
	public GameObject checkpointUI;
	public float timer=  3.0f;

	public int playerID;
	private Player player;

	public GameObject pauseMenu;
	private bool gamePaused;
	public GameObject rewiredPrefab;

	void Awake()
	{
		//GameObject rewired = (GameObject)Instantiate(rewiredPrefab,transform.position,transform.rotation);
	}
	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer(playerID);
		Anim = GetComponent<Animator> ();
		myBody = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Floor" || col.gameObject.tag == "Falling tile" ) 
		{
			Anim.SetTrigger ("Grounded");
			Anim.ResetTrigger ("Jump");
			Anim.ResetTrigger ("InAir");
			canJump = true;
		}

	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Cp1") 
		{
			checkpoint1 = true;
			Checkpoint = col.gameObject;
			Debug.Log ("here1");
			StartCoroutine("CheckpointPopUp");
		}

		if (col.gameObject.tag == "Cp2") 
		{
			checkpoint2 = false;
			checkpoint3= true;
			Checkpoint = col.gameObject;
			Debug.Log ("here2");
			StartCoroutine("CheckpointPopUp");
			//this.GetComponent<MovingPlatform> ().enabled = false;
		}

		if (col.gameObject.tag == "Cp3") 
		{
			checkpoint1 = false;
			checkpoint2 = true;
			Checkpoint = col.gameObject;
			Debug.Log ("here2");
			StartCoroutine("CheckpointPopUp");
			//this.GetComponent<MovingPlatform> ().enabled = false;
		}



	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Cannon") 
		{
			if (Input.GetKey (KeyCode.E)|| player.GetButton("Use"))
			{
				EnterCannon ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();

		if (player.GetButton ("Pause"))
		{
			if (gamePaused) 
			{
				Unpause ();
				gamePaused = false;
			}
			else {
				Pause ();
				gamePaused = true;
			}
		}

		if (Input.GetKey(KeyCode.R)|| player.GetButton("Respawn")) 
		{
			Restart ();
		}

		if (this.GetComponent<Cannon> ().Shot == true) 
		{
			ExitCannon ();
		}

		//Anim.ResetTrigger ("Resurrection");
	}

	void Restart()
	{
		if (checkpoint1 || checkpoint2 || checkpoint3)
		{
			this.transform.position = Checkpoint.gameObject.transform.position;
			Anim.SetTrigger ("Resurrection");

		}
		else
		{
		SceneManager.LoadScene ("Game Scene");
			Anim.SetTrigger ("Resurrection");
		}
	}

	void Movement()
	{
		//Movement
		float move = Input.GetAxis ("Vertical");
		Anim.SetFloat ("Speed", move);
		if (Input.GetKey(KeyCode.W) || (player.GetAxis("Forward") > 0.0f)) 
		{

			myBody.AddForce (this.transform.forward * speed, ForceMode.Force);
			Anim.SetTrigger ("Walk");
		}



		if (Input.GetKey(KeyCode.S)|| (player.GetAxis("Forward") < 0.0f)) 
		{

			myBody.AddForce (-this.transform.forward * speed, ForceMode.Force);
			Anim.SetTrigger ("Walk");
		}

		if (Input.GetKey(KeyCode.D)|| (player.GetAxis("Right") > 0.0f)) 
		{

			myBody.AddForce (this.transform.right * speed, ForceMode.Force);
			Anim.SetTrigger ("Walk");
		}

		if (Input.GetKey(KeyCode.A)|| (player.GetAxis("Right") < 0.0f)) 
		{

			myBody.AddForce (-this.transform.right * speed, ForceMode.Force);
			Anim.SetTrigger ("Walk");
		}

		//Run
		bool run = Input.GetKey (KeyCode.LeftShift);
		if (run || player.GetButton("Run"))
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
		if (Input.GetKeyDown (KeyCode.Space)|| player.GetButton("Jump"))
		{
			if (canJump) {
				Anim.SetTrigger ("Jump");
				Anim.SetTrigger ("InAir");
				myBody.AddForce (this.transform.up * jumpForce, ForceMode.Impulse);
				canJump = false;
			}
		}

		//Camera and rotation
		yaw += horizontalCameraSpeed * Input.GetAxis ("Mouse X");
		pitch -= verticalCameraSpeed * Input.GetAxis ("Mouse Y");

		yaw += horizontalCameraSpeed * player.GetAxis ("LookRight");
		pitch -= verticalCameraSpeed * player.GetAxis ("LookUp");

		pitch = Mathf.Clamp (pitch, FOVmin, FOVmax);
		myBody.AddTorque (0, yaw * horizontalCameraSpeed, 0);
		transform.localEulerAngles = new Vector3 (0.0f, yaw, 0.0f);
		mainCamera.transform.localEulerAngles = new Vector3 (pitch, 0.0f, 0.0f);

	}

	IEnumerator CheckpointPopUp()
	{
		checkpointUI.SetActive (true);

		yield return new WaitForSeconds(3.0f);
	
		checkpointUI.SetActive (false);
	
		yield return null;
	}

	void EnterCannon()
	{
		Debug.Log ("InCannon");
		skeleton.SetActive (false);
		cannonCamera.enabled = true;
		this.GetComponentInChildren<Camera> ().enabled = false;
		this.transform.position = new Vector3 (31.0f, 32.0f, -57.5f);
		this.transform.rotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
		this.GetComponent<Cannon> ().enabled = true;
		cannonUI.SetActive (true);
		this.GetComponent<SkeletonController> ().enabled = false;
	}

	void ExitCannon()
	{
		skeleton.SetActive (true);
		cannonCamera.enabled = false;
		this.GetComponentInChildren<Camera> ().enabled = true;
		this.GetComponent<Cannon> ().enabled = false;
		cannonUI.SetActive (false);
	}

	void Pause()
	{
		pauseMenu.SetActive (true);
		Time.timeScale = 0.0f;
	}

	public void Unpause()
	{
		pauseMenu.SetActive (false);
		Time.timeScale = 1.0f;
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene ("HowToPlay");
		Time.timeScale = 1.0f;
	}

	public void QuitToMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}
}
