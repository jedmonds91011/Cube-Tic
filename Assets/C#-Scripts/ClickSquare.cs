using UnityEngine;
using System.Collections;

public class ClickSquare : MonoBehaviour {
	
	public int side;
	public int x;
	public int y;
	
	private bool isUsed = false;
	private GameManager gameManager;
	
	// Use this for initialization
	void Awake () 	
	{
		gameManager = GameObject.FindGameObjectWithTag("GLogic").GetComponent<GameManager>();
	}
	
	// Change for touch
	void OnMouseDown () 
	{
		if (gameManager.hasBeenSpun)
		{
			if(!isUsed)
			{
				foreach(Transform child in transform)
				{
					if(child.name == gameManager.playerObjName[gameManager.currentPlayer])
					{
						//child.active = true; DIDN'T WORK W/ 5.0
						child.gameObject.SetActive(true);
						//Debug.Log( "BEFORE " + this.gameObject.name + " asides[" + side + "]= " + gameManager.aSides[side] + " @ " + Time.time);
						isUsed = true;
						int answer = (side * 100) + (x * 10) + (y * 1);
						gameManager.aSquares[answer] = gameManager.currentPlayer;
						gameManager.clickCount++;
						gameManager.aSides[side]++;
						gameManager.hasBeenSpun = false;
						//Debug.Log( "AFTER IS........ asides[" + side + "]= " + gameManager.aSides[side] + " @ " + Time.time);
					}	
				}
				CheckForWinner();
				ChangePlayer();
				if(gameManager.gameIsOver) gameManager.UpdateInfo("Ok, " + gameManager.playerName[gameManager.currentPlayer] + " it's your turn to Spin");
			}
		}
	}
	
	void ChangePlayer()
	{
		gameManager.currentPlayer++;
		if(gameManager.currentPlayer > 1)
		{
			gameManager.currentPlayer = 0;
		}
	}
	
	void UpdateX(int cbx)
	{
		gameManager.xCount++;
		gameManager.wCombos[cbx] =1;
		gameManager.UpdateXCount(gameManager.xCount);
	}
	
