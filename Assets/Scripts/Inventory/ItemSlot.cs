using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    // ���� ---------------------------------------------------------------------------------------
    // ���Կ� �ִ� ������(ItemData)
    ItemData slotItemData;

    // ������ ����(int)
    uint itemCount = 0;

    // ������ ��񿩺�
    bool itemEquiped = false;

    // ������Ƽ ------------------------------------------------------------------------------------

    /// <summary>
    /// ���Կ� �ִ� ������(ItemData)
    /// </summary>
    public ItemData SlotItemData
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();  // ������ �Ͼ�� ��������Ʈ ����(�ַ� ȭ�� ���ſ�)
            }
        }
    }

    /// <summary>
    /// ���Կ� ����ִ� ������ ����
    /// </summary>
    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            itemCount = value;
            onSlotItemChange?.Invoke();  // ������ �Ͼ�� ��������Ʈ ����(�ַ� ȭ�� ���ſ�)
        }
    }

    public bool ItemEquiped
    {
        get => itemEquiped;
        set
        {
            itemEquiped = value;
            onSlotItemChange?.Invoke();
        }
    }

    // ��������Ʈ ----------------------------------------------------------------------------------
    /// <summary>
    /// ���Կ� ����ִ� �������� ������ ������ ����� �� ����Ǵ� ��������Ʈ
    /// </summary>
    public System.Action onSlotItemChange;

    // �Լ� ---------------------------------------------------------------------------------------

    /// <summary>
    /// �����ڵ�
    /// </summary>
    public ItemSlot() { }
    public ItemSlot(ItemData data, uint count)
    {
        slotItemData = data;
        itemCount = count;
    }
    public ItemSlot(ItemSlot other)
    {
        slotItemData = other.SlotItemData;
        itemCount = other.ItemCount;
    }

    /// <summary>
    /// ���Կ� �������� �����ϴ� �Լ� 
    /// </summary>
    /// <param name="itemData">���Կ� ������ ItemData</param>
    /// /// <param name="count">���Կ� ������ ������ ����</param>
    public void AssignSlotItem(ItemData itemData, uint count = 1)
    {
        ItemCount = count;
        SlotItemData = itemData;
    }

    /// <summary>
    /// ���� ������ �������� �߰��� ������ ������ �����ϴ� ��Ȳ�� ���
    /// </summary>
    /// <param name="count">������ų ����</param>
    /// <returns>�ִ�ġ�� �Ѿ ����. 0�̸� �� ������Ų ��Ȳ</returns>
    public uint IncreaseSlotItem(uint count = 1)
    {
        uint newCount = ItemCount + count;
        int overCount = (int)newCount - (int)SlotItemData.maxStackCount;    // ��ģ ���� ���
        if (overCount > 0)
        {
            // ���ƴ�.
            ItemCount = SlotItemData.maxStackCount;
        }
        else
        {
            // ����� �߰� �����ϴ�.
            ItemCount = newCount;
            overCount = 0;
        }
        return (uint)overCount; // ��ģ ���� �����ֱ�
    }

    /// <summary>
    /// ���Կ��� ������ ���� ���� ��Ű��
    /// </summary>
    /// <param name="count">���ҽ�ų ����</param>
    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)ItemCount - (int)count;
        if (newCount < 1)   // ���������� ������ 0�̵Ǹ� ���� ����
        {
            // �� ����.
            ClearSlotItem();
        }
        else
        {
            ItemCount = (uint)newCount;
        }
    }

    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    public void ClearSlotItem()
    {
        SlotItemData = null;
        ItemCount = 0;
        ItemEquiped = false;
    }

    /// <summary>
    /// �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="target">�������� ȿ���� ���� ���(���� �÷��̾�)</param>
    public void UseSlotItem(GameObject target = null)
    {
        IUsable usable = SlotItemData as IUsable;   // �� �������� ��밡���� ���������� Ȯ��
        if (usable != null)
        {
            // �������� ��밡���ϸ�
            usable.Use(target); // ������ ����ϰ�
            DecreaseSlotItem(); // ���� �ϳ� ����
        }
    }

    /// <summary>
    /// �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="target">�������� ����ϴ� ���</param>
    public bool EquipSlotItem(GameObject target = null)
    {
        bool result = false;
        IEquipArtifact equipItem = SlotItemData as IEquipArtifact;  // �� ������ �������� ��� ������ ���������� Ȯ��
        if (equipItem != null)
        {
            // �������� ��񰡴��ϴ�.

            ItemData_Artifact artifactData = SlotItemData as ItemData_Artifact;   // ������ ������ ���� ����
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>(); // �������� ����� ����� �������� ����� �� �ִ��� Ȯ��
            if (equipTarget != null)
            {
                // ����� Ư�� ������ �������� ����ϰ� �ִ�. �׸��� �������� ���Ǿ� �ִ�.
                if (equipTarget.EquipItemSlot != null)    // ���⸦ ����ϰ� �մ��� Ȯ��
                {
                    // ���⸦ ����ϰ� �ִ�.

                    if (equipTarget.EquipItemSlot != this)      // ����ϰ� �ִ� �������� ������ Ŭ���ߴ��� Ȯ��
                    {
                        // �ٸ� ������ ����ϰ� �ִ�.
                        equipTarget.UnEquipWeapon();            // �ϴ� ���⸦ ���´�.
                        equipTarget.EquipWeapon(this);    // �ٸ� ���⸦ ����Ѵ�.
                        result = true;
                    }
                    else
                    {
                        equipTarget.UnEquipWeapon();            // ���� ���⸦ ����� ��Ȳ�̸� ���⸸ �Ѵ�.
                    }
                }
                else
                {
                    // ���⸦ ����ϰ� ���� �ʴ�. => �׳� ���
                    equipTarget.EquipWeapon(this);
                    result = true;
                }
            }
        }
        return result;
    }


    // �Լ�(�鿣��) --------------------------------------------------------------------------------

    /// <summary>
    /// ������ ������� Ȯ�����ִ� �Լ�
    /// </summary>
    /// <returns>true�� ����ִ� �Լ�</returns>
    public bool IsEmpty()
    {
        return slotItemData == null;
    }
}