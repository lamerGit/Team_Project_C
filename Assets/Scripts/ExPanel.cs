using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ExPanel : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public void TextChange(int number)
    {
        if(number==0)
        {
            textMeshProUGUI.text = "������ Ÿ��\n" +
                "Ÿ����Ÿ� : ª��\n" +
                "���ݼӵ� : ����\n";
        }else if(number==1)
        {
            textMeshProUGUI.text = "ȭ�� Ÿ��\n" +
                "Ÿ����Ÿ� : ���\n" +
                "���ݼӵ� : ����\n";
        }
    }

}
