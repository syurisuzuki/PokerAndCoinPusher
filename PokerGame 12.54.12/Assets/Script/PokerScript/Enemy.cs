using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;


public class Enemy : MonoBehaviour {

	Card carde;
	Judge judge;
	//ゲームの親か
	[SerializeField] bool IsParent = false;

	public List<int> EnemyCardNum;
	public List<int> EnemyCardMark;
	public List<GameObject> EnemyCardObject;

	SpriteRenderer MainSpriteRenderer;
	public Sprite[] Faces;
	public Text coment;

	public int useAIkeyValue1;
	public int useAIkeyValue2;

	// Use this for initialization
	void Start () {
		MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		//MainSpriteRenderer.sprite = Faces[9];
				//coment.text = "よろしくね！";

		judge = GetComponent<Judge> ();
		carde = FindObjectOfType<Card>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InitAllListCPU(){
		EnemyCardMark.Clear ();
		EnemyCardNum.Clear ();
		EnemyCardObject.Clear ();
	}


	/// <summary>
	/// CPUを親にする.
	/// </summary>
	public void enemyIsParent(){
		IsParent = true;
	}

	/// <summary>
	/// CPUを親から外す.
	/// </summary>
	public void enemyNoParent(){
		IsParent = false;
	}

	/// <summary>
		/// エネミーのドローの処理、リストをもらってくる.
		/// </summary>
		/// <param name="numList">Number list.</param>
		/// <param name="markList">Mark list.</param>
		/// <param name="cardObj">Card object.</param>
	public void drawCardEnemy(List<int> numList,List<int> markList,List<GameObject> cardObj){
		for(int i = 0;i<numList.Count;i++){
			EnemyCardNum.Add (numList [i]);
			EnemyCardMark.Add (markList [i]);
			EnemyCardObject.Add (cardObj [i]);
			}
	}

	public string ChengeCommentText(){

		string com = null;

		if(judge.IsFlush(EnemyCardMark) == true){
			com = "ふふん♪いい感じね♪";
			MainSpriteRenderer.sprite = Faces[10];
			return com;
		}

		if(judge.IsThreeCard(EnemyCardNum) == true){
			com = "あら、まぁまぁね!!";
			return com;
		}

		if(judge.IsPair(EnemyCardNum) == true){
			com = "あちゃーこりゃだめね";
			return com;
		}

		com = "むむむ・・・";

		return com;
	}

	public void FaceFirstDraw(){
		//初期役ありなしで表情がよくなる&セリフへんか

		if(judge.IsFlush(EnemyCardMark) == true){
			coment.text = "ふふん♪いい感じね♪";
			MainSpriteRenderer.sprite = Faces[10];
			return;
		}

		if(judge.IsThreeCard(EnemyCardNum) == true){
			coment.text = "あら、まぁまぁね!!";
			MainSpriteRenderer.sprite = Faces[4];
			return;
		}

		if(judge.IsPair(EnemyCardNum) == true){
			coment.text = "あちゃーこりゃだめね";
			MainSpriteRenderer.sprite = Faces[12];
			return;
		}
						
		//役なし沈黙
		coment.text = "・・・";
		MainSpriteRenderer.sprite = Faces[15];
	}

	public int CPUParentBet(){
		if(judge.IsFlush(EnemyCardMark) == true){
			return 2;
		}

		if(judge.IsStraight(EnemyCardNum) == true){
			return 2;
		}

		if(judge.IsThreeCard(EnemyCardNum) == true){
			if(judge.IsFullHouse(EnemyCardNum) == true){
				return 2;
			}
			return 2;
		}

		if(judge.IsPair(EnemyCardNum) == true){
			if(judge.IsTwoPair(EnemyCardNum) == true){
				return 2;
			}
			return 1;
		}

		return 1;
	}

	public int thinkBet(int nowhavemedal,int playerhavemedal){
		if(nowhavemedal>playerhavemedal){
			if(judge.IsFlush(EnemyCardMark) == true){
				return 2;
			}

			if(judge.IsStraight(EnemyCardNum) == true){
				return 2;
			}

			if(judge.IsThreeCard(EnemyCardNum) == true){
				if(judge.IsFullHouse(EnemyCardNum) == true){
					return 2;
				}
				return 1;
			}

			if(judge.IsPair(EnemyCardNum) == true){
				if(judge.IsTwoPair(EnemyCardNum) == true){
					return 2;
				}
				return 1;
			}

			return 0;
		}else{
			int randamAI = Random.Range (1, 3);
			return randamAI;
		}
	}

	public bool DoCall(){
		if(judge.IsFlush(EnemyCardMark) == true){
			return true;
		}

		if(judge.IsStraight(EnemyCardNum) == true){
			return true;
		}

		if(judge.IsThreeCard(EnemyCardNum) == true){
			if(judge.IsFullHouse(EnemyCardNum) == true){
				return true;
			}
			return true;
		}

		if(judge.IsPair(EnemyCardNum) == true){
			if(judge.IsTwoPair(EnemyCardNum) == true){
				return true;
			}
			return true;
		}

		int randamAI = Random.Range (0, 20);
		if (randamAI > 5) {
			return true;
		}else{
			return false;
		}
	}

	public int ThinkAI(){
		//場に残っているカードの取得

		List<int> FieldCardsNum = new List<int> ();
		List<int> FieldCardsMark = new List<int> ();

		foreach(GameObject obj in EnemyCardObject){
			int mark = obj.GetComponent<CardInfo> ().Mark;
			int cardn = obj.GetComponent<CardInfo> ().Number;
			FieldCardsMark.Add (mark);
			FieldCardsNum.Add (cardn);
		}

		if(judge.IsStraight(EnemyCardNum) == true){
			if(judge.IsFlush(EnemyCardMark)){
				return 0;
			}
		}

		//ファイブカード
		if(judge.IsFiveCard(EnemyCardNum) == true){
			return 0;
		}

		//フォーカード
		if(judge.IsFourCard(EnemyCardNum) == true){
			return 0;
		}

		//フルハウス
		if(judge.IsThreeCard(EnemyCardNum) == true){
			if(judge.IsFullHouse(EnemyCardNum) == true){
				return 0;
			}
		}

		//フラッシュ
		if(judge.IsFlush(EnemyCardNum) == true){
			return 0;
		}

		//ストレイト
		if(judge.IsStraight(EnemyCardNum) == true){
			return 0;
		}

		if(judge.IsThreeCard(EnemyCardNum) == true){
			Debug.Log ("スリーカード");
			return 1;
			//3枚残し
		}

		//ツーペア&ワンペア
		if(judge.IsPair(EnemyCardNum)== true){
			if(judge.IsTwoPair(EnemyCardNum) == true){
				Debug.Log ("ツーペア");
				return 3;
				//1枚残し
			}
			Debug.Log ("ワンペア");
			return 2;
			//2枚残し
		}

		//フラッシュ可能か(手札に３枚以上あるか?)

		//4 Spade 5 Heart 6 Dia 7 Clover 残し
		List<int> CPUSpadeList = EnemyCardMark.Select (c => c).Where (s => s == 3).ToList ();
		List<int> CPUHeartList = EnemyCardMark.Select (c => c).Where (s => s == 4).ToList ();
		List<int> CPUDiaList = EnemyCardMark.Select (c => c).Where (s => s == 2).ToList ();
		List<int> CPUCloverList = EnemyCardMark.Select (c => c).Where (s => s == 1).ToList ();

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
		if(judge.CountsJoker(EnemyCardNum) > 0){
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
		//全チェンジ
		return 9;

	}

}
