using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaSelect : MonoBehaviour {

	public UIButton[] charaGame;
	public UIButton[] charaShop;

	public UILabel chutorialtext;
	public GameObject chutorialtextRoot;

	[SerializeField, MultilineAttribute (3)]
	public string[] helpTexts;

	public UISprite c1H1;
	public UISprite c1H2;
	public UISprite c1H3;
	public UISprite c2H1;
	public UISprite c2H2;
	public UISprite c2H3;
	public UISprite c3H1;
	public UISprite c3H2;
	public UISprite c3H3;
	public UISprite c4H1;
	public UISprite c4H2;
	public UISprite c4H3;
	public UISprite c5H1;
	public UISprite c5H2;
	public UISprite c5H3;
	public UISprite c6H1;
	public UISprite c6H2;
	public UISprite c6H3;
	public UISprite c7H1;
	public UISprite c7H2;
	public UISprite c7H3;
	public UISprite c8H1;
	public UISprite c8H2;
	public UISprite c8H3;
	public UISprite c9H1;
	public UISprite c9H2;
	public UISprite c9H3;
	public UISprite c10H1;
	public UISprite c10H2;
	public UISprite c10H3;
	public UISprite c11H1;
	public UISprite c11H2;
	public UISprite c11H3;
	public UISprite c12H1;
	public UISprite c12H2;
	public UISprite c12H3;
	Dictionary<string, UISprite> data = new Dictionary<string, UISprite>();

	int kokand1;
	int kokand2;
	int kokand3;
	int kokand4;
	int kokand5;
	int kokand6;
	int kokand7;
	int kokand8;
	int kokand9;
	int kokand10;
	int kokand11;
	int kokand12;

	int Gending;
	int Nending;

	// Use this for initialization
	void Start () {

		//PlayerPrefs.SetInt ("MEDALS", 1000000000);

		Singleton<SoundPlayer>.instance.playBGM( "Select",0 );

		data.Add ("1l1", c1H1);
		data.Add ("1l2", c1H2);
		data.Add ("1l3", c1H3);
		data.Add ("2l1", c2H1);
		data.Add ("2l2", c2H2);
		data.Add ("2l3", c2H3);
		data.Add ("3l1", c3H1);
		data.Add ("3l2", c3H2);
		data.Add ("3l3", c3H3);
		data.Add ("4l1", c4H1);
		data.Add ("4l2", c4H2);
		data.Add ("4l3", c4H3);
		data.Add ("5l1", c5H1);
		data.Add ("5l2", c5H2);
		data.Add ("5l3", c5H3);
		data.Add ("6l1", c6H1);
		data.Add ("6l2", c6H2);
		data.Add ("6l3", c6H3);
		data.Add ("7l1", c7H1);
		data.Add ("7l2", c7H2);
		data.Add ("7l3", c7H3);
		data.Add ("8l1", c8H1);
		data.Add ("8l2", c8H2);
		data.Add ("8l3", c8H3);
		data.Add ("9l1", c9H1);
		data.Add ("9l2", c9H2);
		data.Add ("9l3", c9H3);
		data.Add ("10l1", c10H1);
		data.Add ("10l2", c10H2);
		data.Add ("10l3", c10H3);
		data.Add ("11l1", c11H1);
		data.Add ("11l2", c11H2);
		data.Add ("11l3", c11H3);
		data.Add ("12l1", c12H1);
		data.Add ("12l2", c12H2);
		data.Add ("12l3", c12H3);

		int chutorialendint = PlayerPrefs.GetInt ("ChutorialEnd", 0);
		if (chutorialendint == 0) {
			StartChutorial ();
		} else {
			CheckChara ();
		}
	}

	void StartChutorial(){

		string serchkey = "Actor1Unlock";
		Debug.Log (serchkey);
		int isunlock = PlayerPrefs.GetInt (serchkey, 0);
		if (isunlock == 1) {
			for (int i = 0; i < charaGame.Length; i++) {
				charaGame [i].isEnabled = false;
				charaShop [i].isEnabled = false;
			}
			charaGame [0].isEnabled = true;
			charaShop [0].isEnabled = true;
			chutorialtextRoot.SetActive (true);
			chutorialtext.text = "さぁ、エメラルドと\nポーカーをしましょう。";
		} else {
			for (int i = 0; i < charaGame.Length; i++) {
				charaGame [i].isEnabled = false;
				charaShop [i].isEnabled = false;
			}
			charaShop [0].isEnabled = true;
			StartCoroutine ("ChutorialCoroutine");
		}
	}

	IEnumerator ChutorialCoroutine(){
		chutorialtextRoot.SetActive (true);
		yield break;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CheckChara(){
		string serchkey = "";
		int charaid;
		int flag;
		Gending = 0;
		Nending = 0;
		for (int i = 0; i < charaGame.Length; i++) {
			charaid = i + 1;
			serchkey = "Actor" + charaid + "Unlock";
			flag = PlayerPrefs.GetInt (serchkey, 0);
			if (flag == 1) {
				charaGame [i].isEnabled = true;
			} else {
				charaGame [i].isEnabled = false;
			}

		}

		for (int i = 1; i < 13; i++) {
			string getkokandky = "Actor" + i + "LovePoint";
			int lp = PlayerPrefs.GetInt (getkokandky,0);
			if (lp > 200) {
				string charalevel3 = "Actor"+i+"Level3";
				int f = PlayerPrefs.GetInt (charalevel3, 0);
				if (f != 0) {
					var key = i+"l3";
					UISprite sp = data[key];
					sp.gameObject.SetActive (true);
					Gending++;
				}
				string charalevel2 = "Actor"+i+"Level2";
				f = PlayerPrefs.GetInt (charalevel2, 0);
				if (f != 0) {
					var key = i+"l2";
					UISprite sp = data[key];
					sp.gameObject.SetActive (true);
					Nending++;
				}
				string charalevel1 = "Actor"+i+"Level1";
				f = PlayerPrefs.GetInt (charalevel1, 0);
				if (f != 0) {
					var key = i+"l1";
					UISprite sp = data[key];
					sp.gameObject.SetActive (true);
				}
			}else if(lp > 100){
				string charalevel2 = "Actor"+i+"Level2";
				int f = PlayerPrefs.GetInt (charalevel2, 0);
				if (f != 0) {
					var key = i+"l2";
					UISprite sp = data[key];
					sp.gameObject.SetActive (true);
					Nending++;
				}
				string charalevel1 = "Actor"+i+"Level1";
				f = PlayerPrefs.GetInt (charalevel1, 0);
				if (f != 0) {
					var key = i+"l1";
					UISprite sp = data[key];
					sp.gameObject.SetActive (true);
				}
			}else if(lp > 0){
				string charalevel1 = "Actor"+i+"Level1";
				int f = PlayerPrefs.GetInt (charalevel1, 0);
				if (f != 0) {
					var key = i+"l1";
					UISprite sp = data[key];
					sp.gameObject.SetActive (true);
					Nending++;
				}
			}
		}
	}

	public void MoveShopScene(){
		Singleton<SoundPlayer>.instance.playSE( "click" );
		string id = UIButton.current.name;
		int idint = int.Parse(id);
		PlayerPrefs.SetInt ("selectChara", idint);

		Application.LoadLevel("Shop");
	}

	public void MoveGameScene(){
		Singleton<SoundPlayer>.instance.playSE( "click" );
		PlayerPrefs.SetInt ("ChutorialEnd", 1);
		string id = UIButton.current.name;
		int idint = int.Parse(id);
		PlayerPrefs.SetInt ("selectChara", idint);


		Application.LoadLevel("Game");
	}

	public void MoveLvUpShop(){
		Singleton<SoundPlayer>.instance.playSE( "click" );
		Application.LoadLevel("EauipShop");
	}

	public void GoEnding(){
		//Debug.Log (Gending + "ll" + Nending);
		if (Gending == 12) {
			Application.LoadLevel ("GoodEnding");
		} else {
			if (Nending == 12) {
				Application.LoadLevel ("NormalEnding");
			} else {
				Application.LoadLevel("BadEnding");
			}
		}
	}


}
