using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    // �⺻ ������ ---------------------------------------------------------------------------------
    /// <summary>
    /// ������ ���� ���̵�
    /// </summary>
    uint id;

    /// <summary>
    /// �� ����UI���� ������ ���� ItemSlot(inventoryŬ������ ������ �ִ� ItemSlot�� �ϳ�)
    /// </summary>
    protected ItemSlot itemSlot;

    // �ֿ� �κ��丮 UI ������ �ֱ� -----------------------------------------------------------------

    /// <summary>
    /// �κ��丮 UI
    /// </summary>
    InventoryUI invenUI;

    /// <summary>
    /// �� ����â
    /// </summary>
    DetailInfoUI detailUI;

    // UIó���� ������ -----------------------------------------------------------------------------

    /// <summary>
    /// �������� Icon�� ǥ���� �̹��� ������Ʈ
    /// </summary>
    protected Image itemImage;

    /// <summary>
    /// �������� ������ ǥ���� Text ������Ʈ
    /// </summary>
    protected TextMeshProUGUI countText;

    /// <summary>
    /// �������� ��� ���θ� ǥ���� Text ������Ʈ
    /// </summary>
    protected TextMeshProUGUI equipMark;



    // ������Ƽ�� ----------------------------------------------------------------------------------

    /// <summary>
    /// ������ ���� ���̵�(�б� ����)
    /// </summary>
    public uint ID { get => id; }

    /// <summary>
    /// �� ����UI���� ������ ���� ItemSlot(�б� ����)
    /// </summary>
    public ItemSlot ItemSlot { get => itemSlot; }

    // �Լ��� --------------------------------------------------------------------------------------
    protected virtual void Awake()  // �������̵� �����ϵ��� virtual �߰�
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();    // ������ ǥ�ÿ� �̹��� ������Ʈ ã�Ƴ���
        countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        equipMark = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        equipMark.gameObject.SetActive(false);
    }

    /// <summary>
    /// ItemSlotUI�� �ʱ�ȭ �۾�
    /// </summary>
    /// <param name="newID">�� ������ ID</param>
    /// <param name="targetSlot">�� �����̶� ����� ItemSlot</param>
    public void Initialize(uint newID, ItemSlot targetSlot)
    {
        invenUI = GameManager.INSTANCE.InvenUI; // �̸� ã�Ƴ���
        detailUI = invenUI.Detail;

        id = newID;
        itemSlot = targetSlot;
        itemSlot.onSlotItemChange = Refresh; // ItemSlot�� �������� ����� ��� ����� ��������Ʈ�� �Լ� ���        
    }

    /// <summary>
    /// ���Կ��� ǥ�õǴ� ������ �̹��� ���ſ� �Լ�
    /// </summary>
    public void Refresh()
    {
        if (itemSlot.SlotItemData != null)
        {
            // �� ���Կ� �������� ������� ��
            itemImage.sprite = itemSlot.SlotItemData.itemIcon;  // ������ �̹��� �����ϰ�
            itemImage.color = Color.white;  // �������ϰ� �����
            countText.text = itemSlot.ItemCount.ToString();

            // equipMark�� ���������� ������� ��Ȳ�϶��� ��������
            equipMark.gameObject.SetActive((itemSlot.SlotItemData is ItemData_Artifact) && itemSlot.ItemEquiped);

        }
        else
        {
            // �� ���Կ� �������� ���� ��
            itemImage.sprite = null;        // ������ �̹��� �����ϰ�
            itemImage.color = Color.clear;  // �����ϰ� �����
            countText.text = "";
            equipMark.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �������� ���콺 �����Ͱ� ������ ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSlot.SlotItemData != null)
        {
            //Debug.Log($"���콺�� {gameObject.name}������ ���Դ�.");
            detailUI.Open(itemSlot.SlotItemData);
        }
    }

    /// <summary>
    /// ���������� ���콺 �����Ͱ� ������ ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"���콺�� {gameObject.name}���� ������.");
        detailUI.Close();
    }

    /// <summary>
    /// ���������� ���콺 �����Ͱ� ������ ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log($"���콺�� {gameObject.name}�ȿ��� �����δ�.");
        Vector2 mousePos = eventData.position;

        // ������â�� ȭ���� ������� üũ
        RectTransform rect = (RectTransform)detailUI.transform;
        if ((mousePos.x + rect.sizeDelta.x) > Screen.width)
        {
            mousePos.x -= rect.sizeDelta.x; // ������� ������â�� ���콺 �������� �̵���Ŵ)
        }

        detailUI.transform.position = mousePos; // ������â�� ���콺 Ŀ�� ��ġ�� ����
    }

    /// <summary>
    /// ������ ���콺�� Ŭ������ ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // ���콺 ���� ��ư Ŭ���� ��
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            TempItemSlotUI temp = invenUI.TempSlotUI;

            if (Keyboard.current.leftShiftKey.ReadValue() > 0 && temp.IsEmpty())
            {
                // ����Ʈ ��Ŭ���ϰ� temp�� ����� �� ������ ����â ����.

                //Debug.Log("Shift+��Ŭ�� => ����â ����");
                invenUI.SpliterUI.Open(this);   // ������ ����â ����
                detailUI.Close();               // ������â �ݱ�
                detailUI.IsPause = true;        // ������â �Ͻ� ����
            }
            else
            {
                if (!temp.IsEmpty())  // temp�� ItemSlot�� ����ִ� => �������� ��� ��Ȳ�̴�.                
                {
                    bool isEquipItem = temp.ItemSlot.ItemEquiped;
                    // ��� �ִ� �ӽ� �������� ���Կ� �ֱ�                
                    if (ItemSlot.IsEmpty())
                    {
                        // Ŭ���� ������ ��ĭ�̴�.

                        // temp�� �ִ� ������ �� ���Կ� �� �ֱ�
                        itemSlot.AssignSlotItem(temp.ItemSlot.SlotItemData, temp.ItemSlot.ItemCount);
                        (temp.ItemSlot.ItemEquiped, itemSlot.ItemEquiped) = (itemSlot.ItemEquiped, temp.ItemSlot.ItemEquiped);
                        temp.Close();   // tempĭ ����
                    }
                    else if (temp.ItemSlot.SlotItemData == ItemSlot.SlotItemData)
                    {
                        // �� ���Կ��� ���� ������ �������� ����ִ�.

                        // ��� ����� ���� ����
                        uint remains = ItemSlot.SlotItemData.maxStackCount - ItemSlot.ItemCount;
                        // �ӽý����� ������ �ִ� �Ͱ� ���� ���� �� �� ���� ���� ����
                        //uint small = System.Math.Min(remains, temp.ItemSlot.ItemCount);
                        uint small = (uint)Mathf.Min((int)remains, (int)temp.ItemSlot.ItemCount);

                        ItemSlot.IncreaseSlotItem(small);
                        temp.ItemSlot.DecreaseSlotItem(small);
                        (temp.ItemSlot.ItemEquiped, itemSlot.ItemEquiped) = (itemSlot.ItemEquiped, temp.ItemSlot.ItemEquiped);

                        if (temp.ItemSlot.ItemCount < 1)    // �ӽ� ���Կ� �ִ� ���� ���� �־��� ���� �ݾƶ�
                        {
                            temp.Close();
                        }
                    }
                    else
                    {
                        // �ٸ� ������ �������̴�. => ���� ����
                        ItemData tempData = temp.ItemSlot.SlotItemData;
                        uint tempCount = temp.ItemSlot.ItemCount;
                        temp.ItemSlot.AssignSlotItem(itemSlot.SlotItemData, itemSlot.ItemCount);
                        itemSlot.AssignSlotItem(tempData, tempCount);
                        (temp.ItemSlot.ItemEquiped, itemSlot.ItemEquiped) = (itemSlot.ItemEquiped, temp.ItemSlot.ItemEquiped);
                    }

                    if (isEquipItem) // ������� �������� �ű�� ��Ȳ�̸� �ϴ� �����ϰ� �ٽ� ���
                    {
                        //GameManager.INSTANCE.PLAYER.UnEquipWeapon();
                        //GameManager.INSTANCE.PLAYER.EquipWeapon(ItemSlot);
                    }

                    detailUI.IsPause = false;   // ������â �Ͻ����� Ǯ��
                }
                else
                {
                    // �׳� Ŭ���� ��Ȳ
                    if (!ItemSlot.IsEmpty())
                    {
                        // ������ ��� �õ�
                        ItemSlot.UseSlotItem(GameManager.INSTANCE.PLAYER.gameObject);
                        if (ItemSlot.IsEmpty())
                        {
                            invenUI.Detail.Close();
                        }

                        // ������ ��� �õ�
                        bool isEquiped = ItemSlot.EquipSlotItem(GameManager.INSTANCE.PLAYER.gameObject);
                        if (isEquiped)
                        {
                            invenUI.ClearAllEquipMark();
                        }
                        ItemSlot.ItemEquiped = isEquiped;
                    }
                }
            }
        }
    }

    public void ClearEquipMark()
    {
        equipMark.gameObject.SetActive(false);
    }
}
