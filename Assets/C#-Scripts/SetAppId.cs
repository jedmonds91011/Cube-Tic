using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

public class SetAppId : MonoBehaviour {
	bool setNM = false;
	void Update () 
	{
		if (NetworkLobbyManager.singleton != null && setNM == false) 
		{
			NetworkLobbyManager.singleton.StartMatchMaker ();
			NetworkLobbyManager.singleton.matchMaker.SetProgramAppID ((AppID)207851);
			setNM = true;
		}
	}
	
}
