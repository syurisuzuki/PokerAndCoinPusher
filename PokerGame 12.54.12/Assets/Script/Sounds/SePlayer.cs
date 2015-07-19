using UnityEngine;
using System.Collections;

public class SePlayer : MonoBehaviour {


	public void ReadMagicSe(){
		//soundplay
		Singleton<SoundPlayer>.instance.playSE( "majicread" );
	}

	public void ExplosionSe(){
		Singleton<SoundPlayer>.instance.playSE( "explosion" );
	}

	public void SlashSe(){
		Singleton<SoundPlayer>.instance.playSE( "slash" );
	}

	public void BulletSe(){
		Singleton<SoundPlayer>.instance.playSE( "bullet" );
	}

	public void HealSe(){
		Singleton<SoundPlayer>.instance.playSE( "heal" );
	}

	public void Sp1Se(){
		//Singleton<SoundPlayer>.instance.playSE( "sp1" );
	}

	public void Sp2Se(){
		//Singleton<SoundPlayer>.instance.playSE( "sp2" );
	}



}
