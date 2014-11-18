using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// ポーカーゲームシーンへの移動.
	/// </summary>
	public void PushPoker(){
		Application.LoadLevel (1);
	}
	//実機動作不安定のため一時保留
	/// <summary>
	/// コインプッシャーシーンへ移動.
	/// </summary>
	public void PushCoinPusher(){
		Application.LoadLevel (2);
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
		Application.LoadLevel (3);
	}

}
