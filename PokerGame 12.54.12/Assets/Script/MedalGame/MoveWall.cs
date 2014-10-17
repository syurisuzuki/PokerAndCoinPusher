using UnityEngine;
using System.Collections;

public class MoveWall : MonoBehaviour {
		//public float speed = 3;
		public Vector3 origin;
		public float startTime;
	// Use this for initialization
	void Start () {
				origin = transform.position;
				startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
				Vector3 vec = new Vector3(0, 0, Mathf.Sin(Time.time-startTime + Mathf.Deg2Rad*90 ));
				rigidbody.MovePosition(origin + vec );
	}
}
