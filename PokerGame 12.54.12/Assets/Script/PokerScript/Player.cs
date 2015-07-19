using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// プレーヤーのクラス.
/// </summary>
public class Player : MonoBehaviour {

	public Card cards;
	public Judge judgep;
	//ゲームの親か
	public bool IsParent = true;

	//リストの宣言
	public List<GameObject> handCardList = new List<GameObject>();
	public List<int> handCardNum = new List<int>();
	public List<int> handCardMark = new List<int>();
	public List<int> handCardScore = new List<int>();
	public List<int> handcardColor = new List<int> ();
	//public List<int> equipAccesary = new List<int> ();

	public int attackPower;
	public int hp;
	public int defence;
	public int chengecount;

	void Awake(){
			cards = FindObjectOfType<Card>();
		attackPower = PlayerPrefs.GetInt ("P_Attack", 3);
		hp = PlayerPrefs.GetInt ("P_health", 220);
		defence = PlayerPrefs.GetInt ("P_Defence", 1);
	}

	void Start(){
		judgep = GetComponent<Judge> ();

	}

	/// <summary>
	/// リストの初期化.
	/// </summary>
	public void InitAllListPlayer(){
		handCardList.Clear ();
		handCardNum.Clear ();
		handCardMark.Clear ();
		handCardScore.Clear ();
		handcardColor.Clear ();
	}


	/// <summary>
	/// プレイヤーを親にする.
	/// </summary>
	public void playerIsParent(){
		IsParent = true;
	}

	/// <summary>
	/// プレイヤーを親から外す.
	/// </summary>
	public void playerNoParent(){
		IsParent = false;
	}

	/// <summary>
	/// ドローの処理、リストをもらってくる.
	/// </summary>
	/// <param name="numList">Number list.</param>
	/// <param name="markList">Mark list.</param>
	/// <param name="cardObj">Card object.</param>
	public void drawCard(List<int> numList,List<int> markList,List<GameObject> cardObj,List<int> scoreList,List<int> color){
			for(int i = 0;i<numList.Count;i++){
					handCardNum.Add (numList [i]);
					handCardMark.Add (markList [i]);
					handCardList.Add (cardObj [i]);
			handCardScore.Add (scoreList [i]);
			handcardColor.Add (color [i]);
			}

		//Debug.Log (handCardNum.Count);
	}

	public void removeListp(int index){
		handCardList.RemoveAt (index);
	}

	public void addListp(GameObject obj){
		handCardList.Add(obj);
	}

	public int ThinkAIP(){
		//場に残っているカードの取得
		List<int> FieldCardsNum = new List<int> ();
		List<int> FieldCardsMark = new List<int> ();

		foreach(GameObject obj in handCardList){
			int mark = obj.GetComponent<CardInfo> ().Mark;
			int cardn = obj.GetComponent<CardInfo> ().Number;
			FieldCardsMark.Add (mark);
			FieldCardsNum.Add (cardn);
		}

		if(judgep.IsStraight(handCardNum) == true){
			if(judgep.IsFlush(handCardMark)){
				return 0;
			}
		}

		//ファイブカード
		if(judgep.IsFiveCard(handCardNum) == true){
			return 0;
		}

		//フォーカード
		if(judgep.IsFourCard(handCardNum) == true){
			return 0;
		}

		//フルハウス
		if(judgep.IsThreeCard(handCardNum) == true){
			if(judgep.IsFullHouse(handCardNum) == true){
				return 0;
			}
		}

		//フラッシュ
		if(judgep.IsFlush(handCardNum) == true){
			return 0;
		}

		//ストレイト
		if(judgep.IsStraight(handCardNum) == true){
			return 0;
		}

		if(judgep.IsThreeCard(handCardNum) == true){
			//Debug.Log ("スリーカード");
			return 1;
			//3枚残し
		}

		//ツーペア&ワンペア
		if(judgep.IsPair(handCardNum)== true){
			if(judgep.IsTwoPair(handCardNum) == true){
				//Debug.Log ("ツーペア");
				return 3;
				//1枚残し
			}
			return 2;
			//2枚残し
		}

		//フラッシュ可能か(手札に３枚以上あるか?)

		//4 Spade 5 Heart 6 Dia 7 Clover 残し
		List<int> CPUSpadeList = handCardMark.Select (c => c).Where (s => s == 3).ToList ();
		List<int> CPUHeartList = handCardMark.Select (c => c).Where (s => s == 4).ToList ();
		List<int> CPUDiaList = handCardMark.Select (c => c).Where (s => s == 2).ToList ();
		List<int> CPUCloverList = handCardMark.Select (c => c).Where (s => s == 1).ToList ();

		List<int> FieldSpadeList = FieldCardsMark.Select (c => c).Where (s => s == 3).ToList ();
		List<int> FieldHeartList = FieldCardsMark.Select (c => c).Where (s => s == 4).ToList ();
		List<int> FieldDiaList = FieldCardsMark.Select (c => c).Where (s => s == 2).ToList ();
		List<int> FieldCloverList = FieldCardsMark.Select (c => c).Where (s => s == 1).ToList ();

		int SpadeCounts = CPUSpadeList.Count + FieldSpadeList.Count;
		int HeartCounts = CPUHeartList.Count + FieldHeartList.Count;
		int DiaCounts = CPUDiaList.Count + FieldDiaList.Count;
		int CloverCounts = CPUCloverList.Count + FieldCloverList.Count;

		if(CPUSpadeList.Count>2){
			return 4;
		}

		if(CPUHeartList.Count>2){
			return 5;
		}

		if(CPUDiaList.Count>2){
			return 6;
		}

		if(CPUCloverList.Count>2){
			return 7;
		}

		//ジョーカー残し
		if(judgep.CountsJoker(handCardNum) > 0){
			return 8;
		}

		if(SpadeCounts>11){
			return 4;
		}

		if(HeartCounts>11){
			return 5;
		}

		if(DiaCounts>11){
			return 6;
		}

		if(CloverCounts>11){
			return 7;
		}

		int white = 0;
		int black = 0;
		for (int c = 0; c < handcardColor.Count; c++) {
			if (handcardColor [c] == 1) {
				black++;
			} else {
				white++;
			}
		}

		if (black > white) {
			return 11;
		} else {
			return 10;
		}
	}

}
