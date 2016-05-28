using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PowerInfo : MonoBehaviour {

    public UILabel text;
    public FieldCard field;

    public GameObject attackbtn;

    

    public void Math_DamageRate() {
        var f = damageRate();
        if (f == 0f)
        {
            attackbtn.SetActive(false);
        }
        else {
            attackbtn.SetActive(true);
        }
        text.text = f+"/";
    }

    public float damageRate() {

        if (field.selectingCard.Count < 2)
            return 0;

        var mathbasecard = field.selectingCard[0];

        var isFlush = field.selectingCard.Where(card => card.mark == mathbasecard.mark).ToList();

        if (isFlush.Count == field.selectingCard.Count)
            return 1.5f;


        return 1;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
