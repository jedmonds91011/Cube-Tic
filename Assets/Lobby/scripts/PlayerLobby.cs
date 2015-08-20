using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.EventSystems;

public class PlayerLobby : NetworkLobbyPlayer
{
	public Canvas playerCanvasPrefab;

	public Canvas playerCanvas;

	public int m_NetworkID;
	public bool isHost;

	// cached components
	ColorControl cc;
	NetworkLobbyPlayer lobbyPlayer;

	void Awake()
	{
		cc = GetComponent<ColorControl>();
		lobbyPlayer = GetComponent<NetworkLobbyPlayer>();
	}

	public override void OnClientEnterLobby()
	{
		if (playerCanvas == null)
		{
			playerCanvas = (Canvas)Instantiate(playerCanvasPrefab, Vector3.zero, Quaternion.identity);
			playerCanvas.sortingOrder = 1;
		}

		var hooks = playerCanvas.GetComponent<PlayerCanvasHooks>();
		hooks.panelPos.anchoredPosition = new Vector3(0, GetPlayerPos(lobbyPlayer.slot), 0);
		hooks.SetColor(cc.myColor);
		hooks.SetReady(lobbyPlayer.readyToBegin);


		EventSystem.current.SetSelectedGameObject(hooks.colorButton.gameObject);
	}

	public override void OnClientExitLobby()
	{
		if (playerCanvas != null)
		{
			Destroy(playerCanvas.gameObject);
		}
	}

	public override void OnClientReady(bool readyState)
	{
		var hooks = playerCanvas.GetComponent<PlayerCanvasHooks>();
		hooks.SetReady(readyState);
	}

	float GetPlayerPos(int slot)
	{
		var lobby = NetworkLobbyManager.singleton as GuiLobbyManager;
		if (lobby == null)
		{
			// no lobby?
			return slot * 50;
		}

		// this spreads the player canvas panels out across the screen
//		var screenHeight = Screen.height;//.pixelRect.height;
//		//screenHeight -= 200; // border padding
//		var playerHeight = playerCanvas.pixelRect.height;//screenHeight / (lobby.maxPlayers-1);
//		return -(screenHeight/2) + (playerHeight * slot) - 50;
		var screenHeight = playerCanvas.pixelRect.height;//Screen.height;
		screenHeight += 400;
		var playerHeight = screenHeight/(lobby.maxPlayers-1);
		return -(screenHeight/2) + (slot+1) * playerHeight;
		//return -(playerHeight/2 * slot) - ((screenHeight/10) * slot);
	}

	public override void OnStartLocalPlayer()
	{
		if (playerCanvas == null)
		{
			playerCanvas = (Canvas)Instantiate(playerCanvasPrefab, Vector3.zero, Quaternion.identity);
			playerCanvas.sortingOrder = 1;
		}

		// setup button hooks
		var hooks = playerCanvas.GetComponent<PlayerCanvasHooks>();
		hooks.panelPos.anchoredPosition = new Vector3(0, GetPlayerPos(lobbyPlayer.slot), 0);

		hooks.SetColor(cc.myColor);

		hooks.OnColorChangeHook = OnGUIColorChange;
		hooks.OnReadyHook = OnGUIReady;
		hooks.OnRemoveHook = OnGUIRemove;
		hooks.SetLocalPlayer();
	}

	void OnDestroy()
	{
		if (playerCanvas != null)
		{
			Destroy(playerCanvas.gameObject);
		}
	}

	public void SetColor(Color color)
	{
		var hooks = playerCanvas.GetComponent<PlayerCanvasHooks>();
		hooks.SetColor(color);
	}

	public void SetReady(bool ready)
	{
		var hooks = playerCanvas.GetComponent<PlayerCanvasHooks>();
		hooks.SetReady(ready);
	}

	[Command]
	public void CmdExitToLobby()
	{
		var lobby = NetworkLobbyManager.singleton as GuiLobbyManager;
		if (lobby != null)
		{
			lobby.ServerReturnToLobby();
		}
	}

	// events from UI system

	void OnGUIColorChange()
	{
		if (isLocalPlayer)
			cc.ClientChangeColor();
	}

	void OnGUIReady()
	{
		if (isLocalPlayer)
			lobbyPlayer.SendReadyToBeginMessage();
	}

	void OnGUIRemove()
	{
		if (isLocalPlayer)
		{
			ClientScene.RemovePlayer(lobbyPlayer.playerControllerId);

			var lobby = NetworkLobbyManager.singleton as GuiLobbyManager;
			if (lobby != null)
			{
				lobby.SetFocusToAddPlayerButton();
			}
		}
	}
}

