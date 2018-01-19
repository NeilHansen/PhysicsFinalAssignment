using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using UnityEngine.EventSystems;

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
	private int health = 3;
	public GameObject heart;
	public GameObject heart1;
	public GameObject heart2;

	public GameObject loosePanel;
	public GameObject button;

	public AudioSource source;
	public AudioClip clip;
	public AudioClip gameOverClip;
	public AudioClip yay;
	public AudioSource music;



	void Awake()
	{
		Time.timeScale = 1.0f;
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

		if (col.gameObject.tag == "Bullet")
		{
			Ouch ();
			Debug.Log ("OUCH!");
			Destroy (col.gameObject);
			health--;
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

		switch (health) 
		{
		case 3:
			heart.SetActive (true);
			heart1.SetActive (true);
			heart2.SetActive (true);
			break;
		case 2:
			heart.SetActive (true);
			heart1.SetActive (true);
			heart2.SetActive (false);
			break;
		case 1:
			heart.SetActive (true);
			heart1.SetActive (false);
			heart2.SetActive (false);
			break;
		case 0:
			Death ();
			heart.SetActive (false);
			heart1.SetActive (false);
			heart2.SetActive (false);
			break;
		}

		if (player.GetButtonDown ("Pause"))
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

	void Death()
	{
		Debug.Log ("Dead");
		Anim.SetTrigger ("Death");

		music.Pause ();
		Invoke ("StopGame", 1.6f);

	}

	void StopGame()
	{
		source.PlayOneShot (gameOverClip, 0.7f);
		Debug.Log ("YOULOOSE");
		Time.timeScale = 0.0f;
		loosePanel.SetActive (true);
		EventSystem.current.SetSelectedGameObject (button, null);
	}

	void Ouch()
	{
		source.PlayOneShot (clip, 0.7f);
	}
}
