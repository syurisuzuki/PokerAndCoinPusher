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
	public UILabel gamecount;
	public int gamecounts;
	public UILabel getmedals;
	public int getmedalcounts;
	public UILabel playerhandsandpoint;
	public UILabel helpandenemyhandstext;

	public UILabel cpucomment;
	public UILabel cpustatus1;
	public UILabel cpustatus2;

	//画面右から123
	public UIButton btn1;
	public UIButton btn2;
	public UIButton btn3;

	//プレイヤー予想倍率
	public int myyosou;
	//敵予想倍率
	public int enemyyosou;







	//true:プレイヤーが親　false:CPUが親
	public bool whoisparenet = true;

	public bool pl_IsDrop;
	public bool cpu_IsDrop;


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
			Instantiate (Enemyp, new Vector3(-1.8f,5.2f,0), Quaternion.identity);
		}

		//エネミースクリプトを取得
		enemy = FindObjectOfType<Enemy>();
		players = FindObjectOfType<Player> ();
		elice = FindObjectOfType<EliceComent> ();
		judges = FindObjectOfType<Judge> ();

		//テキストを変更する

		gamecount.text = "残りゲーム数:" + gamecounts;
		getmedals.text = "獲得メダル:" + getmedalcounts;
		cpucomment.text = "よろしくね";
		playerhandsandpoint.text = "スタートボタンを押してください。";
		helpandenemyhandstext.text = "えええええ。";
	
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
			int ph = judges.PokarHandsInt(enemy.EnemyCardNum,enemy.EnemyCardMark);
			cpucoment = elice.pokarfacecoment(lovepoint,ph);
			helpandenemyhandstext.text = "あなたの手は\nどうですか？";
			PlayerBetter ();
			//プレイヤー親の処理
			//betさせる

			nowTurn = gameTurn.PLAYER_TURN;
			//UI_Animation ();
		}else{
			int bt = enemy.CPUParentBet ();
			int ph = judges.PokarHandsInt(enemy.EnemyCardNum,enemy.EnemyCardMark);
			cpucoment = elice.pokarfacecoment(lovepoint,ph);

			string tdd = "sda";

			switch (bt) {
			case 1:
				tdd = "自信満々";
				break;
			case 2:
				tdd = "自信あり";
				break;
			case 3:
				tdd = "自信なし";
				break;
			}

			helpandenemyhandstext.text = "相手は"+tdd+"\nのようです\nあなたの手は\nどうですか？";

			Playerthink ();

		}

	}

	void Playerthink(){
		AllBtnEnable (true);
		UILabel btntext;
		btntext = btn1.GetComponentInChildren<UILabel> ();
		btntext.text = "絶対負けない";
		btntext = btn2.GetComponentInChildren<UILabel> ();
		btntext.text = "自信ある";
		btntext = btn3.GetComponentInChildren<UILabel> ();
		btntext.text = "負けそう";
		EventDelegate.Set(btn1.onClick, RaiseP);
		EventDelegate.Set(btn2.onClick, CallP);
		EventDelegate.Set(btn3.onClick, DropP);
	}

	void PlayerBetter(){
		AllBtnEnable (true);
		UILabel btntext;
		btntext = btn1.GetComponentInChildren<UILabel> ();
		btntext.text = "絶対負けない";
		btntext = btn2.GetComponentInChildren<UILabel> ();
		btntext.text = "自信ある";
		btntext = btn3.GetComponentInChildren<UILabel> ();
		btntext.text = "負けそう";
		EventDelegate.Set(btn1.onClick, WinWinYosou);
		EventDelegate.Set(btn2.onClick, WinYosou);
		EventDelegate.Set(btn3.onClick, loseYosou);
	}

	void CallP(){
		Invoke ("btnset", 1);
	}

	void RaiseP(){
		Invoke ("btnset", 1);
	}

	void DropP(){
		Invoke ("btnset", 1);
	}


	void WinWinYosou(){
		myyosou = 3;
		EnemyYosou (myyosou);
	}

	void WinYosou(){
		myyosou = 2;
		EnemyYosou (myyosou);
	}

	void loseYosou(){
		myyosou = 1;
		EnemyYosou (myyosou);
	}

	void AllBtnEnable(bool btnbool){
		btn1.enabled = btnbool;
		btn2.enabled = btnbool;
		btn3.enabled = btnbool;
	}

	void btnset(){
		helpandenemyhandstext.text = "カードを交換してください";
		UILabel btntext;
		btntext = btn1.GetComponentInChildren<UILabel> ();
		btntext.text = "交換";
		btntext = btn2.GetComponentInChildren<UILabel> ();
		btntext.text = "オート選択";
		btntext = btn3.GetComponentInChildren<UILabel> ();
		btntext.text = "全残し";
		EventDelegate.Set(btn1.onClick, CardChenge);
		EventDelegate.Set(btn2.onClick, SelectAuto);
		EventDelegate.Set(btn3.onClick, SelectCancel);
		touchBool = true;
	}

	void EnemyYosou(int playeryosou){

		//敵の予想の後にカードの交換へ
		Invoke ("btnset", 1);


		switch(enemy.thinkBet()){
		case 0:
			//自信なし　等倍
			enemyyosou = 1;
			switch (playeryosou) {
			case 1:
				cpucomment.text = "自信なし自信なし";
				break;
			case 2:
				cpucomment.text = "自信あり自信なし";
				break;
			case 3:
				cpucomment.text = "自信超あり自信なし";
				break;
			}
			break;
		case 1:
			//自信あり　２倍
			enemyyosou = 2;
			switch (playeryosou) {
			case 1:
				cpucomment.text = "自信なし自信あり";
				break;
			case 2:
				cpucomment.text = "自信あり自信あり";
				break;
			case 3:
				cpucomment.text = "自信超あり自信あり";
				break;
			}
			break;
		case 2:
			//自信あり　３倍
			enemyyosou = 3;
			switch (playeryosou) {
			case 1:
				cpucomment.text = "自信なし自信超あり";
				break;
			case 2:
				cpucomment.text = "自信あり自信超あり";
				break;
			case 3:
				cpucomment.text = "自信超あり自信超あり";
				break;
			}
			break;
		default:
			Debug.Log ("BetError");
			break;
		}

	}

	void CardChenge(){
		//プレイヤー手札のタッチ済みのカードをチェンジする。
		cpucoment = "さあ、勝負よ。";
		enemy.ChengeFaceSprite (2);
		card.SerchIsTouched ("PLAYER");
		touchBool = false;
		AllBtnEnable (false);
	}

	void SelectAuto(){
		//自動選択
		card.AutoSelectPlayerCard ();
	}

	void SelectCancel(){
		//全てタッチ
		card.PlayerAllselect ();
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
			//cpucoment = enemy.ChengeCommentCall ();
			//enemy.ChengeFaceSprite (15);
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
			switch(enemy.thinkBet()){
			case 0:
				//ドロップ
				playerHavsMedalCount += playeyNowBets;
				playeyNowBets = 0;
				cpuNowBets = 0;

				cpucoment = elice.dropcoment(lovepoint);

				//enemy.ChengeFaceSprite (8);
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
				//enemy.ChengeFaceSprite (12);
				helpandhandstext = "残したいカードをタッチしてください。";
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
				//enemy.ChengeFaceSprite (4);
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
			helpandenemyhandstext.text = helpandhandstext;
			//TextUpdate ();
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
			//enemy.ChengeFaceSprite (12);
			helpandhandstext = "残したいカードをタッチしてください。";

			TextUpdate ();

			nowTurn = gameTurn.CHENGE_TURN;
			UI_Animation ();

		}else{
			playerHavsMedalCount += playeyNowBets;
			playeyNowBets = cpuNowBets;
			playerHavsMedalCount -= playeyNowBets;
			cpucoment ="へぇ、乗ってくるの？\nいいのかしら？";
			//enemy.ChengeFaceSprite (4);
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
		helpandhandstext = "勝負をおりました。50メダル失いました";
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
		card.DrawCard(5);
		AllBtnEnable (false);
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
	/// テキストの内容を更新する.
	/// </summary>
	public void TextUpdate(){
		cpucomment.text = cpucoment;
		helpandenemyhandstext.text = helpandhandstext;
	}

	/// <summary>
	/// 勝負結果に対してのメダル数の変更処理.
	/// </summary>
	/// <param name="winner">Winner.</param>
	public void WinLose(string winner,int playerhand,int cpuhand,int pokarHandsScore){
		string phand = handintforstring (playerhand);
		string cpuhandss = handintforstring(cpuhand);

		if(winner == "ENEMY"){
			//アタック値の計算>CPUの所持カードの得点を取得しベットで乗算する

			int Attackint = 0;

			foreach (int score in enemy.EnemyCardScore) {
				Attackint += score;
			}



			helpandenemyhandstext.text = phand+"\nVS\n"+cpuhandss+"\nあなたの負け";

			int damage = (Attackint + pokarHandsScore) * enemyyosou * myyosou;

			cpucoment = elice.Wincoment(lovepoint);
			//enemy.ChengeFaceSprite (13);
		}else if(winner == "PLAYER"){

			//アタック値の計算>PLAYERの所持カードの得点を取得しベットで乗算する

			int Attackint = 0;

			foreach(int score in players.handCardScore){
				Attackint += score;
			}

			int damage = (Attackint + pokarHandsScore) * enemyyosou * myyosou;

			helpandenemyhandstext.text = phand+"\nVS\n"+cpuhandss+"\nあなたの勝ち";
			cpucoment = elice.Losecoment(lovepoint);
			//enemy.ChengeFaceSprite (7);
			lovepoint += 1;
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
				helpandenemyhandstext.text = pscore+"\nVS\n"+cpuscore+"\nあなたの勝ち";
				cpucoment = elice.Losecoment(lovepoint);
				SaveMedal();
				//enemy.ChengeFaceSprite (7);
			}else if(pscore < cpuscore){
				int damage = cpuscore - pscore;

				helpandenemyhandstext.text = pscore+"\nVS\n"+cpuscore+"\nあなたの負け";
				cpucoment = elice.Wincoment(lovepoint);
				SaveMedal();
				//enemy.ChengeFaceSprite (13);
			}else{
				cpuHavsMedalCount += 1;
				playerHavsMedalCount += 1;
				cpuNowBets = 0;
				playeyNowBets = 0;
				helpandhandstext = "ドローです、場代は返還されます。";
				cpucoment = elice.Drawcoment(lovepoint);
				SaveMedal();
				//enemy.ChengeFaceSprite (6);
			}
		}

		//親の変更
		if(whoisparenet == true){
			whoisparenet = false;
		}else{
			whoisparenet = true;
		}
		nowTurn = gameTurn.JUDGE_TURN;

		//継続するかしないか決める
		UILabel btntext;
		btntext = btn1.GetComponentInChildren<UILabel> ();
		btntext.text = "続ける";
		btn1.enabled = true;
		btntext = btn2.GetComponentInChildren<UILabel> ();
		btntext.text = "やめる";
		btn2.enabled = true;
		btntext = btn3.GetComponentInChildren<UILabel> ();
		btntext.text = "";
		btn3.enabled = false;
		EventDelegate.Set(btn1.onClick, continewButton);
		EventDelegate.Set(btn2.onClick, ExitGameButton);
	}


	/// <summary>
	/// コンティニューボタン
	/// </summary>
	public void continewButton(){
		//コンティニューボタン
		//プッシュ時にリスト及び全状態を初期化させる

		if (gamecounts > 0) {
			gamecounts--;
			card.InitGame ();
			nowTurn = gameTurn.START_TURN;
			UI_Animation ();
			cpucoment = elice.Continwecoment (lovepoint);
			enemy.ChengeFaceSprite (10);

			UILabel btntext;
			btntext = btn1.GetComponentInChildren<UILabel> ();
			btntext.text = "スタート";
			btn1.enabled = true;
			EventDelegate.Set(btn1.onClick, startButton);


			TextUpdate ();
			SaveMedal ();
		
		} else {
			helpandenemyhandstext.text = "ゲーム終了です";
			btn1.enabled = false;
		}


	}

	/// <summary>
	/// ゲーム終了ボタン.
	/// </summary>
	public void ExitGameButton(){
		PlayerPrefs.SetInt("LP",lovepoint);
		PlayerPrefs.SetInt ("HaveMedals",getmedalcounts);
		Application.LoadLevel (0);
	}



	public void SaveMedal(){
		PlayerPrefs.SetInt("PMedals",playerHavsMedalCount);
		PlayerPrefs.SetInt("CMedals",cpuHavsMedalCount);
	}

	public static string handintforstring(int id){
		switch (id) {
		case 0:
			return "ノーペア";
			break;
		case 1:
			return "ワンペア";
			break;
		case 2:
			return "ツーペア";
			break;
		case 3:
			return "スリーカード";
			break;
		case 4:
			return "ストレイト";
			break;
		case 5:
			return "フラッシュ";
			break;
		case 6:
			return "フルハウス";
			break;
		case 7:
			return "フォーカード";
			break;
		case 8:
			return "ストレイトフラッシュ";
			break;
		case 9:
			return "ファイブカード";
			break;
		case 10:
			return "ロイヤルストレートフラッシュ";
			break;
		}
		return "あああ";
	}

	public void NowhandsText(int hands){
		var ss = 0;
		foreach(int score in players.handCardScore){
			ss += score;
		}
		switch (hands){
		case 0:
			playerhandsandpoint.text = "現在\nノーペア\n強さ"+ss;
			break;
		case 1:
			playerhandsandpoint.text = "現在\nワンペア\n強さ"+ss;
			break;
		case 2:
			playerhandsandpoint.text = "現在\nツーペア\n強さ"+ss;
			break;
		case 3:
			playerhandsandpoint.text = "現在\nスリーカード\n強さ"+ss;
			break;
		case 4:
			playerhandsandpoint.text = "現在\nストレイト\n強さ"+ss;
			break;
		case 5:
			playerhandsandpoint.text = "現在\nフラッシュ\n強さ"+ss;
			break;
		case 6:
			playerhandsandpoint.text = "現在\nフルハウス\n強さ"+ss;
			break;
		case 7:
			playerhandsandpoint.text = "現在\nフォーカード\n強さ"+ss;
			break;
		case 8:
			playerhandsandpoint.text = "現在\nストレイトフラッシュ\n強さ"+ss;
			break;
		case 9:
			playerhandsandpoint.text = "現在\nファイブカード\n強さ"+ss;
			break;
		case 10:
			playerhandsandpoint.text = "現在\nロイヤルストレートフラッシュ\n強さ"+ss;
			break;
		default :
			break;
		}



	}

			
}
