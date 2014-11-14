using UnityEngine;
using System.Collections;

public class Artifact : MonoBehaviour {
		public GameObject artifact1;
		public GameObject artifact2;
		public GameObject artifact3;
		public GameObject artifact4;
		public GameObject artifact5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		//x-2 2 y3 z2 5
		public void Artifactappear(){
				int a = Random.Range (0, 5);
				int x = Random.Range (-2,2);
				int z = Random.Range (2, 5);
				switch (a) {
				case 0:
						Instantiate (artifact1, new Vector3 (x, 3, z), Quaternion.identity);
						break;
				case 1:
						Instantiate (artifact2, new Vector3 (x, 3, z), Quaternion.identity);
						break;
				case 2:
						Instantiate (artifact3, new Vector3 (x, 3, z), Quaternion.identity);
						break;
				case 3:
						Instantiate (artifact4, new Vector3 (x, 3, z), Quaternion.identity);
						break;
				case 4:
						Instantiate (artifact5, new Vector3 (x, 3, z), Quaternion.identity);
						break;
				default:
						break;
				}
		}

}
