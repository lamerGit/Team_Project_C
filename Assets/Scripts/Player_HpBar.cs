using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HpBar : MonoBehaviour
{
    //�÷��̾� ü���� ǥ�����ִ� ��ũ��Ʈ

    IHealth target;
    Image hp;
    private void Start()
    {
        hp=GetComponentInChildren<Image>();
        target=GameManager.INSTANCE.PLAYER.GetComponent<IHealth>();
        target.onHealthChange += SetHp_Value;
        //gameObject.SetActive(false);
    }

    /// <summary>
    /// �÷��̾��� onHealthChange��������Ʈ�� �Ҵ��ؼ� hp�� ���Ҷ��� hp�ٸ� �����ϼ� �ְ� �ϴ� �Լ�
    /// </summary>
    void SetHp_Value()
    {
        if(target!=null)
        {
            float ratio = target.HP / target.MaxHP;
            hp.fillAmount = ratio;
        }
    }    
}
