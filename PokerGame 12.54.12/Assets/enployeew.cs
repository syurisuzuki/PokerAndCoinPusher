using UnityEngine;
using System.Collections;

public class enployeew : MonoBehaviour {

		Taxmann t;

		int getMoney;
		int totalGetMoney;

		int getTax;
		int totalGetTax;

		public void GetMoney(int gm){
				t = FindObjectOfType<Taxmann>();
				int taxedmoney = t.sendMoney (gm);
				getMoney = gm - taxedmoney;
				totalGetMoney += getMoney;
				Debug.Log ("今月の税金:" + getTax + "税金の集計:" + totalGetTax);
				Debug.Log ("今月の給料"+getMoney+"給料の総額"+totalGetMoney);
		}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
