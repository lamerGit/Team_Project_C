using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trap�� ������ ���� ����� ��ũ��Ʈ
/// </summary>
[CreateAssetMenu(fileName = "New Trap", menuName = "Scriptable Object/Item Data - Trap", order = 3)]
public class ItemData_Trap : ItemData, IUsable
{
    [Header("Ʈ�� ������")]
    //float trapDamage = 20.0f;
    public GameObject trap;
    Transform trapPos;

    public void Use(GameObject target = null)
    {
        trapPos = GameObject.FindObjectOfType<FindTrapPos>().transform;
        Instantiate(trap,trapPos);
    }
}