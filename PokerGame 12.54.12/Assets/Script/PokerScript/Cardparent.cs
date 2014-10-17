using UnityEngine;
using System.Collections;

public class Cardparent : MonoBehaviour {

		public Card card;

	// Use this for initialization
	void Start () {
				card = GameObject.FindObjectOfType<Card>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



		public void RestartcardDel(){
				foreach(Transform child in transform) {
						GameObject target = child.gameObject;
						Destroy (target);
				}


		}

}
