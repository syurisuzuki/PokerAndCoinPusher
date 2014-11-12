using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

		enployeew en;

		public GameObject ennnnn;
		public GameObject taxxxx;

		public int encount;

		[SerializeField] int basemoney;

	// Use this for initialization
	void Start () {

				Instantiate (taxxxx, new Vector3 (0, 0, 0), Quaternion.identity);

				for(int i = 0;i<encount;i++){
						GameObject ee = (GameObject)Instantiate (ennnnn, new Vector3 (0, 0, 0), Quaternion.identity);

				}

	}

}
