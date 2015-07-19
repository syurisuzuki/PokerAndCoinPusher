using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll ();
		Singleton<SoundPlayer>.instance.playBGM( "Title",0 );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// ポーカーゲームシーンへの移動.
	/// </summary>
	public void PushPoker(){
		int chutorialendint = PlayerPrefs.GetInt ("ChutorialEnd", 0);
		if (chutorialendint == 0) {
			Application.LoadLevel ("chutorial");
		} else {
			Application.LoadLevel ("CharaSelect");
		}

	}


	/// <summary>
	/// タイトル画面へ戻る.
	/// </summary>
	public void PushBackTitle(){
		Application.LoadLevel (0);
	}

	/// <summary>
	/// 説明シーンへ移動.
	/// </summary>
	public void GoInfoScene(){
		Application.LoadLevel (2);
	}

}
