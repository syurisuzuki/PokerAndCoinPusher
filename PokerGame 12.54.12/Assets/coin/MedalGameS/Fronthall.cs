using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fronthall : MonoBehaviour {

		public GameObject mscript;
		public GameObject Artscript;
		public int coinfall;
		public int sidewall;
		public float walltime = 20;

		public GameObject LeftWall;
		public GameObject RightWall;
		public bool sidewallismoved;

		public Text walltimetext;

	// Use this for initialization
	void Start () {
				walltimetext.text = "";
				coinfall = 0;
				sidewall = 0;
				Artscript = GameObject.Find ("Artifact");
				mscript = GameObject.Find ("UIman");
	}
	
	// Update is called once per frame
	void Update () {
				if(sidewallismoved == true){
						walltime -= Time.deltaTime;
						walltimetext.text = "WallTime:"+walltime;
						if (walltime <= 0.0f)
						{
								walltime = 0.0f;
								WallBack ();
						}
				}

				if(coinfall>=30){
						Artscript.SendMessage("Artifactappear");
						coinfall = 0;
				}

				if(sidewall>=20){
						SideWall ();
						sidewall = 0;
				}
	
	}
		void OnCollisionEnter(Collision col){
				if (col.gameObject.tag == "Coin") {
						Destroy (col.gameObject);
						mscript.SendMessage("MoneyChenge", 1);
						coinfall++;
						sidewall++;
				}else if(col.gameObject.tag == "Artifact"){
						Destroy (col.gameObject);
						mscript.SendMessage("MoneyChenge", 50);
				}
		}

		public void SideWall(){
				if(sidewallismoved == true){
						walltime += 20f;
				}else{
						LeftWall.transform.Translate (0,6.5f,0);
						RightWall.transform.Translate (0, -8.9f, 0);
						sidewallismoved = true;
				}
		}

		public void WallBack(){
				walltimetext.text = "";
				LeftWall.transform.Translate (0,-6.5f,0);
				RightWall.transform.Translate (0, 8.9f, 0);
				sidewallismoved = false;
		}

}
