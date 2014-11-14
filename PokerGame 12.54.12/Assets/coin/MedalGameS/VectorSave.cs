using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VectorSave : MonoBehaviour {

	
		static public List<Vector3> Coinvec = new List<Vector3> ();
		static public List<Quaternion> CoinRo = new List<Quaternion> ();
		public GameObject Coin;
		public GameObject Fcoin;


	// Use this for initialization
	void Start () {
				for(int i = 0;i<Coinvec.Count;i++){
						Vector3 vec = Coinvec [i];
						Quaternion qu = CoinRo [i];

						GameObject coin = (GameObject)Instantiate (Coin, vec,qu);
						coin.transform.parent = Fcoin.transform;
				}
				Coinvec.Clear ();
				CoinRo.Clear ();
	}

		/*void Awake() {
				DontDestroyOnLoad(this);
		}*/
	
	// Update is called once per frame
	void Update () {
	
	}

		public void VectorSaveList(Vector3 vec){
				Coinvec.Add(vec);
		}

		public void RotateSaveList(Quaternion ro){
				CoinRo.Add (ro);
		}
}