	void UpdateO(int cbo)
	{
		gameManager.oCount++;
		gameManager.wCombos[cbo] = 2;
		gameManager.UpdateOCount(gameManager.oCount);
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
		if ((gameManager.aSquares[100] + gameManager.aSquares[111] + gameManager.aSquares[122] == 0) && (gameManager.wCombos[11] == 0)) UpdateX(11);
		if ((gameManager.aSquares[102] + gameManager.aSquares[111] + gameManager.aSquares[120] == 0) && (gameManager.wCombos[12] == 0)) UpdateX(12);
		if ((gameManager.aSquares[100] + gameManager.aSquares[111] + gameManager.aSquares[122] == 3) && (gameManager.wCombos[11] == 0)) UpdateO(11);
		if ((gameManager.aSquares[102] + gameManager.aSquares[111] + gameManager.aSquares[120] == 3) && (gameManager.wCombos[12] == 0)) UpdateO(12);
		// Horizontal
		if ((gameManager.aSquares[100] + gameManager.aSquares[110] + gameManager.aSquares[120] == 0) && (gameManager.wCombos[13] == 0)) UpdateX(13);
		if ((gameManager.aSquares[101] + gameManager.aSquares[111] + gameManager.aSquares[121] == 0) && (gameManager.wCombos[14] == 0)) UpdateX(14);
		if ((gameManager.aSquares[102] + gameManager.aSquares[112] + gameManager.aSquares[122] == 0) && (gameManager.wCombos[15] == 0)) UpdateX(15);
		if ((gameManager.aSquares[100] + gameManager.aSquares[110] + gameManager.aSquares[120] == 3) && (gameManager.wCombos[13] == 0)) UpdateO(13);
		if ((gameManager.aSquares[101] + gameManager.aSquares[111] + gameManager.aSquares[121] == 3) && (gameManager.wCombos[14] == 0)) UpdateO(14);
		if ((gameManager.aSquares[102] + gameManager.aSquares[112] + gameManager.aSquares[122] == 3) && (gameManager.wCombos[15] == 0)) UpdateO(15);
		// Vertical
		if ((gameManager.aSquares[100] + gameManager.aSquares[101] + gameManager.aSquares[102] == 0) && (gameManager.wCombos[16] == 0)) UpdateX(16);
		if ((gameManager.aSquares[110] + gameManager.aSquares[111] + gameManager.aSquares[112] == 0) && (gameManager.wCombos[17] == 0)) UpdateX(17);
		if ((gameManager.aSquares[120] + gameManager.aSquares[121] + gameManager.aSquares[122] == 0) && (gameManager.wCombos[18] == 0)) UpdateX(18);
		if ((gameManager.aSquares[100] + gameManager.aSquares[101] + gameManager.aSquares[102] == 3) && (gameManager.wCombos[16] == 0)) UpdateO(16);
		if ((gameManager.aSquares[110] + gameManager.aSquares[111] + gameManager.aSquares[112] == 3) && (gameManager.wCombos[17] == 0)) UpdateO(17);
		if ((gameManager.aSquares[120] + gameManager.aSquares[121] + gameManager.aSquares[122] == 3) && (gameManager.wCombos[18] == 0)) UpdateO(18);
		
		// ********** BOTTOM GREEN SIDE =2 ************
		// Cross wised
		if ((gameManager.aSquares[200] + gameManager.aSquares[211] + gameManager.aSquares[222] == 0) && (gameManager.wCombos[21] == 0)) UpdateX(21);
		if ((gameManager.aSquares[202] + gameManager.aSquares[211] + gameManager.aSquares[220] == 0) && (gameManager.wCombos[22] == 0)) UpdateX(22);
		if ((gameManager.aSquares[200] + gameManager.aSquares[211] + gameManager.aSquares[222] == 3) && (gameManager.wCombos[21] == 0)) UpdateO(21);
		if ((gameManager.aSquares[202] + gameManager.aSquares[211] + gameManager.aSquares[220] == 3) && (gameManager.wCombos[22] == 0)) UpdateO(22);
		// Horizontal
		if ((gameManager.aSquares[200] + gameManager.aSquares[210] + gameManager.aSquares[220] == 0) && (gameManager.wCombos[23] == 0)) UpdateX(23);
		if ((gameManager.aSquares[201] + gameManager.aSquares[211] + gameManager.aSquares[221] == 0) && (gameManager.wCombos[24] == 0)) UpdateX(24);
		if ((gameManager.aSquares[202] + gameManager.aSquares[212] + gameManager.aSquares[222] == 0) && (gameManager.wCombos[25] == 0)) UpdateX(25);
		if ((gameManager.aSquares[200] + gameManager.aSquares[210] + gameManager.aSquares[220] == 3) && (gameManager.wCombos[23] == 0)) UpdateO(23);
		if ((gameManager.aSquares[201] + gameManager.aSquares[211] + gameManager.aSquares[221] == 3) && (gameManager.wCombos[24] == 0)) UpdateO(24);
		if ((gameManager.aSquares[202] + gameManager.aSquares[212] + gameManager.aSquares[222] == 3) && (gameManager.wCombos[25] == 0)) UpdateO(25);
		// Vertical
		if ((gameManager.aSquares[200] + gameManager.aSquares[201] + gameManager.aSquares[202] == 0) && (gameManager.wCombos[26] == 0)) UpdateX(26);
		if ((gameManager.aSquares[210] + gameManager.aSquares[211] + gameManager.aSquares[212] == 0) && (gameManager.wCombos[27] == 0)) UpdateX(27);
		if ((gameManager.aSquares[220] + gameManager.aSquares[221] + gameManager.aSquares[222] == 0) && (gameManager.wCombos[28] == 0)) UpdateX(28);
		if ((gameManager.aSquares[200] + gameManager.aSquares[201] + gameManager.aSquares[202] == 3) && (gameManager.wCombos[26] == 0)) UpdateO(26);
		if ((gameManager.aSquares[210] + gameManager.aSquares[211] + gameManager.aSquares[212] == 3) && (gameManager.wCombos[27] == 0)) UpdateO(27);
		if ((gameManager.aSquares[220] + gameManager.aSquares[221] + gameManager.aSquares[222] == 3) && (gameManager.wCombos[28] == 0)) UpdateO(28);
		
		// ********** FRONT BLUE SIDE =3 ************
		// Cross wised
		if ((gameManager.aSquares[300] + gameManager.aSquares[311] + gameManager.aSquares[322] == 0) && (gameManager.wCombos[31] == 0)) UpdateX(31);
		if ((gameManager.aSquares[302] + gameManager.aSquares[311] + gameManager.aSquares[320] == 0) && (gameManager.wCombos[32] == 0)) UpdateX(32);
		if ((gameManager.aSquares[300] + gameManager.aSquares[311] + gameManager.aSquares[322] == 3) && (gameManager.wCombos[31] == 0)) UpdateO(31);
		if ((gameManager.aSquares[302] + gameManager.aSquares[311] + gameManager.aSquares[320] == 3) && (gameManager.wCombos[32] == 0)) UpdateO(32);
		// Horizontal
		if ((gameManager.aSquares[300] + gameManager.aSquares[310] + gameManager.aSquares[320] == 0) && (gameManager.wCombos[33] == 0)) UpdateX(33);
		if ((gameManager.aSquares[301] + gameManager.aSquares[311] + gameManager.aSquares[321] == 0) && (gameManager.wCombos[34] == 0)) UpdateX(34);
		if ((gameManager.aSquares[302] + gameManager.aSquares[312] + gameManager.aSquares[322] == 0) && (gameManager.wCombos[35] == 0)) UpdateX(35);
		if ((gameManager.aSquares[300] + gameManager.aSquares[310] + gameManager.aSquares[320] == 3) && (gameManager.wCombos[33] == 0)) UpdateO(33);
		if ((gameManager.aSquares[301] + gameManager.aSquares[311] + gameManager.aSquares[321] == 3) && (gameManager.wCombos[34] == 0)) UpdateO(34);
		if ((gameManager.aSquares[302] + gameManager.aSquares[312] + gameManager.aSquares[322] == 3) && (gameManager.wCombos[35] == 0)) UpdateO(35);
		// Vertical
		if ((gameManager.aSquares[300] + gameManager.aSquares[301] + gameManager.aSquares[302] == 0) && (gameManager.wCombos[36] == 0)) UpdateX(36);
		if ((gameManager.aSquares[310] + gameManager.aSquares[311] + gameManager.aSquares[312] == 0) && (gameManager.wCombos[37] == 0)) UpdateX(37);
		if ((gameManager.aSquares[320] + gameManager.aSquares[321] + gameManager.aSquares[322] == 0) && (gameManager.wCombos[38] == 0)) UpdateX(38);
		if ((gameManager.aSquares[300] + gameManager.aSquares[301] + gameManager.aSquares[302] == 3) && (gameManager.wCombos[36] == 0)) UpdateO(36);
		if ((gameManager.aSquares[310] + gameManager.aSquares[311] + gameManager.aSquares[312] == 3) && (gameManager.wCombos[37] == 0)) UpdateO(37);
		if ((gameManager.aSquares[320] + gameManager.aSquares[321] + gameManager.aSquares[322] == 3) && (gameManager.wCombos[38] == 0)) UpdateO(38);
		
		// ********** LEFT RED =4 ************
		// Cross wised
		if ((gameManager.aSquares[400] + gameManager.aSquares[411] + gameManager.aSquares[422] == 0) && (gameManager.wCombos[41] == 0)) UpdateX(41);
		if ((gameManager.aSquares[402] + gameManager.aSquares[411] + gameManager.aSquares[420] == 0) && (gameManager.wCombos[42] == 0)) UpdateX(42);
		if ((gameManager.aSquares[400] + gameManager.aSquares[411] + gameManager.aSquares[422] == 3) && (gameManager.wCombos[41] == 0)) UpdateO(41);
		if ((gameManager.aSquares[402] + gameManager.aSquares[411] + gameManager.aSquares[420] == 3) && (gameManager.wCombos[42] == 0)) UpdateO(42);
		// Horizontal
		if ((gameManager.aSquares[400] + gameManager.aSquares[410] + gameManager.aSquares[420] == 0) && (gameManager.wCombos[43] == 0)) UpdateX(43);
		if ((gameManager.aSquares[401] + gameManager.aSquares[411] + gameManager.aSquares[421] == 0) && (gameManager.wCombos[44] == 0)) UpdateX(44);
		if ((gameManager.aSquares[402] + gameManager.aSquares[412] + gameManager.aSquares[422] == 0) && (gameManager.wCombos[45] == 0)) UpdateX(45);
		if ((gameManager.aSquares[400] + gameManager.aSquares[410] + gameManager.aSquares[420] == 3) && (gameManager.wCombos[43] == 0)) UpdateO(43);
		if ((gameManager.aSquares[401] + gameManager.aSquares[411] + gameManager.aSquares[421] == 3) && (gameManager.wCombos[44] == 0)) UpdateO(44);
		if ((gameManager.aSquares[402] + gameManager.aSquares[412] + gameManager.aSquares[422] == 3) && (gameManager.wCombos[45] == 0)) UpdateO(45);
		// Vertical
		if ((gameManager.aSquares[400] + gameManager.aSquares[401] + gameManager.aSquares[402] == 0) && (gameManager.wCombos[46] == 0)) UpdateX(46);
		if ((gameManager.aSquares[410] + gameManager.aSquares[411] + gameManager.aSquares[412] == 0) && (gameManager.wCombos[47] == 0)) UpdateX(47);
		if ((gameManager.aSquares[420] + gameManager.aSquares[421] + gameManager.aSquares[422] == 0) && (gameManager.wCombos[48] == 0)) UpdateX(48);
		if ((gameManager.aSquares[400] + gameManager.aSquares[401] + gameManager.aSquares[402] == 3) && (gameManager.wCombos[46] == 0)) UpdateO(46);
		if ((gameManager.aSquares[410] + gameManager.aSquares[411] + gameManager.aSquares[412] == 3) && (gameManager.wCombos[47] == 0)) UpdateO(47);
		if ((gameManager.aSquares[420] + gameManager.aSquares[421] + gameManager.aSquares[422] == 3) && (gameManager.wCombos[48] == 0)) UpdateO(48);
		
		// ********** RIGHT WHITE =5 ************
		// Cross wised
		if ((gameManager.aSquares[500] + gameManager.aSquares[511] + gameManager.aSquares[522] == 0) && (gameManager.wCombos[51] == 0)) UpdateX(51);
		if ((gameManager.aSquares[502] + gameManager.aSquares[511] + gameManager.aSquares[520] == 0) && (gameManager.wCombos[52] == 0)) UpdateX(52);
		if ((gameManager.aSquares[500] + gameManager.aSquares[511] + gameManager.aSquares[522] == 3) && (gameManager.wCombos[51] == 0)) UpdateO(51);
		if ((gameManager.aSquares[502] + gameManager.aSquares[511] + gameManager.aSquares[520] == 3) && (gameManager.wCombos[52] == 0)) UpdateO(52);
		// Horizontal
		if ((gameManager.aSquares[500] + gameManager.aSquares[510] + gameManager.aSquares[520] == 0) && (gameManager.wCombos[53] == 0)) UpdateX(53);
		if ((gameManager.aSquares[501] + gameManager.aSquares[511] + gameManager.aSquares[521] == 0) && (gameManager.wCombos[54] == 0)) UpdateX(54);
		if ((gameManager.aSquares[502] + gameManager.aSquares[512] + gameManager.aSquares[522] == 0) && (gameManager.wCombos[55] == 0)) UpdateX(55);
		if ((gameManager.aSquares[500] + gameManager.aSquares[510] + gameManager.aSquares[520] == 3) && (gameManager.wCombos[53] == 0)) UpdateO(53);
		if ((gameManager.aSquares[501] + gameManager.aSquares[511] + gameManager.aSquares[521] == 3) && (gameManager.wCombos[54] == 0)) UpdateO(54);
		if ((gameManager.aSquares[502] + gameManager.aSquares[512] + gameManager.aSquares[522] == 3) && (gameManager.wCombos[55] == 0)) UpdateO(55);
		// Vertical
		if ((gameManager.aSquares[500] + gameManager.aSquares[501] + gameManager.aSquares[502] == 0) && (gameManager.wCombos[56] == 0)) UpdateX(56);
		if ((gameManager.aSquares[510] + gameManager.aSquares[511] + gameManager.aSquares[512] == 0) && (gameManager.wCombos[57] == 0)) UpdateX(57);
		if ((gameManager.aSquares[520] + gameManager.aSquares[521] + gameManager.aSquares[522] == 0) && (gameManager.wCombos[58] == 0)) UpdateX(58);
		if ((gameManager.aSquares[500] + gameManager.aSquares[501] + gameManager.aSquares[502] == 3) && (gameManager.wCombos[56] == 0)) UpdateO(56);
		if ((gameManager.aSquares[510] + gameManager.aSquares[511] + gameManager.aSquares[512] == 3) && (gameManager.wCombos[57] == 0)) UpdateO(57);
		if ((gameManager.aSquares[520] + gameManager.aSquares[521] + gameManager.aSquares[522] == 3) && (gameManager.wCombos[58] == 0)) UpdateO(58);
		
		// ********** TOP ORANGE =6 ************
		// Cross wised
		if ((gameManager.aSquares[600] + gameManager.aSquares[611] + gameManager.aSquares[622] == 0) && (gameManager.wCombos[61] == 0)) UpdateX(61);
		if ((gameManager.aSquares[602] + gameManager.aSquares[611] + gameManager.aSquares[620] == 0) && (gameManager.wCombos[62] == 0)) UpdateX(62);
		if ((gameManager.aSquares[600] + gameManager.aSquares[611] + gameManager.aSquares[622] == 3) && (gameManager.wCombos[61] == 0)) UpdateO(61);
		if ((gameManager.aSquares[602] + gameManager.aSquares[611] + gameManager.aSquares[620] == 3) && (gameManager.wCombos[62] == 0)) UpdateO(62);
		// Horizontal
		if ((gameManager.aSquares[600] + gameManager.aSquares[610] + gameManager.aSquares[620] == 0) && (gameManager.wCombos[63] == 0)) UpdateX(63);
		if ((gameManager.aSquares[601] + gameManager.aSquares[611] + gameManager.aSquares[621] == 0) && (gameManager.wCombos[64] == 0)) UpdateX(64);
		if ((gameManager.aSquares[602] + gameManager.aSquares[612] + gameManager.aSquares[622] == 0) && (gameManager.wCombos[65] == 0)) UpdateX(65);
		if ((gameManager.aSquares[600] + gameManager.aSquares[610] + gameManager.aSquares[620] == 3) && (gameManager.wCombos[63] == 0)) UpdateO(63);
		if ((gameManager.aSquares[601] + gameManager.aSquares[611] + gameManager.aSquares[621] == 3) && (gameManager.wCombos[64] == 0)) UpdateO(64);
		if ((gameManager.aSquares[602] + gameManager.aSquares[612] + gameManager.aSquares[622] == 3) && (gameManager.wCombos[65] == 0)) UpdateO(65);
		// Vertical
		if ((gameManager.aSquares[600] + gameManager.aSquares[601] + gameManager.aSquares[602] == 0) && (gameManager.wCombos[66] == 0)) UpdateX(66);
		if ((gameManager.aSquares[610] + gameManager.aSquares[611] + gameManager.aSquares[612] == 0) && (gameManager.wCombos[67] == 0)) UpdateX(67);
		if ((gameManager.aSquares[620] + gameManager.aSquares[621] + gameManager.aSquares[622] == 0) && (gameManager.wCombos[68] == 0)) UpdateX(68);
		if ((gameManager.aSquares[600] + gameManager.aSquares[601] + gameManager.aSquares[602] == 3) && (gameManager.wCombos[66] == 0)) UpdateO(66);
		if ((gameManager.aSquares[610] + gameManager.aSquares[611] + gameManager.aSquares[612] == 3) && (gameManager.wCombos[67] == 0)) UpdateO(67);
		if ((gameManager.aSquares[620] + gameManager.aSquares[621] + gameManager.aSquares[622] == 3) && (gameManager.wCombos[68] == 0)) UpdateO(68);
		
		//Debug.Log("Finished @ " + Time.time);
		
		
		if (gameManager.clickCount >= 54)
		{
			gameManager.gameIsOver = true;
			if (gameManager.xCount == gameManager.oCount) 
			{
				gameManager.UpdateInfo("Game Over!, It's a TIE, NOOOOOOOO");
			}
			else if (gameManager.xCount > gameManager.oCount)
			{
				gameManager.UpdateInfo("Game Over!, X Wins!, Hooray!");
			}
			else 
			{
				gameManager.UpdateInfo("Game Over!, O Wins!, BOOOOO!");		
			}
			Debug.Log("after game info X =" + gameManager.xCount + " O =" + gameManager.oCount);
		}
		else
		{
			gameManager.UpdateClickCount(gameManager.clickCount);
		}
	}
}