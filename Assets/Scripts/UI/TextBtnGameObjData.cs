using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TextBtnGameObjData : MonoBehaviour
{
    [SerializeField] private GameObject[] CharParamsTexts;
    [SerializeField] private GameObject[] UpgradeParamsButtons;

    public GameObject[] SendText() 
    {
        return CharParamsTexts;
    }

    public GameObject[] SendButtons() 
    {
        return UpgradeParamsButtons;
    }
}
