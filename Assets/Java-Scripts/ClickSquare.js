
var side:int;
var x:int;
var y:int;

private var isUsed : boolean = false;
private var gameLogic : GameLogic;

function Awake()
{
	gameLogic = GameObject.FindGameObjectWithTag("GLogic").GetComponent(GameLogic);
}

function OnMouseDown()
{
	if (gameLogic.hasBeenSpun)
	{
		if (!isUsed)
		{
			for (var child : Transform in transform) 
			{
				if (child.name == gameLogic.playerObjName[gameLogic.currentPlayer])
				{					
					//child.active = true; DIDN'T WORK W/ 5.0
					child.gameObject.SetActive(true);
					//Debug.Log( "BEFORE " + this.gameObject.name + " asides[" + side + "]= " + gameLogic.aSides[side] + " @ " + Time.time);
					isUsed = true;
					var answer : int = (side * 100) + (x * 10) + (y * 1);
					gameLogic.aSquares[answer] = gameLogic.currentPlayer;
					gameLogic.clickCount++;
					gameLogic.aSides[side-1]++;
					gameLogic.hasBeenSpun = false;
					//Debug.Log( "AFTER IS........ asides[" + side + "]= " + gameLogic.aSides[side] + " @ " + Time.time);					
				}
			}
			CheckForWinner();
			ChangePlayer();		
			if (!gameLogic.gameIsOver) gameLogic.UpdateInfo("Ok, " + gameLogic.playerName[gameLogic.currentPlayer] + " it's your turn to Spin");
		}
		else
		{
			gameLogic.UpdateInfo("Come on, Pick an Empty Square!");
		}
	}
	else
	{
		gameLogic.UpdateInfo("Gotta Spin before you can Click!");
	}
	
}

function ChangePlayer()
{
	// Change the Current Player Number
	gameLogic.currentPlayer++;
	if (gameLogic.currentPlayer > 1) gameLogic.currentPlayer = 0;
}

function UpdateX(cbx : int)
{
	//Debug.Log("CBX=" + cbx + " before update X=" + gameLogic.xCount);
	gameLogic.xCount++;
	gameLogic.wCombos[cbx] = 1;	
	gameLogic.UpdateXcount(gameLogic.xCount);
	//Debug.Log("after update Combo[cbx]=" + gameLogic.wCombos[cbx] + " click cnt=" + gameLogic.clickCount);
}

function UpdateO(cbo : int)
{
	//Debug.Log("CBO=" + cbo + " before update O=" + gameLogic.oCount);	
	gameLogic.oCount++;
	gameLogic.wCombos[cbo] = 2;	
	gameLogic.UpdateOcount(gameLogic.oCount);	
	//Debug.Log("after update Combo[cbo]=" + gameLogic.wCombos[cbo] + " click cnt=" + gameLogic.clickCount);	
	//Debug.Log("debug - cbo=" + cbo + " combos=" + gameLogic.wCombos[cbo] + " Ocnt=" + gameLogic.oCount);	
}

