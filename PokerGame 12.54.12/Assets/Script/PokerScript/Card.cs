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

	List<int> fieldCardNum_Buck;
	List<int> fieldCardMark_Buck;
	List<GameObject> fieldCardObj_Buck;

	//UI用の変数
	[SerializeField] Text tex;

	//カードをインスタンス化する場所
	[SerializeField] Vector3 cardInstantiateVector;

	//カードをめくる音
	public AudioClip audioClip;
	AudioSource audioSource;

	string target = "Test";

	//手札用の一時保存リスト
	List<int> sendCardNum = new List<int> ();
	List<int> sendCardMark = new List<int> ();
	List<GameObject> sendCardObj = new List<GameObject> ();

	TouchMan turn;
	Player playerd;
	Enemy enemyd;
	Judge judge;

	public GameObject CpuParent;
	public GameObject PlayerParent;

	int keyvalue = 0;
	int index = 0;

	void Start () {
		fieldCardObj_Buck = new List<GameObject> (fieldCardObj);
		fieldCardNum_Buck = new List<int> (fieldCardNum);
		fieldCardMark_Buck = new List<int> (fieldCardMark);

		judge = GetComponent<Judge>();
		audioSource = GetComponent<AudioSource> ();
		turn = GetComponent<TouchMan>();
	}

	void Update () {
	}

	public void InitGame(){
		fieldCardNum = new List<int> (fieldCardNum_Buck);
		fieldCardMark = new List<int> (fieldCardMark_Buck);
		fieldCardObj = new List<GameObject> (fieldCardObj_Buck);
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
		Debug.Log ("DRAW!!!!!!");
		StartCoroutine ("DrawCards",drawnum);
	}


	/// <summary>
		/// ドローのコルーチン処理.
		/// </summary>
		/// <returns>The cards.</returns>
		/// <param name="drawnum">Drawnum.</param>
	IEnumerator DrawCards(int drawnum){

		turn.Chenge_Draw_Turn ();

		playerd = FindObjectOfType<Player>();

		float xVec = cardInstantiateVector.x;
		float yVec = cardInstantiateVector.y;
		float zVec = cardInstantiateVector.z;

		sendCardNum.Clear ();
		sendCardMark.Clear ();
		sendCardObj.Clear ();

		for (int h = 0; h < drawnum; h++) {
			//ランダムに取り出す
			int cardnum = Random.Range (0, fieldCardNum.Count);

			//カードの生成
			GameObject card = (GameObject)Instantiate (fieldCardObj [cardnum], new Vector3(xVec + h,yVec,zVec), Quaternion.identity);
			card.name = "Card:" + h;
			card.tag = "PlayerCard";
			card.transform.parent = PlayerParent.transform;
			Animator panim = card.GetComponent<Animator> ();
			panim.SetBool ("CardAnim", true);
			sendCardObj.Add (card);

			//効果音の再生
			//audioSource.PlayOneShot (audioClip);

			//カード情報の取得
			int mark = card.GetComponent<CardInfo> ().Mark;
			int cardn = card.GetComponent<CardInfo>().Number;

			//カードのリストに加える
			sendCardMark.Add (mark);
			sendCardNum.Add (cardn);

			//場のカードから引いたカードを削除
			fieldCardObj.RemoveAt (cardnum);
			fieldCardNum.RemoveAt (cardnum);
			fieldCardMark.RemoveAt (cardnum);

			yield return new WaitForSeconds(1);
		}


		Debug.Log (sendCardNum.Count);
		playerd.drawCard (sendCardNum, sendCardMark, sendCardObj);

		//一時保存用のリストを初期化
		sendCardNum.Clear ();
		sendCardMark.Clear ();
		sendCardObj.Clear ();

		enemyd = FindObjectOfType<Enemy>();

		//CPUのドロー
		for (int h = 0; h < drawnum; h++) {
			//ランダムに取り出す
			int cardnum = Random.Range (0, fieldCardNum.Count);

			//カードの生成
			GameObject card = (GameObject)Instantiate (fieldCardObj [cardnum], new Vector3(xVec + h,yVec + 3.2f,zVec), Quaternion.identity);
			card.name = "Card:" + h;
			card.transform.parent = CpuParent.transform;
			card.tag = "CpuCard";
			Animator panim = card.GetComponent<Animator> ();
			//panim.SetBool ("CardAnim", true);
			sendCardObj.Add (card);

			//効果音の再生
			//audioSource.PlayOneShot (audioClip);

			//カード情報の取得
			int mark = card.GetComponent<CardInfo> ().Mark;
			int cardn = card.GetComponent<CardInfo>().Number;

			//カードのリストに加える
			sendCardMark.Add (mark);
			sendCardNum.Add (cardn);

			//場のカードから引いたカードを削除
			fieldCardObj.RemoveAt (cardnum);
			fieldCardNum.RemoveAt (cardnum);
			fieldCardMark.RemoveAt (cardnum);

			yield return new WaitForSeconds(0.5f);
		}

		enemyd.drawCardEnemy (sendCardNum, sendCardMark, sendCardObj);

		if(judge.IsPair (sendCardNum) == true){
			Debug.Log ("pair");
		}else{
			Debug.Log ("nopair");
		}

		turn.Chenge_player_Turn ();
		yield break;

	}

	public void SerchIsTouched(string target){
		if(target == "PLAYER"){
			StartCoroutine ("ChengeCard", true);
		}else{
			StartCoroutine ("ChengeCard", false);
		}
	}

	IEnumerator ChengeCard(bool player){
		int listn = 0;
		int looping;
		if(player == true){
			int count = playerd.handCardList.Count;
			looping = 0;

			for(int i = 0;i<count;i++){
				Vector3 cardVector = playerd.handCardList [i - looping].transform.position;
				bool IsTouched = playerd.handCardList [i - looping].GetComponent<CardInfo> ().touched;

				//Destroy (playerd.handCardList [i]);

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

					Destroy (playerd.handCardList [listn - looping]);

					playerd.handCardList.RemoveAt (listn - looping);
					playerd.handCardMark.RemoveAt (listn - looping);
					playerd.handCardNum.RemoveAt (listn - looping);

					int cardnum = Random.Range (0, fieldCardObj.Count);
					playerd.handCardList.Add (fieldCardObj [cardnum]);

					GameObject card = (GameObject)Instantiate (fieldCardObj[cardnum], cardVector, Quaternion.identity);
					card.name = ""+listn;
					card.tag="PlayerCard";
					card.transform.parent = PlayerParent.transform;
					Animator panim = card.GetComponent<Animator> ();
					panim.SetBool ("CardAnim", true);
					int mark = card.GetComponent<CardInfo> ().Mark;
					int cardn = card.GetComponent<CardInfo> ().Number;


					playerd.handCardMark.Add (mark);
					playerd.handCardNum.Add (cardn);

					fieldCardObj.RemoveAt (cardnum);
					looping++;
					yield return new WaitForSeconds(1);
				}

			}

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

				foreach(int i in enemyd.EnemyCardNum){
					List<int> cardList = enemyd.EnemyCardNum.Select (c => c).Where (s => s == i || s == 0).ToList ();
					//Debug.Log ("count:" + cardList.Count);
					if (cardList.Count == 3) {
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
			case 2:
				//ワンペア残し
				keyvalue = 0;
				index = 0;

				foreach(int i in enemyd.EnemyCardNum){
					List<int> cardList = enemyd.EnemyCardNum.Select (c => c).Where (s => s == i || s == 0).ToList ();
					//Debug.Log ("count:" + cardList.Count);
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


	public void ChengeAndOpenCPUcards(){
		int counts = enemyd.EnemyCardObject.Count;
		int loopings = 0;
		int listindex = 0;

		for(int i = 0;i<counts;i++){
			Vector3 cardVector = enemyd.EnemyCardObject [i - loopings].transform.position;
			bool IsTouched = enemyd.EnemyCardObject [i - loopings].GetComponent<CardInfo> ().touched;

			//Destroy (playerd.handCardList [i]);

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

				Destroy (enemyd.EnemyCardObject [listindex - loopings]);

				enemyd.EnemyCardObject.RemoveAt (listindex - loopings);
				enemyd.EnemyCardNum.RemoveAt (listindex - loopings);
				enemyd.EnemyCardMark.RemoveAt (listindex - loopings);

				int cardnum = Random.Range (0, fieldCardObj.Count);
				enemyd.EnemyCardObject.Add (fieldCardObj [cardnum]);

				GameObject card = (GameObject)Instantiate (fieldCardObj [cardnum], cardVector, Quaternion.identity);
				card.name = "" + listindex;
				card.tag = "CpuCard";
				card.transform.parent = CpuParent.transform;
				Animator panim = card.GetComponent<Animator> ();
				panim.SetBool ("CardAnim", true);
				int mark = card.GetComponent<CardInfo> ().Mark;
				int cardn = card.GetComponent<CardInfo> ().Number;


				enemyd.EnemyCardMark.Add (mark);
				enemyd.EnemyCardNum.Add (cardn);

				fieldCardObj.RemoveAt (cardnum);
				loopings++;
			} else {
				Animator panim = enemyd.EnemyCardObject[i-loopings].GetComponent<Animator> ();
				panim.SetBool ("CpuCardTouch", true);
				panim.SetBool ("CardAnim", true);
			}

		}
		turn.Chenge_Judge_Turn ();
		JudgePokarHand ();

	}

	public void JudgePokarHand(){
		//プレイヤーの約判定
		int playerStrong = judge.PokarHandsInt (playerd.handCardNum, playerd.handCardMark);
		int cpuStrong = judge.PokarHandsInt (enemyd.EnemyCardNum, enemyd.EnemyCardMark);

		if(playerStrong>cpuStrong){
			turn.WinLose ("PLAYER");
		}
		if(playerStrong==cpuStrong){
			turn.WinLose ("DRAW");
		}
		if(playerStrong<cpuStrong){
			turn.WinLose ("ENEMY");
		}

	}

}
