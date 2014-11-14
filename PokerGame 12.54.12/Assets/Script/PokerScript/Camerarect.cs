using UnityEngine;
using System.Collections;

/// <summary>
/// カメラのアスペクト比を固定.
/// </summary>
public class Camerarect : MonoBehaviour {

	#region(inspector settings)
	public int fixWidth = 1280;
	public int fixHeight = 720;
	public bool portrait = false;
	public Camera[] fixedCamera;
	#endregion

	public static float resolutionScale = -1.0f;

	void Awake() {
		int fw = portrait ? this.fixHeight : this.fixWidth;
		int fh = portrait ? this.fixWidth : this.fixHeight;

		// camera
		if( this.fixedCamera != null ){
			Rect set_rect = this.calc_aspect(fw, fh, out resolutionScale);
			foreach( Camera cam in this.fixedCamera ){
				cam.rect = set_rect;
			}
		}

		// MEMO:NGUIのmanualHeight設定は不要、
		// UI Root下のカメラのアスペクト比固定すればよい
		// UI RootのAutomaticはOFF, Manual Heightは想定heightを設定する

		// アスペクト比を設定のみなので、設定後は削除
		this.Destroy(this);
	}

	// アスペクト比 固定するようにcameraのrect取得
	Rect calc_aspect(float width, float height, out float res_scale) {
		float target_aspect = width / height;
		float window_aspect = (float)Screen.width / (float)Screen.height;
		float scale = window_aspect / target_aspect;

		Rect rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
		if( 1.0f > scale ){
			rect.x = 0;
			rect.width = 1.0f;
			rect.y = (1.0f - scale) / 2.0f;
			rect.height = scale;
			res_scale = (float)Screen.width / width;
		} else {
			scale = 1.0f / scale;
			rect.x = (1.0f - scale) / 2.0f;
			rect.width = scale;
			rect.y = 0.0f;
			rect.height = 1.0f;
			res_scale = (float)Screen.height / height;
		}

		return rect;
	}
}