﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonUI : MonoBehaviour {

	public bool displayUi;
	public GameObject[] display;
	public string text;


	void OnMouseOver()
	{
		displayUi = true;
	}

	void OnMouseExit()
	{
		displayUi = false;
	}
	// Use this for initialization
	void Start () {
		
	}
		
	// Update is called once per frame
	void Update () {
		if (displayUi) 
		{
			for(int i = 0; i<display.Length; i++ )
			{
				display[i].SetActive (true);
				display [i].GetComponentInChildren<Text> ().text = text;
			}
		} 
		else
		{
			for(int i = 0; i<display.Length; i++ )
			{
				display[i].SetActive (false);
			}
		}
	}
}
