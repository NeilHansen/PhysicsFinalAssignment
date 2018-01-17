using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Chestcontroller : MonoBehaviour {
	public GameObject closedChest;
	public GameObject openChest;
	private Player player;
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
		closedChest.SetActive (false);
		openChest.SetActive (true);
		Time.timeScale = 0.0f;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
