using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class networkTest : NetworkBehaviour
{
	public GameObject spinButton;
	void Start()
	{
		//spinButton = (Button)GameObject.FindGameObjectWithTag ("SpinButton").GetComponent<Button>();
		spinButton.GetComponent<Button>().onClick.AddListener (() => {
			CmdSpinCube ();
		});
		if (!isClient)
			return;
		GameManager.instance.networkIds.Add (this.netId);
		GameManager.instance.players.Add (this);
		GameManager.instance.serverText.text = this.netId.ToString ();
	}

	[Command]
	public void CmdSpinCube()
	{

		//GameManager.instance.SpinCube ();
		GameManager.instance.serverText.text = this.GetComponent<NetworkIdentity>().netId + " You are not the active player...";
		if (!isClient) {
			GameManager.instance.serverText.text = " this is the server.. shame";
			return;
		}
		else if (this.GetComponent<NetworkIdentity>().netId == GameManager.instance.networkIds [GameManager.instance.currentPlayer]) {
			setSpinParameters ();
			spin ();
			if(!isLocalPlayer)
				return;
			GameManager.instance.SpinCube();
		} else {
			Debug.LogError("You are not active player... netID: "+this.netId + " vs. GM NetworkID: " + GameManager.instance.networkIds [GameManager.instance.currentPlayer]);
			GameManager.instance.serverText.text = this.GetComponent<NetworkIdentity>().netId + " You are not the active player...";
		}
		//GameManager.instance.SpinCube ();
	}

	void Update()
	{
		if (!isClient)
			return;

		if(Input.GetKeyDown(KeyCode.Space))
		{
			CmdSpinCube();
		}
	}
	[ClientRpc]
	public void RpcSpin(Vector3 index)
	{
		if (!isClient)
			return;

		setSpinFace (index);
		GameManager.instance.SpinCube ();

	}

	public void spin()
	{
		if (!isServer)
			return;
		Debug.LogError ("RPC being called");
		RpcSpin (GameManager.instance.endCubeVector);
	}

	public void setSpinParameters()
	{
		GameManager.instance.cubeSpinSpeed = 50;
		GameManager.instance.numCubeSpins = 15;
		GameManager.instance.getEndCubeFace ();
	}
	public void setSpinFace(Vector3 cVector)
	{
		//GameManager.instance.endFaceIndex = index;
		GameManager.instance.endCubeVector = cVector;
	}

	[Command]
	public void CmdPlayPiece(GameObject cubePlayed)
	{
		RpcdisablePiece(cubePlayed);
	}

	public void playSquare(GameObject cube)
	{
		CmdPlayPiece (cube);
	}

	[ClientRpc]
	public void RpcdisablePiece(GameObject piece)
	{
		GameManager.instance.playGamePiece (piece);
	}


}