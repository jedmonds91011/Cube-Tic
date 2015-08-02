using UnityEngine;
using System.Collections;

public class SpinIt : MonoBehaviour {
	
	private float smooth = 5.0f;
	private float tiltAngle = 30.0f;
	private int number = 0;
	
	public float moveSpeed = 10.0f;
	public float turnSpeed = 1.0f;
	
	private GameManager gameManager;
	
	// Use this for initialization
	void Awake () 
	{
		gameManager = GameObject.FindGameObjectWithTag("GLogic").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.Insert))
		{
			transform.Rotate(new Vector3(number, 0, 8), turnSpeed * Time.deltaTime);
			number++;
			gameManager.hasBeenSpun = true;
		}    
		
		if(Input.GetKey(KeyCode.Delete))
		{
			transform.Rotate(new Vector3(number, 0, 8), -turnSpeed * Time.deltaTime);	
			number++;
			gameManager.hasBeenSpun = true;
		}
		
		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.Rotate(new Vector3(0, 8, number), turnSpeed * Time.deltaTime);
			number++;
			gameManager.hasBeenSpun = true;
		}    
		
		if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.Rotate(new Vector3(0, 8, number), -turnSpeed * Time.deltaTime);
			number++;
			gameManager.hasBeenSpun = true;
		}    
		
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Rotate(new Vector3(0, number, 8), -turnSpeed * Time.deltaTime);
			number++;
			gameManager.hasBeenSpun = true;
		}
		
		if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.Rotate(new Vector3(0, number, 8), turnSpeed * Time.deltaTime);				
			number++;
			gameManager.hasBeenSpun = true;
		}
		
		if(Input.GetKey("escape"))
		{
			Application.Quit();
		}
		//Debug.Log(number);
	}
}