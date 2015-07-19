using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// カードの基本情報.
/// </summary>
public class Card : MonoBehaviour {

	//カードの情報
	[SerializeField] List<int> fieldCardNum = new List<int> ();
	[SerializeField] List<int> fieldCardMark = new List<int> ();
	[SerializeField] List<GameObject> fieldCardObj = new List<GameObject> ();

	//カードのバックアップ
	List<int> fieldCardNum_Buck;
	List<int> fieldCardMark_Buck;
	List<GameObject> fieldCardObj_Buck;

	//UI用の変数
	[SerializeField] Text tex;

	//カードをインスタンス化する場所
	[SerializeField] Vector3 cardInstantiateVector;

	//カードをめくる音
	public AudioClip audioClip;

	//手札用の一時保存リスト
	List<int> sendCardNum = new List<int> ();
	List<int> sendCardMark = new List<int> ();
	List<GameObject> sendCardObj = new List<GameObject> ();
	List<int> sendCardScore = new List<int> ();
	List<int> sendCardColor = new List<int> ();

	//スクリプト
	TouchMan turn;
	Player playerd;
	Enemy enemyd;
	Judge judge;

	//カードの親
	public GameObject CpuParent;
	public GameObject PlayerParent;

	public UILabel p_score;
	public UILabel p_color1;
	public UILabel p_color2;
	public UILabel p_color3;
	public UILabel p_hand;

	public UILabel c_score;
	public UILabel c_color1;
	public UILabel c_color2;
	public UILabel c_color3;
	public UILabel c_hand;

	public UILabel winlose;

	public UISprite p_skillbg;
	public UISprite c_skillbg;

	public UILabel wincoin;
	public UILabel losecoin;

	bool bairituDouble;

	//CPUチェンジの処理に使う
	int keyvalue = 0;
	int index = 0;

	void Start () {
		bairituDouble = false;
		int specialitemw = PlayerPrefs.GetInt ("Special", 0);
		if (specialitemw == 7) {
			bairituDouble = true;
			PlayerPrefs.SetInt ("Special", 0);
		}
		//Listを全てコピー
		fieldCardObj_Buck = new List<GameObject> (fieldCardObj);
		fieldCardNum_Buck = new List<int> (fieldCardNum);
		fieldCardMark_Buck = new List<int> (fieldCardMark);


		//ゲットコンポーネントする
		judge = GetComponent<Judge>();
		turn = GetComponent<TouchMan>();
	}

	/// <summary>
	/// ゲームの初期化.
	/// </summary>
	public void InitGame(){
		fieldCardNum = new List<int> (fieldCardNum_Buck);
		fieldCardMark = new List<int> (fieldCardMark_Buck);
		fieldCardObj = new List<GameObject> (fieldCardObj_Buck);

		//表示オブジェクトの削除
		enemyd.InitAllListCPU ();
		playerd.InitAllListPlayer ();
		CpuParent.SendMessage ("removeAllciledrenCard");
		PlayerParent.SendMessage ("removeAllciledrenCard");
	}


	/// <summary>
	/// カードのドロー処理
	/// </summary>
	/// <param name="drawnum">Drawnum.</param>
	public void DrawCard(int drawnum){
		StartCoroutine ("DrawCards",drawnum);
	}


