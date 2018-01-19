using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class Chestcontroller : MonoBehaviour {
	public GameObject closedChest;
	public GameObject openChest;
	private Player player;
	public GameObject winPanel;
	public AudioSource source;
	public AudioClip clip;

	public GameObject button;
	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer(0);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") 
		{
			Win ();
		}
			
	}

	void Win()
	{
		source.PlayOneShot (clip, 0.7f);
		closedChest.SetActive (false);
		openChest.SetActive (true);
		Invoke ("WinPanel", 2.0f);

	}

	void WinPanel()
	{
		winPanel.SetActive (true);
		EventSystem.current.SetSelectedGameObject (button, null);
		Time.timeScale = 0.0f;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
