using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

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
