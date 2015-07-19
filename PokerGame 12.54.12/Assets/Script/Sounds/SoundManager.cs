using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// サウンド全般を管理するクラス.
/// </summary>
public class SoundPlayer {

	BGMPlayer curBGMPlayer;
	BGMPlayer fadeOutBGMPlayer;

	GameObject soundPlayerObj;
	AudioSource audioSource;
	Dictionary<string, AudioClipInfo> audioClips = new Dictionary<string, AudioClipInfo>();
	
	// AudioClip information
	class AudioClipInfo {
		public string resourceName;
		public string name;
		public AudioClip clip;
		
		public AudioClipInfo( string resourceName, string name ) {
			this.resourceName = resourceName;
			this.name = name;
		}
	}
	
	public SoundPlayer() {
		audioClips.Add( "click", new AudioClipInfo( "Click", "click" ) );
		audioClips.Add( "Touch", new AudioClipInfo( "CardTouch", "Touch" ) );
		audioClips.Add( "item", new AudioClipInfo( "ItemBuy", "item" ) );
		audioClips.Add( "draw", new AudioClipInfo( "draw", "draw" ) );
		audioClips.Add( "bclick", new AudioClipInfo( "BattlwClick", "bclick" ) );


		audioClips.Add( "battle", new AudioClipInfo( "Battle", "battle" ) );
		audioClips.Add( "battle2", new AudioClipInfo( "Battle2", "battle2" ) );
		audioClips.Add( "battle3", new AudioClipInfo( "Battle3", "battle3" ) );
		audioClips.Add( "boss", new AudioClipInfo( "Boss1", "boss" ) );
		audioClips.Add( "boss2", new AudioClipInfo( "Boss2", "boss2" ) );
		audioClips.Add( "boss3", new AudioClipInfo( "Boss3", "boss3" ) );
		audioClips.Add( "Title", new AudioClipInfo( "Title", "Title" ) );
		audioClips.Add( "Story", new AudioClipInfo( "Story", "Story" ) );
		audioClips.Add( "Shop", new AudioClipInfo( "Shop", "Shop" ) );
		audioClips.Add( "Ending", new AudioClipInfo( "Ending", "Ending" ) );
		audioClips.Add( "Select", new AudioClipInfo( "CharaSelect", "Select" ) );
	}
	
	public bool playSE( string seName ) {
		if ( audioClips.ContainsKey( seName ) == false )
			return false; // not register
		
		AudioClipInfo info = audioClips[ seName ];
		
		// Load
		if ( info.clip == null )
			info.clip = (AudioClip)Resources.Load( info.resourceName );
		
		if ( soundPlayerObj == null ) {
			soundPlayerObj = new GameObject( "SoundPlayer" ); 
			audioSource = soundPlayerObj.AddComponent<AudioSource>();
			//audioSource.volume = 0.5f; 

			GameObject camera = GameObject.Find("Main Camera");
			soundPlayerObj.transform.position = camera.transform.position;
			soundPlayerObj.transform.parent = camera.transform;

		}
		
		// Play SE
		audioSource.PlayOneShot( info.clip );
		
		return true;
	}

	public void playBGM( string bgmName, float fadeTime ) {
		// destory old BGM
		if ( fadeOutBGMPlayer != null )
			fadeOutBGMPlayer.destory();
		
		// change to fade out for current BGM
		if ( curBGMPlayer != null ) {
			curBGMPlayer.stopBGM( fadeTime );
			fadeOutBGMPlayer = curBGMPlayer;
		}
		
		// play new BGM
		if ( audioClips.ContainsKey( bgmName ) == false ) {
			// null BGM
			curBGMPlayer = new BGMPlayer();
		} else {
			curBGMPlayer = new BGMPlayer( audioClips[ bgmName ].resourceName );
			curBGMPlayer.playBGM( fadeTime );
		}
	}

	public void playBGM() {
		if ( curBGMPlayer != null )
			curBGMPlayer.playBGM();
		if ( fadeOutBGMPlayer != null )
			fadeOutBGMPlayer.playBGM();
	}
	
	public void pauseBGM() {
		if ( curBGMPlayer != null )
			curBGMPlayer.pauseBGM();
		if ( fadeOutBGMPlayer != null )
			fadeOutBGMPlayer.pauseBGM();
	}
	
	public void stopBGM( float fadeTime ) {
		if ( curBGMPlayer != null )
			curBGMPlayer.stopBGM( fadeTime );
		if ( fadeOutBGMPlayer != null )
			fadeOutBGMPlayer.stopBGM( fadeTime );
	}
}
