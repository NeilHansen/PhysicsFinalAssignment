﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManeger : MonoBehaviour {

	// Use this for initialization
	public string sceneName;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadScene()
	{
		SceneManager.LoadScene (sceneName);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
