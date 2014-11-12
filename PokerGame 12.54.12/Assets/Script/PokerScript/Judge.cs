using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Judge : MonoBehaviour {

	public int jokerCount;
	public int handStrong;

	private List<int> CNumList;
	private List<int> CMarkList;
	private List<int> DamyList;

	private int PairNum;

	/// <summary>
	/// 強さの判定
	/// </summary>
	/// <param name="plmag">Plmag.</param>
	/*public void WinLose(float plmag){
			money += Mathf.CeilToInt (bet * plmag);
			PlayerPrefs.SetInt ("MONEY", money);
			int debug = PlayerPrefs.GetInt ("MONEY", 100);
			Debug.Log (debug);
			bet = 0;
			Money.text =money+"";
			Bet.text = bet+"";
			UIanim.SetBool ("Startpush",false);
	}*/

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
			if(DamyList[0] == 1 && DamyList[DamyList.Count - 1] == 13){
					DamyList.RemoveAt(0);
					DamyList.Add (14);
			}
			if(DamyList[1] == 1 && DamyList[DamyList.Count - 1] == 13){
					DamyList.RemoveAt(1);
					DamyList.Add (14);
			}
			if(DamyList[2] == 1 && DamyList[DamyList.Count - 1] == 13){
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
	/// ストレイトか否か
	/// </summary>
	/// <returns><c>true</c> if is straight the specified Numlist; otherwise, <c>false</c>.</returns>
	/// <param name="Numlist">Numlist.</param>
	public bool IsStraight(List<int> Numlist){
	
			CreateStraightList (Numlist);

			int StartIndex = CountsJoker(DamyList);
			int nowNumber = DamyList [StartIndex];
			int jokercounts = CountsJoker (Numlist);

			for (int i = StartIndex; i < DamyList.Count; i++) {
					while (nowNumber < DamyList [i]) {
							if (jokercounts > 0) {
									nowNumber++;
									jokercounts--;
									continue;
							}
							return false;
					}
					if (nowNumber == DamyList [i]) {
							nowNumber++;
							continue;
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
					List<int> cardList = CNumList.Select (c => c).Where (s => s == i && s != PairNum).ToList ();
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

		if(IsFiveCard(NumList) == true){
			return 9;
		}

		if(IsStraight(NumList) == true){
			if(IsFlush(MarkList) == true){
				return 8;
			}
		}

		if(IsFourCard(NumList) == true){
			return 7;
		}

		if(IsThreeCard(NumList) == true){
			if(IsFullHouse(NumList)==true){
				return 6;
			}
		}

		if(IsFlush(MarkList) == true){
			return 5;
		}

		if(IsStraight(NumList) == true){
			return 4;
		}

		if(IsThreeCard(NumList) == true){
			return 3;
		}

		if(IsPair(NumList) == true){
			if(IsTwoPair(NumList) == true){
				return 2;
			}
			return 1;
		}

		return 0;

	}

}

