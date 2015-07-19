using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {

	[SerializeField, MultilineAttribute (5)]
	public string[] Storys;

	int key;

	public void SkipChutorial(){
		Application.LoadLevel ("CharaSelect");
	}
}
