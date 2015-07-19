using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// 役の判定などを行うクラス.
/// </summary>
public class Judge : MonoBehaviour {

	//ジョーカーのカウント
	public int jokerCount;

	//役の強さ
	public int handStrong;

	//リストの選定
	private List<int> CNumList;
	private List<int> CMarkList;
	//ストレイト用に値を調整したリスト
	private List<int> DamyList;

	private int PairNum;

	/// <summary>
	/// 所持カードリストのコピー.
	/// </summary>
	/// <param name="Numlist">Numlist.</param>
	public void CopyCardList(List<int> Numlist){
			CNumList = new List<int> (Numlist);
			CNumList.Sort ();
	}

	/// <summary>
	/// 所持マークリストのコピー.
	/// </summary>
	/// <returns>The mark list.</returns>
	/// <param name="MarkList">Mark list.</param>
	public void CopyMarkList(List<int> MarkList){
			CMarkList = new List<int> (MarkList);
			CMarkList.Sort ();
	}

	/// <summary>
	/// ストレイト用に数字を変換したリストを作る.
	/// </summary>
	/// <param name="NumList">Number list.</param>
	public void CreateStraightList(List<int> NumList){
			DamyList = new List<int> (NumList);
			DamyList.Sort ();
		//Debug.Log ("-------------------");
		for (int y = 0; y < DamyList.Count; y++) {
			//Debug.Log (DamyList[y]);
		}
		//Debug.Log ("-------------------");

			if(DamyList[0] == 1){
					DamyList.RemoveAt(0);
					DamyList.Add (14);
			}
			if(DamyList[1] == 1){
					DamyList.RemoveAt(1);
					DamyList.Add (14);
			}
			if(DamyList[2] == 1){
					DamyList.RemoveAt(2);
					DamyList.Add (14);
			}

	}

	/// <summary>
	/// ジョーカーの数を数える.
	/// </summary>
	/// <returns>The joker.</returns>
	/// <param name="CardNum">Card number.</param>
	public int CountsJoker(List<int> CardNum){
			int[] JokerArr = CardNum.Select (c => c).Where (s => s == 0).ToArray();
			return JokerArr.Length;
	}

	/// <summary>
	/// ストレイトか否か.
	/// </summary>
	/// <returns><c>true</c> if is straight the specified Numlist; otherwise, <c>false</c>.</returns>
	/// <param name="Numlist">Numlist.</param>
	public bool IsStraight(List<int> Numlist){
	
		CreateStraightList (Numlist);

		int StartIndex = CountsJoker(DamyList);
		int nowNumber = DamyList [StartIndex];
		for (int i = StartIndex; i < DamyList.Count - 1; i++) {
			if (nowNumber + 1 == DamyList [i + 1]) {
				nowNumber++;
				continue;
			} else {
				if (nowNumber == DamyList [i + 1]) {
					return false;
				}

				if (Mathf.Abs (nowNumber - DamyList [i + 1]) > 2) {
					return false;
				}

				if (StartIndex > 0) {
					nowNumber = DamyList [i + 1];
					StartIndex--;
					continue;
				} else {
					return false;
				}


			}
		}
		return true;
	}

	/// <summary>
	/// フラッシュか否か
	/// </summary>
	/// <returns><c>true</c> if is flush the specified Marklist; otherwise, <c>false</c>.</returns>
	/// <param name="Marklist">Marklist.</param>
	public bool IsFlush(List<int> Marklist){

			CopyMarkList (Marklist);

			int Mark = CMarkList [CMarkList.Count - 1];
			int[] FlushArr = CMarkList.Select (c => c).Where (s => s == 0|| s == Mark).ToArray();

			if(FlushArr.Length == 5){
					return true;
			}else{
					return false;
			}
	}

