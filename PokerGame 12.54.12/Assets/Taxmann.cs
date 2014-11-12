using UnityEngine;
using System.Collections;

public class Taxmann : MonoBehaviour {

		float taxMag = 0.1f;
		public int sendMoney(int paydMoney){
				return Mathf.CeilToInt(paydMoney*taxMag);
		}
}
