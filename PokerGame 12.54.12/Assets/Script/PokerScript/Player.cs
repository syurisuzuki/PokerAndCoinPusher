using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// プレーヤーのクラス.
/// </summary>
public class Player : MonoBehaviour {

	Card cards;
	//ゲームの親か
	public bool IsParent = true;

	public List<GameObject> handCardList = new List<GameObject>();
	public List<int> handCardNum = new List<int>();
	public List<int> handCardMark = new List<int>();

	void Awake(){
			cards = FindObjectOfType<Card>();
	}

	void Start(){

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
	public void drawCard(List<int> numList,List<int> markList,List<GameObject> cardObj){
			for(int i = 0;i<numList.Count;i++){
					handCardNum.Add (numList [i]);
					handCardMark.Add (markList [i]);
					handCardList.Add (cardObj [i]);
			}
			Debug.Log (handCardNum.Count);
	}
}