	/// <summary>
		/// ドローのコルーチン処理.
		/// </summary>
		/// <returns>The cards.</returns>
		/// <param name="drawnum">Drawnum.</param>
	IEnumerator DrawCards(int drawnum){

		playerd = FindObjectOfType<Player>();

		float xVec = cardInstantiateVector.x;
		float yVec = cardInstantiateVector.y;
		float zVec = cardInstantiateVector.z;

		//初期化
		sendCardNum.Clear ();
		sendCardMark.Clear ();
		sendCardObj.Clear ();
		sendCardColor.Clear ();

		for (int h = 0; h < drawnum; h++) {
			//ランダムに取り出す
			int cardnum = Random.Range (0, fieldCardNum.Count);

			//カードの生成
			GameObject card = (GameObject)Instantiate (fieldCardObj [cardnum], new Vector3(xVec + h,yVec,zVec), Quaternion.identity);
			card.name = "Card:" + h;
			card.tag = "PlayerCard";
			card.transform.parent = PlayerParent.transform;

			//黒１
			int color = card.GetComponent<CardInfo> ().black;

			int score = 0;

			Animator panim = card.GetComponent<Animator> ();
			panim.SetBool ("CardAnim", true);
			sendCardObj.Add (card);

			//効果音の再生
			Singleton<SoundPlayer>.instance.playSE( "draw" );

			//カード情報の取得
			int mark = card.GetComponent<CardInfo> ().Mark;
			int cardn = card.GetComponent<CardInfo>().Number;

			//A->14変換
			if(cardn==1){
				score += 14;
			}else{
				score += cardn;
			}

			//カードのリストに加える
			sendCardMark.Add (mark);
			sendCardNum.Add (cardn);
			sendCardColor.Add (color);
			sendCardScore.Add (score);

			//場のカードから引いたカードを削除
			fieldCardObj.RemoveAt (cardnum);
			fieldCardNum.RemoveAt (cardnum);
			fieldCardMark.RemoveAt (cardnum);

			yield return new WaitForSeconds(0.4f);
		}
			
		//リストを渡す
		playerd.drawCard (sendCardNum, sendCardMark, sendCardObj,sendCardScore,sendCardColor);

		//一時保存用のリストを初期化
		sendCardNum.Clear ();
		sendCardMark.Clear ();
		sendCardObj.Clear ();
		sendCardScore.Clear();
		sendCardColor.Clear ();

		enemyd = FindObjectOfType<Enemy>();

		//CPUのドロー
		for (int h = 0; h < drawnum; h++) {
			//ランダムに取り出す
			int cardnum = Random.Range (0, fieldCardNum.Count);

			//カードの生成
			GameObject card = (GameObject)Instantiate (fieldCardObj [cardnum], new Vector3(xVec + h,yVec + 4.8f,zVec), Quaternion.identity);
			card.name = "Card:" + h;
			card.transform.parent = CpuParent.transform;
			card.tag = "CpuCard";
			sendCardObj.Add (card);

			int color = card.GetComponent<CardInfo> ().black;

			//キラカードの選定
			int score = 0;

			//効果音の再生
			Singleton<SoundPlayer>.instance.playSE( "draw" );

			//カード情報の取得
			int mark = card.GetComponent<CardInfo> ().Mark;
			int cardn = card.GetComponent<CardInfo>().Number;

			if(cardn==1){
				score += 14;
			}else{
				score += cardn;
			}

			//カードのリストに加える
			sendCardMark.Add (mark);
			sendCardNum.Add (cardn);
			sendCardColor.Add (color);
			sendCardScore.Add(score);

			//場のカードから引いたカードを削除
			fieldCardObj.RemoveAt (cardnum);
			fieldCardNum.RemoveAt (cardnum);
			fieldCardMark.RemoveAt (cardnum);

			yield return new WaitForSeconds(0.2f);
		}

		//リストを渡す
		enemyd.drawCardEnemy (sendCardNum, sendCardMark, sendCardObj,sendCardScore,sendCardColor);


		//ステートをチェンジ
		turn.Chenge_player_Turn ();
		yield break;

	}

	/// <summary>
	/// タッチされたカードを探す.
	/// </summary>
	/// <param name="target">Target.</param>
	public void SerchIsTouched(string target){
		if(target == "PLAYER"){
			StartCoroutine ("ChengeCard", true);
		}else{
			StartCoroutine ("ChengeCard", false);
		}
	}

