using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITest : MonoBehaviour {
		public Text Bet;
		public Text Money;
		public int money;
		public int bet;
		public GameObject card;

		public GameObject canvas;
		public Animator UIanim;
		public GameObject touch;

	// Use this for initialization
	void Start () {
				money = PlayerPrefs.GetInt ("MONEY", 100);
				Money.text = money+"";
				Bet.text = bet+"";
				UIanim = canvas.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public void WinLose(float plmag){
				money += Mathf.CeilToInt (bet * plmag);
				PlayerPrefs.SetInt ("MONEY", money);
				int debug = PlayerPrefs.GetInt ("MONEY", 100);
				Debug.Log (debug);
				bet = 0;
				Money.text =money+"";
				Bet.text = bet+"";
				UIanim.SetBool ("Startpush",false);
		}

		public void StartPush(){
				UIanim.SetBool ("Startpush",true);
				UIanim.SetBool ("ContPush",false);
				card.SendMessage("DrawCard",5);
				touch.SendMessage ("Continue");
		}

		public void BetPush(){
				if(money>=10){
						money -= 10;
						bet += 10;
						Money.text =money+"";
						Bet.text = bet+"";
				}
		}
		public void ResetPush(){
				money += bet;
				bet = 0;
				Money.text =money+"";
				Bet.text = bet+"";
		}
		public void Test()
		{
				card.SendMessage ("Restart");
		}

		public void Chengebtn(){
				UIanim.SetBool ("ChengePush",true);
				UIanim.SetBool ("Startpush",false);
				card.SendMessage ("ChengeCard");
		}
				

		public void ContinuePush(){
				UIanim.SetBool ("ContPush",true);
				UIanim.SetBool ("ChengePush",false);
		}

		public void Test2(){
				Application.LoadLevel (2);
		}
}
