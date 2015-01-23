using UnityEngine;
using System.Collections;

/// <summary>
/// カードの情報.
/// </summary>
public class CardInfo : MonoBehaviour {
	public int Mark;
	public int Number;

	public int Score;

	public int Skill_Num;

	public bool touched;

	public bool isKira = false;

	public Animator anim;
	SpriteRenderer MainSpriteRenderer;
	public Sprite realcard;
	public Sprite Cardback;

	public Card cardm;
	public GameObject Cdc;
// Use this for initialization
	void Start () {

		MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		realcard = gameObject.GetComponent<SpriteRenderer> ().sprite;
		cardm = FindObjectOfType<Card>();
		anim = GetComponent<Animator> ();
		touched = false;
		MainSpriteRenderer.sprite = Cardback;

	}

// Update is called once per frame
	void Update () {

	}

	public void TouchCard(){
		if(touched==false){
			this.transform.Translate (0,-0.3f,0);
			anim.SetBool ("Touched",true);
			touched = true;
		}else{
			this.transform.Translate (0,0.3f,0);
			anim.SetBool ("Touched",false);
			touched = false;
		}
	}

	public void TouchCardCpu(){
		if(touched==false){
			this.transform.Translate (0,0.3f,0);
			anim.SetBool ("Touched",true);
			touched = true;
		}else{
			this.transform.Translate (0,-0.3f,0);
			anim.SetBool ("Touched",false);
			touched = false;
		}
	}

	public void CardVectorSave(){
		if(touched==true){
			//Debug.Log ("残るカード");
		}else{
			Cdc = GameObject.Find ("Card_Manager");
			Vector3 cardvec = transform.position;
			Cdc.SendMessage ("ChengeChild", cardvec);
			//cardm.ChengeChild (cardvec);
			//Debug.Log ("消えるカード"+cardvec);
			Destroy (this.gameObject);
		}
	}

	public void CardAnimation(){
		MainSpriteRenderer.sprite = realcard;
		anim.SetBool("CardAnim",true);
	}

	public void StopParticleSystem(){
		particleSystem.Stop ();
	}

}
