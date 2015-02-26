using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// エネミークラス.
/// </summary>
public class Enemy : MonoBehaviour {

	public Card carde;
	public Judge judge;
	//ゲームの親か
	//[SerializeField] bool IsParent = false;

	//カードの情報
	public List<int> EnemyCardNum;
	public List<int> EnemyCardMark;
	public List<GameObject> EnemyCardObject;
	public List<int> EnemyCardScore;

	//顔アイコン
	//SpriteRenderer MainSpriteRenderer;
	//public Sprite[] Faces;

	//コメント
	public Text coment;

	//キーの値を格納する
	public int useAIkeyValue1;
	public int useAIkeyValue2;

	// Use this for initialization
	void Start () {
		//MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		//MainSpriteRenderer.sprite = Faces[9];
				//coment.text = "よろしくね！";

		judge = GetComponent<Judge> ();
		carde = FindObjectOfType<Card>();
	}

	/// <summary>
	/// CPUのFaceIconを変更する.
	/// </summary>
	/// <param name="index">Index.</param>
	public void ChengeFaceSprite(int index){
		//MainSpriteRenderer.sprite = Faces[index];
	}

	/// <summary>
	/// CPUの持つリストの全初期化
	/// </summary>
	public void InitAllListCPU(){
		EnemyCardMark.Clear ();
		EnemyCardNum.Clear ();
		EnemyCardObject.Clear ();
		EnemyCardScore.Clear ();
	}

	/// <summary>
		/// エネミーのドローの処理、リストをもらってくる.
		/// </summary>
		/// <param name="numList">Number list.</param>
		/// <param name="markList">Mark list.</param>
		/// <param name="cardObj">Card object.</param>
	public void drawCardEnemy(List<int> numList,List<int> markList,List<GameObject> cardObj,List<int> scoreList){
		for(int i = 0;i<numList.Count;i++){
			EnemyCardNum.Add (numList [i]);
			EnemyCardMark.Add (markList [i]);
			EnemyCardObject.Add (cardObj [i]);
			EnemyCardScore.Add (scoreList [i]);
			}
	}

	public void removeListc(int index){
		EnemyCardObject.RemoveAt (index);
	}

	public void addListc(GameObject obj){
		EnemyCardObject.Add(obj);
	}

	/// <summary>
	/// Call時のコメントの変更.
	/// </summary>
	/// <returns>The comment call.</returns>
	public string ChengeCommentCall(){
		string com = null;

		if(judge.IsFlush(EnemyCardMark) == true){
			com = "ふふん♪いい感じね♪";
			//MainSpriteRenderer.sprite = Faces[10];
			return com;
		}

		if(judge.IsThreeCard(EnemyCardNum) == true){
			com = "あら、まぁまぁね!!";
			return com;
		}

		if(judge.IsPair(EnemyCardNum) == true){
			com = "あちゃーこりゃだめねー";
			return com;
		}

		com = "むむむ・・・";

		return com;
	}
		
	/// <summary>
	/// CPUの親時のベット枚数.
	/// </summary>
	/// <returns>The parent bet.</returns>
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

	/// <summary>
	/// コールかレイズかドロップかを返すint
	/// </summary>
	/// <returns>The bet.</returns>
	/// <param name="nowhavemedal">Nowhavemedal.</param>
	/// <param name="playerhavemedal">Playerhavemedal.</param>
	public int thinkBet(int nowhavemedal,int playerhavemedal){

		//2がレイズ1がコール0がドロップ
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

	/// <summary>
	/// Callするかしないかのbool.
	/// </summary>
	/// <returns><c>true</c>, if call was done, <c>false</c> otherwise.</returns>
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

	/// <summary>
	/// チェンジするカードの選定を行う.
	/// </summary>
	/// <returns>The A.</returns>
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
