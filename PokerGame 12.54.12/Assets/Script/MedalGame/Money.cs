using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Money : MonoBehaviour {

		public float savetime = 5;
		public int money;
		public Text moneytext;
		public GameObject Coin;

		public GameObject Fieldcoin;

		private Vector3 clickPosition;

	// Use this for initialization
	void Start () {

				money = PlayerPrefs.GetInt ("MONEY", 100);
				moneytext.text = "Money:" + money;
	}

	// Update is called once per frame
	void Update () {
				if (Input.GetMouseButtonDown(0)) {
						Coinshot ();
				}
	}

		void Awake(){
				InvokeRepeating("Savemoney", savetime, savetime);
		}

		public void MoneyChenge(int m){
				money += m;
				moneytext.text = "Money:" + money;
		}

		void Savemoney(){
				if(money<100){
						MoneyChenge (1);
				}
				PlayerPrefs.SetInt ("MONEY", money);
		}
		public void Coinshot(){
				if(money>0){
						float x = Random.Range (-2,2.1f);
						float y = Random.Range (4,6);
						float z = Random.Range (7.4f,8);
						clickPosition = new Vector3 (x, y, z);
						GameObject coins = (GameObject)Instantiate(Coin, clickPosition, Quaternion.identity);
						DontDestroyOnLoad(coins);
						coins.transform.parent = Fieldcoin.transform;
						MoneyChenge (-1);
				}
		}

		public void LoadScene(){
				Fieldcoin.SendMessage ("Vectorsavetest");
				Application.LoadLevel (1);
		}

}
