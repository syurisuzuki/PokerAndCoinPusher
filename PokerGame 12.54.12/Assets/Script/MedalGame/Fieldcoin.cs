using UnityEngine;
using System.Collections;

public class Fieldcoin : MonoBehaviour {

		public GameObject Save;


	// Use this for initialization
	void Start () {
				Save = GameObject.Find ("VectorSave");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public void Vectorsavetest(){
				foreach(Transform child in gameObject.transform){
						Vector3 coinvec = child.transform.position;
						//Vector3 vec3 = child.transform.rotation;
						//Quaternion qua = new Quaternion();
						//qua.SetLookRotation( vec3 );
						//qua.SetFromToRotation( vec3 );
						Save.SendMessage ("VectorSaveList", coinvec);
						Save.SendMessage ("RotateSaveList", child.rotation);
				}
				Application.LoadLevel (0);
		}

}
