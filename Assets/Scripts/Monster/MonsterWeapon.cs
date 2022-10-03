using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    public WeaponType type = WeaponType.NomalWeapon;

    public float attackPower = 10.0f; //���ݷ�
    public float bossPower = 2.0f;  // ���� ���ݷ� ���


    //������ ���⿡ �ִ� ��ũ��Ʈ

    /// <summary>
    /// �÷��̾�� ������ ������
    /// </summary>
    /// <param name="other">�÷��̾ ��� �±׸� ������</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattle battle = other.GetComponent<IBattle>();
            if (battle != null)
            {
                if(type == WeaponType.NomalWeapon)
                {
                    battle.TakeDamage(attackPower);
                }

                if(type==WeaponType.BossWeapon)  // ���� Ÿ���� ��������� ���� ���ݷ� = ���ݷ� * ���ݷ� ���
                {
                    battle.TakeDamage(attackPower * bossPower);
                }
            }
            return;
            
        }
    }

}

