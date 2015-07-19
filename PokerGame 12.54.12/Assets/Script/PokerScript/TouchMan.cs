using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 現在の状態を管理し
/// UI,Touch系の機能の実装クラス.
/// </summary>
public class TouchMan : MonoBehaviour {

	//ゲームの総プレイヤー人数
	[SerializeField] int totalPlayerCount;

	//プレイヤーオブジェクト
	public GameObject Player;
	//CPUオブジェクト
	public GameObject Enemyp;
	public GameObject[] Enemys;

	//カードのスクリプト
	Card card;
	Enemy enemy;
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
	public float myyosou;
	//敵予想倍率
	public int enemyyosou;
	public int nowgamecount;
	public UILabel gamecountL;

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

	//コールの有無
	public bool continuecall = false;

	//UIのstring
	public string helpandhandstext;
	public string cpucoment;

	//exitbtnのオブジェクト
	public GameObject exitbtn;
	public int koukand;
	//タッチの有効無効を判断するbool
	public bool touchBool;
	public UILabel p_hp;
	public UILabel p_at;
	public UILabel p_de;

	public UILabel c_hp;
	public UILabel c_at;
	public UILabel c_de;

	int php;
	public int pat;
	public int pde;

	int chp;
	public int cat;
	public int cde;

	public GameObject windowobj;
	public GameObject chutoobj;

	public UILabel p_heal;
	public UILabel p_dam;

	public UILabel c_heal;
	public UILabel c_dam;

	int specialitem;
	bool reave;
	bool healDouble;
	bool noheal;
	bool coinDouble;

	string charalevel;

	int bonus;

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

		noheal = false;
		coinDouble = false;
		reave = false;
		healDouble = false;
		nowgamecount = 0;
		playerHavsMedalCount = 1000;
		card = GetComponent<Card>();

		//プレイヤーをインスタンス化
		Instantiate (Player, transform.position, Quaternion.identity);
		int chara = PlayerPrefs.GetInt ("selectChara", 1);
		bonus = 1000 * chara;
		string kokandkey = "Actor"+chara+"LovePoint";
		koukand = PlayerPrefs.GetInt (kokandkey, 0);
		Instantiate (Enemys[chara - 1], new Vector3(-1.6f,5.2f,0), Quaternion.identity);

		enemy = FindObjectOfType<Enemy>();
		elice = FindObjectOfType<EliceComent> ();

		//テキストを変更する

		gamecount.text = "現在のゲーム数:" + gamecounts;
		getmedals.text = "所持メダル:" + playerHavsMedalCount;

		specialitem = PlayerPrefs.GetInt ("Special", 0);

		php = FindObjectOfType<Player> ().hp;
		pat = FindObjectOfType<Player> ().attackPower;
		pde = FindObjectOfType<Player> ().defence;

		if (specialitem == 0) {
		} else if (specialitem == 1) {
			php *= 3;
			PlayerPrefs.SetInt ("Special", 0);
		} else if (specialitem == 2) {
			pat *= 2;
			PlayerPrefs.SetInt ("Special", 0);
		}else if (specialitem == 3) {
			pde *= 2;
			PlayerPrefs.SetInt ("Special", 0);
		}else if (specialitem == 6) {
			pat *= 2;
			pde *= 2;
			noheal = true;
			PlayerPrefs.SetInt ("Special", 0);
		}else if (specialitem == 4) {
			reave = true;
			PlayerPrefs.SetInt ("Special", 0);
		}else if (specialitem == 5) {
			healDouble = true;
			PlayerPrefs.SetInt ("Special", 0);
		}else if (specialitem == 8) {
			coinDouble = true;
			PlayerPrefs.SetInt ("Special", 0);
		}


		p_hp.text = "HP:"+php;
		p_at.text = "攻撃:"+pat;
		p_de.text = "防御:"+pde;

		chp = FindObjectOfType<Enemy> ().hp;
		cat = FindObjectOfType<Enemy> ().at;
		cde = FindObjectOfType<Enemy> ().de;

		int music = Random.Range (0, 6);

		switch (music) {
		case 0:
			Singleton<SoundPlayer>.instance.playBGM( "battle",0 );
			break;
		case 1:
			Singleton<SoundPlayer>.instance.playBGM( "battle2",0 );
			break;
		case 2:
			Singleton<SoundPlayer>.instance.playBGM( "battle3",0 );
			break;
		case 3:
			Singleton<SoundPlayer>.instance.playBGM( "boss",0 );
			break;
		case 4:
			Singleton<SoundPlayer>.instance.playBGM( "boss2",0 );
			break;
		case 5:
			Singleton<SoundPlayer>.instance.playBGM( "boss3",0 );
			break;
		default:
			break;
		}


