using UnityEngine;
using System;
using System.Collections;

public enum CardMark {
SPADE = 0,
HEART = 1,
DIA = 2,
CLOVER = 3,
JOKAR = 5
}

[System.Serializable]
public class CardBase : MonoBehaviour {

    public CardMark mark;
    public int real_num;
    public int strong_num;

    public UISprite mySprite;

    public bool isTouch = false;
    public bool isCharge = false;

    public GameObject hold;

    public UISprite cardFrame;

    public FieldCard fw;

    public UIButton button;

    public Animator animator;

    public AudioSource audios;
    public AudioClip ac;

    private Vector3 tmpPos;
    public TweenPosition tp;

    private string tmpspritename;

    public void InitCardSprite() {
        string speitename = "";
        switch (mark) {
            case CardMark.CLOVER:
                speitename = "c";
                break;
            case CardMark.SPADE:
                speitename = "s";
                break;
            case CardMark.HEART:
                speitename = "h";
                break;
            case CardMark.DIA:
                speitename = "d";
                break;
            default:
                speitename = "z";
                break;
        }
        if (real_num < 10) {
            speitename += "0" + real_num;
        }
        else{
            speitename += real_num;
        }
        tmpspritename = speitename;
        mySprite.spriteName = "z01";
    }

    public void SEPlay() {
        audios.PlayOneShot(ac);
    }

    public void DrawRotateAnimation() {
        animator = this.gameObject.GetComponent<Animator>();
        animator.SetTrigger("Rotate");
    }

    public void TouchCallBack() {

        if (isCharge = true && isTouch == false) {
            fw.TouchCard(this);
            isTouch = !isTouch;
            hold.SetActive(isTouch);
            return;
        }

        if (isTouch && fw.isCharging == false)
        {
            fw.isCharging = true;
            isCharge = true;
            isTouch = false;
            fw.TouchCard(this);
            hold.SetActive(false);
            GoChargeCardPos();
            return;

        }

        if (isCharge) {
            fw.isCharging = false;
            isCharge = false;
            BackToPosition();

            return;
        }
        
        fw.TouchCard(this);
        isTouch = !isTouch;
        hold.SetActive(isTouch);
    }

    public void CardRotateAnimChengeSprite() {
        mySprite.spriteName = tmpspritename;
        button.normalSprite = tmpspritename;
    }

    public void GoChargeCardPos() {
        tmpPos = this.gameObject.transform.localPosition;
        tp.from.x = tmpPos.x;
        tp.from.y = tmpPos.y;
        tp.to.x = 53;
        tp.to.y = 240;
        fw.chageCard = gameObject;
        tp.PlayForward();
    }

    public void BackToPosition() {
        tp.PlayReverse();
    }

    public void TweenCallBack() {
        if (isCharge == true)
        {
            button.defaultColor = Color.gray;
            
        }
        else {
            button.defaultColor = Color.white;
        }
    }

    public void DecideSelect() {
        tmpPos = this.gameObject.transform.localPosition;
        tp.from.x = tmpPos.x;
        tp.from.y = tmpPos.y;
        tp.to.x = 300;
        tp.to.y = 466;
        EventDelegate.Add(tp.onFinished, CardDelete);
        tp.PlayForward();
    }

    public void CardDelete() {
        gameObject.transform.parent = null;
        fw.handCardsObj.Remove(this.gameObject);
        fw.handCards.Remove(this);
        Destroy(this.gameObject);
        fw.root_cards.Reposition();
    }



}
