using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public AudioSource source;
	public int currentPlayer = 0;
	public string[] playerObjName = new string[] {"X-obj", "O-obj"};
	public string[] playerName = new string[] {"Scott", "Elliot"};
	public bool gameIsOver = false;
	public bool hasBeenSpun = false;
	public bool cubeSpinning = false;
	public int clickCount;
	public int xCount;
	public int oCount;
	public int[] aSquares = new int[700];
	public int[] wCombos = new int[100];
	public int[] aSides = new int[7];
	
	public GameObject cube;
	public Text iBox;
	public Text xBox;
	public Text oBox;
	public Text cBox;
	public Text spinButtonText;
	public Button spinButton;
	public Button resetButton;
	public Button endGameButton;

	public GameObject PanelXPlayer;
	public GameObject PanelOPlayer;
	private Vector3[] endVector = new Vector3[6];
	public List<Vector3> endCube = new List<Vector3>();
	public List<AudioClip> soundClips = new List<AudioClip> ();

	public int numCubeSpins;
	public float cubeSpinSpeed;
	//public Vector3[] endCube = new Vector3[9];
	
	// Use this for initialization
	void Awake()
	{
		instance = this;
		source = GetComponent<AudioSource> ();
		if (Application.platform == RuntimePlatform.Android) {
			spinButton.enabled= false;
			spinButton.image.enabled = false;
			spinButtonText.enabled = false;

		}
	}
	void Start () 
	{

		UpdateInfo("Player " + playerName[currentPlayer] + " begins the game, Give it a good Spin!");
		
		endVector[0] = (new Vector3(20,180,-135));	// Yellow side	#1  Id-0
		endVector[1] = (new Vector3(45,72,63));		// Green side	#2  Id-1
		endVector[2] = (new Vector3(-20,0,45));		// Blue side	#3  Id-2
		endVector[3] = (new Vector3(-45,-105,23));	// Red side		#4  Id-3
		endVector[4] = (new Vector3(45,73,-25));	// White side	#5  Id-4
		endVector[5] = (new Vector3(-45,-107,113));	// Magenta side	#6  Id-5

		PanelXPlayer.SetActive (true);
		PanelOPlayer.SetActive (false);
		
		//what does this do??
		for (var j = 100; j < 623; j++) aSquares[j] = 9;
		numCubeSpins = 15;
		cubeSpinSpeed = 50;
	}

	public void SpinButton ()
	{
		if(!gameIsOver)
		{
			if (!hasBeenSpun)
			{
				SpinCube();
			}
			else
			{
				UpdateInfo("Sorry, already Spun! Now you gotta Click!");
			}
		}
		else
		{
			//UpdateInfo ("
			spinButton.interactable = false;
		}
	}
	
	public void SpinCube()
	{
		// clear any prev cube destination
		//System.Array.Clear(endCube,0,endCube.Length);
		cubeSpinning = true;
		source.PlayOneShot (soundClips [0]);
		endCube.Clear();
		for (int i = 0; i < 6; i++)
		{	
			for (int j = 0; j < (9 - aSides[i]); j++)
			{
				endCube.Add(endVector[i]);
			}
		}
		
		StartCoroutine(CubeRotation());
	}
	
	IEnumerator CubeRotation ()
	{
		float elapsedTime = 0;
		float time = 0.1f;
		
		int spinCenter = 0;
		int max = endCube.Count;
		
		/*while(spinCenter < 10)
		{
			while (elapsedTime < time) {
				elapsedTime += Time.deltaTime; 
				
				// Rotations
				cube.transform.rotation = Quaternion.Slerp(cube.transform.rotation, Random.rotation,  (elapsedTime / time));
				yield return new WaitForEndOfFrame ();
			}
			spinCenter++;
		}*/
		
//		while (spinCenter < 10)
//		{
//			cube.transform.rotation = Random.rotation;
//			yield return new WaitForSeconds(0.08f); // and let Unity free till the next frame	
//			spinCenter++;
//		}

		//float speed =40f;
		//int faceIndex = Random.Range (0, 6);
		//Quaternion rotationTo = Quaternion.Euler(endVector[Random.Range(0,6)]);
		Quaternion rotationTo = Random.rotationUniform;
		//Quaternion rotationTo = Quaternion.Euler (new Vector3(cube.transform.localRotation.x - 180f, cube.transform.localRotation.y - 180f, cube.transform.localRotation.z- 180f));
		while (spinCenter < numCubeSpins)//elapsedTime < time)
		{
			//cube.transform.eulerAngles = Vector3.Lerp(cube.transform.eulerAngles, endCube[id], (elapsedTime / time));
			
			elapsedTime += (cubeSpinSpeed * Time.deltaTime);
			cube.transform.rotation = Quaternion.Lerp (cube.transform.rotation,rotationTo,cubeSpinSpeed*Time.deltaTime);
			yield return new WaitForEndOfFrame();
			if(Quaternion.Angle(cube.transform.rotation,rotationTo) <= 1.0f)
			{
				//faceIndex++;
				//faceIndex %= endVector.Length;
				spinCenter++;
				elapsedTime = 0;
				//rotationTo = Quaternion.Euler(endVector[Random.Range (0,6)]);
				rotationTo = Random.rotationUniform;

			}
		}

		//final rotate, get the id of the final rotational axis from endCube bag.
		//Reset elapsed time for Slerp to final destination.
		int id = Random.Range (0, max);
		elapsedTime = 0f;
		rotationTo = Quaternion.Euler (endCube [id]);
		while (Quaternion.Angle(cube.transform.rotation,rotationTo) >= 1.0f)//elapsedTime < time)
		{
			//cube.transform.eulerAngles = Vector3.Lerp(cube.transform.eulerAngles, endCube[id], (elapsedTime / time));

			elapsedTime += (cubeSpinSpeed * Time.deltaTime);
			cube.transform.rotation = Quaternion.Lerp (cube.transform.rotation,rotationTo,7.0f*Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		cube.transform.rotation = rotationTo;
		cubeSpinning = false;
		hasBeenSpun = true;
		cubeSpinSpeed = 50;
		numCubeSpins = 15;
		source.PlayOneShot (soundClips [1]);
		UpdateInfo("Good Spin, " + playerName[currentPlayer] + " it's time to pick a square");
	}
	
	public void UpdateClickCount(int clkCnt)
	{
		cBox.text = clkCnt.ToString();
	}
	
	public void UpdateInfo (string text)
	{
		iBox.text = text;	
	}
	
	public void UpdateXCount(int xCentr)
	{
		xBox.text = xCentr.ToString();
	}
	
	public void UpdateOCount(int oCntr)
	{
		oBox.text = oCntr.ToString();
	}
	
	public void reStart()
	{
		Application.LoadLevel(0);
	}
}