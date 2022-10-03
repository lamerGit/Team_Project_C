using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HealingPotion�� ������ ���� ����� ��ũ��Ʈ
/// </summary>
[CreateAssetMenu(fileName = "New HealingPotion", menuName = "Scriptable Object/Item Data - HealingPotion", order = 2)]
public class ItemData_HealingPotion : ItemData, IUsable
{
    [Header("�������� ������")]
    float healPoint = 20.0f;
    
    public void Use(GameObject target = null)
    {
        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            //health.HP += healPoint;
            health.TakeHeal(healPoint);
            Debug.Log($"{itemName}�� ����߽��ϴ�. HP�� {healPoint} ȸ���Ǿ����ϴ�. ���� HP�� {health.HP}�Դϴ�.");
        }
    }
}