	/// <summary>
	/// プレイヤー&CPUのカードの変更コルーチン
	/// </summary>
	/// <returns>The card.</returns>
	/// <param name="player">If set to <c>true</c> player.</param>
	IEnumerator ChengeCard(bool player){
		int listn = 0;
		int looping;
		if(player == true){
			int count = playerd.handCardList.Count;
			looping = 0;

			for(int i = 0;i<count;i++){
				Vector3 cardVector = playerd.handCardList [i - looping].transform.position;
				bool IsTouched = playerd.handCardList [i - looping].GetComponent<CardInfo> ().touched;

				if(IsTouched==false){

					if(cardVector.x==-2.0){
						listn = 0;
					}else if(cardVector.x==-1.0){
						listn = 1;
					}else if(cardVector.x==0){
						listn = 2;
					}else if(cardVector.x==1.0){
						listn = 3;
					}else if(cardVector.x==2.0){
						listn = 4;
					}

					//削除
					Destroy (playerd.handCardList [listn - looping]);

					playerd.removeListp (listn - looping);

					//生成
					int cardnum = Random.Range (0, fieldCardObj.Count);

					GameObject card = (GameObject)Instantiate (fieldCardObj[cardnum], cardVector, Quaternion.identity);
					card.name = ""+listn;
					card.tag="PlayerCard";
					card.transform.parent = PlayerParent.transform;

					//キラカードの選定
					int score = 0;

					Animator panim = card.GetComponent<Animator> ();
					panim.SetBool ("CardAnim", true);
					Singleton<SoundPlayer>.instance.playSE( "draw" );
					int mark = card.GetComponent<CardInfo> ().Mark;
					int cardn = card.GetComponent<CardInfo> ().Number;

					if(cardn==1){
						score += 14;
					}else{
						score += cardn;
					}

					int colorcc = card.GetComponent<CardInfo> ().black;

					playerd.handCardMark.RemoveAt(listn - looping);
					playerd.handCardNum.RemoveAt(listn - looping);
					playerd.handCardScore.RemoveAt(listn - looping);
					playerd.handcardColor.RemoveAt(listn - looping);

					playerd.addListp (card);

					playerd.handCardMark.Add (mark);
					playerd.handCardNum.Add (cardn);
					playerd.handCardScore.Add (score);
					playerd.handcardColor.Add(colorcc);

					fieldCardObj.RemoveAt (cardnum);
					looping++;

				}
			}
			yield return new WaitForSeconds(0.5f);
			//ここから敵のチェンジの処理

			//変数の初期化
			listn = 0;
			looping = 0;

			enemyd = FindObjectOfType<Enemy>();

			count = enemyd.EnemyCardNum.Count;

			//タッチさせるためにスイッチで処理を分ける
			switch(enemyd.ThinkAI()){
			case 0:
				//チェンジしない
				foreach (GameObject obj in enemyd.EnemyCardObject) {
					obj.SendMessage ("TouchCardCpu");
				}
				ChengeAndOpenCPUcards ();
				break;
			case 1:
				//スリーカード残し
				keyvalue = 0;
				index = 0;
				int ddd = 0; 

				foreach(int i in enemyd.EnemyCardNum){
					List<int> cardList = enemyd.EnemyCardNum.Select (c => c).Where (s => s == i || s == 0).ToList ();
					if (cardList.Count == 3) {
						keyvalue = ddd;
					}
				}

				foreach(int i in enemyd.EnemyCardNum){
					if(i == keyvalue){
						enemyd.EnemyCardObject [i].SendMessage ("TouchCardCpu");
					}
					ddd++;
				}
				ChengeAndOpenCPUcards ();
				break;
			case 2:
				//ワンペア残し
				keyvalue = 0;
				index = 0;

				foreach(int i in enemyd.EnemyCardNum){
					List<int> cardList = enemyd.EnemyCardNum.Select (c => c).Where (s => s == i || s == 0).ToList ();
					if (cardList.Count == 2) {
						keyvalue = i;
					}
				}

				foreach(int i in enemyd.EnemyCardNum){
					if(i == keyvalue){
						enemyd.EnemyCardObject [index].SendMessage ("TouchCardCpu");
					}
					index++;
				}
				ChengeAndOpenCPUcards ();
				break;
			case 3:
				//ツーペア残し
				keyvalue = 0;
				index = 0;
				int keyvaluetwo = 0;

				foreach(int i in enemyd.EnemyCardNum){
					List<int> cardList = enemyd.EnemyCardNum.Select (c => c).Where (s => s == i || s == 0).ToList ();
					if (cardList.Count == 2) {
						keyvalue = i;
					}
				}

				foreach(int i in enemyd.EnemyCardNum){
					List<int> cardList = enemyd.EnemyCardNum.Select (c => c).Where (s => s == i && s != keyvalue).ToList ();
					if (cardList.Count == 2) {
						keyvaluetwo = i;
					}
				}

				foreach(int i in enemyd.EnemyCardNum){
					if(i == keyvalue || i == keyvaluetwo){
						enemyd.EnemyCardObject [index].SendMessage ("TouchCardCpu");
					}
					index++;
				}
				ChengeAndOpenCPUcards ();
				break;
			case 4:
				//スペードのフラッシュ狙い3
				index = 0;

				foreach(int i in enemyd.EnemyCardMark){
					if(i == 3){
						enemyd.EnemyCardObject [index].SendMessage ("TouchCardCpu");
					}
					index++;
				}
				ChengeAndOpenCPUcards ();
				break;
			case 5:
				//ハートのフラッシュ狙い4
				index = 0;

				foreach(int i in enemyd.EnemyCardMark){
					if(i == 4){
						enemyd.EnemyCardObject [index].SendMessage ("TouchCardCpu");
					}
					index++;
				}
				ChengeAndOpenCPUcards ();
				break;
			case 6:
				//ダイアのフラッシュ狙い2
				index = 0;

				foreach(int i in enemyd.EnemyCardMark){
					if(i == 2){
						enemyd.EnemyCardObject [index].SendMessage ("TouchCardCpu");
					}
					index++;
				}
				ChengeAndOpenCPUcards ();
				break;
			case 7:
				//クローバーのフラッシュ狙い1
				index = 0;

				foreach(int i in enemyd.EnemyCardMark){
					if(i == 1){
						enemyd.EnemyCardObject [index].SendMessage ("TouchCardCpu");
					}
					index++;
				}
				ChengeAndOpenCPUcards ();
				break;
			case 8:
				//ジョーカー残し
				index = 0;

				foreach(int i in enemyd.EnemyCardMark){
					if(i == 0){
						enemyd.EnemyCardObject [index].SendMessage ("TouchCardCpu");
					}
					index++;
				}
				ChengeAndOpenCPUcards ();
				break;
			case 9:
				//全チェンジ
				ChengeAndOpenCPUcards ();
				break;
			}
		}
		yield break;
	}

