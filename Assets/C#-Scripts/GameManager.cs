﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	public int currentPlayer = 0;
	public string[] playerObjName = new string[] {"X-obj", "O-obj"};
	public string[] playerName = new string[] {"Scott", "Elliot"};
	public bool gameIsOver = false;
	public bool hasBeenSpun = false;
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
	
	private Vector3[] endVector = new Vector3[6];
	private Vector3[] endCube = new Vector3[9];
	
	// Use this for initialization
	void Start () 
	{
		UpdateInfo("Player " + playerName[currentPlayer] + " begins the game, Give it a good Spin!");
		
		endVector[0] = (new Vector3(20,180,-135));	// Yellow side	#1  Id-0
		endVector[1] = (new Vector3(45,72,63));		// Green side	#2  Id-1
		endVector[2] = (new Vector3(-20,0,45));		// Blue side	#3  Id-2
		endVector[3] = (new Vector3(-45,-105,23));	// Red side		#4  Id-3
		endVector[4] = (new Vector3(45,73,-25));	// White side	#5  Id-4
		endVector[5] = (new Vector3(-45,-107,113));	// Magenta side	#6  Id-5
		
		//what does this do??
		for (var j = 100; j < 623; j++) aSquares[j] = 9;
		
	}
	
	public void SpinButton ()
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
	
	void SpinCube()
	{
		// clear any prev cube destination
		System.Array.Clear(endCube,0,endCube.Length);
		
		
		for (int i = 0; i < 6; i++)
		{	
			for (int j = 0; j < (9 - aSides[i]); j++)
			{
				endCube[i] = endVector[i];
			}
		}
		
		StartCoroutine( CubeRotation());
		
	}
	
	IEnumerator CubeRotation ()
	{
		
		float elapsedTime = 0;
		float time = 0.1f;
		
		int spinCenter = 0;
		int max = endCube.Length;
		
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
		
		while (spinCenter < 10)
		{
			cube.transform.rotation = Random.rotation;
			yield return new WaitForSeconds(0.08f); // and let Unity free till the next frame	
			spinCenter++;
		}
		
		//int id = Random.Range(0, max);
		int id = Random.Range (0, endVector.Length);
		elapsedTime = 0;
		//final rotate
		while (elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			
			cube.transform.eulerAngles = Vector3.Lerp(cube.transform.eulerAngles, endVector[id], (elapsedTime / time));
		}
		
		hasBeenSpun = true;
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