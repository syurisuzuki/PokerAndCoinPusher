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
	Player players;
	Judge judges;
	EliceComent elice;

	//UI用のテキストの宣言
	public Text playerHaveMedaltext;
	public Text cpuHaveMedaltext;
	public Text pokarHandsAndHelptext;
	public Text cpuCommenttext;

	public Text pScore;
	public Text cScore;

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


	public int lovepoint;


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
		lovepoint = PlayerPrefs.GetInt("LP",0);
		playerHavsMedalCount = PlayerPrefs.GetInt("PMedals",1000);
		cpuHavsMedalCount = PlayerPrefs.GetInt("CMedals",1000);
		card = GetComponent<Card>();
		UIanim = canvas.GetComponent<Animator> ();

		nowTurn = gameTurn.START_TURN;

		//プレイヤーをインスタンス化
		Instantiate (Player, transform.position, Quaternion.identity);

		//totalPlayerCount-1 の数だけEnemyをインスタンス化
		for(int i = 0;i<totalPlayerCount - 1;i++){
			Instantiate (Enemyp, new Vector3(-2,5.2f,0), Quaternion.identity);
		}

		//エネミースクリプトを取得
		enemy = FindObjectOfType<Enemy>();
		players = FindObjectOfType<Player> ();
		elice = FindObjectOfType<EliceComent> ();

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
		if(touchBool == true){
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

			//po-ka-face



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
			touchBool = true;
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
			if(playeyNowBets == 3){
				helpandhandstext = "これ以上ベット出来ません。";
				TextUpdate ();
				betDesideButton ();
			}
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

				cpucoment = elice.dropcoment(lovepoint);

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
				cpucoment = elice.callcoment(lovepoint);
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
				cpucoment = elice.raisecoment(lovepoint);
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
			cpucoment = elice.raisecallcoment(lovepoint);
			enemy.ChengeFaceSprite (13);
			helpandhandstext = "残したいカードをタッチしてください。";
			TextUpdate ();
			nowTurn = gameTurn.CHENGE_TURN;
			UI_Animation ();
		}else{
			playerHavsMedalCount += playeyNowBets;
			playeyNowBets = 0;
			cpuNowBets = 0;
			cpucoment = elice.dropcoment(lovepoint);
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
		playeyNowBets = 0;
		playerHavsMedalCount -= 50;
		cpuHavsMedalCount += cpuNowBets;
		cpuNowBets = 0;
		helpandhandstext = "勝負をおりました。";
		cpucoment = elice.PlayerDropcoment(lovepoint);
		enemy.ChengeFaceSprite (13);
		TextUpdate ();
		nowTurn = gameTurn.DROP_TURN;
		UI_Animation ();
	}

	/// <summary>
	/// スタートボタン
	/// </summary>
	public void startButton(){
		lovepoint += 1;
		PlayerPrefs.SetInt("LP",lovepoint);
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
		//プレイヤー手札のタッチ済みのカードをチェンジする。
		if(touchBool == true){
			cpucoment = "さあ、勝負よ。";
			enemy.ChengeFaceSprite (2);
			card.SerchIsTouched ("PLAYER");
			touchBool = false;
		}

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
		cpucoment = elice.Continwecoment(lovepoint);
		enemy.ChengeFaceSprite (10);
		pScore.text = "";
		cScore.text = "";
		TextUpdate ();
	}

	/// <summary>
	/// ゲーム終了ボタン.
	/// </summary>
	public void ExitGameButton(){
		PlayerPrefs.SetInt("LP",lovepoint);
		PlayerPrefs.SetInt ("Pmedals",1000);
		PlayerPrefs.SetInt ("CMedals", 1000);
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
	/// 勝負結果に対してのメダル数の変更処理.
	/// </summary>
	/// <param name="winner">Winner.</param>
	public void WinLose(string winner,int pokarHandsScore){
		if(winner == "ENEMY"){
			Debug.Log (winner);

			//アタック値の計算>CPUの所持カードの得点を取得しベットで乗算する

			int Attackint = 0;

			foreach (int score in enemy.EnemyCardScore) {
				Attackint += score;
			}

			int damage = Attackint * cpuNowBets + pokarHandsScore;

			pScore.text = "-"+damage;

			playerHavsMedalCount -= damage;
			cpuNowBets = 0;
			playeyNowBets = 0;
			helpandhandstext = "YOU LOSE "+damage+" 失いました";
			cpucoment = elice.Wincoment(lovepoint);
			//enemy.ChengeFaceSprite (13);
			lovepoint += 3;
			TextUpdate ();
		}else if(winner == "PLAYER"){

			//アタック値の計算>PLAYERの所持カードの得点を取得しベットで乗算する

			int Attackint = 0;

			foreach(int score in players.handCardScore){
				Attackint += score;
			}

			int damage = Attackint * playeyNowBets + pokarHandsScore;

			cScore.text = "-"+damage;

			cpuHavsMedalCount -= damage;
			cpuNowBets = 0;
			playeyNowBets = 0;
			helpandhandstext = "YOU WIN "+damage+" 減らしました";
			cpucoment = elice.Losecoment(lovepoint);
			//enemy.ChengeFaceSprite (7);
			lovepoint += 2;
			TextUpdate ();
		}else if(winner == "DRAW"){
			int pscore = 0;
			int cpuscore = 0;

			//アタック値の計算>PLAYERとCPUのアタック値で大きい方から小さい方を引く

			foreach(int score in players.handCardScore){
				pscore += score;
			}

			foreach (int score in enemy.EnemyCardScore) {
				cpuscore += score;
			}

			if(pscore > cpuscore){
				int damage = pscore - cpuscore;

				cpuHavsMedalCount -= damage;
				cpuNowBets = 0;
				playeyNowBets = 0;
				cScore.text = "-"+damage;
				helpandhandstext = "DRAW-WIN "+damage+" 減らしました";
				cpucoment = elice.Losecoment(lovepoint);
				lovepoint += 2;
				SaveMedal();
				//enemy.ChengeFaceSprite (7);
			}else if(pscore < cpuscore){
				int damage = cpuscore - pscore;

				playerHavsMedalCount -= damage;
				cpuNowBets = 0;
				playeyNowBets = 0;
				pScore.text = "-"+damage;
				helpandhandstext = "DRAW-LOSE "+damage+" 失いました";
				cpucoment = elice.Wincoment(lovepoint);
				lovepoint += 3;
				SaveMedal();
				//enemy.ChengeFaceSprite (13);
			}else{
				cpuHavsMedalCount += 1;
				playerHavsMedalCount += 1;
				cpuNowBets = 0;
				playeyNowBets = 0;
				helpandhandstext = "ドローです、場代は返還されます。";
				cpucoment = elice.Drawcoment(lovepoint);
				lovepoint += 1;
				SaveMedal();
				//enemy.ChengeFaceSprite (6);
			}

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


	public void SaveMedal(){
		PlayerPrefs.SetInt("PMedals",playerHavsMedalCount);
		PlayerPrefs.SetInt("CMedals",cpuHavsMedalCount);
	}

			
}
