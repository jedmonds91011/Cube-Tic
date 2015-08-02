#pragma strict
import UnityEngine.UI;

	static var currentPlayer : int = 0;
	static var playerObjName = ["X-obj", "O-obj"];	
	static var playerName = ["Scott", "Elliot"];		
	static var gameIsOver : boolean = false;
	static var hasBeenSpun : boolean = false;	
	static var clickCount : int;
	static var xCount : int;
	static var oCount : int;	
	static var aSquares : int[] = new int[700];
	static var wCombos : int[] = new int[100];	
	static var aSides : int[] = new int[7];		
	
	var cube : GameObject;
	var iBox : UI.Text;
	var xBox : UI.Text;
	var oBox : UI.Text;
	var cBox : UI.Text;
	
	private var endVector = new Array();
	private	var endCube = new Array();
	
function Start()
{
	UpdateInfo("Player " + playerName[currentPlayer] + " begins the game, Give it a good Spin!");

	endVector[0] = (Vector3(20,180,-135));	// Yellow side	#1  Id-0
	endVector[1] = (Vector3(45,72,63));		// Green side	#2  Id-1
	endVector[2] = (Vector3(-20,0,45));		// Blue side	#3  Id-2
	endVector[3] = (Vector3(-45,-105,23));	// Red side		#4  Id-3
	endVector[4] = (Vector3(45,73,-25));	// White side	#5  Id-4
	endVector[5] = (Vector3(-45,-107,113));	// Magenta side	#6  Id-5
	
	for (var j = 100; j < 623; j++) aSquares[j] = 9;
}

function SpinButton () {
	if (!hasBeenSpun)
	{
		LeterRip();
	}
	else
	{
		UpdateInfo("Sorry, already Spun! Now you gotta Click!");
	}
}
 
function LeterRip()
 {
	 //Debug.Log("Started Leterip @ " + Time.time);
	 endCube.Clear();
	 for (var i = 0; i < 6; i++)
	 {
		for (var j = 0; j < (9 - aSides[i]); j++)
		{
			endCube.Push(endVector[i]);
		}
	 }

	var SpinCntr : int =0;
	var max : int = endCube.length;
	while (SpinCntr < 10)
	{
		cube.transform.rotation = Random.rotation;
		yield WaitForSeconds(.08); // and let Unity free till the next frame	
		SpinCntr++;
	}
	
	var id = Random.Range(0,max);
	cube.transform.eulerAngles = endCube[id];
	hasBeenSpun = true;
	UpdateInfo("Good Spin, " + playerName[currentPlayer] + " it's time to pick a square");
	//Debug.Log( this.gameObject.name + " random value = " + id + " XYZ are " + endCube[id] + " asides[" + id + "]= " + aSides[id] + " @ " + Time.time);
	//Debug.Log("Ended Leterip Id " + id + " V3 = " + endCube[id] + " length=" + endCube.length);
	//print("Current position is" + cube.transform.rotation.x + "," + cube.transform.rotation.y + "," + cube.transform.rotation.z);
	
}

public function UpdateClickCount(clkCnt : int){
	cBox.text = clkCnt.ToString();
}

public function UpdateInfo(txt : String){
	iBox.text = txt;
}

public function UpdateXcount(xcntr : int){
	xBox.text = xcntr.ToString();
}

public function UpdateOcount(ocntr : int){
	oBox.text = ocntr.ToString();
}

function LetSee()
{
	print("Current position is" + cube.transform.rotation.x + "," + cube.transform.rotation.y + "," + cube.transform.rotation.z);
	print("XXXXXXXXX  aSides Array  XXXXXXXXXXXXXXXXXXXXX");
	for (var k = 0; k <= 5; k++)
	{
		Debug.Log("End asides[" + k + "]= " + aSides[k] + " @ " + Time.time);
	}
	
	print("XXXXXXXXX  Remaining EndCube Array  XXXXXXXXXXXXXXXXXXXXX");
	for (var l = 0; l < endCube.length; l++)
	{
		Debug.Log("EndCube = " + l + " XYZ are " + endCube[l]);
	}
}

function reStart(){
	Application.LoadLevel(0);
}

