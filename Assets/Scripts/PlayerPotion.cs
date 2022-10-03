using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPotion : MonoBehaviour
{
    //���ǿ� ���� ��ũ��Ʈ

    bool isDelay=false;
    float delayTime = 5.0f;
    float accumTime;
    float PotionHealPoint = 20.0f;


    IHealth PlayerHealth;
    void Start()
    {
        PlayerHealth=GameManager.INSTANCE.PLAYER.GetComponent<IHealth>();
    }

    /// <summary>
    /// �÷��̾� ü���� ȸ�������ִ� �Լ�
    /// </summary>
    void Healing()
    {
        PlayerHealth.TakeHeal(PotionHealPoint);
    }

    /// <summary>
    /// Healing�Լ��� ��������ָ鼭 ��Ÿ�� �ڷ�ƾ�� ��������ִ� �Լ�
    /// </summary>

    public void OnDrinkPotion()
    {
        if(isDelay==false)
        {
            isDelay=true;
            StartCoroutine(DrinkPotionDelay());
            Healing();
        }
        else
        {
            Debug.Log("���� ��Ÿ���� ���ҽ��ϴ�");
        }
    }
    /// <summary>
    /// ��Ÿ�ӿ� IEnumerator
    /// </summary>
    /// <returns>delayTime�ڿ� isDelay�� false�� ����</returns>
    IEnumerator DrinkPotionDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
    
}
