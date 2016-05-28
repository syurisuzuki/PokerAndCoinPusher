using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public enum PokarHands {
    NOPAIR,
    MINI_STRAIGHT,
    ON_PAIR,
    MINI_FLUSH,
    MINI_STRAIGHT_FLUSH,
    MIDDLE_STRAIGHT,
    THREEOFKIND,
    MIDDLE_FLUSH,
    TWO_PAIR,
    MIDDLE_STRAIGHT_FLUSH,
    HEAVY_STRAIGHT,
    HEAVY_FLUSH,
    HEAVY_STRAIGHT_FLUSH,
    FULL_STRAIGHT,
    FULL_FLUSH,
    FULL_HOUSE,
    FOUR_OF_KIND,
    STRAIGHT_FLUSH,
    ROYAL_STRAIGHT_FLUSH,
}

public class Judge : MonoBehaviour {

    List<CardMark> markList = new List<CardMark>();
    List<int> numList = new List<int>();

    public PokarHands GetPokarHand(List<CardBase> selectCard) {

        markList.Clear();numList.Clear();

        for (int i = 0; i < selectCard.Count; i++) {
            markList.Add(selectCard[i].mark);
            numList.Add(selectCard[i].real_num);
        }

        numList.Sort();

        if (isStraight(numList) && isFlush(markList))
        {
            switch (selectCard.Count)
            {
                case 2:
                    return PokarHands.MINI_STRAIGHT_FLUSH;
                case 3:
                    return PokarHands.MIDDLE_STRAIGHT_FLUSH;
                case 4:
                    return PokarHands.HEAVY_STRAIGHT_FLUSH;
                case 5:
                    return PokarHands.FULL_STRAIGHT;
            }
        }
        else if (isStraight(numList) && isFlush(markList) == false)
        {

            switch (selectCard.Count)
            {
                case 2:
                    return PokarHands.MINI_STRAIGHT;
                case 3:
                    return PokarHands.MIDDLE_STRAIGHT;
                case 4:
                    return PokarHands.HEAVY_STRAIGHT;
                case 5:
                    return PokarHands.FULL_STRAIGHT;
            }
        }
        else if (isStraight(numList) == false && isFlush(markList))
        {
            switch (selectCard.Count)
            {
                case 2:
                    return PokarHands.MINI_FLUSH;
                case 3:
                    return PokarHands.MIDDLE_FLUSH;
                case 4:
                    return PokarHands.HEAVY_FLUSH;
                case 5:
                    return PokarHands.FULL_FLUSH;
            }
        }
        else {}

        return PokarHands.NOPAIR;
    }

    bool isStraight(List<int> num) {

        for (int i = 0; i < num.Count - 1; i++)
        {
            if (num[i] + 1 == num[i + 1])
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    bool isFlush(List<CardMark> mark) {

        CardMark cm = mark[0];

        if (mark.Count(m => m == cm) != mark.Count)
            return false;

        return true;

    }

    PokarHands isPair(List<int> num) {
        return PokarHands.NOPAIR;
    }
}