function CheckForWinner()
{
	//******************************************************************************************************
	// array aSquares 100 - 622 are all set to 9 @ gameLogic Start
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
	if ((gameLogic.aSquares[100] + gameLogic.aSquares[111] + gameLogic.aSquares[122] == 0) && (gameLogic.wCombos[11] == 0)) UpdateX(11);
	if ((gameLogic.aSquares[102] + gameLogic.aSquares[111] + gameLogic.aSquares[120] == 0) && (gameLogic.wCombos[12] == 0)) UpdateX(12);
	if ((gameLogic.aSquares[100] + gameLogic.aSquares[111] + gameLogic.aSquares[122] == 3) && (gameLogic.wCombos[11] == 0)) UpdateO(11);
	if ((gameLogic.aSquares[102] + gameLogic.aSquares[111] + gameLogic.aSquares[120] == 3) && (gameLogic.wCombos[12] == 0)) UpdateO(12);
	// Horizontal
	if ((gameLogic.aSquares[100] + gameLogic.aSquares[110] + gameLogic.aSquares[120] == 0) && (gameLogic.wCombos[13] == 0)) UpdateX(13);
	if ((gameLogic.aSquares[101] + gameLogic.aSquares[111] + gameLogic.aSquares[121] == 0) && (gameLogic.wCombos[14] == 0)) UpdateX(14);
	if ((gameLogic.aSquares[102] + gameLogic.aSquares[112] + gameLogic.aSquares[122] == 0) && (gameLogic.wCombos[15] == 0)) UpdateX(15);
	if ((gameLogic.aSquares[100] + gameLogic.aSquares[110] + gameLogic.aSquares[120] == 3) && (gameLogic.wCombos[13] == 0)) UpdateO(13);
	if ((gameLogic.aSquares[101] + gameLogic.aSquares[111] + gameLogic.aSquares[121] == 3) && (gameLogic.wCombos[14] == 0)) UpdateO(14);
	if ((gameLogic.aSquares[102] + gameLogic.aSquares[112] + gameLogic.aSquares[122] == 3) && (gameLogic.wCombos[15] == 0)) UpdateO(15);
	// Vertical
	if ((gameLogic.aSquares[100] + gameLogic.aSquares[101] + gameLogic.aSquares[102] == 0) && (gameLogic.wCombos[16] == 0)) UpdateX(16);
	if ((gameLogic.aSquares[110] + gameLogic.aSquares[111] + gameLogic.aSquares[112] == 0) && (gameLogic.wCombos[17] == 0)) UpdateX(17);
	if ((gameLogic.aSquares[120] + gameLogic.aSquares[121] + gameLogic.aSquares[122] == 0) && (gameLogic.wCombos[18] == 0)) UpdateX(18);
	if ((gameLogic.aSquares[100] + gameLogic.aSquares[101] + gameLogic.aSquares[102] == 3) && (gameLogic.wCombos[16] == 0)) UpdateO(16);
	if ((gameLogic.aSquares[110] + gameLogic.aSquares[111] + gameLogic.aSquares[112] == 3) && (gameLogic.wCombos[17] == 0)) UpdateO(17);
	if ((gameLogic.aSquares[120] + gameLogic.aSquares[121] + gameLogic.aSquares[122] == 3) && (gameLogic.wCombos[18] == 0)) UpdateO(18);
	
	// ********** BOTTOM GREEN SIDE =2 ************
	// Cross wised
	if ((gameLogic.aSquares[200] + gameLogic.aSquares[211] + gameLogic.aSquares[222] == 0) && (gameLogic.wCombos[21] == 0)) UpdateX(21);
	if ((gameLogic.aSquares[202] + gameLogic.aSquares[211] + gameLogic.aSquares[220] == 0) && (gameLogic.wCombos[22] == 0)) UpdateX(22);
	if ((gameLogic.aSquares[200] + gameLogic.aSquares[211] + gameLogic.aSquares[222] == 3) && (gameLogic.wCombos[21] == 0)) UpdateO(21);
	if ((gameLogic.aSquares[202] + gameLogic.aSquares[211] + gameLogic.aSquares[220] == 3) && (gameLogic.wCombos[22] == 0)) UpdateO(22);
	// Horizontal
	if ((gameLogic.aSquares[200] + gameLogic.aSquares[210] + gameLogic.aSquares[220] == 0) && (gameLogic.wCombos[23] == 0)) UpdateX(23);
	if ((gameLogic.aSquares[201] + gameLogic.aSquares[211] + gameLogic.aSquares[221] == 0) && (gameLogic.wCombos[24] == 0)) UpdateX(24);
	if ((gameLogic.aSquares[202] + gameLogic.aSquares[212] + gameLogic.aSquares[222] == 0) && (gameLogic.wCombos[25] == 0)) UpdateX(25);
	if ((gameLogic.aSquares[200] + gameLogic.aSquares[210] + gameLogic.aSquares[220] == 3) && (gameLogic.wCombos[23] == 0)) UpdateO(23);
	if ((gameLogic.aSquares[201] + gameLogic.aSquares[211] + gameLogic.aSquares[221] == 3) && (gameLogic.wCombos[24] == 0)) UpdateO(24);
	if ((gameLogic.aSquares[202] + gameLogic.aSquares[212] + gameLogic.aSquares[222] == 3) && (gameLogic.wCombos[25] == 0)) UpdateO(25);
	// Vertical
	if ((gameLogic.aSquares[200] + gameLogic.aSquares[201] + gameLogic.aSquares[202] == 0) && (gameLogic.wCombos[26] == 0)) UpdateX(26);
	if ((gameLogic.aSquares[210] + gameLogic.aSquares[211] + gameLogic.aSquares[212] == 0) && (gameLogic.wCombos[27] == 0)) UpdateX(27);
	if ((gameLogic.aSquares[220] + gameLogic.aSquares[221] + gameLogic.aSquares[222] == 0) && (gameLogic.wCombos[28] == 0)) UpdateX(28);
	if ((gameLogic.aSquares[200] + gameLogic.aSquares[201] + gameLogic.aSquares[202] == 3) && (gameLogic.wCombos[26] == 0)) UpdateO(26);
	if ((gameLogic.aSquares[210] + gameLogic.aSquares[211] + gameLogic.aSquares[212] == 3) && (gameLogic.wCombos[27] == 0)) UpdateO(27);
	if ((gameLogic.aSquares[220] + gameLogic.aSquares[221] + gameLogic.aSquares[222] == 3) && (gameLogic.wCombos[28] == 0)) UpdateO(28);
	
	// ********** FRONT BLUE SIDE =3 ************
	// Cross wised
	if ((gameLogic.aSquares[300] + gameLogic.aSquares[311] + gameLogic.aSquares[322] == 0) && (gameLogic.wCombos[31] == 0)) UpdateX(31);
	if ((gameLogic.aSquares[302] + gameLogic.aSquares[311] + gameLogic.aSquares[320] == 0) && (gameLogic.wCombos[32] == 0)) UpdateX(32);
	if ((gameLogic.aSquares[300] + gameLogic.aSquares[311] + gameLogic.aSquares[322] == 3) && (gameLogic.wCombos[31] == 0)) UpdateO(31);
	if ((gameLogic.aSquares[302] + gameLogic.aSquares[311] + gameLogic.aSquares[320] == 3) && (gameLogic.wCombos[32] == 0)) UpdateO(32);
	// Horizontal
	if ((gameLogic.aSquares[300] + gameLogic.aSquares[310] + gameLogic.aSquares[320] == 0) && (gameLogic.wCombos[33] == 0)) UpdateX(33);
	if ((gameLogic.aSquares[301] + gameLogic.aSquares[311] + gameLogic.aSquares[321] == 0) && (gameLogic.wCombos[34] == 0)) UpdateX(34);
	if ((gameLogic.aSquares[302] + gameLogic.aSquares[312] + gameLogic.aSquares[322] == 0) && (gameLogic.wCombos[35] == 0)) UpdateX(35);
	if ((gameLogic.aSquares[300] + gameLogic.aSquares[310] + gameLogic.aSquares[320] == 3) && (gameLogic.wCombos[33] == 0)) UpdateO(33);
	if ((gameLogic.aSquares[301] + gameLogic.aSquares[311] + gameLogic.aSquares[321] == 3) && (gameLogic.wCombos[34] == 0)) UpdateO(34);
	if ((gameLogic.aSquares[302] + gameLogic.aSquares[312] + gameLogic.aSquares[322] == 3) && (gameLogic.wCombos[35] == 0)) UpdateO(35);
	// Vertical
	if ((gameLogic.aSquares[300] + gameLogic.aSquares[301] + gameLogic.aSquares[302] == 0) && (gameLogic.wCombos[36] == 0)) UpdateX(36);
	if ((gameLogic.aSquares[310] + gameLogic.aSquares[311] + gameLogic.aSquares[312] == 0) && (gameLogic.wCombos[37] == 0)) UpdateX(37);
	if ((gameLogic.aSquares[320] + gameLogic.aSquares[321] + gameLogic.aSquares[322] == 0) && (gameLogic.wCombos[38] == 0)) UpdateX(38);
	if ((gameLogic.aSquares[300] + gameLogic.aSquares[301] + gameLogic.aSquares[302] == 3) && (gameLogic.wCombos[36] == 0)) UpdateO(36);
	if ((gameLogic.aSquares[310] + gameLogic.aSquares[311] + gameLogic.aSquares[312] == 3) && (gameLogic.wCombos[37] == 0)) UpdateO(37);
	if ((gameLogic.aSquares[320] + gameLogic.aSquares[321] + gameLogic.aSquares[322] == 3) && (gameLogic.wCombos[38] == 0)) UpdateO(38);
	
	// ********** LEFT RED =4 ************
	// Cross wised
	if ((gameLogic.aSquares[400] + gameLogic.aSquares[411] + gameLogic.aSquares[422] == 0) && (gameLogic.wCombos[41] == 0)) UpdateX(41);
	if ((gameLogic.aSquares[402] + gameLogic.aSquares[411] + gameLogic.aSquares[420] == 0) && (gameLogic.wCombos[42] == 0)) UpdateX(42);
	if ((gameLogic.aSquares[400] + gameLogic.aSquares[411] + gameLogic.aSquares[422] == 3) && (gameLogic.wCombos[41] == 0)) UpdateO(41);
	if ((gameLogic.aSquares[402] + gameLogic.aSquares[411] + gameLogic.aSquares[420] == 3) && (gameLogic.wCombos[42] == 0)) UpdateO(42);
	// Horizontal
	if ((gameLogic.aSquares[400] + gameLogic.aSquares[410] + gameLogic.aSquares[420] == 0) && (gameLogic.wCombos[43] == 0)) UpdateX(43);
	if ((gameLogic.aSquares[401] + gameLogic.aSquares[411] + gameLogic.aSquares[421] == 0) && (gameLogic.wCombos[44] == 0)) UpdateX(44);
	if ((gameLogic.aSquares[402] + gameLogic.aSquares[412] + gameLogic.aSquares[422] == 0) && (gameLogic.wCombos[45] == 0)) UpdateX(45);
	if ((gameLogic.aSquares[400] + gameLogic.aSquares[410] + gameLogic.aSquares[420] == 3) && (gameLogic.wCombos[43] == 0)) UpdateO(43);
	if ((gameLogic.aSquares[401] + gameLogic.aSquares[411] + gameLogic.aSquares[421] == 3) && (gameLogic.wCombos[44] == 0)) UpdateO(44);
	if ((gameLogic.aSquares[402] + gameLogic.aSquares[412] + gameLogic.aSquares[422] == 3) && (gameLogic.wCombos[45] == 0)) UpdateO(45);
	// Vertical
	if ((gameLogic.aSquares[400] + gameLogic.aSquares[401] + gameLogic.aSquares[402] == 0) && (gameLogic.wCombos[46] == 0)) UpdateX(46);
	if ((gameLogic.aSquares[410] + gameLogic.aSquares[411] + gameLogic.aSquares[412] == 0) && (gameLogic.wCombos[47] == 0)) UpdateX(47);
	if ((gameLogic.aSquares[420] + gameLogic.aSquares[421] + gameLogic.aSquares[422] == 0) && (gameLogic.wCombos[48] == 0)) UpdateX(48);
	if ((gameLogic.aSquares[400] + gameLogic.aSquares[401] + gameLogic.aSquares[402] == 3) && (gameLogic.wCombos[46] == 0)) UpdateO(46);
	if ((gameLogic.aSquares[410] + gameLogic.aSquares[411] + gameLogic.aSquares[412] == 3) && (gameLogic.wCombos[47] == 0)) UpdateO(47);
	if ((gameLogic.aSquares[420] + gameLogic.aSquares[421] + gameLogic.aSquares[422] == 3) && (gameLogic.wCombos[48] == 0)) UpdateO(48);

	// ********** RIGHT WHITE =5 ************
	// Cross wised
	if ((gameLogic.aSquares[500] + gameLogic.aSquares[511] + gameLogic.aSquares[522] == 0) && (gameLogic.wCombos[51] == 0)) UpdateX(51);
	if ((gameLogic.aSquares[502] + gameLogic.aSquares[511] + gameLogic.aSquares[520] == 0) && (gameLogic.wCombos[52] == 0)) UpdateX(52);
	if ((gameLogic.aSquares[500] + gameLogic.aSquares[511] + gameLogic.aSquares[522] == 3) && (gameLogic.wCombos[51] == 0)) UpdateO(51);
	if ((gameLogic.aSquares[502] + gameLogic.aSquares[511] + gameLogic.aSquares[520] == 3) && (gameLogic.wCombos[52] == 0)) UpdateO(52);
	// Horizontal
	if ((gameLogic.aSquares[500] + gameLogic.aSquares[510] + gameLogic.aSquares[520] == 0) && (gameLogic.wCombos[53] == 0)) UpdateX(53);
	if ((gameLogic.aSquares[501] + gameLogic.aSquares[511] + gameLogic.aSquares[521] == 0) && (gameLogic.wCombos[54] == 0)) UpdateX(54);
	if ((gameLogic.aSquares[502] + gameLogic.aSquares[512] + gameLogic.aSquares[522] == 0) && (gameLogic.wCombos[55] == 0)) UpdateX(55);
	if ((gameLogic.aSquares[500] + gameLogic.aSquares[510] + gameLogic.aSquares[520] == 3) && (gameLogic.wCombos[53] == 0)) UpdateO(53);
	if ((gameLogic.aSquares[501] + gameLogic.aSquares[511] + gameLogic.aSquares[521] == 3) && (gameLogic.wCombos[54] == 0)) UpdateO(54);
	if ((gameLogic.aSquares[502] + gameLogic.aSquares[512] + gameLogic.aSquares[522] == 3) && (gameLogic.wCombos[55] == 0)) UpdateO(55);
	// Vertical
	if ((gameLogic.aSquares[500] + gameLogic.aSquares[501] + gameLogic.aSquares[502] == 0) && (gameLogic.wCombos[56] == 0)) UpdateX(56);
	if ((gameLogic.aSquares[510] + gameLogic.aSquares[511] + gameLogic.aSquares[512] == 0) && (gameLogic.wCombos[57] == 0)) UpdateX(57);
	if ((gameLogic.aSquares[520] + gameLogic.aSquares[521] + gameLogic.aSquares[522] == 0) && (gameLogic.wCombos[58] == 0)) UpdateX(58);
	if ((gameLogic.aSquares[500] + gameLogic.aSquares[501] + gameLogic.aSquares[502] == 3) && (gameLogic.wCombos[56] == 0)) UpdateO(56);
	if ((gameLogic.aSquares[510] + gameLogic.aSquares[511] + gameLogic.aSquares[512] == 3) && (gameLogic.wCombos[57] == 0)) UpdateO(57);
	if ((gameLogic.aSquares[520] + gameLogic.aSquares[521] + gameLogic.aSquares[522] == 3) && (gameLogic.wCombos[58] == 0)) UpdateO(58);

	// ********** TOP ORANGE =6 ************
	// Cross wised
	if ((gameLogic.aSquares[600] + gameLogic.aSquares[611] + gameLogic.aSquares[622] == 0) && (gameLogic.wCombos[61] == 0)) UpdateX(61);
	if ((gameLogic.aSquares[602] + gameLogic.aSquares[611] + gameLogic.aSquares[620] == 0) && (gameLogic.wCombos[62] == 0)) UpdateX(62);
	if ((gameLogic.aSquares[600] + gameLogic.aSquares[611] + gameLogic.aSquares[622] == 3) && (gameLogic.wCombos[61] == 0)) UpdateO(61);
	if ((gameLogic.aSquares[602] + gameLogic.aSquares[611] + gameLogic.aSquares[620] == 3) && (gameLogic.wCombos[62] == 0)) UpdateO(62);
	// Horizontal
	if ((gameLogic.aSquares[600] + gameLogic.aSquares[610] + gameLogic.aSquares[620] == 0) && (gameLogic.wCombos[63] == 0)) UpdateX(63);
	if ((gameLogic.aSquares[601] + gameLogic.aSquares[611] + gameLogic.aSquares[621] == 0) && (gameLogic.wCombos[64] == 0)) UpdateX(64);
	if ((gameLogic.aSquares[602] + gameLogic.aSquares[612] + gameLogic.aSquares[622] == 0) && (gameLogic.wCombos[65] == 0)) UpdateX(65);
	if ((gameLogic.aSquares[600] + gameLogic.aSquares[610] + gameLogic.aSquares[620] == 3) && (gameLogic.wCombos[63] == 0)) UpdateO(63);
	if ((gameLogic.aSquares[601] + gameLogic.aSquares[611] + gameLogic.aSquares[621] == 3) && (gameLogic.wCombos[64] == 0)) UpdateO(64);
	if ((gameLogic.aSquares[602] + gameLogic.aSquares[612] + gameLogic.aSquares[622] == 3) && (gameLogic.wCombos[65] == 0)) UpdateO(65);
	// Vertical
	if ((gameLogic.aSquares[600] + gameLogic.aSquares[601] + gameLogic.aSquares[602] == 0) && (gameLogic.wCombos[66] == 0)) UpdateX(66);
	if ((gameLogic.aSquares[610] + gameLogic.aSquares[611] + gameLogic.aSquares[612] == 0) && (gameLogic.wCombos[67] == 0)) UpdateX(67);
	if ((gameLogic.aSquares[620] + gameLogic.aSquares[621] + gameLogic.aSquares[622] == 0) && (gameLogic.wCombos[68] == 0)) UpdateX(68);
	if ((gameLogic.aSquares[600] + gameLogic.aSquares[601] + gameLogic.aSquares[602] == 3) && (gameLogic.wCombos[66] == 0)) UpdateO(66);
	if ((gameLogic.aSquares[610] + gameLogic.aSquares[611] + gameLogic.aSquares[612] == 3) && (gameLogic.wCombos[67] == 0)) UpdateO(67);
	if ((gameLogic.aSquares[620] + gameLogic.aSquares[621] + gameLogic.aSquares[622] == 3) && (gameLogic.wCombos[68] == 0)) UpdateO(68);
	
	//Debug.Log("Finished @ " + Time.time);
	

	if (gameLogic.clickCount >= 54)
	{
		gameLogic.gameIsOver = true;
		if (gameLogic.xCount == gameLogic.oCount) 
		{
			gameLogic.UpdateInfo("Game Over!, It's a TIE, NOOOOOOOO");
		}
		else if (gameLogic.xCount > gameLogic.oCount)
		{
			gameLogic.UpdateInfo("Game Over!, X Wins!, Hooray!");
		}
		else 
		{
			gameLogic.UpdateInfo("Game Over!, O Wins!, BOOOOO!");		
		}
		Debug.Log("after game info X =" + gameLogic.xCount + " O =" + gameLogic.oCount);
	}
	else
	{
		gameLogic.UpdateClickCount(gameLogic.clickCount);
	}
}