﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
	public void NextPlayScene()
	{
		SceneManager.LoadScene("MainScenes");
	}
}