function ChkSquares()
{
	// Cross wised
	Debug.Log(aSquares[100] + "-" + aSquares[111] + "-" + aSquares[122] + "==" + wCombos[11]);
	Debug.Log(aSquares[102] + "-" + aSquares[111] + "-" + aSquares[120] + "==" + wCombos[12]);

	// Horizontal
	Debug.Log(aSquares[100] + "-" + aSquares[110] + "-" + aSquares[120] + "==" + wCombos[13]);
	Debug.Log(aSquares[101] + "-" + aSquares[111] + "-" + aSquares[121] + "==" + wCombos[14]);
	Debug.Log(aSquares[102] + "-" + aSquares[112] + "-" + aSquares[122] + "==" + wCombos[15]);

	// Vertical
	Debug.Log(aSquares[100] + "-" + aSquares[101] + "-" + aSquares[102] + "==" + wCombos[16]);
	Debug.Log(aSquares[110] + "-" + aSquares[111] + "-" + aSquares[112] + "==" + wCombos[17]);
	Debug.Log(aSquares[120] + "-" + aSquares[121] + "-" + aSquares[122] + "==" + wCombos[18]);	

	Debug.Log("XXXXXXXX  Green side  XXXXXXXX");
	// Cross wised
	Debug.Log(aSquares[200] + "-" + aSquares[211] + "-" + aSquares[222] + "==" + wCombos[21]);
	Debug.Log(aSquares[202] + "-" + aSquares[211] + "-" + aSquares[220] + "==" + wCombos[22]);

	// Horizontal
	Debug.Log(aSquares[200] + "-" + aSquares[210] + "-" + aSquares[220] + "==" + wCombos[23]);
	Debug.Log(aSquares[201] + "-" + aSquares[211] + "-" + aSquares[221] + "==" + wCombos[24]);
	Debug.Log(aSquares[202] + "-" + aSquares[212] + "-" + aSquares[222] + "==" + wCombos[25]);

	// Vertical
	Debug.Log(aSquares[200] + "-" + aSquares[201] + "-" + aSquares[202] + "==" + wCombos[26]);
	Debug.Log(aSquares[210] + "-" + aSquares[211] + "-" + aSquares[212] + "==" + wCombos[27]);
	Debug.Log(aSquares[220] + "-" + aSquares[221] + "-" + aSquares[222] + "==" + wCombos[28]);	

	Debug.Log("XXXXXXXX  Blue side  XXXXXXXX");	
	// Cross wised
	Debug.Log(aSquares[300] + "-" + aSquares[311] + "-" + aSquares[322] + "==" + wCombos[31]);
	Debug.Log(aSquares[302] + "-" + aSquares[311] + "-" + aSquares[320] + "==" + wCombos[32]);

	// Horizontal
	Debug.Log(aSquares[300] + "-" + aSquares[310] + "-" + aSquares[320] + "==" + wCombos[33]);
	Debug.Log(aSquares[301] + "-" + aSquares[311] + "-" + aSquares[321] + "==" + wCombos[34]);
	Debug.Log(aSquares[302] + "-" + aSquares[312] + "-" + aSquares[322] + "==" + wCombos[35]);

	// Vertical
	Debug.Log(aSquares[300] + "-" + aSquares[301] + "-" + aSquares[302] + "==" + wCombos[36]);
	Debug.Log(aSquares[310] + "-" + aSquares[311] + "-" + aSquares[312] + "==" + wCombos[37]);
	Debug.Log(aSquares[320] + "-" + aSquares[321] + "-" + aSquares[322] + "==" + wCombos[38]);	

	Debug.Log("XXXXXXXX  Red side  XXXXXXXX");	
	// Cross wised
	Debug.Log(aSquares[400] + "-" + aSquares[411] + "-" + aSquares[422] + "==" + wCombos[41]);
	Debug.Log(aSquares[402] + "-" + aSquares[411] + "-" + aSquares[420] + "==" + wCombos[42]);

	// Horizontal
	Debug.Log(aSquares[400] + "-" + aSquares[410] + "-" + aSquares[420] + "==" + wCombos[43]);
	Debug.Log(aSquares[401] + "-" + aSquares[411] + "-" + aSquares[421] + "==" + wCombos[44]);
	Debug.Log(aSquares[402] + "-" + aSquares[412] + "-" + aSquares[422] + "==" + wCombos[45]);

	// Vertical
	Debug.Log(aSquares[400] + "-" + aSquares[401] + "-" + aSquares[402] + "==" + wCombos[46]);
	Debug.Log(aSquares[410] + "-" + aSquares[411] + "-" + aSquares[412] + "==" + wCombos[47]);
	Debug.Log(aSquares[420] + "-" + aSquares[421] + "-" + aSquares[422] + "==" + wCombos[48]);	

	Debug.Log("XXXXXXXX  White side  XXXXXXXX");	
	// Cross wised
	Debug.Log(aSquares[500] + "-" + aSquares[511] + "-" + aSquares[522] + "==" + wCombos[51]);
	Debug.Log(aSquares[502] + "-" + aSquares[511] + "-" + aSquares[520] + "==" + wCombos[52]);

	// Horizontal
	Debug.Log(aSquares[500] + "-" + aSquares[510] + "-" + aSquares[520] + "==" + wCombos[53]);
	Debug.Log(aSquares[501] + "-" + aSquares[511] + "-" + aSquares[521] + "==" + wCombos[54]);
	Debug.Log(aSquares[502] + "-" + aSquares[512] + "-" + aSquares[522] + "==" + wCombos[55]);

	// Vertical
	Debug.Log(aSquares[500] + "-" + aSquares[501] + "-" + aSquares[502] + "==" + wCombos[56]);
	Debug.Log(aSquares[510] + "-" + aSquares[511] + "-" + aSquares[512] + "==" + wCombos[57]);
	Debug.Log(aSquares[520] + "-" + aSquares[521] + "-" + aSquares[522] + "==" + wCombos[58]);	

	Debug.Log("XXXXXXXX  Magenta side  XXXXXXXX");	
	// Cross wised
	Debug.Log(aSquares[600] + "-" + aSquares[611] + "-" + aSquares[622] + "==" + wCombos[61]);
	Debug.Log(aSquares[602] + "-" + aSquares[611] + "-" + aSquares[620] + "==" + wCombos[62]);

	// Horizontal
	Debug.Log(aSquares[600] + "-" + aSquares[610] + "-" + aSquares[620] + "==" + wCombos[63]);
	Debug.Log(aSquares[601] + "-" + aSquares[611] + "-" + aSquares[621] + "==" + wCombos[64]);
	Debug.Log(aSquares[602] + "-" + aSquares[612] + "-" + aSquares[622] + "==" + wCombos[65]);

	// Vertical
	Debug.Log(aSquares[600] + "-" + aSquares[601] + "-" + aSquares[602] + "==" + wCombos[66]);
	Debug.Log(aSquares[610] + "-" + aSquares[611] + "-" + aSquares[612] + "==" + wCombos[67]);
	Debug.Log(aSquares[620] + "-" + aSquares[621] + "-" + aSquares[622] + "==" + wCombos[68]);	
}
