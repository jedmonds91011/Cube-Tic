using UnityEngine;
using UnityEngine.Networking;

public class networkTest : NetworkBehaviour
{
	[Command]
	public void CmdSpinCube()
	{
		//GameManager.instance.SpinCube ();
		setSpinParameters ();
		spin ();
		GameManager.instance.SpinCube ();
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
		setSpinFace (index);
		GameManager.instance.SpinButton ();
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

}