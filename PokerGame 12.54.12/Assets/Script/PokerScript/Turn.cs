using UnityEngine;
using System.Collections;

public class Turn : MonoBehaviour {
	
	//変数宣言
	[SerializeField] enum gameTurn{
		START_TURN = 0,
		CARD_DRAW_TIME = 1,
		PLAYER_TURN = 2,
		CHENGE_TURN = 3,
		ENEMY_TURN = 4,
		JUDGE_TURN = 5,
	}
	gameTurn nowTurn;
	TouchMan touchManager;
	
	void Start () {
		nowTurn = gameTurn.START_TURN;
		touchManager = GetComponent<TouchMan> ();
		touchManager.touchEnableChenge (true);
		Debug.Log (nowTurn);
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
