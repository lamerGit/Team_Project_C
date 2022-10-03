using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyChange : MonoBehaviour
{
    //�� ǥ�ÿ� ��ũ��Ʈ
    TextMeshProUGUI textMeshProUGUI;

    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        GameManager.INSTANCE.PLAYER.GetComponent<PlayerWolf>().MoneyChange += Change;
        Change();
    }

    void Change()
    {
        textMeshProUGUI.text = $"{GameManager.INSTANCE.PLAYER.GetComponent<PlayerWolf>().MONEY}";
    }
}
