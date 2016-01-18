using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour {


	public void letsPlayButtonAction(bool isNetworkGame)
    {
        //Application.LoadLevel("test2.5d");
		if(isNetworkGame)
		{
			SceneManager.LoadScene("MultiplayerScene", LoadSceneMode.Single);
		}
		else
		{
			SceneManager.LoadScene("SinglePlayerScene", LoadSceneMode.Single);
		}
        
    }
}
