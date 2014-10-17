using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public void PushPoker(){
				Application.LoadLevel (1);
		}

		public void PushCoinPusher(){
				Application.LoadLevel (2);
		}

}
