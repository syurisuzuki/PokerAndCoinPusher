using UnityEngine;
using System.Collections;

public class InputMan : MonoBehaviour {

	public GameObject hidePanel;

	public void RestBtnPush(){
		hidePanel.SetActive (true);
	}

	public void LpRest(){
		PlayerPrefs.SetInt("LP",0);
		hidePanel.SetActive (false);
	}

	public void Cancel(){
		hidePanel.SetActive (false);
	}

}
