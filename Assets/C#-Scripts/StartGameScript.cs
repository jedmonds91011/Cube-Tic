﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour {


    public void letsPlayButtonAction()
    {
        //Application.LoadLevel("test2.5d");
		SceneManager.LoadScene("MultiplayerScene", LoadSceneMode.Single);
        
    }
}