	/// <summary>
	/// CPUカードのチェンジを行い、
	/// オープンする.
	/// </summary>
	public void ChengeAndOpenCPUcards(){
		int counts = enemyd.EnemyCardObject.Count;
		int loopings = 0;
		int listindex = 0;
		for(int i = 0;i<counts;i++){
			Vector3 cardVector = enemyd.EnemyCardObject [i - loopings].transform.position;
			bool IsTouched = enemyd.EnemyCardObject [i - loopings].GetComponent<CardInfo> ().touched;

			if (IsTouched == false) {

				if (cardVector.x == -2.0) {
					listindex = 0;
				} else if (cardVector.x == -1.0) {
					listindex = 1;
				} else if (cardVector.x == 0) {
					listindex = 2;
				} else if (cardVector.x == 1.0) {
					listindex = 3;
				} else if (cardVector.x == 2.0) {
					listindex = 4;
				}

				Destroy (enemyd.EnemyCardObject [listindex- loopings]);

				int cardnum = Random.Range (0, fieldCardObj.Count);

				enemyd.removeListc (listindex - loopings);

				GameObject card = (GameObject)Instantiate (fieldCardObj [cardnum], cardVector, Quaternion.identity);
				card.name = "" + listindex;
				card.tag = "CpuCard";
				card.transform.parent = CpuParent.transform;

				int c = card.GetComponent<CardInfo> ().black;

				//キラカードの選定
				int score = 0;

				Animator panim = card.GetComponent<Animator> ();
				panim.SetBool ("CardAnim", true);
				int mark = card.GetComponent<CardInfo> ().Mark;
				int cardn = card.GetComponent<CardInfo> ().Number;
				if(cardn==1){
					score += 14;
				}else{
					score += cardn;
				}

				enemyd.EnemyCardNum.RemoveAt (listindex - loopings);
				enemyd.EnemyCardMark.RemoveAt (listindex - loopings);
				enemyd.EnemyCardScore.RemoveAt (listindex - loopings);
				enemyd.EnemyCardColor.RemoveAt (listindex - loopings);
				//enemyd.EnemyCardObject[i] = card;

				enemyd.EnemyCardMark.Add (mark);
				enemyd.EnemyCardNum.Add (cardn);
				enemyd.EnemyCardScore.Add (score);
				enemyd.EnemyCardColor.Add (c);
				enemyd.addListc (card);
				fieldCardObj.RemoveAt (cardnum);
				loopings++;
				Singleton<SoundPlayer>.instance.playSE( "draw" );
			} else {
				Animator panim = enemyd.EnemyCardObject[i-loopings].GetComponent<Animator> ();
				panim.SetBool ("CpuCardTouch", true);
				panim.SetBool ("CardAnim", true);
			}
		}
		JudgePokarHand ();

	}

