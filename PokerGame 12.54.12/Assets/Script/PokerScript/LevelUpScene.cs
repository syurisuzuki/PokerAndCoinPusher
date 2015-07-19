using UnityEngine;
using System.Collections;

/// <summary>
/// Shop scene.
/// </summary>
public class LevelUpScene : MonoBehaviour {

	public int havemedal;
	public UILabel havemedallabel;
	public UILabel title;

	public UIButton[] buyBtns;

	public UILabel chutorialtext;
	public GameObject chutorialtextRoot;
	//bool ischutorial;
	public UIButton buck;

	int attackPower;
	int hp;
	int defenece;

	int specialP;

	public UILabel HPtext;
	public UILabel Attext;
	public UILabel Detext;
	public UILabel SPtext;

	// Use this for initialization
	void Start () {
		Singleton<SoundPlayer>.instance.playBGM( "Shop",0 );
		attackPower = PlayerPrefs.GetInt ("P_Attack", 2);
		hp = PlayerPrefs.GetInt ("P_health", 120);
		defenece = PlayerPrefs.GetInt ("P_Defence", 0);
		specialP = PlayerPrefs.GetInt ("Special", 0);

		int chutorialendint = PlayerPrefs.GetInt ("ChutorialEnd_LV", 0);
		if (chutorialendint == 0) {
			//ischutorial = true;
			havemedallabel.text = "0";
			chutorialtextRoot.gameObject.SetActive (true);
			//buck.isEnabled = false;
			StartChutorialShop ();
		} else {
			//ischutorial = false;
			havemedal = PlayerPrefs.GetInt ("MEDALS", 0);

			//int id = PlayerPrefs.GetInt ("selectChara", 1);
			ChackValue ();
			//SetCharaInfo (id);
		}
	}

	void StartChutorialShop(){
		for (int i = 0; i < buyBtns.Length; i++) {
			buyBtns [i].isEnabled = false;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void ChackValue(){

		specialP = PlayerPrefs.GetInt ("Special", 0);

		HPtext.text = ""+hp;
		Detext.text = ""+defenece;
		Attext.text = ""+attackPower;

		switch (specialP) {
		case 0:
			SPtext.text = "なし";
			break;
		case 1:
			SPtext.text = "HP3倍";
			break;
		case 2:
			SPtext.text = "攻撃3倍";
			break;
		case 3:
			SPtext.text = "防御3倍";
			break;
		case 4:
			SPtext.text = "復活付与";
			break;
		case 5:
			SPtext.text = "回復2倍";
			break;
		case 6:
			SPtext.text = "背水の陣";
			break;
		case 7:
			SPtext.text = "倍率2倍";
			break;
		case 8:
			SPtext.text = "金貨2倍";
			break;
		default:
			SPtext.text = "なし";
			break;
		}

		havemedallabel.text = ""+havemedal;
		for (int i = 0; i < buyBtns.Length; i++) {
			string serchindex = "Value" + i;
			//Debug.Log (serchindex);
			int value = int.Parse( GameObject.Find (serchindex).GetComponent<UILabel> ().text );
			if (value > havemedal) {
				buyBtns [i].isEnabled = false;
			} else {
				buyBtns [i].isEnabled = true;
				if (i > 2) {
					if(specialP != 0){
						buyBtns [i].isEnabled = false;
					}
				}
			}
		}
	}

	void SetCharaInfo(int charaid){
	}

	public void PushCharaUnlock(){

	}

	public void PushBuyItem(){
		Singleton<SoundPlayer>.instance.playSE( "item");
		string id = UIButton.current.name;
		int idint = int.Parse(id);
		if (idint == 0) {
			hp += 100;
			PlayerPrefs.SetInt ("P_health", hp);
		} else if (idint == 1) {
			attackPower += 1;
			hp += 50;
			PlayerPrefs.SetInt ("P_Attack", attackPower);
			PlayerPrefs.SetInt ("P_health", hp);
		} else if (idint == 2) {
			defenece += 1;
			hp += 50;
			PlayerPrefs.SetInt ("P_Defence", defenece);
			PlayerPrefs.SetInt ("P_health", hp);
		} else if (idint == 3) {
			specialP = 1;
			PlayerPrefs.SetInt ("Special", specialP);
		} else if (idint == 4) {
			specialP = 2;
			PlayerPrefs.SetInt ("Special", specialP);
		} else if (idint == 5) {
			specialP = 3;
			PlayerPrefs.SetInt ("Special", specialP);
		} else if (idint == 6) {
			specialP = 4;
			PlayerPrefs.SetInt ("Special", specialP);
		} else if (idint == 7) {
			specialP = 5;
			PlayerPrefs.SetInt ("Special", specialP);
		} else if (idint == 8) {
			specialP = 6;
			PlayerPrefs.SetInt ("Special", specialP);
		} else if (idint == 9) {
			specialP = 7;
			PlayerPrefs.SetInt ("Special", specialP);
		} else if (idint == 10) {
			specialP = 8;
			PlayerPrefs.SetInt ("Special", specialP);
		}

		string target = "Value" + idint;

		int value = int.Parse (GameObject.Find (target).GetComponent<UILabel> ().text);

		havemedal -= value;

		PlayerPrefs.SetInt ("MEDALS", havemedal);

		ChackValue ();
	}

	public void PushBuck(){
		Singleton<SoundPlayer>.instance.playSE( "click");
		PlayerPrefs.SetInt ("ChutorialEnd_LV", 1);
		Application.LoadLevel("CharaSelect");
	}
}
