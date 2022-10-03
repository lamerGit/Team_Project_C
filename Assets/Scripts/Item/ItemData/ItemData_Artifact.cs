using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �����͸� �����ϴ� ������ ������ ����� ���ִ� ��ũ��Ʈ
/// </summary>
[CreateAssetMenu(fileName = "New Artifact Item Data", menuName = "Scriptable Object/Item Data - Artifact", order = 5)]
public class ItemData_Artifact : ItemData, IEquipArtifact
{
    [Header("��Ƽ��Ʈ ������")]
    public float attackPower = 10.0f;
    public float attackSpeed = 1.0f;
    public float healingPerSec = 1.0f;

    /// <summary>
    /// ������ ���
    /// </summary>
    /// <param name="target">�������� ����� ���</param>
    public void EquipItem(IEquipTarget target)
    {
        //target.EquipWeapon(this);
    }

    /// <summary>
    /// ������ ���/���� ���
    /// </summary>
    /// <param name="target">�������� ����� ���</param>
    public void ToggleEquipItem(IEquipTarget target)
    {
        //if(target.IsWeaponEquiped)
        //{
        //    target.UnEquipWeapon();     // ���Ǿ������� �����ϰ�
        //}
        //else
        //{
        //    target.EquipWeapon(this);   // ���ȵǾ������� ���
        //}
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="target">�������� ������ ���</param>
    public void UnEquipItem(IEquipTarget target)
    {
        //target.UnEquipWeapon();
    }
}