	/// <summary>
	/// プレイヤーの役とCPUの役のジャッジを行う.
	/// </summary>
	public void JudgePokarHand(){
		int playerStrong = judge.PokarHandsInt (playerd.handCardNum, playerd.handCardMark);
		int cpuStrong = judge.PokarHandsInt (enemyd.EnemyCardNum, enemyd.EnemyCardMark);
		int pcolor = judge.IsColorHand (playerd.handcardColor);
		int ccolor = judge.IsColorHand (enemyd.EnemyCardColor);

		string phand = turn.handintforstring (playerStrong);
		p_hand.text = phand;

		string chand = turn.handintforstring (cpuStrong);
		c_hand.text = chand;

		p_skillbg.gameObject.SetActive (true);
		c_skillbg.gameObject.SetActive (true);

		int p_handcardscore = 0;
		for (int p = 0; p < playerd.handCardScore.Count; p++) {
			p_handcardscore += playerd.handCardScore [p];
		}

		int c_handcardscore = 0;
		for (int p = 0; p < enemyd.EnemyCardScore.Count; p++) {
			c_handcardscore += enemyd.EnemyCardScore [p];
		}

		int p_power = judge.PokarHandsScore (playerStrong);
		if (bairituDouble == true) {
			p_power *= 2;
		}
		int c_power = judge.PokarHandsScore (cpuStrong);

		if (pcolor < 4) {
			p_score.text = "回復行動"+p_power+"倍の威力!";
		} else {
			p_score.text = "攻撃行動"+p_power+"倍の威力!";
		}

		if (ccolor < 4) {
			c_score.text = "回復行動"+c_power+"倍の威力!";
		} else {
			c_score.text = "攻撃行動"+c_power+"倍の威力!";
		}

		float f = (turn.pat * p_power) * turn.myyosou;
		float l = (turn.cat * c_power) * turn.myyosou;
		int d = (int)Mathf.Round (f);
		int e = (int)Mathf.Round (l);
		if (pcolor < 4) {
			d *= -1;
		} else {
			d -= turn.cde;
			if (d < 0) {
				d = 0;
			}
		}

		if (ccolor < 4) {
			e *= -1;
		} else {
			e -= turn.pde;
			if (e < 0) {
				e = 0;
			}
		}
			
		int s = Random.Range (0, 2);
		turn.WinLose (d,e,s);
	}

	public void PlayerAllselect(){
		playerd = FindObjectOfType<Player>();
		foreach (GameObject obj in playerd.handCardList) {
			if (obj.GetComponent<CardInfo> ().touched == false) {
				obj.SendMessage ("TouchCard");
			}
		}
	}

	public void AutoSelectPlayerCard(){
		//変数の初期化
		//int listnd = 0;
		//int loopingss= 0;
		playerd = FindObjectOfType<Player>();
		//タッチさせるためにスイッチで処理を分ける
		switch(playerd.ThinkAIP()){

		case 0:
			//チェンジしない
			foreach (GameObject obj in playerd.handCardList) {
				if (obj.GetComponent<CardInfo> ().touched == false) {
					obj.SendMessage ("TouchCard");
				}
			}
			break;
		case 1:
			//スリーカード残し
			keyvalue = 0;
			index = 0;
			foreach(int i in playerd.handCardNum){
				List<int> cardList = playerd.handCardNum.Select (c => c).Where (s => s == i || s == 0).ToList ();
				if (cardList.Count == 3) {
					keyvalue = i;
				}
			}

			foreach(int i in playerd.handCardNum){
				if (i == keyvalue || i == 0) {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == false) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				} else {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == true) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				}
				index++;
			}
			break;
		case 2:
			//ワンペア残し
			keyvalue = 0;
			index = 0;
			foreach(int i in playerd.handCardNum){
				List<int> cardList = playerd.handCardNum.Select (c => c).Where (s => s == i || s == 0).ToList ();
				if (cardList.Count == 2) {
					keyvalue = i;
				}
			}

