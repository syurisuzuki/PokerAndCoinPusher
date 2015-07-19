using UnityEngine;
using System.Collections;

public class StoryCreater : MonoBehaviour {

	Story story;
	//public UILabel storyLabel;
	public TypewriterEffect te;
	bool next;
	public UIGrid grid;

	string storytext;
	int nowpage;
	int maxpage;

	public GameObject labelobj;
	public GameObject parentobj;
	public GameObject rirekiwindowRoot;
	public GameObject touchText;

	public bool isRirekiWindowOpen;

	// Use this for initialization
	void Start () {
		Singleton<SoundPlayer>.instance.playBGM( "Story",0 );
		nowpage = 0;
		story = GetComponent<Story> ();

		maxpage = story.Storys.Length;
		isRirekiWindowOpen = false;
		StoryReader ();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if (next == true && isRirekiWindowOpen == false) {
				StoryReader ();
				Debug.Log ("NEXT!!");
			} else {
				Debug.Log ("Touch");
			}
		}
	}

	void StoryReader(){
		next = false;
		touchText.SetActive (false);
		storytext = "";

		if (GameObject.Find ("Label(Clone)") != null) {
			UILabel ula = GameObject.Find ("Label(Clone)").GetComponent<UILabel> ();
			ula.GetComponent<TypewriterEffect> ().enabled = false;
			ula.transform.parent = parentobj.transform;

//NGUITools.AddChild(parentobj, ula);
			grid.Reposition();
		};

		var parent = GameObject.Find ("LParent");
		if (nowpage >= maxpage) {
			/*Debug.Log ("BUCK!!");
			nowpage = 0;
			next = true;*/
			Application.LoadLevel ("CharaSelect");
			return;
		}

		NGUITools.AddChild(parent, labelobj);
		UILabel ul = GameObject.Find ("Label(Clone)").GetComponent<UILabel> ();
		ul.gameObject.SetActive (false);
		storytext = story.Storys [nowpage];
		ul.text = storytext;

		nowpage++;

		TypewriterEffect te = ul.GetComponent<TypewriterEffect> ();

		EventDelegate.Set(te.onFinished, onFinishTweenAlpha);
		StartCoroutine ("DelayMethod", ul);
	}


	private IEnumerator DelayMethod(UILabel ul)
	{
		yield return new WaitForSeconds(1);
		ul.gameObject.SetActive (true);
	}

	void DelayTextAppear(UILabel ul){
		ul.gameObject.SetActive (true);

	}

	public void onFinishTweenAlpha(){
		Debug.Log ("Finish!!");
		touchText.SetActive (true);
		next = true;
	}

	public void Rirekiwindow(){
		if (isRirekiWindowOpen == true) {
			rirekiwindowRoot.SetActive (false);
			isRirekiWindowOpen = false;
		} else {
			rirekiwindowRoot.SetActive (true);
			isRirekiWindowOpen = true;
		}
	}
}
