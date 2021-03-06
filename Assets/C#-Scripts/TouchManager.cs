﻿using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour, IPointerDownHandler {

	private Vector2 touchStart;
	private Vector2 touchEnd;
	private bool dragging;
	private float timeDifference;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.touches.Length >0 && !GameManager.instance.gameIsOver)
		{
			Touch myTouch = Input.GetTouch(0);
			if(myTouch.phase == TouchPhase.Began)
			{
				touchStart = myTouch.position;
				touchEnd = myTouch.position;
				timeDifference = Time.time;
			}
			if(myTouch.phase == TouchPhase.Moved)
			{
				touchEnd = myTouch.position;
				if(timeDifference - Time.time > 0.1f)
				{
					GameManager.instance.resetButton.interactable = false;
					GameManager.instance.endGameButton.interactable = false;
				}
			}
			if(myTouch.phase == TouchPhase.Ended && !GameManager.instance.cubeSpinning && !GameManager.instance.hasBeenSpun)
			{
				float touchDelta = Mathf.Abs(touchEnd.x - touchStart.x)+ Mathf.Abs (touchEnd.y - touchStart.y);
				if(touchDelta > 75)
				{
					timeDifference = Time.time - timeDifference;
					GameManager.instance.cubeSpinSpeed = Mathf.Clamp ((75 - (timeDifference * 100)),40,60);
					GameManager.instance.numCubeSpins = Mathf.Clamp (Mathf.RoundToInt(GameManager.instance.cubeSpinSpeed-45),5,25);
					GameManager.instance.SpinCube();
				}
				GameManager.instance.resetButton.interactable = true;
				GameManager.instance.endGameButton.interactable = true;
			}
		}	
//		if(Input.GetMouseButtonDown(0) && !GameManager.instance.cubeSpinning && !GameManager.instance.hasBeenSpun)
//		{
//			touchStart = Input.mousePosition;
//			timeDifference = Time.time;
//		}
//		if(Input.GetMouseButtonUp(0) && !GameManager.instance.cubeSpinning && !GameManager.instance.hasBeenSpun)
//		{
//			touchEnd = Input.mousePosition;
//			timeDifference = Time.time - timeDifference;
//
//			float touchDelta = Mathf.Abs(touchEnd.x - touchStart.x)+ Mathf.Abs (touchEnd.y - touchStart.y);
//			if(touchDelta > 75)
//			{
//				GameManager.instance.cubeSpinSpeed = Mathf.Clamp ((75 - (timeDifference * 100)),40,60);
//				GameManager.instance.numCubeSpins = Mathf.Clamp (Mathf.RoundToInt(GameManager.instance.cubeSpinSpeed-45),5,25);
//				GameManager.instance.SpinCube();
////				Debug.LogError (timeDifference * 100);
////				Debug.LogError ("New # of spins:" + Mathf.Clamp (Mathf.RoundToInt(GameManager.instance.cubeSpinSpeed-45),5,25));
////				Debug.LogError("New cube speed: " +Mathf.Clamp ((75 - (timeDifference * 100)),40,60));
//			}
//			GameManager.instance.cubeSpinSpeed = 40;
//			GameManager.instance.numCubeSpins = 10;
//		}
//	
	}

	public void OnPointerDown(PointerEventData data)
	{
		dragging = true;
		
	}
}