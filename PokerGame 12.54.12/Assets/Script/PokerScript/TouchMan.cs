using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 現在の状態を管理し
/// UI,Touch系の機能の実装クラス.
/// </summary>
public class TouchMan : MonoBehaviour {

	//ゲームの現在のステート
	[SerializeField] gameTurn nowTurn;
	//ゲームの総プレイヤー人数
	[SerializeField] int totalPlayerCount;

	//プレイヤーオブジェクト
	public GameObject Player;
	//CPUオブジェクト
	public GameObject Enemyp;

	//カードのスクリプト
	Card card;
	Enemy enemy;

	//UI用のテキストの宣言
	public Text playerHaveMedaltext;
	public Text cpuHaveMedaltext;
	public Text pokarHandsAndHelptext;
	public Text cpuCommenttext;

	//true:プレイヤーが親　false:CPUが親
	public bool whoisparenet = true;

	public bool pl_IsDrop;
	public bool cpu_IsDrop;

	//現在のベット総額用テキスト
	public Text playerNowBettext;
	public Text cpuNowBettext;

	//UI,ゲーム用のメダル所持数に関する宣言
	public int playeyNowBets;
	public int cpuNowBets;

	public int startMedalCount;
	int cpuHavsMedalCount;
	int playerHavsMedalCount;

	//UIのキャンバスの宣言
	public GameObject canvas;
	public Animator UIanim;

	public bool continuecall = false;

	public string helpandhandstext;
	public string cpucoment;


	//タッチの有効無効を判断するbool
	public bool touchBool;

	enum gameTurn{
		START_TURN,
		CARD_DRAW_TIME,
		PLAYER_TURN,//プレイヤー親の場合
		ENEMY_TURN,//CPU親の場合
		RAISE_CALL_TURN,
		DROP_TURN,//ドロップした時
		CHENGE_TURN,//カードのチェンジが出来るとき
		JUDGE_TURN,//約判定の時
	}


	void Start () {
		card = GetComponent<Card>();
		UIanim = canvas.GetComponent<Animator> ();

		nowTurn = gameTurn.START_TURN;

		//メダルの初期値を代入
		cpuHavsMedalCount = startMedalCount;
		playerHavsMedalCount = startMedalCount;

		//プレイヤーをインスタンス化
		Instantiate (Player, transform.position, Quaternion.identity);

		//totalPlayerCount-1 の数だけEnemyをインスタンス化
		for(int i = 0;i<totalPlayerCount - 1;i++){
			Instantiate (Enemyp, new Vector3(-2,5.2f,0), Quaternion.identity);
		}

		enemy = FindObjectOfType<Enemy>();

		//テキストを変更する

		playerHaveMedaltext.text = "残りメダル:" + playerHavsMedalCount;
		cpuHaveMedaltext.text = "残りメダル:" + cpuHavsMedalCount;
		cpuNowBettext.text = "CPU Bet:" + cpuNowBets;
		playerNowBettext.text = "Player Bet:" + playeyNowBets;
		//cpuCommenttext.text = "よろしくね";
		pokarHandsAndHelptext.text = "スタートボタンを押してください。";
	}

