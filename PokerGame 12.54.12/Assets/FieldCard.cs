using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class FieldCard : MonoBehaviour {

    public GameObject cardPrefab;
    public GameObject nextPrefab;

    public UIGrid root_cards;
    public UIGrid root_next;

    public List<CardBase> fieldCards = new List<CardBase>();
    public List<CardBase> handCards = new List<CardBase>();
    public List<CardBase> nextCards = new List<CardBase>();
    public List<GameObject> handCardsObj = new List<GameObject>();
    public List<GameObject> nextCardsObj = new List<GameObject>();

    public List<CardBase> selectingCard = new List<CardBase>();

    public PowerInfo pw;

    public GameObject chageCard;

    public bool isCharging;

    public bool decideCharge = false;

    public GameObject uiRoot;

    public Judge judge;

    public IEnumerator AddDraw() {

        if (chageCard != null && decideCharge == false) {
            decideCharge = true;
            chageCard.transform.parent = uiRoot.transform;
            handCardsObj.Remove(chageCard);
            var cc = chageCard.GetComponent<CardBase>();
            cc.isTouch = false;
            handCards.Remove(cc);
        }

        int count = selectingCard.Count();

        for (int i = 0; i < selectingCard.Count; i++)
        {

            selectingCard[i].DecideSelect();
        }

        selectingCard.Clear();

        yield return new WaitForSeconds(.4f);

        root_cards.Reposition();

        int u = 0;

        for (int i = 0; i < 3; i++)
        {

            if (handCards.Count < 5)
            {
                u++;
                var cardbase = nextCards[0];
                nextCards.Remove(cardbase);



                var card = NGUITools.AddChild(root_cards.gameObject, cardPrefab);
                var card_cb = card.GetComponent<CardBase>();
                handCardsObj.Add(card);
                card_cb.real_num = cardbase.real_num;
                card_cb.mark = cardbase.mark;
                card_cb.fw = this;
                card_cb.InitCardSprite();
                card_cb.DrawRotateAnimation();
                handCards.Add(card_cb);

                var obj = cardbase.gameObject;
                obj.transform.parent = null;
                nextCardsObj.Remove(obj);
                Destroy(obj);
                root_cards.Reposition();
            }
        }


        DrawNext(u);
        yield return 0;
    }

    public void OnPressAttack() {

        Debug.Log(judge.GetPokarHand(selectingCard));

        StartCoroutine(AddDraw());

    }

    public void TouchCard(CardBase cb) {
        if (selectingCard.Any(card => card == cb))
        {
            selectingCard.Remove(cb);
        }
        else {
            selectingCard.Add(cb);
        }

        pw.Math_DamageRate();

    }

    public void Init() {
        fieldCards.Clear();
        for (int i = 0; i < 4; i++) {
            for (int j = 1; j < 14; j++) {
                CardBase card = new CardBase();
                card.mark = (CardMark)i;
                card.real_num = j;
                
                fieldCards.Add(card);
            }
        }
        Draw(5);
    }

    public void DrawNext(int count) {
        for (int i = 0; i < count; i++)
        {
            var index = UnityEngine.Random.Range(0, fieldCards.Count);
            CardBase choicecard = fieldCards[index];
            fieldCards.RemoveAt(index);
            
            var card = NGUITools.AddChild(root_next.gameObject, nextPrefab);
            nextCardsObj.Add(card);
            var card_cb = card.GetComponent<CardBase>();
            card_cb.real_num = choicecard.real_num;
            card_cb.mark = choicecard.mark;
            card_cb.InitCardSprite();
            card_cb.DrawRotateAnimation();
            nextCards.Add(card_cb);
        }
        root_next.Reposition();

        
        
    }

    public void Draw(int count) {
        for (int i = 0; i < count; i++) {
            var index = UnityEngine.Random.Range(0, fieldCards.Count);
            CardBase choicecard = fieldCards[index];
            fieldCards.RemoveAt(index);
            
            var card = NGUITools.AddChild(root_cards.gameObject, cardPrefab);
            var card_cb = card.GetComponent<CardBase>();
            handCardsObj.Add(card);
            card_cb.real_num = choicecard.real_num;
            card_cb.mark = choicecard.mark;
            card_cb.fw = this;
            card_cb.InitCardSprite();
            handCards.Add(card_cb);
        }
        root_cards.Reposition();
        
    }

	// Use this for initialization
	void Start () {
        Init();
        DrawNext(3);
        StartCoroutine(CardRotate());

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator CardRotate() {
        yield return new WaitForSeconds(.1f);
        for (int j = 0; j < handCards.Count; j++)
        {
            Debug.Log(j);
            handCards[j].DrawRotateAnimation();
            yield return new WaitForSeconds(.2f);
        }
        yield return null;
    }
}
