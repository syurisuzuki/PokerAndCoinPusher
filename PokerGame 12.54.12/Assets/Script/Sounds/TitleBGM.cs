using UnityEngine;
using System.Collections;

public class TitleBGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Singleton<SoundPlayer>.instance.playBGM( "Title",1f );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
