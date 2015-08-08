using UnityEngine;
using System.Collections;

public class NetworkController : MonoBehaviour {

	Vector3 lastPosition;
    float minimumMovement = .05f;
    NetworkView networkView;

    void Start()
    {
        networkView = GetComponent<NetworkView>();
    }
	// Update is called once per frame
	void Update () {
	
		if(Network.isServer)
		{
			Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
			float speed = 5;
			transform.Translate (speed *moveDir * Time.deltaTime);

            if(Vector3.Distance(transform.position, lastPosition) > minimumMovement)
            {
                lastPosition = transform.position;
                networkView.RPC("SetPosition", RPCMode.Others, transform.position);
            }
		}
     
	}
    [RPC]
    void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
//	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
//	{
//		if (stream.isWriting)
//        {
//            Vector3 pos = transform.position;
//            stream.Serialize(ref pos);
//        } else
//        {
//            Vector3 receivedPosition = Vector3.zero;
//            stream.Serialize(ref receivedPosition);
//            transform.position = receivedPosition;
//        }
//	}
}
