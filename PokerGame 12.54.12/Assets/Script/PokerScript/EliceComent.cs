using UnityEngine;
using System.Collections;

/// <summary>
/// Elice coment.
/// </summary>
public class EliceComent : MonoBehaviour {

	[SerializeField, MultilineAttribute (5)]
	public string coment;

	[SerializeField, MultilineAttribute (2)]
	public string l1hello;
	[SerializeField, MultilineAttribute (2)]
	public string l2hello;
	[SerializeField, MultilineAttribute (2)]
	public string l3hello;

	[SerializeField, MultilineAttribute (2)]
	public string l1cardsend;
	[SerializeField, MultilineAttribute (2)]
	public string l2cardsend;
	[SerializeField, MultilineAttribute (2)]
	public string l3cardsend;

	[SerializeField, MultilineAttribute (2)]
	public string l1heal;
	[SerializeField, MultilineAttribute (2)]
	public string l2heal;
	[SerializeField, MultilineAttribute (2)]
	public string l3heal;

	[SerializeField, MultilineAttribute (2)]
	public string l1Attack;
	[SerializeField, MultilineAttribute (2)]
	public string l2Attack;
	[SerializeField, MultilineAttribute (2)]
	public string l3Attack;

	[SerializeField, MultilineAttribute (2)]
	public string l1call;
	[SerializeField, MultilineAttribute (2)]
	public string l2call;
	[SerializeField, MultilineAttribute (2)]
	public string l3call;

	[SerializeField, MultilineAttribute (2)]
	public string l1raise;
	[SerializeField, MultilineAttribute (2)]
	public string l2raise;
	[SerializeField, MultilineAttribute (2)]
	public string l3raise;

	[SerializeField, MultilineAttribute (2)]
	public string l1drop;
	[SerializeField, MultilineAttribute (2)]
	public string l2drop;
	[SerializeField, MultilineAttribute (2)]
	public string l3drop;

	[SerializeField, MultilineAttribute (2)]
	public string l1gift1;
	[SerializeField, MultilineAttribute (2)]
	public string l2gift1;
	[SerializeField, MultilineAttribute (2)]
	public string l3gift1;

	[SerializeField, MultilineAttribute (2)]
	public string l1gift2;
	[SerializeField, MultilineAttribute (2)]
	public string l2gift2;
	[SerializeField, MultilineAttribute (2)]
	public string l3gift2;

	[SerializeField, MultilineAttribute (2)]
	public string l1gift3;
	[SerializeField, MultilineAttribute (2)]
	public string l2gift3;
	[SerializeField, MultilineAttribute (2)]
	public string l3gift3;

	//Judge judge;

	SpriteRenderer MainSpriteRenderer;
	public Sprite[] Faces;

	void Start(){
		//judge = GetComponent<Judge>();
		MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		//MainSpriteRenderer.sprite = Faces[12];
	}

