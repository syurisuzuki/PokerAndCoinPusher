using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour {

		public GameObject[] card;
		public List<GameObject> CardList = new List<GameObject>();
		public List<GameObject> CPUcard = new List<GameObject>();
		public List<int> CPUcardnum = new List<int> ();
		public List<int> CPUcardmark = new List<int> ();
		public List<GameObject> Mycard = new List<GameObject>();
		public List<int> Mycardnum = new List<int> ();
		public List<int> Mycardmark = new List<int> ();

		//joker入りの手札の判別用list//
		public List<int> jocker1 = new List<int> ();
		public List<int> jocker2 = new List<int> ();

		public GameObject cardparent;
		public GameObject cpucard;

		public int parecount;
		public int Jockercount;
		public int connectnumcount;

		public string Yaku;
		public Canvas d;
		public int listn;

		//役の倍率
		public float mag;

		public GameObject canvas;
		public UITest test;
		public Text yakutext;

	// Use this for initialization
	void Start () {

				test = FindObjectOfType<UITest>();
				cpucard = GameObject.Find ("Cpucard");
				cardparent = GameObject.Find ("cardparent");

				for(int i = 0;i<card.Length;i++){
						CardList.Add (card[i]);
				}

				Yaku = "BETを決めて下さい";
	}
	void Update () {
				yakutext.text = Yaku;
	}


		public void DrawCard(int drawnum){
				//プレイヤーカード
				Yaku = "残したいカードを選んで下さい";
				for (int h = 0;h<drawnum;h++){
						int cardnum = Random.Range (0, CardList.Count);
						GameObject panel0 = (GameObject)Instantiate (CardList[cardnum], new Vector3 (h-2, -0.5f, 0), Quaternion.identity);
						panel0.name = ""+h;
						panel0.tag = "PlayerCard";
						panel0.transform.parent = cardparent.transform;
						Animator panim = panel0.GetComponent<Animator> ();
						panim.SetBool ("CardAnim",true);
						Mycard.Add (panel0);
						int mark = Mycard [h].GetComponent<CardInfo> ().Mark;
						int cardn = Mycard [h].GetComponent<CardInfo> ().Number;
						Mycardmark.Add (mark);
						Mycardnum.Add (cardn);
						CardList.RemoveAt (cardnum);
				}
				//CPUカード
				for(int e = 0;e<drawnum;e++){
						int cardnum = Random.Range (0, CardList.Count);
						GameObject panel0 = (GameObject)Instantiate (CardList[cardnum], new Vector3 (e-2, 3.5f, 0), Quaternion.identity);
						panel0.name = ""+e;
						panel0.tag = "CpuCard";
						panel0.transform.parent = cpucard.transform;
						CPUcard.Add (panel0);
						int marke = CPUcard [e].GetComponent<CardInfo> ().Mark;
						int cardne = CPUcard [e].GetComponent<CardInfo> ().Number;
						CPUcardmark.Add (marke);
						CPUcardnum.Add (cardne);
						CardList.RemoveAt (cardnum);

				}
		}
				
		public void Judge_num(List<int> list){
				List<int> copylist = new List<int> (list);
				copylist.Sort ();
				parecount = 0;
				Jockercount = 0;
				int[] cardcopy = copylist.ToArray ();
				for (int w = 0;w<cardcopy.Length;w++){
						int wild = cardcopy [w];
						if(wild == 0){
								Jockercount++;
								copylist.RemoveAt (w);
						}
				}
				copylist.Sort ();
				cardcopy = copylist.ToArray ();

				int fnum = cardcopy [0];
				int lnum = cardcopy [cardcopy.Length - 1];

				for(int i = 0;i<cardcopy.Length;i++){
						int number = copylist [i];
						for(int h = i+1;h<cardcopy.Length;h++){
								int number2 = copylist [h];
								//ワンペアならば
								if(number==number2){
										copylist.RemoveAt (h);
										copylist.RemoveAt (i);
										copylist.Sort ();
										//昇格役判定//ワンペア->スリーカード
										for(int j = 0;j<copylist.Count;j++){
												number2 = copylist [j];
												if(number == number2){
														copylist.RemoveAt (j);
														copylist.Sort ();

														//昇格役判定//スリーカード->フォーカード
														for(int f = 0;f<copylist.Count;f++){
																number2 = copylist [f];
																if(number == number2){
																		copylist.RemoveAt (f);
																		copylist.Sort ();

																		//昇格役判定//フォーカード->ファイブカード
																		for(int fi = 0;fi<copylist.Count;fi++){
																				number2 = copylist [fi];
																				if(number == number2){
																						copylist.RemoveAt (fi);
																						copylist.Sort ();
																						Yaku = "ファイブカード";
																						mag = 6;
																						CpuCardChenge ();
																						test.WinLose (mag);
																						return;
																				}
																		}
																		//最終役
																		if(Jockercount>0){
																				Yaku = "ファイブカード";
																				mag = 6;
																				CpuCardChenge ();
																				test.WinLose (mag);
																				return;
																		}
																		Yaku = "フォーカード";
																		mag = 4;
																		CpuCardChenge ();
																		test.WinLose (mag);
																		return;
																}
														}

														//昇格役判定//スリーカード->フルハウス
														if(copylist.Count==2){
																if(copylist[0]==copylist[1]){
																		Yaku = "フルハウス";
																		mag = 3.5f;
																		CpuCardChenge ();
																		test.WinLose (mag);
																		return;
																}
														}

														if(Jockercount==1){
																Yaku = "フォーカード";
																mag = 4;
																CpuCardChenge ();
																test.WinLose (mag);
																return;
														}else if(Jockercount==2){
																Yaku = "ファイブカード";
																mag = 6;
																CpuCardChenge ();
																test.WinLose (mag);
																return;
														}
														Yaku = "スリーカード";
														mag = 2;
														CpuCardChenge ();
														test.WinLose (mag);
														return;
												}
										}
										//昇格役判定//ワンペア->ツーペア
										for(int y=0;y<copylist.Count;y++){
												for(int t=y+1;t<copylist.Count;t++){
														if(copylist[y]==copylist[t]){
																if(Jockercount==1){
																		Yaku = "フルハウス";
																		mag = 3.5f;
																		CpuCardChenge ();
																		test.WinLose (mag);
																		return;
																}
																Yaku = "ツーペア";
																mag = 1.5f;
																CpuCardChenge ();
																test.WinLose (mag);
																return;
														}
												}

										}
										if(Jockercount==1){
												Yaku = "スリーカード";
												mag = 2;
												CpuCardChenge ();
												test.WinLose (mag);
												return;
										}else if(Jockercount==2){
												Yaku = "フォーカード";
												mag = 4;
												CpuCardChenge ();
												test.WinLose (mag);
												return;
										}
										Yaku = "ワンペア";
										mag = 1;
										CpuCardChenge ();
										test.WinLose (mag);
										return;
								}

						}
				}
				copylist.Sort ();
				cardcopy = copylist.ToArray ();

				fnum = cardcopy [0];
				lnum = cardcopy [cardcopy.Length - 1];

				int fnumplus4 = fnum + 4;

				if (Jockercount == 0) {
						if (fnum == 1 && lnum == 13) {
								if (cardcopy [1] == 2) {
										if (cardcopy [2] == 3) {
												if (cardcopy [3] == 4 || cardcopy [3] == 12) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}
										} else if (cardcopy [2] == 11) {
												if(cardcopy [3] == 12){
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}
										}else{
												Yaku = "ノーペア";
												Judge_mark (Mycardmark, 0);
												return;
										}

								} else if (cardcopy [1] == 10) {
										if (cardcopy [2] == 11) {
												if(cardcopy [3] == 12){
														Yaku = "straight";
														Judge_mark (Mycardmark, 2);
														mag = 2.5f;
														return;
												}else{
														Yaku = "ノーペア";
														Judge_mark (Mycardmark, 0);
														return;
												}
										}else{
												Yaku = "ノーペア";
												Judge_mark (Mycardmark, 0);
												return;
										}
								}else{
										Yaku = "ノーペア";
										Judge_mark (Mycardmark, 0);
										return;
								}
						}else if(fnumplus4 >= lnum){
								Yaku = "straightIIII";
								mag = 2.5f;
								Judge_mark (Mycardmark, 1);
								return;
						}else{
								Yaku = "ノーペア";
								Judge_mark (Mycardmark, 0);
								return;
						}
				} else if (Jockercount == 1) {
						if(fnum==1){
								if (cardcopy [1] == 2) {
										if (cardcopy [2] == 3) {
												if (cardcopy [3] == 4 || cardcopy [3] == 12 || cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										} else if (cardcopy [2] == 4) {
												if (cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										} else if (cardcopy [2] == 11) {
												if (cardcopy [3] == 12 || cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										} else if (cardcopy [2] == 12) {
												if (cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										}

								} else if (cardcopy [1] == 3) {
										if (cardcopy [2] == 4 || cardcopy [2] == 12) {
												if (cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										}else{
												Yaku = "ワンペア";
												mag = 1;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else if (cardcopy [1] == 10) {
										if (cardcopy [2] == 11) {
												if (cardcopy [3] == 12 || cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										} else if (cardcopy [2] == 12) {
												if (cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										}else{
												Yaku = "ワンペア";
												mag = 1;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else if (cardcopy [1] == 11) {
										if (cardcopy [2] == 12) {
												if (cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										}else{
												Yaku = "ワンペア";
												mag = 1;
												Judge_mark (Mycardmark,0);
												return;
										}
								}else{
										Yaku = "ワンペア";
										mag = 1;
										Judge_mark (Mycardmark,0);
										return;
								}
						}else if(fnum==2){
								if (cardcopy [1] == 3) {
										if (cardcopy [2] == 4 || cardcopy [2] == 12) {
												if (cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										}else{
												Yaku = "ワンペア";
												mag = 1;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else if (cardcopy [1] == 11) {
										if (cardcopy [2] == 12) {
												if (cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 1);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										}else{
												Yaku = "ワンペア";
												mag = 1;
												Judge_mark (Mycardmark,0);
												return;
										}
								}else{
										Yaku = "ワンペア";
										mag = 1;
										Judge_mark (Mycardmark,0);
										return;
								}
						}else if(fnum==10){
								if (cardcopy [1] == 11) {
										if (cardcopy [2] == 12) {
												if (cardcopy [3] == 13) {
														Yaku = "straight";
														mag = 2.5f;
														Judge_mark (Mycardmark, 2);
														return;
												}else{
														Yaku = "ワンペア";
														mag = 1;
														Judge_mark (Mycardmark,0);
														return;
												}
										}else{
												Yaku = "ワンペア";
												mag = 1;
												Judge_mark (Mycardmark,0);
												return;
										}
								}else{
										Yaku = "ワンペア";
										mag = 1;
										Judge_mark (Mycardmark,0);
										return;
								}

						}else if(fnumplus4 >= lnum){
								Yaku = "straightIIII";
								mag = 2.5f;
								Judge_mark (Mycardmark, 1);
								return;
						}else{
								Yaku = "ワンペア";
								mag = 1;
								Judge_mark (Mycardmark,0);
								return;
						}
				}else if (Jockercount==2){
						if (fnum == 1) {
								if (cardcopy [1] == 2) {
										if (cardcopy [2] == 3 || cardcopy [2] == 4 || cardcopy [2] == 11 || cardcopy [2] == 12 || cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 1);
												return;		
										} else {
												Yaku = "スリーカード";
												mag = 2f;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else if (cardcopy [1] == 10) {
										if (cardcopy [2] == 11 || cardcopy [2] == 12 || cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 2);
												return;		
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else {
										Yaku = "スリーカード";
										mag = 2;
										Judge_mark (Mycardmark,0);
										return;
								}
						} else if (fnum == 2) {
								if (cardcopy [1] == 3) {
										if (cardcopy [2] == 4 || cardcopy [2] == 12 || cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 1);
												return;		
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else if (cardcopy [1] == 4) {
										if (cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 1);
												return;		
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else if (cardcopy [1] == 11) {
										if (cardcopy [2] == 12 || cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 1);
												return;		
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else if (cardcopy [1] == 12) {
										if (cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 1);
												return;		
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else {
										Yaku = "スリーカード";
										mag = 2;
										Judge_mark (Mycardmark,0);
										return;
								}
						} else if (fnum == 3) {
								if (lnum == 13) {
										if (cardcopy [1] == 4 || cardcopy [1] == 12) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 1);
												return;	
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else {
										Yaku = "スリーカード";
										mag = 2;
										Judge_mark (Mycardmark,0);
										return;
								}
						} else if (fnum == 10) {
								if (cardcopy [1] == 11) {
										if (cardcopy [2] == 12 || cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 2);
												return;	
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark,0);
												return;
										}

								} else if (cardcopy [1] == 12) {
										if (cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 2);
												return;	
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark,0);
												return;
										}
								} else {
										Yaku = "スリーカード";
										mag = 2;
										Judge_mark (Mycardmark,0);
										return;
								}
						} else if (fnum == 11) {
								if (cardcopy [1] == 12) {
										if (cardcopy [2] == 13) {
												Yaku = "straight";
												mag = 2.5f;
												Judge_mark (Mycardmark, 2);
												return;
										} else {
												Yaku = "スリーカード";
												mag = 2;
												Judge_mark (Mycardmark, 0);
												return;
										}
								} else {
										Yaku = "スリーカード";
										mag = 2;
										Judge_mark (Mycardmark, 0);
										return;
								}

						}else if(fnumplus4 >= lnum){
								Yaku = "straightIIII";
								mag = 2.5f;
								Judge_mark (Mycardmark, 1);
								return;
						}else{
								Yaku = "スリーカード";
								mag = 2;
								Judge_mark (Mycardmark,0);
								return;
						}
				}

		}

		//フラッシュの判定
		public void Judge_mark(List<int> mlist,int strong){
				List<int> copymlist = new List<int> (mlist);
				copymlist.Sort ();
				//ジョーカー除き
				for (int m = 0;m<copymlist.Count;m++){
						int wildm = copymlist [m];
						if(wildm == 0){
								Jockercount++;
								copymlist.RemoveAt (m);
						}
				}
				for(int n = 1;n<copymlist.Count;n++){
						if(copymlist[0]!=copymlist[n]){
								CpuCardChenge ();
								test.WinLose (mag);
								return;
						}
				}

				if (strong == 2) {
						Yaku = "ロイヤルストレイトフラッシュ";
						mag = 5;
				} else if (strong == 1) {
						Yaku = "ストレイトフラッシュ";
						mag = 4.5f;
						Debug.Log ("ストレイトフラッシュ");
				} else if(strong == 0){
						Yaku = "フラッシュ";
						mag = 3;
						Debug.Log ("フラッシュ");
				}
				CpuCardChenge ();
				test.WinLose (mag);


		}

		public void Restart(){
				CPUcard.Clear ();
				CPUcardmark.Clear ();
				CPUcardnum.Clear ();
				Mycard.Clear ();
				Mycardmark.Clear ();
				Mycardnum.Clear ();
				CardList.Clear ();
				for(int i = 0;i<card.Length;i++){
						CardList.Add (card[i]);
				}
						
				cpucard.SendMessage ("RestartcardDel");
				cardparent.SendMessage ("RestartcardDel");

				Yaku = "BETを決めて下さい";
		}

		public void CpuCardChenge(){
				//CPUのノーマルAI -> ジョーカー残しつつフラッシュが可能か　それ以外は堅実にペアを残す
				int Hart = 0;//4
				int Clo = 0;//1
				int Spade = 0;//3
				int Dia = 0;//2
				Debug.Log ("~~~~~~~~~~~~~");
				//ジョーカー残し　マークの数え上げ
				for(int i = 0;i<CPUcardmark.Count;i++){
						if(CPUcardmark[i]==0){
								Debug.Log ("Joあり");
								CPUcard [i].SendMessage ("TouchCardCpu");
						}else if(CPUcardmark[i]==1){
								Clo++;
						}else if(CPUcardmark[i]==2){
								Dia++;
						}else if(CPUcardmark[i]==3){
								Spade++;
						}else if(CPUcardmark[i]==4){
								Hart++;
						}
				}
				for(int i = 0;i<CPUcardmark.Count;i++){
						Debug.Log (CPUcardmark[i]);
				}

				//優先:フラッシュの可能性は高いか(同マーク3枚~)
				if(3<=Hart){
						for(int m = 0;m<CPUcardmark.Count;m++){
								if(CPUcardmark[m]!=4){
										Debug.Log ("ハートのフラッシュ可能性あり");
										CPUcard [m].SendMessage ("TouchCardCpu");
										StartCoroutine ("CpuCardChengeAndOpen");
										return;
								}
						}
				}
				if(3<=Clo){
						for(int m = 0;m<CPUcardmark.Count;m++){
								if(CPUcardmark[m]!=1){
										Debug.Log ("クローバーのフラッシュ可能性あり");
										CPUcard [m].SendMessage ("TouchCardCpu");
										StartCoroutine ("CpuCardChengeAndOpen");
										return;
								}
						}
				}
				if(3<=Spade){
						for(int m = 0;m<CPUcardmark.Count;m++){
								if(CPUcardmark[m]!=3){
										Debug.Log ("スペードのフラッシュ可能性あり");
										CPUcard [m].SendMessage ("TouchCardCpu");
										StartCoroutine ("CpuCardChengeAndOpen");
										return;
								}
						}
				}
				if(3<=Dia){
						for(int m = 0;m<CPUcardmark.Count;m++){
								if(CPUcardmark[m]!=2){
										Debug.Log ("ダイアのフラッシュ可能性あり");
										CPUcard [m].SendMessage ("TouchCardCpu");
										StartCoroutine ("CpuCardChengeAndOpen");
										return;
								}
						}
				}
				//ペアは揃っているか
				List<int> copylistnum = new List<int>(CPUcardnum);
				copylistnum.Sort ();

				int matchnum = 0;
				int matchnum2 = 0;
				int cardstrog = copylistnum[copylistnum.Count - 1];
				for (int l = 1; l < copylistnum.Count; l++) {
						int currentnum = copylistnum [l-1];
						if(currentnum == copylistnum[l]){
								if(matchnum==currentnum){
										matchnum2 = currentnum;
								}else{
										matchnum = currentnum;
								}
						}
				}

				if(0<matchnum){
						for(int h = 0;h<CPUcardnum.Count;h++){
								if(matchnum == CPUcardnum[h]){
										Debug.Log ("ペア１");
										CPUcard [h].SendMessage ("TouchCardCpu");
								}
						}
				}
				if(0<matchnum2){
						for(int h = 0;h<CPUcardnum.Count;h++){
								if(matchnum == CPUcardnum[h]){
										Debug.Log ("ペア２");
										CPUcard [h].SendMessage ("TouchCardCpu");
								}
						}
				}

				if(0<matchnum){
						if(0<matchnum2){
								StartCoroutine ("CpuCardChengeAndOpen");
								return;
						}
						StartCoroutine ("CpuCardChengeAndOpen");
						return;
				}

				//それ以外 > カードの番号の大きい

				for(int i = 0;i<CPUcardnum.Count;i++){
						if(cardstrog == CPUcardnum[i]){
								Debug.Log ("other");
								CPUcard [i].SendMessage ("TouchCardCpu");
						}
				}

				StartCoroutine ("CpuCardChengeAndOpen");

				Debug.Log ("~~~~~~~~~~~~~");
		}
		IEnumerator CpuCardChengeAndOpen(){
				List<GameObject> copylistG = new List<GameObject> (CPUcard);
				GameObject[] cardcopyss = copylistG.ToArray ();
				int lengt = cardcopyss.Length;
				int v = 0;
				for(int y=0;y<lengt;y++){

						bool ggg = cardcopyss [y].GetComponent<CardInfo> ().touched;
						if(ggg == false){

								Vector3 savedvec = cardcopyss [y].transform.position;
								Debug.Log (savedvec);
								CPUcard.RemoveAt (y-v);
								CPUcardmark.RemoveAt (y-v);
								CPUcardnum.RemoveAt (y-v);
								Destroy (cardcopyss[y]);
								v++;

								int randomnum = Random.Range (0, CardList.Count);
								GameObject panel0 = (GameObject)Instantiate (CardList[randomnum], savedvec, Quaternion.identity);
								panel0.name = "chengedCard";
								panel0.tag = "CpuCard";
								panel0.transform.parent = cpucard.transform;

								int mark = panel0.GetComponent<CardInfo> ().Mark;
								int cardn = panel0.GetComponent<CardInfo> ().Number;
								CPUcard.Add (panel0);
								CPUcardmark.Add (mark);
								CPUcardnum.Add (cardn);
								CardList.RemoveAt (randomnum);

						}else if(ggg == true){
								Animator panim = cardcopyss[y].GetComponent<Animator> ();
								panim.SetBool ("Touched",false);
						}

				}
				copylistG = new List<GameObject> (CPUcard);
				cardcopyss = copylistG.ToArray ();
				for(int y=0;y<cardcopyss.Length;y++){
						Animator panim = cardcopyss[y].GetComponent<Animator> ();
							panim.SetBool ("CardAnim",true);
							yield return new WaitForSeconds(1);
				}
				yield break;

				//このあとに
				//CPU役判定>プレイヤー役と勝負
		}

		IEnumerator Chengecard(){
				List<GameObject> copylistG = new List<GameObject> (Mycard);
				GameObject[] cardcopyss = copylistG.ToArray ();
				int lengt = cardcopyss.Length;
				int v = 0;
				for(int y=0;y<lengt;y++){

						bool ggg = cardcopyss [y].GetComponent<CardInfo> ().touched;
						if(ggg == false){
								Vector3 savedvec = cardcopyss [y].transform.position;
								Debug.Log (savedvec);
								Mycard.RemoveAt (y-v);
								Mycardmark.RemoveAt (y-v);
								Mycardnum.RemoveAt (y-v);
								Destroy (cardcopyss[y]);
								v++;

								int randomnum = Random.Range (0, CardList.Count);
								GameObject panel0 = (GameObject)Instantiate (CardList[randomnum], savedvec, Quaternion.identity);
								panel0.name = "chengedCard";
								panel0.tag = "PlayerCard";
								panel0.transform.parent = cardparent.transform;
								Animator panim = panel0.GetComponent<Animator> ();
								panim.SetBool ("CardAnim",true);

								int mark = panel0.GetComponent<CardInfo> ().Mark;
								int cardn = panel0.GetComponent<CardInfo> ().Number;
								Mycard.Add (panel0);
								Mycardmark.Add (mark);
								Mycardnum.Add (cardn);
								CardList.RemoveAt (randomnum);
								yield return new WaitForSeconds(1);
						}

				}
				Judge_num (Mycardnum);
				yield break;
		}

		public void ChengeCard(){
				StartCoroutine ("Chengecard");
				}


		public void ChengeChild(Vector3 vec){
				if(vec.x==-2.0){
						listn = 0;
				}else if(vec.x==-1.0){
						listn = 1;
				}else if(vec.x==0){
						listn = 2;
				}else if(vec.x==1.0){
						listn = 3;
				}else if(vec.x==2.0){
						listn = 4;
				}
				Mycard.RemoveAt (listn);
				Mycardmark.RemoveAt (listn);
				Mycardnum.RemoveAt (listn);
				int cardnum = Random.Range (0, CardList.Count);
				Mycard.Add (CardList [cardnum]);

				GameObject panel0 = (GameObject)Instantiate (CardList[cardnum], vec, Quaternion.identity);
				panel0.name = ""+listn;
				panel0.tag="PlayerCard";
				panel0.transform.parent = cardparent.transform;
				int mark = panel0.GetComponent<CardInfo> ().Mark;
				int cardn = panel0.GetComponent<CardInfo> ().Number;
				Debug.Log (CardList[cardnum]+":"+mark+":"+cardn);
				Mycardmark.Add (mark);
				Mycardnum.Add (cardn);
				CardList.RemoveAt (cardnum);
		}
}
