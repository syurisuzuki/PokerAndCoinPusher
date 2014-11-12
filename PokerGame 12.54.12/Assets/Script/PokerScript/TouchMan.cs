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


	//タッチの有効無効を判断するbool
	public bool touchBool;

	enum gameTurn{
		START_TURN,
		CARD_DRAW_TIME,
		PLAYER_TURN,//プレイヤー親の場合
		ENEMY_TURN,//CPU親の場合
		RAISE_CALL_TURN,
		CHENGE_TURN,
		JUDGE_TURN,
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
		pokarHandsAndHelptext.text = "あなたは親　スタートボタンを押してください。";
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

			}
			break;
		case gameTurn.JUDGE_TURN:
			break;
		case gameTurn.CHENGE_TURN:
			cpuCommenttext.text = enemy.ChengeCommentText ();
			UIanim.SetTrigger ("StateCHENGE");
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
		cpuCommenttext.text = "あなたの親番ね、どうするの？";
		pokarHandsAndHelptext.text = "あなたの親番　ベットを決めてください";
		nowTurn = gameTurn.PLAYER_TURN;
		UI_Animation ();
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
			playerNowBettext.text = "Player Bet:" + playeyNowBets;
			playerHaveMedaltext.text = "残りメダル:" + playerHavsMedalCount;
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
				cpuCommenttext.text = "これはダメね、降りるわ";
				pokarHandsAndHelptext.text = "相手が降りました。";
				whoisparenet = false;
				playeyNowBets = 0;

				break;
			case 1:
				//コール
				int calls = playeyNowBets;
				cpuNowBets += calls;
				cpuHavsMedalCount -= calls;

				cpuNowBettext.text = "CPU Bet:" + cpuNowBets;
				cpuHaveMedaltext.text = "残りメダル:" + cpuHavsMedalCount;
				playerNowBettext.text = "Player Bet:" + playeyNowBets;
				playerHaveMedaltext.text = "残りメダル:" + playerHavsMedalCount;

				cpuCommenttext.text = "コールよ。";
				pokarHandsAndHelptext.text = "相手がコールしました、継続します。";
				nowTurn = gameTurn.CHENGE_TURN;
				UI_Animation ();

				break;
			case 2:
				//レイズ
				int raise = playeyNowBets + 1;
				cpuNowBets += raise;
				cpuHavsMedalCount -= raise;

				cpuNowBettext.text = "CPU Bet:" + cpuNowBets;
				cpuHaveMedaltext.text = "残りメダル:" + cpuHavsMedalCount;

				cpuCommenttext.text = "レイズよ。さぁ、どうするの？";
				pokarHandsAndHelptext.text = "相手がレイズしました、どうしますか？";
				nowTurn = gameTurn.RAISE_CALL_TURN;
				UI_Animation ();

				break;
			default:
				Debug.Log ("BetError");
				break;
			}
		}else{
			//ヘルプのテキストで別途させるテキストを表示
			pokarHandsAndHelptext.text = "ベットしてください!!";
		}
	}

	/// <summary>
	/// レイズボタンの処理.
	/// </summary>
	public void raiseButton(){
		//レイズボタンの処理
	}

	/// <summary>
	/// コールボタンの処理.
	/// </summary>
	public void callButton(){
		//レイズボタンの処理
		if(continuecall == true){
			totalPlayerCount += playeyNowBets;
			playeyNowBets = cpuNowBets;

			playerNowBettext.text = "Player Bet:" + playeyNowBets;
			playerHaveMedaltext.text = "残りメダル:" + playerHavsMedalCount;

			nowTurn = gameTurn.CHENGE_TURN;
			UI_Animation ();

		}else{

		}
	}

	/// <summary>
	/// ドロップボタンの処理.
	/// </summary>
	public void dropButton(){
		//レイズボタンの処理
		playeyNowBets = 0;
		cpuNowBets = 0;

		playerNowBettext.text = "Player Bet:" + playeyNowBets;
		cpuNowBettext.text = "CPU Bet:" + cpuNowBets;

		pokarHandsAndHelptext.text = "勝負をおりました。";

		nowTurn = gameTurn.START_TURN;
		UI_Animation ();
	}

	/// <summary>
	/// スタートボタン
	/// </summary>
	public void startButton(){
	//スタートボタン
	//ゲームのスタート
	//各プレイヤーのドロー処理
		if(nowTurn == gameTurn.START_TURN){
			nowTurn = gameTurn.CARD_DRAW_TIME;
			UI_Animation ();
			card.DrawCard(5);
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
	}

	public void Test2(){
		Application.LoadLevel (2);
	}

			
}