		if (koukand < 100) {
			charalevel = "Actor" + chara + "Level1";
			cpucomment.text = FindObjectOfType<EliceComent>().l1hello;
		} else if (koukand < 200 && koukand >= 100) {
			charalevel = "Actor" + chara + "Level2";
			cpucomment.text = FindObjectOfType<EliceComent>().l2hello;
			chp *= 2;
			cat *= 2;
			cde *= 2;
			bonus *= 2;
		} else if (koukand >= 200) {
			charalevel = "Actor" + chara + "Level3";
			cpucomment.text = FindObjectOfType<EliceComent>().l3hello;
			chp *= 3;
			cat *= 3;
			cde *= 3;
			//elice.FaceChenge (4);
			bonus *= 3;
		}
		c_hp.text = "HP:"+chp;
		c_at.text = "攻撃:"+cat;
		c_de.text = "防御:"+cde;

		int chu = PlayerPrefs.GetInt ("GameIsChutorialEnd", 0);
		if (chu == 0) {
			ShowChutorial ();
		}
	
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
						Singleton<SoundPlayer>.instance.playSE( "Touch" );
							obj.SendMessage ("TouchCard");
							break;
					default:
							break;
					}
				}
			}
		}
	}		
	/// <summary>
	/// プレイヤーターンにチェンジする
	/// </summary>
	public void Chenge_player_Turn(){
		if(whoisparenet == true){
			PlayerBetter ();
			//nowTurn = gameTurn.PLAYER_TURN;
		}else{
			int bt = enemy.CPUParentBet ();
			switch (bt) {
			case 1:
				if (koukand < 100) {
					cpucomment.text = FindObjectOfType<EliceComent>().l1raise;
					elice.FaceChenge (7);
				} else if (koukand <= 200 && koukand >= 100) {
					cpucomment.text = FindObjectOfType<EliceComent>().l2raise;
					elice.FaceChenge (7);
				} else if (koukand > 200) {
					cpucomment.text = FindObjectOfType<EliceComent>().l3raise;
					elice.FaceChenge (7);
				}
				break;
			case 2:
				if (koukand < 100) {
					cpucomment.text = FindObjectOfType<EliceComent>().l1call;
					elice.FaceChenge (5);
				} else if (koukand <= 200 && koukand >= 100) {
					cpucomment.text = FindObjectOfType<EliceComent>().l2call;
					elice.FaceChenge (5);
				} else if (koukand > 200) {
					cpucomment.text = FindObjectOfType<EliceComent>().l3call;
					elice.FaceChenge (5);
				}
				break;
			case 3:
				if (koukand < 100) {
					cpucomment.text = FindObjectOfType<EliceComent>().l1drop;
					elice.FaceChenge (2);
				} else if (koukand <= 200 && koukand >= 100) {
					cpucomment.text = FindObjectOfType<EliceComent>().l2drop;
					elice.FaceChenge (2);
				} else if (koukand > 200) {
					cpucomment.text = FindObjectOfType<EliceComent>().l3drop;
					elice.FaceChenge (2);
				}
				break;
			}
			Playerthink ();

		}

	}

	void Playerthink(){
		myyosou = 1;
		Invoke ("btnset", 0.5f);
	}

	void PlayerBetter(){
		myyosou = 1;
		EnemyYosou (myyosou);
	}

	void AllBtnEnable(bool btnbool){
		btn1.enabled = btnbool;
		btn2.enabled = btnbool;
		btn3.enabled = btnbool;
	}

	void btnset(){
		AllBtnEnable (true);
		UILabel btntext;
		btntext = btn3.GetComponentInChildren<UILabel> ();
		btntext.text = "交換";
		btntext = btn2.GetComponentInChildren<UILabel> ();
		btntext.text = "自動選択";
		btntext = btn1.GetComponentInChildren<UILabel> ();
		btntext.text = "全て残す";
		EventDelegate.Set(btn3.onClick, CardChenge);
		EventDelegate.Set(btn2.onClick, SelectAuto);
		EventDelegate.Set(btn1.onClick, SelectCancel);
		touchBool = true;
	}

	void EnemyYosou(float playeryosou){
		//敵の予想の後にカードの交換へ
		Invoke ("btnset", 0.5f);
		switch(enemy.thinkBet()){
		case 0:
			//自信なし　等倍
			if (koukand < 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1drop;
				elice.FaceChenge (2);
			} else if (koukand <= 200 && koukand >= 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l2drop;
				elice.FaceChenge (2);
			} else if (koukand > 200) {
				cpucomment.text = FindObjectOfType<EliceComent>().l3drop;
				elice.FaceChenge (2);
			}
			break;
		case 1:
			//自信あり　２倍
			if (koukand < 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1call;
				elice.FaceChenge (5);
			} else if (koukand <= 200 && koukand >= 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l2call;
				elice.FaceChenge (5);
			} else if (koukand > 200) {
				cpucomment.text = FindObjectOfType<EliceComent>().l3call;
				elice.FaceChenge (5);
			}
			break;
		case 2:
			//自信あり　３倍
			if (koukand < 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1raise;
				elice.FaceChenge (7);
			} else if (koukand <= 200 && koukand >= 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1raise;
				elice.FaceChenge (7);
			} else if (koukand > 200) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1raise;
				elice.FaceChenge (7);
			}
			break;
		default:
			Debug.Log ("BetError");
			break;
		}

	}

	void CardChenge(){
		//プレイヤー手札のタッチ済みのカードをチェンジする。
		Singleton<SoundPlayer>.instance.playSE( "bclick" );
		card.SerchIsTouched ("PLAYER");
		touchBool = false;
		AllBtnEnable (false);
	}

	void SelectAuto(){
		Singleton<SoundPlayer>.instance.playSE( "bclick" );
		card.AutoSelectPlayerCard ();
	}

	void SelectCancel(){
		Singleton<SoundPlayer>.instance.playSE( "bclick" );
		card.PlayerAllselect ();
	}


	/// <summary>
	/// スタートボタン
	/// </summary>
	public void startButton(){
		Singleton<SoundPlayer>.instance.playSE( "bclick" );
		if (koukand < 100) {
			cpucomment.text = FindObjectOfType<EliceComent>().l1cardsend;
			elice.FaceChenge (0);
		} else if (koukand <= 200 && koukand >= 100) {
			cpucomment.text = FindObjectOfType<EliceComent>().l2cardsend;
			elice.FaceChenge (0);
		} else if (koukand > 200) {
			cpucomment.text = FindObjectOfType<EliceComent>().l3cardsend;
			elice.FaceChenge (4);
		}
		card.DrawCard(5);
		AllBtnEnable (false);
	}

	/// <summary>
	/// チェンジボタン
	/// </summary>
	public void chengeButton(){
		//プレイヤー手札のタッチ済みのカードをチェンジする。
		Singleton<SoundPlayer>.instance.playSE( "bclick" );
		if(touchBool == true){
			card.SerchIsTouched ("PLAYER");
			touchBool = false;
		}

	}

	/// <summary>
	/// 勝負結果に対してのメダル数の変更処理.
	/// </summary>
	/// <param name="winner">Winner.</param>
	public void WinLose(int pokarHandsScore_p,int enemy_p,int dds){

		if (pokarHandsScore_p < 0) {
			pokarHandsScore_p *= -1;
			if (healDouble == true) {
				int f = pokarHandsScore_p * 2;
				p_heal.text = "+"+f;
				php += f;
			}

			if (noheal == true) {
				p_heal.text = "+0";
			} else {
				p_heal.text = "+"+pokarHandsScore_p;
				php += pokarHandsScore_p;
			}


		} else {
			c_dam.text = "-"+pokarHandsScore_p;
			chp -= pokarHandsScore_p;
		}

		if(enemy_p < 0){
			//回復
			if (koukand < 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1heal;
				elice.FaceChenge (4);
			} else if (koukand <= 200 && koukand >= 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1heal;
				elice.FaceChenge (4);
			} else if (koukand > 200) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1heal;
				elice.FaceChenge (4);
			}

		}else{
			//攻撃
			if (koukand < 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l1Attack;
				elice.FaceChenge (1);
			} else if (koukand <= 200 && koukand >= 100) {
				cpucomment.text = FindObjectOfType<EliceComent>().l2Attack;
				elice.FaceChenge (1);
			} else if (koukand > 200) {
				cpucomment.text = FindObjectOfType<EliceComent>().l3Attack;
				elice.FaceChenge (1);
			}

		}

		if (enemy_p < 0) {
			enemy_p *= -1;
			c_heal.text = "+"+enemy_p;
			chp += enemy_p;
		} else {
			p_dam.text = "-"+enemy_p;
			php -= enemy_p; 
		}

		p_hp.text = "HP:" + php;
		c_hp.text = "HP:" + chp;

		int nowmedal = PlayerPrefs.GetInt ("MEDALS", 0);
		if (coinDouble == true) {
			nowmedal += (pokarHandsScore_p * 2);
		} else {
			nowmedal += pokarHandsScore_p;
		}
		PlayerPrefs.SetInt ("MEDALS",nowmedal);

		if (chp <= 0) {
			nowmedal = PlayerPrefs.GetInt ("MEDALS", 0);
			nowmedal += 1000;
			PlayerPrefs.SetInt ("MEDALS",nowmedal);
			PlayerPrefs.SetInt (charalevel, 1);
		}
		//親の変更
		if(whoisparenet == true){
			whoisparenet = false;
		}else{
			whoisparenet = true;
		}

		//継続するかしないか決める
		UILabel btntext;
		btntext = btn1.GetComponentInChildren<UILabel> ();
		btntext.text = "続ける";
		btn1.enabled = true;
		btntext = btn2.GetComponentInChildren<UILabel> ();
		btntext.text = "立ち去る";
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
		Singleton<SoundPlayer>.instance.playSE( "bclick" );
		//コンティニューボタン
		p_heal.text = "";
		c_heal.text = "";
		c_dam.text = "";
		c_dam.text = "";
		//ここ
		card.DeleteSkillWindow ();



		if (php > 0&& chp > 0) {

			if (nowgamecount == 10000) {

			} else {
				gamecounts++;
				card.InitGame ();
				//nowTurn = gameTurn.START_TURN;
				UILabel btntext;
				btntext = btn1.GetComponentInChildren<UILabel> ();
				btntext.text = "";
				btntext = btn2.GetComponentInChildren<UILabel> ();
				btntext.text = "";
				btn1.enabled = false;
				btn2.enabled = false;
				EventDelegate.Set(btn1.onClick, startButton);
			}
		
		} else {
			if (php <= 0) {
				helpandenemyhandstext.text = "あなたの負けです";
				if (reave == true) {
					php = 1000;
					reave = false;
					card.InitGame ();
					//nowTurn = gameTurn.START_TURN;
					UILabel btntext;
					btntext = btn1.GetComponentInChildren<UILabel> ();
					btntext.text = "スタート";
					btn1.enabled = true;
					EventDelegate.Set(btn1.onClick, startButton);
				}
			} else {
				helpandenemyhandstext.text = "あなたの勝ちです!!\nボーナスで"+bonus+"コイン獲得!!";
				int nowmedal = PlayerPrefs.GetInt ("MEDALS", 0);

				nowmedal += bonus;
				PlayerPrefs.SetInt ("MEDALS",nowmedal);
				PlayerPrefs.SetInt (charalevel, 1);
			}

			btn1.enabled = false;
		}


	}

	/// <summary>
	/// ゲーム終了ボタン.
	/// </summary>
	public void ExitGameButton(){

		Singleton<SoundPlayer>.instance.playSE( "click" );
		Application.LoadLevel ("CharaSelect");
	}

	public string handintforstring(int id){
		switch (id) {
		case 0:
			return "ノーペア";
		case 1:
			return "ワンペア";
		case 2:
			return "スリーカード";
		case 3:
			return "ツーペア";
		case 4:
			return "フラッシュ";
		case 5:
			return "フルハウス";
		case 6:
			return "フォーカード";
		case 7:
			return "ストレイト";
		case 8:
			return "ファイブカード";
		case 9:
			return "ストレイトフラッシュ";
		case 10:
			return "ロイヤルストレートフラッシュ";
		}
		return "あああ";
	}

	public void ShowPokarHandsWindow(){
		Singleton<SoundPlayer>.instance.playSE( "click" );
		if (GameObject.Find ("WinBG") == null) {
			windowobj.SetActive (true);
		} else {
			windowobj.SetActive (false);
		}
	}

	public void ShowChutorial(){
		Singleton<SoundPlayer>.instance.playSE( "click" );
		if (GameObject.Find ("chutorialBG") == null) {
			chutoobj.SetActive (true);
		} else {
			chutoobj.SetActive (false);
			PlayerPrefs.SetInt ("GameIsChutorialEnd", 1);
		}

	}
}
