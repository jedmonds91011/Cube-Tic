using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class networkTest : NetworkBehaviour
{
	public Button spinButton;
	public bool myTurn;
	public int myIndex;
	public string myTextBase;
	public int myId; 
	bool isHost;

	void Start()
	{

		GameManager.instance.players.Add (this);
//		spinButton = (Button)GameObject.FindGameObjectWithTag ("SpinButton").GetComponent<Button>();
//		if (!isClient)
//			return;
		myIndex = GameManager.instance.networkIds.Count;
		myTextBase = "Player " + myIndex + ": " + this.netId.ToString();
		GameManager.instance.networkIds.Add (this.netId);
		GameManager.instance.players.Add (this);

		int.TryParse (this.netId.ToString (), out myId);
		if (isServer)
			isHost = true;
	}

	[Command]
	public void CmdSpinCube()
	{
//		if (!isServer)
//			return;
		setSpinParameters ();
		spin ();
//		if (!isLocalPlayer)
//			return;
//		GameManager.instance.SpinCube();

	}

	void Update()
	{
//		if (!isClient)
//			return;
		if (this == GameManager.instance.players [GameManager.instance.currentPlayer]) {
			myTurn = true;
		} else {
			myTurn = false;
		}


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
		myTurn = !myTurn;
	}


	public void GoAheadNSpinIt()
	{
		//if (GameManager.instance.networkIds [GameManager.instance.currentPlayer] == this.netId) {
			CmdSpinCube();
			//this.CmdSpinCube();
//		}
//		else
//		{
//			Debug.LogError ("NetId mismatch");
//		}
	}


}