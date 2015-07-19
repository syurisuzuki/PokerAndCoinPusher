using UnityEngine;
using System.Collections;

/// <summary>
/// Shop scene.
/// </summary>
public class ShopScene : MonoBehaviour {

	public int havemedal;
	public UILabel havemedallabel;
	public UILabel title;

	int receivecharaid;

	public UIButton[] buyBtns;

	public UILabel chutorialtext;
	public GameObject chutorialtextRoot;
	bool ischutorial;
	public UIButton buck;

	bool c1l1clear;
	bool c1l2clear;
	bool c1l3clear;
	bool c2l1clear;
	bool c2l2clear;
	bool c2l3clear;
	bool c3l1clear;
	bool c3l2clear;
	bool c3l3clear;
	bool c4l1clear;
	bool c4l2clear;
	bool c4l3clear;
	bool c5l1clear;
	bool c5l2clear;
	bool c5l3clear;
	bool c6l1clear;
	bool c6l2clear;
	bool c6l3clear;
	bool c7l1clear;
	bool c7l2clear;
	bool c7l3clear;
	bool c8l1clear;
	bool c8l2clear;
	bool c8l3clear;
	bool c9l1clear;
	bool c9l2clear;
	bool c9l3clear;
	bool c10l1clear;
	bool c10l2clear;
	bool c10l3clear;
	bool c11l1clear;
	bool c11l2clear;
	bool c11l3clear;
	bool c12l1clear;
	bool c12l2clear;
	bool c12l3clear;


	// Use this for initialization
	void Start () {
		Singleton<SoundPlayer>.instance.playBGM( "Shop",0 );

		int chutorialendint = PlayerPrefs.GetInt ("ChutorialEnd", 0);
		if (chutorialendint == 0) {
			ischutorial = true;
			havemedallabel.text = "0";
			chutorialtextRoot.gameObject.SetActive (true);
			buck.isEnabled = false;
			StartChutorialShop ();
		} else {
			ischutorial = false;
			havemedal = PlayerPrefs.GetInt ("MEDALS", 0);

			int id = PlayerPrefs.GetInt ("selectChara", 1);
			receivecharaid = id;
			ChackValue ();
			SetCharaInfo (id);
		}
	}

	void StartChutorialShop(){
		for (int i = 0; i < buyBtns.Length; i++) {
			buyBtns [i].isEnabled = false;
		}
		buyBtns [0].isEnabled = true;

	}

	// Update is called once per frame
	void Update () {

	}

	void ChackValue(){

		havemedallabel.text = ""+havemedal;
		for (int i = 0; i < buyBtns.Length; i++) {
			string serchindex = "Value" + i;
			int value = int.Parse( GameObject.Find (serchindex).GetComponent<UILabel> ().text );
			if (value >= havemedal) {
				buyBtns [i].isEnabled = false;
			} else {
				buyBtns [i].isEnabled = true;
			}
		}
		CheckMaxKokand ();
	}

	void CheckMaxKokand(){
		int selectid = PlayerPrefs.GetInt ("selectChara", 1);
		string kokandkey = "Actor"+selectid+"LovePoint";
		int kokand = PlayerPrefs.GetInt (kokandkey, 0);
		string charalevel;
		int f;
		if (kokand <= 100 && kokand != 0) {
			charalevel = "Actor" + selectid + "Level2";
			f = PlayerPrefs.GetInt (charalevel, 0);
			if (kokand == 100) {
				if (f == 0) {
					for (int i = 0; i < buyBtns.Length; i++) {
						buyBtns [i].isEnabled = false;
					}
				}
			}
		} else if (kokand <= 200 && kokand > 100) {
			charalevel = "Actor" + selectid + "Level3";
			f = PlayerPrefs.GetInt (charalevel, 0);
			if (kokand == 200) {
				if (f == 0) {
					for (int i = 0; i < buyBtns.Length; i++) {
						buyBtns [i].isEnabled = false;
					}
				}
			}

		} else if(kokand == 0){
			charalevel = "Actor" + selectid + "Level1";
			f = PlayerPrefs.GetInt (charalevel, 0);
			if (kokand == 0) {
				if (f == 0) {
					for (int i = 0; i < buyBtns.Length; i++) {
						buyBtns [i].isEnabled = false;
					}
				}
			}
		}

		SetCharaInfo (receivecharaid);


	}

