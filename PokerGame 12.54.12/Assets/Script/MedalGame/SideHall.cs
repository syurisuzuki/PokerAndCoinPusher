using UnityEngine;
using System.Collections;

public class SideHall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		//void OnTriggerEnter2D (Collider2D c)
		void OnCollisionEnter(Collision col){
				if (col.gameObject.tag == "Coin"||col.gameObject.tag == "Artifact") {
						Destroy (col.gameObject);
				}
		}
}