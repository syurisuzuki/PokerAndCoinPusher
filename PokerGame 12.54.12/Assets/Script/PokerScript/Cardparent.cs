using UnityEngine;
using System.Collections;

/// <summary>
/// カードの親.
/// </summary>
public class Cardparent : MonoBehaviour {

	public Card card;

	// Use this for initialization
	void Start () {
		card = GameObject.FindObjectOfType<Card>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// 子要素のオブジェクトを全て削除する.
	/// </summary>
	public void removeAllciledrenCard(){
		foreach(Transform child in transform) {
			GameObject target = child.gameObject;
			Destroy (target);
		}
	}
}
