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

	//コールの有無
	public bool continuecall = false;

	//UIのstring
	public string helpandhandstext;
	public string cpucoment;

	//exitbtnのオブジェクト
	public GameObject exitbtn;


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

		//エネミースクリプトを取得
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

	/// <summary>
	/// 現在のステートに対するUIのアニメーションを
	/// 行う.
	/// </summary>
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
			UIanim.SetTrigger ("StateCHENGE");
			break;
		case gameTurn.DROP_TURN:
			if(whoisparenet == true){
				whoisparenet = false;
			}else{
				whoisparenet = true;
			}
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

	/// <summary>
	/// ドローターンにチェンジする.
	/// </summary>
	public void Chenge_Draw_Turn(){
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
			cpucoment = "あなたの親番ね、\nどうするの？";
			pokarHandsAndHelptext.text = "あなたの親番ですベットを決めてください";
			nowTurn = gameTurn.PLAYER_TURN;
			UI_Animation ();
		}else{
			int bt = enemy.CPUParentBet ();
			Debug.Log (bt);
			cpuHavsMedalCount += cpuNowBets;
			cpuNowBets += bt;
			cpuHavsMedalCount -= cpuNowBets;
			cpucoment = "まあまあね♪";
			enemy.ChengeFaceSprite (9);
			helpandhandstext = "CPUのベットが決まりました。どうしますか？";
			TextUpdate ();

			nowTurn = gameTurn.RAISE_CALL_TURN;
			UI_Animation ();
		}

	}

	/// <summary>
	/// ベットボタン
	/// </summary>
	public void betButton(){
		//押してベットが増える
		if(playerHavsMedalCount>0){
			playerHavsMedalCount--;
			playeyNowBets++;
			helpandhandstext = "ベット確定ボタンを押してください";
			cpucoment = enemy.ChengeCommentCall ();
			enemy.ChengeFaceSprite (15);
			TextUpdate ();
		}
	}

	/// <summary>
	/// ベットの決定ボタン.
	/// </summary>
	public void betDesideButton(){
		if(playeyNowBets>1){
			switch(enemy.thinkBet(playerHavsMedalCount,cpuHavsMedalCount)){
			case 0:
				//ドロップ
				playerHavsMedalCount += playeyNowBets;
				playeyNowBets = 0;
				cpuNowBets = 0;
				cpucoment = "これはダメね、\n降りるわ";
				enemy.ChengeFaceSprite (8);
				helpandhandstext = "相手が降りました。";
				TextUpdate ();

				nowTurn = gameTurn.DROP_TURN;
				UI_Animation ();

				break;
			case 1:
				//コール
				cpuHavsMedalCount += cpuNowBets;
				cpuNowBets = playeyNowBets;
				cpuHavsMedalCount -= cpuNowBets;
				cpucoment = "コールよ。";
				enemy.ChengeFaceSprite (12);
				helpandhandstext = "コールされました。カードを交換してください";
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
				cpucoment = "レイズよ。\nさぁ、どうするの？";
				enemy.ChengeFaceSprite (4);
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
			cpucoment ="レイズ!?\n乗るわその勝負。";
			enemy.ChengeFaceSprite (13);
			helpandhandstext = "残したいカードをタッチしてください。";
			TextUpdate ();
			nowTurn = gameTurn.CHENGE_TURN;
			UI_Animation ();
		}else{
			playerHavsMedalCount += playeyNowBets;
			playeyNowBets = 0;
			cpuNowBets = 0;
			cpucoment ="レイズ!?\n降りるわ";
			enemy.ChengeFaceSprite (7);
			helpandhandstext = "相手が降りました。";
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
			cpucoment ="へぇ、\nいいじゃない。";
			enemy.ChengeFaceSprite (12);
			helpandhandstext = "残したいカードをタッチしてください。";

			TextUpdate ();

			nowTurn = gameTurn.CHENGE_TURN;
			UI_Animation ();

		}else{
			playerHavsMedalCount += playeyNowBets;
			playeyNowBets = cpuNowBets;
			playerHavsMedalCount -= playeyNowBets;
			cpucoment ="へぇ、乗ってくるの？\nいいのかしら？";
			enemy.ChengeFaceSprite (4);
			helpandhandstext = "残したいカードをタッチしてください。";

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
		cpuHavsMedalCount += cpuNowBets;
		cpuNowBets = 0;
		helpandhandstext = "勝負をおりました。";
		cpucoment = "良い判断ね。";
		enemy.ChengeFaceSprite (13);
		TextUpdate ();
			//card.InitGame ();
		nowTurn = gameTurn.DROP_TURN;
		UI_Animation ();
	}

	/// <summary>
	/// スタートボタン
	/// </summary>
	public void startButton(){
		if(playerHavsMedalCount <= 0 ||cpuHavsMedalCount <= 0){
			exitbtn.SetActive (true);
			if(playerHavsMedalCount<=0){
				pokarHandsAndHelptext.text = "メダルがありません、あなたの負けです";
			}

			if(cpuHavsMedalCount <=0){
				pokarHandsAndHelptext.text = "メダルがありません、あなたの勝ちです";
			}
		}else{
			if(nowTurn == gameTurn.START_TURN){
				cpucoment = "良いカードがきますように・・・";
				enemy.ChengeFaceSprite (1);
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
		cpucoment = "さあ、勝負よ。";
		enemy.ChengeFaceSprite (2);
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
		cpucoment = "続けるのね、\nよろしく。";
		enemy.ChengeFaceSprite (10);
		TextUpdate ();
	}

	/// <summary>
	/// ゲーム終了ボタン.
	/// </summary>
	public void ExitGameButton(){
		Application.LoadLevel (0);
	}
	/// <summary>
	/// テキストの内容を更新する.
	/// </summary>
	public void TextUpdate(){
		cpuCommenttext.text = cpucoment;
		playerHaveMedaltext.text = "残りメダル:" + playerHavsMedalCount;
		cpuHaveMedaltext.text = "残りメダル:" + cpuHavsMedalCount;
		playerNowBettext.text = "Player Bet:" + playeyNowBets;
		cpuNowBettext.text = "CPU Bet:" + cpuNowBets;
		pokarHandsAndHelptext.text = helpandhandstext;
	}

	/// <summary>
	/// コインプッシャーのシーンへ移動.
	/// </summary>
	public void GoCoinPusherScene(){
		Application.LoadLevel (2);
	}

	/// <summary>
	/// 勝負結果に対してのメダル数の変更処理.
	/// </summary>
	/// <param name="winner">Winner.</param>
	public void WinLose(string winner){
		if(winner == "ENEMY"){
			Debug.Log (winner);
			cpuHavsMedalCount += cpuNowBets + playeyNowBets;
			cpuNowBets = 0;
			playeyNowBets = 0;
			helpandhandstext = "あなたの負けです";
			cpucoment = "あらあら、勝っちゃったわ。";
			enemy.ChengeFaceSprite (13);
			TextUpdate ();
		}else if(winner == "PLAYER"){
			Debug.Log (winner);
			playerHavsMedalCount += cpuNowBets + playeyNowBets;
			cpuNowBets = 0;
			playeyNowBets = 0;
			helpandhandstext = "あなたの勝ちです!";
			cpucoment = "た、たまたまよ！次は勝つわ。";
			enemy.ChengeFaceSprite (7);
			TextUpdate ();
		}else if(winner == "DRAW"){
			Debug.Log (winner);
			cpuHavsMedalCount += 1;
			playerHavsMedalCount += 1;
			cpuNowBets = 0;
			playeyNowBets = 0;
			helpandhandstext = "ドローです、場代は返還されます。";
			cpucoment = "なかなかやるじゃない・・・";
			enemy.ChengeFaceSprite (6);
			TextUpdate ();
		}

		//親の変更
		if(whoisparenet == true){
			whoisparenet = false;
		}else{
			whoisparenet = true;
		}

		nowTurn = gameTurn.JUDGE_TURN;
		UI_Animation ();
	}

			
}
