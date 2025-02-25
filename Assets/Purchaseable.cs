using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Purchaseable : MonoBehaviour
{
    public Castle CastleScript;
    public Button PurchaseButton;
    public int GoldCost, LumberCost;

    void Update()
    {
        if (CastleScript.Gold >= GoldCost && CastleScript.Lumber >= LumberCost)
            PurchaseButton.interactable = true;
        else PurchaseButton.interactable = false;
    }
}
