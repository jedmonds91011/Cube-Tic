#pragma strict

private var smooth : float = 5.0;
private var tiltAngle : float = 30.0;
private var number = 0;

public var moveSpeed : float = 10f;
public var turnSpeed : float = 1f;

private var gameLogic : GameLogic;

function Awake()
{
	gameLogic = GameObject.FindGameObjectWithTag("GLogic").GetComponent(GameLogic);
}

function Update () {
    if(Input.GetKey(KeyCode.Insert)){
        transform.Rotate(Vector3(number, 0, 8), turnSpeed * Time.deltaTime);
		number++;
		gameLogic.hasBeenSpun = true;
	}    
    if(Input.GetKey(KeyCode.Delete)){
        transform.Rotate(Vector3(number, 0, 8), -turnSpeed * Time.deltaTime);	
		number++;
		gameLogic.hasBeenSpun = true;
	}	
    if(Input.GetKey(KeyCode.UpArrow)){
        transform.Rotate(Vector3(0, 8, number), turnSpeed * Time.deltaTime);
		number++;
		gameLogic.hasBeenSpun = true;
	}    
    if(Input.GetKey(KeyCode.DownArrow)){
        transform.Rotate(Vector3(0, 8, number), -turnSpeed * Time.deltaTime);
		number++;
		gameLogic.hasBeenSpun = true;
	}    
    if(Input.GetKey(KeyCode.LeftArrow)){
        transform.Rotate(Vector3(0, number, 8), -turnSpeed * Time.deltaTime);
		number++;
		gameLogic.hasBeenSpun = true;
	}
    if(Input.GetKey(KeyCode.RightArrow)){
		transform.Rotate(Vector3(0, number, 8), turnSpeed * Time.deltaTime);				
		number++;
		gameLogic.hasBeenSpun = true;
	}
    if(Input.GetKey("escape")){
		Application.Quit();
	}
	//Debug.Log(number);
}