using UnityEngine;
using System.Collections;

public class TouchMan : MonoBehaviour {

		public GameObject Player;
		/*public GameObject uiback;

		public GameObject Stackss;

		public GameObject chenge_S;
		public GameObject chenge_A;
		public GameObject chenge_M;
		public GameObject chenge_H;

		public GameObject PSui;
		public bool cantpuch;
		public bool animplay;
		public bool uinow;
		public bool psnow;*/

		public bool enemyturn;

		public enum Turn{
				ENEMY_TURN,
				PLAYER_TURN,
				CHENGE_TURN,
		}

		public Turn state;

				// Use this for initialization
		void Start () {
				state = Turn.CHENGE_TURN;

				}
				// Update is called once per frame
		void Update () {

				if(state==Turn.CHENGE_TURN){

						if (Input.GetMouseButtonDown(0)) {

								Vector3    aTapPoint   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
								Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint);

								if (aCollider2d) {

										GameObject obj = aCollider2d.transform.gameObject;
										//string tagname = aCollider2d.gameObject.tag;
										switch(obj.tag){
										case "PlayerCard":
												Debug.Log (aCollider2d);
												obj.SendMessage ("TouchCard");
												break;
										default:

												Debug.Log ("touch(click)!! objectname:"+obj.name);
												break;
										}
								}
						}
				}
		}

		void Hoge(){
				Player.SendMessage ("Boolre");
				//eneee.SendMessage ("Enemyda");
		}
		public void resettouch(){
				enemyturn = false;
		}

		public void Startbtnpush(){
				state = Turn.ENEMY_TURN;
		}

		public void Continue(){
				state = Turn.CHENGE_TURN;
		}

				
}