	void SetCharaInfo(int charaid){
		int isunlock = 0;
		string serchkey = "";
		switch (charaid) {
		case 1:
			serchkey = "Actor" + receivecharaid + "Unlock";
			//Debug.Log (serchkey);
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "エメラルドへのプレゼント";
			break;
		case 2:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "クルミへのプレゼント";
			break;
		case 3:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "シスタァへのプレゼント";
			break;
		case 4:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "セバスチャンへのプレゼント";
			break;
		case 5:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "エリスへのプレゼント";
			break;
		case 6:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "メルへのプレゼント";
			break;
		case 7:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "ジニアへのプレゼント";
			break;
		case 8:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "ハルカへのプレゼント";
			break;
		case 9:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "レオへのプレゼント";
			break;
		case 10:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "ホーリーへのプレゼント";
			break;
		case 11:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "シャイナへのプレゼント";
			break;
		case 12:
			serchkey = "Actor" + receivecharaid + "Unlock";
			isunlock = PlayerPrefs.GetInt (serchkey, 0);
			if (isunlock == 1) {
				buyBtns [0].isEnabled = false;
			} else {
				buyBtns [0].isEnabled = true;
			}
			title.text = "タケオへのプレゼント";
			break;
		}
	}

	public void PushCharaUnlock(){
		Singleton<SoundPlayer>.instance.playSE( "item");
		if (ischutorial) {
			chutorialtext.text = "これでエメラルドと\nゲームできるようになりました\n下の戻るボタンで戻りましょう";
			string keyc = "Actor1Unlock";
			PlayerPrefs.SetInt (keyc, 1);
			ChackValue ();
			SetCharaInfo (receivecharaid);
			buck.isEnabled = true;
			buyBtns [0].isEnabled = false;
		} else {
			int value = int.Parse (GameObject.Find ("Value0").GetComponent<UILabel> ().text);
			havemedal -= value;
			string key = "Actor" + receivecharaid + "Unlock";
			Debug.Log (key);
			PlayerPrefs.SetInt (key, 1);
			ChackValue ();
			SetCharaInfo (receivecharaid);
		}
	}

	public void PushBuyItem(){
		Singleton<SoundPlayer>.instance.playSE( "item");
		string id = UIButton.current.name;
		int idint = int.Parse(id);
		int uplovepoint = 0;

		//Debug.Log (idint);

		if (idint == 1) {uplovepoint = 1;
		} else if (idint == 2) {uplovepoint = 3;
		} else if (idint == 3) {uplovepoint = 5;
		} else if (idint == 4) {uplovepoint = 9;
		} else if (idint == 5) {uplovepoint = 15;
		} else if (idint == 6) {uplovepoint = 20;
		} else if (idint == 7) {uplovepoint = -999;
		} else if (idint == 8) {uplovepoint = 30;
		} else if (idint == 9) {uplovepoint = 30;
		} else if (idint == 10) {uplovepoint = 50;
		}

		string target = "Value" + idint;

		int value = int.Parse (GameObject.Find (target).GetComponent<UILabel> ().text);

		havemedal -= value;

		string kokandkey = "Actor"+receivecharaid+"LovePoint";
		int nowkokand = PlayerPrefs.GetInt (kokandkey, 0);
		nowkokand += uplovepoint;


		if (nowkokand > 200) {
			string charalevel = "Actor" + receivecharaid + "Level3";
			int f = PlayerPrefs.GetInt (charalevel, 0);
			if (f == 0) {
				nowkokand = 200;
			}
		} else if (nowkokand > 100) {
			string charalevel = "Actor" + receivecharaid + "Level2";
			int f = PlayerPrefs.GetInt (charalevel, 0);
			if (f == 0) {
				nowkokand = 100;
			}
		} else if (nowkokand > 0) {
			string charalevel = "Actor" + receivecharaid + "Level1";
			int f = PlayerPrefs.GetInt (charalevel, 0);
			if (f == 0) {
				nowkokand = 0;
			}
		}

		if (nowkokand < 0) {
			nowkokand = 0;
		}

		PlayerPrefs.SetInt (kokandkey, nowkokand);
		PlayerPrefs.SetInt ("MEDALS", havemedal);
		ChackValue ();
	}

	public void PushBuck(){
		Singleton<SoundPlayer>.instance.playSE( "click");
		Application.LoadLevel("CharaSelect");
	}


}