	// Update is called once per frame
	void Update () {

		//プレイヤーのターンの時にのみカードのタッチが可能
		if(nowTurn == gameTurn.CHENGE_TURN){
			if (Input.GetMouseButtonDown(0)) {

				Vector3    aTapPoint   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint);

				if (aCollider2d) {
					GameObject obj = aCollider2d.transform.gameObject;
					switch(obj.tag){
					case "PlayerCard":
							Debug.Log (aCollider2d);
							obj.SendMessage ("TouchCard");
							break;
					default:
							Debug.Log ("touch(click)!! objectname:"+obj.name);
							break;
					}
				}
			}
		}
	}

	public void UI_Animation(){
		//現在のステートでアニメーションを設定する
		switch(nowTurn){
		case gameTurn.START_TURN:
			UIanim.SetTrigger ("StateSTART");
			break;
		case gameTurn.CARD_DRAW_TIME:
			UIanim.SetTrigger ("StateDRAW");
			break;
		case gameTurn.ENEMY_TURN:
			UIanim.SetBool ("PlayerTurn", false);
			break;
		case gameTurn.PLAYER_TURN:
			UIanim.SetTrigger ("StatePLAYER");
//UIanim.SetBool ("PlayerTurn", true);
			break;
		case gameTurn.RAISE_CALL_TURN:
			if(whoisparenet == true){
				continuecall = true;
				UIanim.SetTrigger ("StatePLAYERPARENT");
			}else{
				UIanim.SetTrigger ("StateCALLRAISE");
			}
			break;
		case gameTurn.JUDGE_TURN:
			UIanim.SetTrigger ("StateJUDGE");
			break;
		case gameTurn.CHENGE_TURN:
			cpuCommenttext.text = enemy.ChengeCommentText ();
			UIanim.SetTrigger ("StateCHENGE");
			break;
		case gameTurn.DROP_TURN:
			UIanim.SetTrigger ("StateDROP");
			break;
		}
	}

	/// <summary>
	/// タッチの有効化と無効化.
	/// </summary>
	/// <param name="touch">If set to <c>true</c> touch.</param>
	public void touchEnableChenge(bool touch){
		touchBool = touch;
		Debug.Log ("タッチが可能か:"+touch);
	}

	public void Chenge_Draw_Turn(){
		cpuCommenttext.text = "良いカードがきますように・・・!!";
		pokarHandsAndHelptext.text = "カードを配っています...";
		nowTurn = gameTurn.CARD_DRAW_TIME;
		UI_Animation ();
	}

	/// <summary>
	/// スタートターンにチェンジする
	/// </summary>
	public void Chenge_Start_Turn(){
		nowTurn = gameTurn.START_TURN;
		UI_Animation ();
	}

	/// <summary>
	/// ジャッジターンにチェンジする
	/// </summary>
	public void Chenge_Judge_Turn(){
		nowTurn = gameTurn.JUDGE_TURN;
		UI_Animation ();
	}
			
	/// <summary>
	/// プレイヤーターンにチェンジする
	/// </summary>
	public void Chenge_player_Turn(){
		if(whoisparenet == true){
			cpuCommenttext.text = "あなたの親番ね、どうするの？";
			pokarHandsAndHelptext.text = "あなたの親番　ベットを決めてください";
			nowTurn = gameTurn.PLAYER_TURN;
			UI_Animation ();
		}else{
			int bt = enemy.CPUParentBet ();
			Debug.Log (bt);
			cpuHavsMedalCount += cpuNowBets;
			cpuNowBets += bt;
			cpuHavsMedalCount -= bt;
			helpandhandstext = "CPUの思考中";
			TextUpdate ();

			nowTurn = gameTurn.RAISE_CALL_TURN;
			UI_Animation ();

			//CPUのべっとからぷれいやーのれいずこーる
		}

	}

	//ボタン系の処理でボタンをタッチして呼び出す

	/// <summary>
	/// ベットボタン
	/// </summary>
	public void betButton(){
		//押してベットが増える
		if(playerHavsMedalCount>0){
			playerHavsMedalCount--;
			playeyNowBets++;
			TextUpdate ();
		}
	}

	/// <summary>
	/// ベットの決定ボタン.
	/// </summary>
	public void betDesideButton(){
		if(playeyNowBets>0){
			switch(enemy.thinkBet(playerHavsMedalCount,cpuHavsMedalCount)){
			case 0:
				//ドロップ
				playerHavsMedalCount += playeyNowBets;
				playeyNowBets = 0;
				cpuNowBets = 0;
				cpucoment = "これはダメね、降りるわ";
				helpandhandstext = "相手が降りました。";
				TextUpdate ();
				if(whoisparenet == true){
					whoisparenet = false;
				}else{
					whoisparenet = true;
				}

				nowTurn = gameTurn.DROP_TURN;
				UI_Animation ();

				break;
			case 1:
				//コール
				cpuHavsMedalCount += cpuNowBets;
				cpuNowBets = playeyNowBets;
				cpuHavsMedalCount -= cpuNowBets;
				cpucoment = "コールよ。";
				helpandhandstext = "コールされました、継続します。カードを交換してください";
				TextUpdate ();
				nowTurn = gameTurn.CHENGE_TURN;
				UI_Animation ();

				break;
			case 2:
				//レイズ
				continuecall = true;
				cpuHavsMedalCount += cpuNowBets;
				cpuNowBets = playeyNowBets + 1;
				cpuHavsMedalCount -= cpuNowBets;
				cpucoment = "レイズよ。さぁ、どうするの？";
				helpandhandstext = "相手がレイズしました、どうしますか？";
				TextUpdate ();
				nowTurn = gameTurn.RAISE_CALL_TURN;
				UI_Animation ();

				break;
			default:
				Debug.Log ("BetError");
				break;
			}
		}else{
			//ヘルプのテキストで別途させるテキストを表示
			helpandhandstext = "ベットしてください!!";
			TextUpdate ();
		}
	}

	/// <summary>
	/// レイズボタンの処理.
	/// </summary>
	public void raiseButton(){
		//レイズボタンの処理
		playerHavsMedalCount += playeyNowBets;
		playeyNowBets = cpuNowBets + 1;
		playerHavsMedalCount -= playeyNowBets;

		TextUpdate ();

		//cpuはこーるするか？
		if(enemy.DoCall() == true){
			cpuHavsMedalCount += cpuNowBets;
			cpuNowBets = playeyNowBets;
			cpuHavsMedalCount -= cpuNowBets;
			TextUpdate ();
			nowTurn = gameTurn.CHENGE_TURN;
			UI_Animation ();
		}else{
			playerHavsMedalCount += playeyNowBets;
			playeyNowBets = 0;
			cpuNowBets = 0;
			TextUpdate ();
			card.InitGame ();
			nowTurn = gameTurn.DROP_TURN;
			UI_Animation ();
		}

	}

	/// <summary>
	/// コールボタンの処理.
	/// </summary>
	public void callButton(){
		//こーるボタンの処理
		if(whoisparenet == true){
			//totalPlayerCount += playeyNowBets;
			playerHavsMedalCount += playeyNowBets;
			playeyNowBets = cpuNowBets;
			playerHavsMedalCount -= playeyNowBets;

			TextUpdate ();

			nowTurn = gameTurn.CHENGE_TURN;
			UI_Animation ();

		}else{
			playerHavsMedalCount += playeyNowBets;
			playeyNowBets = cpuNowBets;
			playerHavsMedalCount -= playeyNowBets;

			TextUpdate ();
			nowTurn = gameTurn.CHENGE_TURN;
			UI_Animation ();
		}
	}

	/// <summary>
	/// ドロップボタンの処理.
	/// </summary>
	public void dropButton(){
		//レイズボタンの処理
		playeyNowBets = 0;
		cpuNowBets = 0;
		helpandhandstext = "勝負をおりました。";

		TextUpdate ();

		nowTurn = gameTurn.DROP_TURN;
		UI_Animation ();
	}

	/// <summary>
	/// スタートボタン
	/// </summary>
	public void startButton(){
	//スタートボタン
	//ゲームのスタート
	//各プレイヤーのドロー処理
		if(playerHavsMedalCount <= 0 ||cpuHavsMedalCount <= 0){
			//めだるないよ
		}else{
			if(nowTurn == gameTurn.START_TURN){
				helpandhandstext = "場代として1メダルをBETします";
				playerHavsMedalCount -= 1;
				cpuHavsMedalCount -= 1;
				playeyNowBets += 1;
				cpuNowBets += 1;
				TextUpdate ();
				nowTurn = gameTurn.CARD_DRAW_TIME;
				UI_Animation ();
				card.DrawCard(5);
			}
		}
	}

	/// <summary>
	/// チェンジボタン
	/// </summary>
	public void chengeButton(){
			//チェンジボタン
			//プレイヤー手札のタッチ済みのカードをチェンジする。
		card.SerchIsTouched ("PLAYER");
	}

	/// <summary>
	/// コンティニューボタン
	/// </summary>
	public void continewButton(){
			//コンティニューボタン
			//プッシュ時にリスト及び全状態を初期化させる
		card.InitGame ();
		nowTurn = gameTurn.START_TURN;
		UI_Animation ();
	}

	public void TextUpdate(){
		cpuCommenttext.text = cpucoment;
		playerHaveMedaltext.text = "残りメダル:" + playerHavsMedalCount;
		cpuHaveMedaltext.text = "残りメダル:" + cpuHavsMedalCount;
		playerNowBettext.text = "Player Bet:" + playeyNowBets;
		cpuNowBettext.text = "CPU Bet:" + cpuNowBets;
		pokarHandsAndHelptext.text = helpandhandstext;
	}

	public void Test2(){
		Application.LoadLevel (2);
	}

	public void WinLose(string winner){
		if(winner == "ENEMY"){
			Debug.Log (winner);
			cpuHavsMedalCount += cpuNowBets + playeyNowBets;
			cpuNowBets = 0;
			playeyNowBets = 0;
			TextUpdate ();
		}else if(winner == "PLAYER"){
			Debug.Log (winner);
			playerHavsMedalCount += cpuNowBets + playeyNowBets;
			cpuNowBets = 0;
			playeyNowBets = 0;
			TextUpdate ();
		}else if(winner == "DRAW"){
			Debug.Log (winner);
			cpuNowBets = 0;
			playeyNowBets = 0;
			TextUpdate ();
		}
		if(whoisparenet == true){
			whoisparenet = false;
		}else{
			whoisparenet = false;
		}
		nowTurn = gameTurn.JUDGE_TURN;
		UI_Animation ();
	}

			
}
