using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipTarget
{
    ItemSlot EquipItemSlot { get; }      // ����� ������(����)

    void EquipWeapon(ItemSlot weaponSlot);   // ������ ����ϱ�
    void UnEquipWeapon();                   // ������ �����ϱ�
}
