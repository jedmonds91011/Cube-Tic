using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof (PhotonView))]
public class GameManager : MonoBehaviour{
	public static GameManager instance;
	public AudioSource source;
	public AudioSource backgroundMusic;
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

    public Image waitingCanvas;
    public Text waitingText;
    public float numDots;

	public GameObject PanelXPlayer;
	public GameObject PanelOPlayer;
	private Vector3[] endVector = new Vector3[6];
	public List<Vector3> endCube = new List<Vector3>();
	public List<AudioClip> soundClips = new List<AudioClip> ();

	public int numCubeSpins;
	public float cubeSpinSpeed;
	public int endFaceIndex;
	public Vector3 endCubeVector;


	public List<int> networkIds;
	public List<networkTest> players;

    public bool networkMatch;
    public bool gameReady;
    public PhotonView photonView;



	//public Vector3[] endCube = new Vector3[9];
	
	// Use this for initialization
	void Awake()
	{
        gameReady = false;
        networkMatch = true;
        if (instance == null)
        {
            instance = this;
        }
        backgroundMusic.Play ();
		source = GetComponent<AudioSource> ();
		gameIsOver = false;
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
                if (!cubeSpinning)
                {
                    if (players[currentPlayer].myPhotonView.isMine && players[currentPlayer].myIndex == currentPlayer)
                    {
                        getEndCubeFace();
                        photonView.RPC("spinCubeRPC", PhotonTargets.AllBufferedViaServer, endCubeVector);
                    }
                    else
                    {
                        UpdateInfo("Sorry,it is not your turn!");
                    }
                }
            }
            else
            {
                if (players[currentPlayer].myPhotonView.isMine && players[currentPlayer].myIndex == currentPlayer)
                {
                    UpdateInfo("The cube has been spun, click a square to place your piece");
                }
                else
                {
                    UpdateInfo("Sorry, it's not your turn!");
                }
            }
		}
        else
        {
            spinButton.interactable = false;
        }
	}

    [PunRPC]
    public void spinCubeRPC(Vector3 target)
    {
        cubeSpinSpeed = 50;
        instance.numCubeSpins = 15;
        endCubeVector = target;
        SpinCube();
    }

    public void SpinCube()
	{
		cubeSpinning = true;
		source.PlayOneShot (soundClips [0]);
		StartCoroutine(CubeRotation());
	}

    IEnumerator CubeRotation ()
	{
		float elapsedTime = 0;
		float time = 0.1f;
		
		int spinCenter = 0;
		int max = endCube.Count;
		
		Quaternion rotationTo = Random.rotationUniform;
		while (spinCenter < numCubeSpins)//elapsedTime < time)
		{
			elapsedTime += (cubeSpinSpeed * Time.deltaTime);
			cube.transform.rotation = Quaternion.Lerp (cube.transform.rotation,rotationTo,cubeSpinSpeed*Time.deltaTime);
			yield return new WaitForEndOfFrame();
			if(Quaternion.Angle(cube.transform.rotation,rotationTo) <= 1.0f)
			{
				spinCenter++;
				elapsedTime = 0;
				rotationTo = Random.rotationUniform;
			}
		}
		//final rotate, get the id of the final rotational axis from endCube bag.
		//Reset elapsed time for Slerp to final destination.
		int id = Random.Range (0, max);
		elapsedTime = 0f;
		rotationTo = Quaternion.Euler (endCubeVector);
		while (Quaternion.Angle(cube.transform.rotation,rotationTo) >= 1.0f)//elapsedTime < time)
		{
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

	public void getEndCubeFace()
	{
		endCube.Clear();
		for (int i = 0; i < 6; i++)
		{	
			for (int j = 0; j < (9 - aSides[i]); j++)
			{
				endCube.Add(endVector[i]);
			}
		}
		endFaceIndex = Random.Range (0, endCube.Count);
		endCubeVector = endCube [endFaceIndex];
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
		Application.LoadLevel(1);
	}

	public void ChangePlayer()
	{
		currentPlayer++;
		if(currentPlayer > 1)
		{
			currentPlayer = 0;
			PanelXPlayer.SetActive(true);
			PanelOPlayer.SetActive(false);
		}
		else
		{
			PanelXPlayer.SetActive(false);
			PanelOPlayer.SetActive(true);
		}
	}

	void UpdateX(int cbx)
	{
		source.PlayOneShot (soundClips [2]);
		//xCount++;
		xCount++;
		xCount = xCount;
		wCombos[cbx] = 1;
		UpdateXCount(xCount);
	}

	void UpdateO(int cbo)
	{
		
		source.PlayOneShot (soundClips [2]);
		//oCount++;
		oCount++;
		oCount = oCount;
		wCombos[cbo] = 2;
		UpdateOCount(oCount);
	}
	
	void CheckForWinner()
	{
		//******************************************************************************************************
		// array aSquares 100 - 622 are all set to 9 @ gameManager Start
		// array wCombos are left at 0
		// when 3 X's = 0 (those squares are set = 0, 3*0=0) it's winning combo
		// when 3 O's = 3 (those squares are set= 1, 3*1=3) it's winning combo
		// any square w/ a 9 adds to > 3 or 0
		// wCombos[1 - 96] is set to X=1, O=2 or non-zero
		// x or o Count is ++ and displayed
		// Can't click in a used square
		// clickCount > 54 means all squares used up -> GAME OVER!!
		//******************************************************************************************************	
		//Debug.Log("Started Winner check @ " + Time.time);
		
		// ********** BACK YELLOW SIDE =1 ************
		// Cross wised
		if ((aSquares[100] + aSquares[111] + aSquares[122] == 0) && (wCombos[11] == 0)) UpdateX(11);
		if ((aSquares[102] + aSquares[111] + aSquares[120] == 0) && (wCombos[12] == 0)) UpdateX(12);
		if ((aSquares[100] + aSquares[111] + aSquares[122] == 3) && (wCombos[11] == 0)) UpdateO(11);
		if ((aSquares[102] + aSquares[111] + aSquares[120] == 3) && (wCombos[12] == 0)) UpdateO(12);
		// Horizontal
		if ((aSquares[100] + aSquares[110] + aSquares[120] == 0) && (wCombos[13] == 0)) UpdateX(13);
		if ((aSquares[101] + aSquares[111] + aSquares[121] == 0) && (wCombos[14] == 0)) UpdateX(14);
		if ((aSquares[102] + aSquares[112] + aSquares[122] == 0) && (wCombos[15] == 0)) UpdateX(15);
		if ((aSquares[100] + aSquares[110] + aSquares[120] == 3) && (wCombos[13] == 0)) UpdateO(13);
		if ((aSquares[101] + aSquares[111] + aSquares[121] == 3) && (wCombos[14] == 0)) UpdateO(14);
		if ((aSquares[102] + aSquares[112] + aSquares[122] == 3) && (wCombos[15] == 0)) UpdateO(15);
		// Vertical
		if ((aSquares[100] + aSquares[101] + aSquares[102] == 0) && (wCombos[16] == 0)) UpdateX(16);
		if ((aSquares[110] + aSquares[111] + aSquares[112] == 0) && (wCombos[17] == 0)) UpdateX(17);
		if ((aSquares[120] + aSquares[121] + aSquares[122] == 0) && (wCombos[18] == 0)) UpdateX(18);
		if ((aSquares[100] + aSquares[101] + aSquares[102] == 3) && (wCombos[16] == 0)) UpdateO(16);
		if ((aSquares[110] + aSquares[111] + aSquares[112] == 3) && (wCombos[17] == 0)) UpdateO(17);
		if ((aSquares[120] + aSquares[121] + aSquares[122] == 3) && (wCombos[18] == 0)) UpdateO(18);
		
		// ********** BOTTOM GREEN SIDE =2 ************
		// Cross wised
		if ((aSquares[200] + aSquares[211] + aSquares[222] == 0) && (wCombos[21] == 0)) UpdateX(21);
		if ((aSquares[202] + aSquares[211] + aSquares[220] == 0) && (wCombos[22] == 0)) UpdateX(22);
		if ((aSquares[200] + aSquares[211] + aSquares[222] == 3) && (wCombos[21] == 0)) UpdateO(21);
		if ((aSquares[202] + aSquares[211] + aSquares[220] == 3) && (wCombos[22] == 0)) UpdateO(22);
		// Horizontal
		if ((aSquares[200] + aSquares[210] + aSquares[220] == 0) && (wCombos[23] == 0)) UpdateX(23);
		if ((aSquares[201] + aSquares[211] + aSquares[221] == 0) && (wCombos[24] == 0)) UpdateX(24);
		if ((aSquares[202] + aSquares[212] + aSquares[222] == 0) && (wCombos[25] == 0)) UpdateX(25);
		if ((aSquares[200] + aSquares[210] + aSquares[220] == 3) && (wCombos[23] == 0)) UpdateO(23);
		if ((aSquares[201] + aSquares[211] + aSquares[221] == 3) && (wCombos[24] == 0)) UpdateO(24);
		if ((aSquares[202] + aSquares[212] + aSquares[222] == 3) && (wCombos[25] == 0)) UpdateO(25);
		// Vertical
		if ((aSquares[200] + aSquares[201] + aSquares[202] == 0) && (wCombos[26] == 0)) UpdateX(26);
		if ((aSquares[210] + aSquares[211] + aSquares[212] == 0) && (wCombos[27] == 0)) UpdateX(27);
		if ((aSquares[220] + aSquares[221] + aSquares[222] == 0) && (wCombos[28] == 0)) UpdateX(28);
		if ((aSquares[200] + aSquares[201] + aSquares[202] == 3) && (wCombos[26] == 0)) UpdateO(26);
		if ((aSquares[210] + aSquares[211] + aSquares[212] == 3) && (wCombos[27] == 0)) UpdateO(27);
		if ((aSquares[220] + aSquares[221] + aSquares[222] == 3) && (wCombos[28] == 0)) UpdateO(28);
		
		// ********** FRONT BLUE SIDE =3 ************
		// Cross wised
		if ((aSquares[300] + aSquares[311] + aSquares[322] == 0) && (wCombos[31] == 0)) UpdateX(31);
		if ((aSquares[302] + aSquares[311] + aSquares[320] == 0) && (wCombos[32] == 0)) UpdateX(32);
		if ((aSquares[300] + aSquares[311] + aSquares[322] == 3) && (wCombos[31] == 0)) UpdateO(31);
		if ((aSquares[302] + aSquares[311] + aSquares[320] == 3) && (wCombos[32] == 0)) UpdateO(32);
		// Horizontal
		if ((aSquares[300] + aSquares[310] + aSquares[320] == 0) && (wCombos[33] == 0)) UpdateX(33);
		if ((aSquares[301] + aSquares[311] + aSquares[321] == 0) && (wCombos[34] == 0)) UpdateX(34);
		if ((aSquares[302] + aSquares[312] + aSquares[322] == 0) && (wCombos[35] == 0)) UpdateX(35);
		if ((aSquares[300] + aSquares[310] + aSquares[320] == 3) && (wCombos[33] == 0)) UpdateO(33);
		if ((aSquares[301] + aSquares[311] + aSquares[321] == 3) && (wCombos[34] == 0)) UpdateO(34);
		if ((aSquares[302] + aSquares[312] + aSquares[322] == 3) && (wCombos[35] == 0)) UpdateO(35);
		// Vertical
		if ((aSquares[300] + aSquares[301] + aSquares[302] == 0) && (wCombos[36] == 0)) UpdateX(36);
		if ((aSquares[310] + aSquares[311] + aSquares[312] == 0) && (wCombos[37] == 0)) UpdateX(37);
		if ((aSquares[320] + aSquares[321] + aSquares[322] == 0) && (wCombos[38] == 0)) UpdateX(38);
		if ((aSquares[300] + aSquares[301] + aSquares[302] == 3) && (wCombos[36] == 0)) UpdateO(36);
		if ((aSquares[310] + aSquares[311] + aSquares[312] == 3) && (wCombos[37] == 0)) UpdateO(37);
		if ((aSquares[320] + aSquares[321] + aSquares[322] == 3) && (wCombos[38] == 0)) UpdateO(38);
		
		// ********** LEFT RED =4 ************
		// Cross wised
		if ((aSquares[400] + aSquares[411] + aSquares[422] == 0) && (wCombos[41] == 0)) UpdateX(41);
		if ((aSquares[402] + aSquares[411] + aSquares[420] == 0) && (wCombos[42] == 0)) UpdateX(42);
		if ((aSquares[400] + aSquares[411] + aSquares[422] == 3) && (wCombos[41] == 0)) UpdateO(41);
		if ((aSquares[402] + aSquares[411] + aSquares[420] == 3) && (wCombos[42] == 0)) UpdateO(42);
		// Horizontal
		if ((aSquares[400] + aSquares[410] + aSquares[420] == 0) && (wCombos[43] == 0)) UpdateX(43);
		if ((aSquares[401] + aSquares[411] + aSquares[421] == 0) && (wCombos[44] == 0)) UpdateX(44);
		if ((aSquares[402] + aSquares[412] + aSquares[422] == 0) && (wCombos[45] == 0)) UpdateX(45);
		if ((aSquares[400] + aSquares[410] + aSquares[420] == 3) && (wCombos[43] == 0)) UpdateO(43);
		if ((aSquares[401] + aSquares[411] + aSquares[421] == 3) && (wCombos[44] == 0)) UpdateO(44);
		if ((aSquares[402] + aSquares[412] + aSquares[422] == 3) && (wCombos[45] == 0)) UpdateO(45);
		// Vertical
		if ((aSquares[400] + aSquares[401] + aSquares[402] == 0) && (wCombos[46] == 0)) UpdateX(46);
		if ((aSquares[410] + aSquares[411] + aSquares[412] == 0) && (wCombos[47] == 0)) UpdateX(47);
		if ((aSquares[420] + aSquares[421] + aSquares[422] == 0) && (wCombos[48] == 0)) UpdateX(48);
		if ((aSquares[400] + aSquares[401] + aSquares[402] == 3) && (wCombos[46] == 0)) UpdateO(46);
		if ((aSquares[410] + aSquares[411] + aSquares[412] == 3) && (wCombos[47] == 0)) UpdateO(47);
		if ((aSquares[420] + aSquares[421] + aSquares[422] == 3) && (wCombos[48] == 0)) UpdateO(48);
		
		// ********** RIGHT WHITE =5 ************
		// Cross wised
		if ((aSquares[500] + aSquares[511] + aSquares[522] == 0) && (wCombos[51] == 0)) UpdateX(51);
		if ((aSquares[502] + aSquares[511] + aSquares[520] == 0) && (wCombos[52] == 0)) UpdateX(52);
		if ((aSquares[500] + aSquares[511] + aSquares[522] == 3) && (wCombos[51] == 0)) UpdateO(51);
		if ((aSquares[502] + aSquares[511] + aSquares[520] == 3) && (wCombos[52] == 0)) UpdateO(52);
		// Horizontal
		if ((aSquares[500] + aSquares[510] + aSquares[520] == 0) && (wCombos[53] == 0)) UpdateX(53);
		if ((aSquares[501] + aSquares[511] + aSquares[521] == 0) && (wCombos[54] == 0)) UpdateX(54);
		if ((aSquares[502] + aSquares[512] + aSquares[522] == 0) && (wCombos[55] == 0)) UpdateX(55);
		if ((aSquares[500] + aSquares[510] + aSquares[520] == 3) && (wCombos[53] == 0)) UpdateO(53);
		if ((aSquares[501] + aSquares[511] + aSquares[521] == 3) && (wCombos[54] == 0)) UpdateO(54);
		if ((aSquares[502] + aSquares[512] + aSquares[522] == 3) && (wCombos[55] == 0)) UpdateO(55);
		// Vertical
		if ((aSquares[500] + aSquares[501] + aSquares[502] == 0) && (wCombos[56] == 0)) UpdateX(56);
		if ((aSquares[510] + aSquares[511] + aSquares[512] == 0) && (wCombos[57] == 0)) UpdateX(57);
		if ((aSquares[520] + aSquares[521] + aSquares[522] == 0) && (wCombos[58] == 0)) UpdateX(58);
		if ((aSquares[500] + aSquares[501] + aSquares[502] == 3) && (wCombos[56] == 0)) UpdateO(56);
		if ((aSquares[510] + aSquares[511] + aSquares[512] == 3) && (wCombos[57] == 0)) UpdateO(57);
		if ((aSquares[520] + aSquares[521] + aSquares[522] == 3) && (wCombos[58] == 0)) UpdateO(58);
		
		// ********** TOP ORANGE =6 ************
		// Cross wised
		if ((aSquares[600] + aSquares[611] + aSquares[622] == 0) && (wCombos[61] == 0)) UpdateX(61);
		if ((aSquares[602] + aSquares[611] + aSquares[620] == 0) && (wCombos[62] == 0)) UpdateX(62);
		if ((aSquares[600] + aSquares[611] + aSquares[622] == 3) && (wCombos[61] == 0)) UpdateO(61);
		if ((aSquares[602] + aSquares[611] + aSquares[620] == 3) && (wCombos[62] == 0)) UpdateO(62);
		// Horizontal
		if ((aSquares[600] + aSquares[610] + aSquares[620] == 0) && (wCombos[63] == 0)) UpdateX(63);
		if ((aSquares[601] + aSquares[611] + aSquares[621] == 0) && (wCombos[64] == 0)) UpdateX(64);
		if ((aSquares[602] + aSquares[612] + aSquares[622] == 0) && (wCombos[65] == 0)) UpdateX(65);
		if ((aSquares[600] + aSquares[610] + aSquares[620] == 3) && (wCombos[63] == 0)) UpdateO(63);
		if ((aSquares[601] + aSquares[611] + aSquares[621] == 3) && (wCombos[64] == 0)) UpdateO(64);
		if ((aSquares[602] + aSquares[612] + aSquares[622] == 3) && (wCombos[65] == 0)) UpdateO(65);
		// Vertical
		if ((aSquares[600] + aSquares[601] + aSquares[602] == 0) && (wCombos[66] == 0)) UpdateX(66);
		if ((aSquares[610] + aSquares[611] + aSquares[612] == 0) && (wCombos[67] == 0)) UpdateX(67);
		if ((aSquares[620] + aSquares[621] + aSquares[622] == 0) && (wCombos[68] == 0)) UpdateX(68);
		if ((aSquares[600] + aSquares[601] + aSquares[602] == 3) && (wCombos[66] == 0)) UpdateO(66);
		if ((aSquares[610] + aSquares[611] + aSquares[612] == 3) && (wCombos[67] == 0)) UpdateO(67);
		if ((aSquares[620] + aSquares[621] + aSquares[622] == 3) && (wCombos[68] == 0)) UpdateO(68);
		
		if (clickCount >= 54)
		{
			gameIsOver = true;
			backgroundMusic.Stop ();
			source.PlayOneShot(soundClips[3]);
			//gameIsOver = true;
			if (xCount == oCount) 
			{
				UpdateInfo("Game Over!, It's a TIE, NOOOOOOOO");
			}
			else if (xCount > oCount)
			{
				UpdateInfo("Game Over!, X Wins!, Hooray!");
			}
			else 
			{
				UpdateInfo("Game Over!, O Wins!, BOOOOO!");		
			}
			Debug.Log("after game info X =" + xCount + " O =" + oCount);
		}
		else
		{
			UpdateClickCount(clickCount);
		}
	}

	public void playSquare(GameObject square)
	{
        if (players[currentPlayer].myPhotonView.isMine && players[currentPlayer].myIndex == currentPlayer)
        {
            photonView.RPC("playSquareRPC", PhotonTargets.AllBufferedViaServer, square.GetPhotonView().viewID);
        }
    }

    [PunRPC]
    public void playSquareRPC(int square)
    {
        ClickSquare pieceScript = PhotonView.Find(square).GetComponent<ClickSquare>(); //piece.GetComponent<ClickSquare>();
        if (!pieceScript.isUsed)
        {
            source.PlayOneShot(soundClips[4]);
            pieceScript.myPieces[currentPlayer].SetActive(true);
            pieceScript.isUsed = true;
            int answer = (pieceScript.side * 100) + (pieceScript.x * 10) + (pieceScript.y * 1);
            aSquares[answer] = currentPlayer;
            clickCount++;
            aSides[pieceScript.side - 1]++;
            hasBeenSpun = false;
        }
        CheckForWinner();
        ChangePlayer();
        if (!GameManager.instance.gameIsOver)
        {
            GameManager.instance.UpdateInfo("Ok, " + GameManager.instance.playerName[GameManager.instance.currentPlayer] + " it's your turn to Spin");
        }
    }

	public void BackToLobby()
	{
		Application.Quit ();
	}

    void Update()
    {
        if (!gameReady && networkMatch)
        {
            if (waitingCanvas != null && waitingText != null)
            {
                numDots += Time.deltaTime;
                numDots %= 3.0f;
                string waitText = "Waiting for other player";
                for (int i = 0; i < numDots; i++)
                {
                    waitText += ".";
                }
                waitingText.text = waitText;
            }
        }
    }

    public void initMultiplayer()
    {
        StartCoroutine(initCoRoutine());
    }

    IEnumerator initCoRoutine()
    {
        waitingText.color = Color.white;
        while (PhotonNetwork.playerList.Length < 2)
        {
            yield return 0;
        }
        Debug.LogError(PhotonNetwork.playerName + " has connected.");
        PhotonNetwork.room.open = false;
        photonView = GetComponent<PhotonView>();
        waitingCanvas.enabled = false;
        waitingText.enabled = false;
        networkTest temp = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0).GetComponent<networkTest>();
        photonView.RPC("initPlayer", PhotonTargets.AllBufferedViaServer,temp.myPhotonView.viewID);
    }

    [PunRPC]
    public void initPlayer(int viewID)
    {
        networkTest temp = PhotonView.Find(viewID).GetComponent<networkTest>();
        temp.myIndex = players.Count;
        players.Add(temp);
    }


}