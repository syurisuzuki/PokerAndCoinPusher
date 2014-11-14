using UnityEngine;
using System.Collections;
/// <summary>
/// BGMを流すクラス.
/// </summary>
public class BGM : MonoBehaviour {

	public AudioClip audioClip;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
			audioSource = gameObject.GetComponent<AudioSource>();
			audioSource.clip = audioClip;
			audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