	/*public string pokarfacecoment(int lp ,int phand){
		if (lp < 25) {
			if(phand<3){
				coment = "これはダメね";
				FaceChenge(2);
			}else{
				coment = "いまいちー";
				FaceChenge(2);
			}

		} else if (25 <= lp && lp < 50) {
			if(phand<3){
				coment = "いけるわ";
				FaceChenge(2);
			}else{
				coment = "ちょっと…";
				FaceChenge(2);
			}
		} else if (50 <= lp && lp < 75) {
			if(phand<3){
				coment = "全然ダメ";
				FaceChenge(2);
			}else{
				coment = "よさげね";
				FaceChenge(2);
			}
		} else if (75 <= lp && lp < 100) {
			if(phand<3){
				coment = "ちょっとキツイなー";
				FaceChenge(2);
			}else{
				coment = "これはいける♪";
				FaceChenge(4);
			}
		} else {
			if(phand<3){
				coment = "もう！ダメダメじゃない！！";
				FaceChenge(9);
			}else{
				coment = "これは勝ったわ！！";
				FaceChenge(4);
			}
		}
		return coment;
	}

	public string dropcoment(int lp){
		if (lp < 25) {
			coment = "これはダメね";
			FaceChenge(2);
		} else if (25 <= lp && lp < 50) {
			coment = "いまいちかなぁー";
			FaceChenge(2);
		} else if (50 <= lp && lp < 75) {
			coment = "いいカードが来ないよぉ…";
			FaceChenge(13);
		} else if (75 <= lp && lp < 100) {
			coment = "つ、次は負けないんだから！！";
			FaceChenge(0);
		} else {
			coment = "おりてあげるね。";
			FaceChenge(4);
		}
		return coment;
	}

	public string callcoment(int lp){
		if (lp < 25) {
			coment = "コールよ";
			FaceChenge(2);
		} else if (25 <= lp && lp < 50) {
			coment = "おりないわよ？";
			FaceChenge(2);
		} else if (50 <= lp && lp < 75) {
			coment = "勝負よ！";
			FaceChenge(11);
		} else if (75 <= lp && lp < 100) {
			coment = "コール♪";
			FaceChenge(4);
		} else {
			coment = "勝つもん！こーる！";
			FaceChenge(3);
		}
		return coment;
	}

	public string raisecoment(int lp){
		if (lp < 25) {
			coment = "レイズ";
			FaceChenge(3);
		} else if (25 <= lp && lp < 50) {
			coment = "勝負よ、レイズ";
			FaceChenge(2);
		} else if (50 <= lp && lp < 75) {
			coment = "悪いけど勝たせてもらうわね";
			FaceChenge(3);
		} else if (75 <= lp && lp < 100) {
			coment = "レーイズ♪";
			FaceChenge(4);
		} else {
			coment = "これで一発逆転なんだから！！";
			FaceChenge(9);
		}
		return coment;
	}

	public string raisecallcoment(int lp){
		if (lp < 25) {
			coment = "乗るわ、コール";
			FaceChenge(2);
		} else if (25 <= lp && lp < 50) {
			coment = "いいのかしら？コール";
			FaceChenge(6);
		} else if (50 <= lp && lp < 75) {
			coment = "いいの？コールでいくね";
			FaceChenge(3);
		} else if (75 <= lp && lp < 100) {
			coment = "楽しくなってきたわ、コール♪";
			FaceChenge(4);
		} else {
			coment = "やるじゃない！コールよ♪";
			FaceChenge(4);
		}
		return coment;
	}

	public string raisedropcoment(int lp){
		if (lp < 25) {
			coment = "おりるわ";
			FaceChenge(10);
		} else if (25 <= lp && lp < 50) {
			coment = "だめね、おりる";
			FaceChenge(2);
		} else if (50 <= lp && lp < 75) {
			coment = "ここはおりとくのがベストなの！！";
			FaceChenge(7);
		} else if (75 <= lp && lp < 100) {
			coment = "自信満々じゃない…おりるわ";
			FaceChenge(7);
		} else {
			coment = "うーん…おりるね";
			FaceChenge(9);
		}
		return coment;
	}

	//P_RAISE -> CPU_CALL
	public string PlayerNotParentCallcoment(int lp){
		if (lp < 25) {
			coment = "コールよ";
			FaceChenge(11);
		} else if (25 <= lp && lp < 50) {
			coment = "乗るわ、その勝負";
			FaceChenge(6);
		} else if (50 <= lp && lp < 75) {
			coment = "おりると思って？コール";
			FaceChenge(3);
		} else if (75 <= lp && lp < 100) {
			coment = "自信満々だね…でも、コール♪";
			FaceChenge(4);
		} else {
			coment = "～♪。コールで！";
			FaceChenge(4);
		}
		return coment;
	}

	public string PlayerDropcoment(int lp){
		if (lp < 25) {
			coment = "いい判断ね";
			FaceChenge(2);
		} else if (25 <= lp && lp < 50) {
			coment = "らっきーらっきー。";
			FaceChenge(1);
		} else if (50 <= lp && lp < 75) {
			coment = "よかっ…な、なんでもないわ";
			FaceChenge(15);
		} else if (75 <= lp && lp < 100) {
			coment = "おりちゃうんだ…？";
			FaceChenge(13);
		} else {
			coment = "えへへ…おりてくれるんだ？";
			FaceChenge(4);
		}
		return coment;
	}

	public string Continwecoment(int lp){
		if (lp < 25) {
			coment = "続けるのね。";
			FaceChenge(10);
		} else if (25 <= lp && lp < 50) {
			coment = "はい、次、次。";
			FaceChenge(3);
		} else if (50 <= lp && lp < 75) {
			coment = "次も楽しもうね。";
			FaceChenge(4);
		} else if (75 <= lp && lp < 100) {
			coment = "もっとしよ？";
			FaceChenge(4);
		} else {
			coment = "えへへ…楽しいね。";
			FaceChenge(4);
		}
		return coment;
	}

	public string Wincoment(int lp){
		if (lp < 25) {
			coment = "私の勝ちね";
			FaceChenge(11);
		} else if (25 <= lp && lp < 50) {
			coment = "負けてくれたのかしら？";
			FaceChenge(1);
		} else if (50 <= lp && lp < 75) {
			coment = "勝っちゃったー";
			FaceChenge(1);
		} else if (75 <= lp && lp < 100) {
			coment = "えへへ、強いでしょ？";
			FaceChenge(6);
		} else {
			coment = "やったー勝ったー♪";
			FaceChenge(4);
		}
		return coment;
	}
	public string Losecoment(int lp){
		if (lp < 25) {
			coment = "まぐれよ";
			FaceChenge(10);
		} else if (25 <= lp && lp < 50) {
			coment = "次よ次！";
			FaceChenge(8);
		} else if (50 <= lp && lp < 75) {
			coment = "負けてくれてもいいんだよ？";
			FaceChenge(15);
		} else if (75 <= lp && lp < 100) {
			coment = "つよいね♪次いこ次！";
			FaceChenge(4);
		} else {
			coment = "次は負けないんだからね！！";
			FaceChenge(9);
		}
		return coment;
	}
	public string Drawcoment(int lp){
		if (lp < 25) {
			coment = "引き分けね。";
			FaceChenge(11);
		} else if (25 <= lp && lp < 50) {
			coment = "引き分けかぁ";
		} else if (50 <= lp && lp < 75) {
			coment = "次は勝つよ？";
			FaceChenge(6);
		} else if (75 <= lp && lp < 100) {
			coment = "くやしいー次いこ次！";
			FaceChenge(15);
		} else {
			coment = "えへへ…引き分けかー";
			FaceChenge(4);
		}
		return coment;
	}*/

	public void FaceChenge(int index){
		MainSpriteRenderer.sprite = Faces[index];
	}

}
