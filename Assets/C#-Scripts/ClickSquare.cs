using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClickSquare : NetworkBehaviour {
	
	public int side;
	public int x;
	public int y;

	[SyncVar]
	public int xCount = 0;
	[SyncVar]
	public int oCount = 0;
	
	private bool isUsed = false;
		
	// Change for touch
	void OnMouseDown () 
	{
		if (GameManager.instance.hasBeenSpun && !GameManager.instance.cubeSpinning)
		{
			if(!isUsed)
			{
				foreach(Transform child in transform)
				{
					if(child.name == GameManager.instance.playerObjName[GameManager.instance.currentPlayer])
					{
						GameManager.instance.source.PlayOneShot(GameManager.instance.soundClips[4]);
						//child.active = true; DIDN'T WORK W/ 5.0
						child.gameObject.SetActive(true);
						//Debug.Log( "BEFORE " + this.gameObject.name + " asides[" + side + "]= " + GameManager.instance.aSides[side] + " @ " + Time.time);
						isUsed = true;
						int answer = (side * 100) + (x * 10) + (y * 1);
						GameManager.instance.aSquares[answer] = GameManager.instance.currentPlayer;
						GameManager.instance.clickCount++;
						GameManager.instance.aSides[side-1]++;
						GameManager.instance.hasBeenSpun = false;
						//Debug.Log( "AFTER IS........ asides[" + side + "]= " + GameManager.instance.aSides[side] + " @ " + Time.time);
					}	
				}
				CheckForWinner();
				ChangePlayer();
				if(!GameManager.instance.gameIsOver) 
				{
					GameManager.instance.UpdateInfo("Ok, " + GameManager.instance.playerName[GameManager.instance.currentPlayer] + " it's your turn to Spin");
			
				}
			}
			else
			{
				GameManager.instance.UpdateInfo("Come on, Pick an Empty Square!");
			}
		}
		else
		{
			GameManager.instance.UpdateInfo("Gotta Spin before you can Click!");
		}
	}

	
	void ChangePlayer()
	{
		GameManager.instance.currentPlayer++;
		if(GameManager.instance.currentPlayer > 1)
		{
			GameManager.instance.currentPlayer = 0;
			GameManager.instance.PanelXPlayer.SetActive(true);
			GameManager.instance.PanelOPlayer.SetActive(false);
		}
		else
		{
			GameManager.instance.PanelXPlayer.SetActive(false);
			GameManager.instance.PanelOPlayer.SetActive(true);
		}
	}
	[ClientRpc]
	void RpcUpdateX()
	{
		Debug.LogError ("Update x count");
	}
	[ClientRpc]
	void RpcUpdateO()
	{
		Debug.LogError ("Update o count");
	}
	void UpdateX(int cbx)
	{
		if (!isServer)
			return;

		GameManager.instance.source.PlayOneShot (GameManager.instance.soundClips [2]);
		//GameManager.instance.xCount++;
		xCount++;
		GameManager.instance.xCount = xCount;
		GameManager.instance.wCombos[cbx] = 1;
		GameManager.instance.UpdateXCount(GameManager.instance.xCount);
		RpcUpdateX ();
	}
	void UpdateO(int cbo)
	{
		if (!isServer)
			return;
		GameManager.instance.source.PlayOneShot (GameManager.instance.soundClips [2]);
		//GameManager.instance.oCount++;
		oCount++;
		GameManager.instance.oCount = oCount;
		GameManager.instance.wCombos[cbo] = 2;
		GameManager.instance.UpdateOCount(GameManager.instance.oCount);
		RpcUpdateO();
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
		if ((GameManager.instance.aSquares[100] + GameManager.instance.aSquares[111] + GameManager.instance.aSquares[122] == 0) && (GameManager.instance.wCombos[11] == 0)) UpdateX(11);
		if ((GameManager.instance.aSquares[102] + GameManager.instance.aSquares[111] + GameManager.instance.aSquares[120] == 0) && (GameManager.instance.wCombos[12] == 0)) UpdateX(12);
		if ((GameManager.instance.aSquares[100] + GameManager.instance.aSquares[111] + GameManager.instance.aSquares[122] == 3) && (GameManager.instance.wCombos[11] == 0)) UpdateO(11);
		if ((GameManager.instance.aSquares[102] + GameManager.instance.aSquares[111] + GameManager.instance.aSquares[120] == 3) && (GameManager.instance.wCombos[12] == 0)) UpdateO(12);
		// Horizontal
		if ((GameManager.instance.aSquares[100] + GameManager.instance.aSquares[110] + GameManager.instance.aSquares[120] == 0) && (GameManager.instance.wCombos[13] == 0)) UpdateX(13);
		if ((GameManager.instance.aSquares[101] + GameManager.instance.aSquares[111] + GameManager.instance.aSquares[121] == 0) && (GameManager.instance.wCombos[14] == 0)) UpdateX(14);
		if ((GameManager.instance.aSquares[102] + GameManager.instance.aSquares[112] + GameManager.instance.aSquares[122] == 0) && (GameManager.instance.wCombos[15] == 0)) UpdateX(15);
		if ((GameManager.instance.aSquares[100] + GameManager.instance.aSquares[110] + GameManager.instance.aSquares[120] == 3) && (GameManager.instance.wCombos[13] == 0)) UpdateO(13);
		if ((GameManager.instance.aSquares[101] + GameManager.instance.aSquares[111] + GameManager.instance.aSquares[121] == 3) && (GameManager.instance.wCombos[14] == 0)) UpdateO(14);
		if ((GameManager.instance.aSquares[102] + GameManager.instance.aSquares[112] + GameManager.instance.aSquares[122] == 3) && (GameManager.instance.wCombos[15] == 0)) UpdateO(15);
		// Vertical
		if ((GameManager.instance.aSquares[100] + GameManager.instance.aSquares[101] + GameManager.instance.aSquares[102] == 0) && (GameManager.instance.wCombos[16] == 0)) UpdateX(16);
		if ((GameManager.instance.aSquares[110] + GameManager.instance.aSquares[111] + GameManager.instance.aSquares[112] == 0) && (GameManager.instance.wCombos[17] == 0)) UpdateX(17);
		if ((GameManager.instance.aSquares[120] + GameManager.instance.aSquares[121] + GameManager.instance.aSquares[122] == 0) && (GameManager.instance.wCombos[18] == 0)) UpdateX(18);
		if ((GameManager.instance.aSquares[100] + GameManager.instance.aSquares[101] + GameManager.instance.aSquares[102] == 3) && (GameManager.instance.wCombos[16] == 0)) UpdateO(16);
		if ((GameManager.instance.aSquares[110] + GameManager.instance.aSquares[111] + GameManager.instance.aSquares[112] == 3) && (GameManager.instance.wCombos[17] == 0)) UpdateO(17);
		if ((GameManager.instance.aSquares[120] + GameManager.instance.aSquares[121] + GameManager.instance.aSquares[122] == 3) && (GameManager.instance.wCombos[18] == 0)) UpdateO(18);
		
		// ********** BOTTOM GREEN SIDE =2 ************
		// Cross wised
		if ((GameManager.instance.aSquares[200] + GameManager.instance.aSquares[211] + GameManager.instance.aSquares[222] == 0) && (GameManager.instance.wCombos[21] == 0)) UpdateX(21);
		if ((GameManager.instance.aSquares[202] + GameManager.instance.aSquares[211] + GameManager.instance.aSquares[220] == 0) && (GameManager.instance.wCombos[22] == 0)) UpdateX(22);
		if ((GameManager.instance.aSquares[200] + GameManager.instance.aSquares[211] + GameManager.instance.aSquares[222] == 3) && (GameManager.instance.wCombos[21] == 0)) UpdateO(21);
		if ((GameManager.instance.aSquares[202] + GameManager.instance.aSquares[211] + GameManager.instance.aSquares[220] == 3) && (GameManager.instance.wCombos[22] == 0)) UpdateO(22);
		// Horizontal
		if ((GameManager.instance.aSquares[200] + GameManager.instance.aSquares[210] + GameManager.instance.aSquares[220] == 0) && (GameManager.instance.wCombos[23] == 0)) UpdateX(23);
		if ((GameManager.instance.aSquares[201] + GameManager.instance.aSquares[211] + GameManager.instance.aSquares[221] == 0) && (GameManager.instance.wCombos[24] == 0)) UpdateX(24);
		if ((GameManager.instance.aSquares[202] + GameManager.instance.aSquares[212] + GameManager.instance.aSquares[222] == 0) && (GameManager.instance.wCombos[25] == 0)) UpdateX(25);
		if ((GameManager.instance.aSquares[200] + GameManager.instance.aSquares[210] + GameManager.instance.aSquares[220] == 3) && (GameManager.instance.wCombos[23] == 0)) UpdateO(23);
		if ((GameManager.instance.aSquares[201] + GameManager.instance.aSquares[211] + GameManager.instance.aSquares[221] == 3) && (GameManager.instance.wCombos[24] == 0)) UpdateO(24);
		if ((GameManager.instance.aSquares[202] + GameManager.instance.aSquares[212] + GameManager.instance.aSquares[222] == 3) && (GameManager.instance.wCombos[25] == 0)) UpdateO(25);
		// Vertical
		if ((GameManager.instance.aSquares[200] + GameManager.instance.aSquares[201] + GameManager.instance.aSquares[202] == 0) && (GameManager.instance.wCombos[26] == 0)) UpdateX(26);
		if ((GameManager.instance.aSquares[210] + GameManager.instance.aSquares[211] + GameManager.instance.aSquares[212] == 0) && (GameManager.instance.wCombos[27] == 0)) UpdateX(27);
		if ((GameManager.instance.aSquares[220] + GameManager.instance.aSquares[221] + GameManager.instance.aSquares[222] == 0) && (GameManager.instance.wCombos[28] == 0)) UpdateX(28);
		if ((GameManager.instance.aSquares[200] + GameManager.instance.aSquares[201] + GameManager.instance.aSquares[202] == 3) && (GameManager.instance.wCombos[26] == 0)) UpdateO(26);
		if ((GameManager.instance.aSquares[210] + GameManager.instance.aSquares[211] + GameManager.instance.aSquares[212] == 3) && (GameManager.instance.wCombos[27] == 0)) UpdateO(27);
		if ((GameManager.instance.aSquares[220] + GameManager.instance.aSquares[221] + GameManager.instance.aSquares[222] == 3) && (GameManager.instance.wCombos[28] == 0)) UpdateO(28);
		
		// ********** FRONT BLUE SIDE =3 ************
		// Cross wised
		if ((GameManager.instance.aSquares[300] + GameManager.instance.aSquares[311] + GameManager.instance.aSquares[322] == 0) && (GameManager.instance.wCombos[31] == 0)) UpdateX(31);
		if ((GameManager.instance.aSquares[302] + GameManager.instance.aSquares[311] + GameManager.instance.aSquares[320] == 0) && (GameManager.instance.wCombos[32] == 0)) UpdateX(32);
		if ((GameManager.instance.aSquares[300] + GameManager.instance.aSquares[311] + GameManager.instance.aSquares[322] == 3) && (GameManager.instance.wCombos[31] == 0)) UpdateO(31);
		if ((GameManager.instance.aSquares[302] + GameManager.instance.aSquares[311] + GameManager.instance.aSquares[320] == 3) && (GameManager.instance.wCombos[32] == 0)) UpdateO(32);
		// Horizontal
		if ((GameManager.instance.aSquares[300] + GameManager.instance.aSquares[310] + GameManager.instance.aSquares[320] == 0) && (GameManager.instance.wCombos[33] == 0)) UpdateX(33);
		if ((GameManager.instance.aSquares[301] + GameManager.instance.aSquares[311] + GameManager.instance.aSquares[321] == 0) && (GameManager.instance.wCombos[34] == 0)) UpdateX(34);
		if ((GameManager.instance.aSquares[302] + GameManager.instance.aSquares[312] + GameManager.instance.aSquares[322] == 0) && (GameManager.instance.wCombos[35] == 0)) UpdateX(35);
		if ((GameManager.instance.aSquares[300] + GameManager.instance.aSquares[310] + GameManager.instance.aSquares[320] == 3) && (GameManager.instance.wCombos[33] == 0)) UpdateO(33);
		if ((GameManager.instance.aSquares[301] + GameManager.instance.aSquares[311] + GameManager.instance.aSquares[321] == 3) && (GameManager.instance.wCombos[34] == 0)) UpdateO(34);
		if ((GameManager.instance.aSquares[302] + GameManager.instance.aSquares[312] + GameManager.instance.aSquares[322] == 3) && (GameManager.instance.wCombos[35] == 0)) UpdateO(35);
		// Vertical
		if ((GameManager.instance.aSquares[300] + GameManager.instance.aSquares[301] + GameManager.instance.aSquares[302] == 0) && (GameManager.instance.wCombos[36] == 0)) UpdateX(36);
		if ((GameManager.instance.aSquares[310] + GameManager.instance.aSquares[311] + GameManager.instance.aSquares[312] == 0) && (GameManager.instance.wCombos[37] == 0)) UpdateX(37);
		if ((GameManager.instance.aSquares[320] + GameManager.instance.aSquares[321] + GameManager.instance.aSquares[322] == 0) && (GameManager.instance.wCombos[38] == 0)) UpdateX(38);
		if ((GameManager.instance.aSquares[300] + GameManager.instance.aSquares[301] + GameManager.instance.aSquares[302] == 3) && (GameManager.instance.wCombos[36] == 0)) UpdateO(36);
		if ((GameManager.instance.aSquares[310] + GameManager.instance.aSquares[311] + GameManager.instance.aSquares[312] == 3) && (GameManager.instance.wCombos[37] == 0)) UpdateO(37);
		if ((GameManager.instance.aSquares[320] + GameManager.instance.aSquares[321] + GameManager.instance.aSquares[322] == 3) && (GameManager.instance.wCombos[38] == 0)) UpdateO(38);
		
		// ********** LEFT RED =4 ************
		// Cross wised
		if ((GameManager.instance.aSquares[400] + GameManager.instance.aSquares[411] + GameManager.instance.aSquares[422] == 0) && (GameManager.instance.wCombos[41] == 0)) UpdateX(41);
		if ((GameManager.instance.aSquares[402] + GameManager.instance.aSquares[411] + GameManager.instance.aSquares[420] == 0) && (GameManager.instance.wCombos[42] == 0)) UpdateX(42);
		if ((GameManager.instance.aSquares[400] + GameManager.instance.aSquares[411] + GameManager.instance.aSquares[422] == 3) && (GameManager.instance.wCombos[41] == 0)) UpdateO(41);
		if ((GameManager.instance.aSquares[402] + GameManager.instance.aSquares[411] + GameManager.instance.aSquares[420] == 3) && (GameManager.instance.wCombos[42] == 0)) UpdateO(42);
		// Horizontal
		if ((GameManager.instance.aSquares[400] + GameManager.instance.aSquares[410] + GameManager.instance.aSquares[420] == 0) && (GameManager.instance.wCombos[43] == 0)) UpdateX(43);
		if ((GameManager.instance.aSquares[401] + GameManager.instance.aSquares[411] + GameManager.instance.aSquares[421] == 0) && (GameManager.instance.wCombos[44] == 0)) UpdateX(44);
		if ((GameManager.instance.aSquares[402] + GameManager.instance.aSquares[412] + GameManager.instance.aSquares[422] == 0) && (GameManager.instance.wCombos[45] == 0)) UpdateX(45);
		if ((GameManager.instance.aSquares[400] + GameManager.instance.aSquares[410] + GameManager.instance.aSquares[420] == 3) && (GameManager.instance.wCombos[43] == 0)) UpdateO(43);
		if ((GameManager.instance.aSquares[401] + GameManager.instance.aSquares[411] + GameManager.instance.aSquares[421] == 3) && (GameManager.instance.wCombos[44] == 0)) UpdateO(44);
		if ((GameManager.instance.aSquares[402] + GameManager.instance.aSquares[412] + GameManager.instance.aSquares[422] == 3) && (GameManager.instance.wCombos[45] == 0)) UpdateO(45);
		// Vertical
		if ((GameManager.instance.aSquares[400] + GameManager.instance.aSquares[401] + GameManager.instance.aSquares[402] == 0) && (GameManager.instance.wCombos[46] == 0)) UpdateX(46);
		if ((GameManager.instance.aSquares[410] + GameManager.instance.aSquares[411] + GameManager.instance.aSquares[412] == 0) && (GameManager.instance.wCombos[47] == 0)) UpdateX(47);
		if ((GameManager.instance.aSquares[420] + GameManager.instance.aSquares[421] + GameManager.instance.aSquares[422] == 0) && (GameManager.instance.wCombos[48] == 0)) UpdateX(48);
		if ((GameManager.instance.aSquares[400] + GameManager.instance.aSquares[401] + GameManager.instance.aSquares[402] == 3) && (GameManager.instance.wCombos[46] == 0)) UpdateO(46);
		if ((GameManager.instance.aSquares[410] + GameManager.instance.aSquares[411] + GameManager.instance.aSquares[412] == 3) && (GameManager.instance.wCombos[47] == 0)) UpdateO(47);
		if ((GameManager.instance.aSquares[420] + GameManager.instance.aSquares[421] + GameManager.instance.aSquares[422] == 3) && (GameManager.instance.wCombos[48] == 0)) UpdateO(48);
		
		// ********** RIGHT WHITE =5 ************
		// Cross wised
		if ((GameManager.instance.aSquares[500] + GameManager.instance.aSquares[511] + GameManager.instance.aSquares[522] == 0) && (GameManager.instance.wCombos[51] == 0)) UpdateX(51);
		if ((GameManager.instance.aSquares[502] + GameManager.instance.aSquares[511] + GameManager.instance.aSquares[520] == 0) && (GameManager.instance.wCombos[52] == 0)) UpdateX(52);
		if ((GameManager.instance.aSquares[500] + GameManager.instance.aSquares[511] + GameManager.instance.aSquares[522] == 3) && (GameManager.instance.wCombos[51] == 0)) UpdateO(51);
		if ((GameManager.instance.aSquares[502] + GameManager.instance.aSquares[511] + GameManager.instance.aSquares[520] == 3) && (GameManager.instance.wCombos[52] == 0)) UpdateO(52);
		// Horizontal
		if ((GameManager.instance.aSquares[500] + GameManager.instance.aSquares[510] + GameManager.instance.aSquares[520] == 0) && (GameManager.instance.wCombos[53] == 0)) UpdateX(53);
		if ((GameManager.instance.aSquares[501] + GameManager.instance.aSquares[511] + GameManager.instance.aSquares[521] == 0) && (GameManager.instance.wCombos[54] == 0)) UpdateX(54);
		if ((GameManager.instance.aSquares[502] + GameManager.instance.aSquares[512] + GameManager.instance.aSquares[522] == 0) && (GameManager.instance.wCombos[55] == 0)) UpdateX(55);
		if ((GameManager.instance.aSquares[500] + GameManager.instance.aSquares[510] + GameManager.instance.aSquares[520] == 3) && (GameManager.instance.wCombos[53] == 0)) UpdateO(53);
		if ((GameManager.instance.aSquares[501] + GameManager.instance.aSquares[511] + GameManager.instance.aSquares[521] == 3) && (GameManager.instance.wCombos[54] == 0)) UpdateO(54);
		if ((GameManager.instance.aSquares[502] + GameManager.instance.aSquares[512] + GameManager.instance.aSquares[522] == 3) && (GameManager.instance.wCombos[55] == 0)) UpdateO(55);
		// Vertical
		if ((GameManager.instance.aSquares[500] + GameManager.instance.aSquares[501] + GameManager.instance.aSquares[502] == 0) && (GameManager.instance.wCombos[56] == 0)) UpdateX(56);
		if ((GameManager.instance.aSquares[510] + GameManager.instance.aSquares[511] + GameManager.instance.aSquares[512] == 0) && (GameManager.instance.wCombos[57] == 0)) UpdateX(57);
		if ((GameManager.instance.aSquares[520] + GameManager.instance.aSquares[521] + GameManager.instance.aSquares[522] == 0) && (GameManager.instance.wCombos[58] == 0)) UpdateX(58);
		if ((GameManager.instance.aSquares[500] + GameManager.instance.aSquares[501] + GameManager.instance.aSquares[502] == 3) && (GameManager.instance.wCombos[56] == 0)) UpdateO(56);
		if ((GameManager.instance.aSquares[510] + GameManager.instance.aSquares[511] + GameManager.instance.aSquares[512] == 3) && (GameManager.instance.wCombos[57] == 0)) UpdateO(57);
		if ((GameManager.instance.aSquares[520] + GameManager.instance.aSquares[521] + GameManager.instance.aSquares[522] == 3) && (GameManager.instance.wCombos[58] == 0)) UpdateO(58);
		
		// ********** TOP ORANGE =6 ************
		// Cross wised
		if ((GameManager.instance.aSquares[600] + GameManager.instance.aSquares[611] + GameManager.instance.aSquares[622] == 0) && (GameManager.instance.wCombos[61] == 0)) UpdateX(61);
		if ((GameManager.instance.aSquares[602] + GameManager.instance.aSquares[611] + GameManager.instance.aSquares[620] == 0) && (GameManager.instance.wCombos[62] == 0)) UpdateX(62);
		if ((GameManager.instance.aSquares[600] + GameManager.instance.aSquares[611] + GameManager.instance.aSquares[622] == 3) && (GameManager.instance.wCombos[61] == 0)) UpdateO(61);
		if ((GameManager.instance.aSquares[602] + GameManager.instance.aSquares[611] + GameManager.instance.aSquares[620] == 3) && (GameManager.instance.wCombos[62] == 0)) UpdateO(62);
		// Horizontal
		if ((GameManager.instance.aSquares[600] + GameManager.instance.aSquares[610] + GameManager.instance.aSquares[620] == 0) && (GameManager.instance.wCombos[63] == 0)) UpdateX(63);
		if ((GameManager.instance.aSquares[601] + GameManager.instance.aSquares[611] + GameManager.instance.aSquares[621] == 0) && (GameManager.instance.wCombos[64] == 0)) UpdateX(64);
		if ((GameManager.instance.aSquares[602] + GameManager.instance.aSquares[612] + GameManager.instance.aSquares[622] == 0) && (GameManager.instance.wCombos[65] == 0)) UpdateX(65);
		if ((GameManager.instance.aSquares[600] + GameManager.instance.aSquares[610] + GameManager.instance.aSquares[620] == 3) && (GameManager.instance.wCombos[63] == 0)) UpdateO(63);
		if ((GameManager.instance.aSquares[601] + GameManager.instance.aSquares[611] + GameManager.instance.aSquares[621] == 3) && (GameManager.instance.wCombos[64] == 0)) UpdateO(64);
		if ((GameManager.instance.aSquares[602] + GameManager.instance.aSquares[612] + GameManager.instance.aSquares[622] == 3) && (GameManager.instance.wCombos[65] == 0)) UpdateO(65);
		// Vertical
		if ((GameManager.instance.aSquares[600] + GameManager.instance.aSquares[601] + GameManager.instance.aSquares[602] == 0) && (GameManager.instance.wCombos[66] == 0)) UpdateX(66);
		if ((GameManager.instance.aSquares[610] + GameManager.instance.aSquares[611] + GameManager.instance.aSquares[612] == 0) && (GameManager.instance.wCombos[67] == 0)) UpdateX(67);
		if ((GameManager.instance.aSquares[620] + GameManager.instance.aSquares[621] + GameManager.instance.aSquares[622] == 0) && (GameManager.instance.wCombos[68] == 0)) UpdateX(68);
		if ((GameManager.instance.aSquares[600] + GameManager.instance.aSquares[601] + GameManager.instance.aSquares[602] == 3) && (GameManager.instance.wCombos[66] == 0)) UpdateO(66);
		if ((GameManager.instance.aSquares[610] + GameManager.instance.aSquares[611] + GameManager.instance.aSquares[612] == 3) && (GameManager.instance.wCombos[67] == 0)) UpdateO(67);
		if ((GameManager.instance.aSquares[620] + GameManager.instance.aSquares[621] + GameManager.instance.aSquares[622] == 3) && (GameManager.instance.wCombos[68] == 0)) UpdateO(68);
		
		//Debug.Log("Finished @ " + Time.time);
		
		
		if (GameManager.instance.clickCount >= 54)
		{
			GameManager.instance.gameIsOver = true;
			GameManager.instance.backgroundMusic.Stop ();
			GameManager.instance.source.PlayOneShot(GameManager.instance.soundClips[3]);
			//GameManager.instance.gameIsOver = true;
			if (GameManager.instance.xCount == GameManager.instance.oCount) 
			{
				GameManager.instance.UpdateInfo("Game Over!, It's a TIE, NOOOOOOOO");
			}
			else if (GameManager.instance.xCount > GameManager.instance.oCount)
			{
				GameManager.instance.UpdateInfo("Game Over!, X Wins!, Hooray!");
			}
			else 
			{
				GameManager.instance.UpdateInfo("Game Over!, O Wins!, BOOOOO!");		
			}
			Debug.Log("after game info X =" + GameManager.instance.xCount + " O =" + GameManager.instance.oCount);
		}
		else
		{
			GameManager.instance.UpdateClickCount(GameManager.instance.clickCount);
		}
	}
}