	/// <summary>
	/// ペアがあるか
	/// </summary>
	/// <returns><c>true</c> if is pair the specified NumList; otherwise, <c>false</c>.</returns>
	/// <param name="NumList">Number list.</param>
	public bool IsPair(List<int> NumList){
			CopyCardList (NumList);

			foreach(int i in CNumList){
					List<int> cardList = CNumList.Select (c => c).Where (s => s == i || s == 0).ToList ();
					//Debug.Log ("count:" + cardList.Count);
					if (cardList.Count == 2) {
							PairNum = i;
							return true;
					}
			}
			return false;
	}

	/// <summary>
	/// スリーカードがあるか.
	/// </summary>
	/// <returns><c>true</c> if this instance is three card the specified NumList; otherwise, <c>false</c>.</returns>
	/// <param name="NumList">Number list.</param>
	public bool IsThreeCard(List<int> NumList){
			CopyCardList (NumList);

			foreach(int i in CNumList){
					List<int> cardList = CNumList.Select (c => c).Where (s => s == i || s == 0).ToList ();
					//Debug.Log ("count:" + cardList.Count);
					if (cardList.Count == 3) {
							PairNum = i;
							return true;
					}
			}
			return false;
	}

	/// <summary>
	/// フルハウス用ジョーカー含まないペア判定
	/// </summary>
	/// <returns><c>true</c> if this instance is full house the specified NumList; otherwise, <c>false</c>.</returns>
	/// <param name="NumList">Number list.</param>
	public bool IsFullHouse(List<int> NumList){
			CopyCardList (NumList);

			foreach(int i in CNumList){
			List<int> cardList = CNumList.Select (c => c).Where (s => s == i && s != PairNum && s != 0).ToList ();
					//Debug.Log ("count:" + cardList.Count);
					if (cardList.Count == 2) {
							//PairNum = i;
							return true;
					}
			}
			return false;
	}

	/// <summary>
	/// ワンペア判定後限定の追加のペア判定.
	/// </summary>
	/// <returns><c>true</c> if this instance is two pair the specified NumList; otherwise, <c>false</c>.</returns>
	/// <param name="NumList">Number list.</param>
	public bool IsTwoPair(List<int> NumList){
			CopyCardList (NumList);

			foreach(int i in CNumList){
					List<int> cardList = CNumList.Select (c => c).Where (s => s == i && s != PairNum).ToList ();
					//Debug.Log ("count:" + cardList.Count);
					if (cardList.Count == 2) {
							PairNum = i;
							return true;
					}
			}
			return false;
	}

	/// <summary>
	/// フォーカードがあるか.
	/// </summary>
	/// <returns><c>true</c> if this instance is four card the specified NumList; otherwise, <c>false</c>.</returns>
	/// <param name="NumList">Number list.</param>
	public bool IsFourCard(List<int> NumList){
			CopyCardList (NumList);

			foreach(int i in CNumList){
					List<int> cardList = CNumList.Select (c => c).Where (s => s == i || s == 0).ToList ();
					//Debug.Log ("count:" + cardList.Count);
					if (cardList.Count == 4) {
							return true;
					}
			}
			return false;
	}

	/// <summary>
	/// ファイブカードがあるか.
	/// </summary>
	/// <returns><c>true</c> if this instance is five card the specified NumList; otherwise, <c>false</c>.</returns>
	/// <param name="NumList">Number list.</param>
	public bool IsFiveCard(List<int> NumList){
			CopyCardList (NumList);

			foreach(int i in CNumList){
					List<int> cardList = CNumList.Select (c => c).Where (s => s == i || s == 0).ToList ();
					//Debug.Log ("count:" + cardList.Count);
					if (cardList.Count == 5) {
							return true;
					}
			}
			return false;
	}

	/// <summary>
	/// ロイヤルストレートフラッシュ.
	/// </summary>
	/// <returns><c>true</c> if this instance is royal straight flush the specified num mark; otherwise, <c>false</c>.</returns>
	/// <param name="num">Number.</param>
	/// <param name="mark">Mark.</param>
	public bool IsRoyalStraightFlush(List<int>num,List<int>mark){
		//ジョーカーアリは認めない
		if (CountsJoker (num) > 0) {
			return false;
		}
		CopyCardList (num);
		CopyMarkList (mark);

		if(IsStraight(CNumList) == true){
			if(IsFlush(CMarkList) == true){
				int j = 0;
				foreach(int i in DamyList){
					j += i;
				}
				if(j == 60){
					return true;
				}
			}
		}

		return false;
	}