			foreach(int i in playerd.handCardNum){
				if(i == keyvalue || i == 0){
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == false) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					} 
				}else {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == true) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				}
				index++;
			}
			break;
		case 3:
			//ツーペア残し
			keyvalue = 0;
			index = 0;
			int keyvaluetwo = 0;
			foreach(int i in playerd.handCardNum){
				List<int> cardList = playerd.handCardNum.Select (c => c).Where (s => s == i || s == 0).ToList ();
				if (cardList.Count == 2) {
					keyvalue = i;
				}
			}

			foreach(int i in playerd.handCardNum){
				List<int> cardList = playerd.handCardNum.Select (c => c).Where (s => s == i && s != keyvalue).ToList ();
				if (cardList.Count == 2) {
					keyvaluetwo = i;
				}
			}

			foreach(int i in playerd.handCardNum){
				if(i == keyvalue || i == keyvaluetwo || i == 0){
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == false) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				} else {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == true) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				}
				index++;
			}
			break;
		case 4:
			//スペードのフラッシュ狙い3
			index = 0;
			foreach(int i in playerd.handCardMark){
				if(i == 3 || i == 0){
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == false) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				} else {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == true) {
						playerd.handCardList [index].SendMessage ("TouchCard");

					}
				}
				index++;
			}
			break;
		case 5:
			//ハートのフラッシュ狙い4
			index = 0;
			foreach(int i in playerd.handCardMark){
				if(i == 4 || i == 0){
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == false) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				} else {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == true) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				}
				index++;
			}
			break;
		case 6:
			//ダイアのフラッシュ狙い2
			index = 0;
			foreach(int i in playerd.handCardMark){
				if(i == 2 || i == 0){
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == false) {
						playerd.handCardList [index].SendMessage ("TouchCard");

					}
				} else {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == true) {
						playerd.handCardList [index].SendMessage ("TouchCard");

					}
				}
				index++;
			}
			break;
		case 7:
			//クローバーのフラッシュ狙い1
			index = 0;
			foreach(int i in playerd.handCardMark){
				if(i == 1 || i == 0){
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == false) {
						playerd.handCardList [index].SendMessage ("TouchCard");

					}
				} else {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == true) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				}
				index++;
			}
			break;
		case 8:
			//ジョーカー残し
			index = 0;

			foreach(int i in playerd.handCardMark){
				if(i == 0){
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == false) {
						playerd.handCardList [index].SendMessage ("TouchCard");
					}
				} else {
					if (playerd.handCardList [index].GetComponent<CardInfo> ().touched == true) {
						playerd.handCardList [index].SendMessage ("TouchCard");

					}
				}
				index++;
			}
			break;
		case 9:
			//キラカードだけ残す
			foreach (GameObject obj in playerd.handCardList) {
				if (obj.GetComponent<CardInfo> ().isKira == true) {
					if (obj.GetComponent<CardInfo> ().touched == false) {
						obj.SendMessage ("TouchCard");
					}

				} else {
					if (obj.GetComponent<CardInfo> ().touched == true) {
						obj.SendMessage ("TouchCard");
					}
				}
			}
			break;
		case 10:
			//ホワイト残し
			//Debug.Log ("10");
			foreach (GameObject obj in playerd.handCardList) {
				if (obj.GetComponent<CardInfo> ().black == 0) {
					if (obj.GetComponent<CardInfo> ().touched == false) {
						obj.SendMessage ("TouchCard");
					}

				} else {
					if (obj.GetComponent<CardInfo> ().touched == true) {
						obj.SendMessage ("TouchCard");
					}
				}
			}
			break;
		case 11:
			//ブラック残し
			//Debug.Log ("11");
			foreach (GameObject obj in playerd.handCardList) {
				if (obj.GetComponent<CardInfo> ().black == 1) {
					if (obj.GetComponent<CardInfo> ().touched == false) {
						obj.SendMessage ("TouchCard");
					}

				} else {
					if (obj.GetComponent<CardInfo> ().touched == true) {
						obj.SendMessage ("TouchCard");
					}
				}
			}
			break;
		}
	}

	public void DeleteSkillWindow(){
		p_skillbg.gameObject.SetActive (false);
		c_skillbg.gameObject.SetActive (false);
		winlose.text = "";
		//p_color3.text = "";
		//c_color3.text = "";
		//p_color2.text = "";
		//c_color2.text = "";
		wincoin.text = "";
		losecoin.text = "";
	}


}
