using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonUI : MonoBehaviour {

	public bool displayUi;
	public GameObject display;


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
			display.SetActive (true);
		} 
		else
		{
			display.SetActive (false);
		}
	}
}