	public int PokarHandsInt(List<int> NumList,List<int> MarkList){

		if(IsRoyalStraightFlush(NumList,MarkList)==true){
			return 10;
		}

		if(IsStraight(NumList) == true){
			if(IsFlush(MarkList) == true){
				return 9;
			}
		}

		if(IsFiveCard(NumList) == true){
			return 8;
		}

		if(IsFourCard(NumList) == true){
			return 6;
		}

		if(IsStraight(NumList) == true){
			return 7;
		}
			


		if(IsThreeCard(NumList) == true){
			if(IsFullHouse(NumList)==true){
				return 5;
			}
			if(IsFlush(MarkList) == true){
				return 4;
			}
			return 2;
		}else if(IsFlush(MarkList) == true){
			return 4;
		}


			
		if(IsPair(NumList) == true){
			if(IsTwoPair(NumList) == true){
				return 3;
			}
			return 1;
		}
		return 0;

	}

	public int PokarHandsScore(int pokarHandid){
		switch(pokarHandid){
		case 0:
			//ノーペア
			return 1;
		case 1:
			//ワンペア
			return 2;
		case 2:
			//ツーペア
			return 3;
		case 3:
			//スリーカード
			return 4;
		case 4:
			//ストレイト
			return 5;
		case 5:
			//フラッシュ
			return 7;
		case 6:
			//フルハウス
			return 10;
		case 7:
			//フォーカード
			return 15;
		case 8:
			//ストレイトフラッシュ
			return 20;
		case 9:
			//ファイブカード
			return 25;
		case 10:
			//ロイヤルストレートフラッシュ
			return 100;
		default:
			return 0;
		}
	}

	public int ColorListCountToScore(int count){
		return 0;
	}

	public int IsColorHand(List<int> colors){

		int color = 0;

		if (IsWhite3Hand(colors) == true) {
			color = 1;
			if(IsWhite4Hand(colors) == true){
				color = 2;
				if(IsWhiteHand(colors) == true){
					color = 3;
				}
			}
		}

		if(IsBlack3Hand(colors) == true){
			color = 4;
			if(IsBlack4Hand(colors) == true){
				color = 5;
				if(IsBlackHand(colors) == true){
					color = 6;
				}
			}
		}

		return color;
	}

	public bool IsBlackHand(List<int>num){
		for (int i = 0; i < num.Count; i++) {
			if (num [i] == 0) {
				return false;
			}
		}
		return true;

	}

	public bool IsWhiteHand(List<int>num){
		for (int i = 0; i < num.Count; i++) {
			if (num [i] == 1) {
				return false;
			}
		}
		return true;
	}

	public bool IsBlack4Hand(List<int>num){
		int blackcount = 0;
		for (int i = 0; i < num.Count; i++) {
			if (num [i] == 1) {
				blackcount++;
			}
		}
		if (blackcount >= 4) {
			return true;
		} else {
			return false;
		}
	}

	public bool IsWhite4Hand(List<int>num){
		int whitecount = 0;
		for (int i = 0; i < num.Count; i++) {
			if (num [i] == 0) {
				whitecount++;
			}
		}
		if (whitecount >= 4) {
			return true;
		} else {
			return false;
		}
	}

	public bool IsBlack3Hand(List<int>num){
		int blackcount = 0;
		for (int i = 0; i < num.Count; i++) {
			if (num [i] == 1) {
				blackcount++;
			}
		}
		if (blackcount >= 3) {
			return true;
		} else {
			return false;
		}
	}

	public bool IsWhite3Hand(List<int>num){
		int whitecount = 0;
		for (int i = 0; i < num.Count; i++) {
			if (num [i] == 0) {
				whitecount++;
			}
		}
		if (whitecount >= 3) {
			return true;
		} else {
			return false;
		}

	}


}

