using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject slotPrefab;

    PlayerWolf player;

    Inventory inven;

    Transform slotParent;

    ItemSlotUI[] slotUIs;

    CanvasGroup canvasGroup;

    public bool inventoryOn;

    //Player_Wolf inputActions;

    // ������ ����

    /// <summary>
    /// �巡�� ���� ǥ�ÿ�
    /// </summary>
    const uint InvalideID = uint.MaxValue;

    /// <summary>
    /// �巡�װ� ���۵� ������ ID
    /// </summary>
    uint dragStartID = InvalideID;

    /// <summary>
    /// �ӽ� ����(������ �巡�׳� ������ �и��� �� ���)
    /// </summary>
    TempItemSlotUI tempItemSlotUI;
    public TempItemSlotUI TempSlotUI => tempItemSlotUI;

    ItemSpliterUI itemSpliterUI;
    public ItemSpliterUI SpliterUI => itemSpliterUI;

    // �� ���� UI
    DetailInfoUI detail;
    public DetailInfoUI Detail => detail;

    // ������ ����
    TextMeshProUGUI goldText;

    // ��������Ʈ
    public Action OnInventoryOpen;
    public Action OnInventoryClose;

    // ����Ƽ �̺�Ʈ �Լ���
    private void Awake()
    {
        // ã�Ƴ���
        canvasGroup = GetComponent<CanvasGroup>();
        goldText = transform.Find("Gold").Find("GoldText").GetComponent<TextMeshProUGUI>();
        slotParent = transform.Find("ItemSlots");
        tempItemSlotUI = GetComponentInChildren<TempItemSlotUI>();
        detail = GetComponentInChildren<DetailInfoUI>();
        itemSpliterUI = GetComponentInChildren<ItemSpliterUI>();

        Button closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(Close);

        //inputActions = new Player_Wolf();
        
    }

    //private void OnEnable()
    //{
    //    inputActions.Inventory.Enable();
        
    //}
    //private void OnDisable()
    //{
    //    inputActions.Inventory.Disable();
    //}
    void Open()
    {
        inventoryOn = true;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnInventoryOpen?.Invoke();
    }
    void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if(!GameManager.INSTANCE.CAMERASWAP)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = false;
        inventoryOn = false;
        OnInventoryClose?.Invoke();
    }

    private void Start()
    {
        player = GameManager.INSTANCE.PLAYER.GetComponent<PlayerWolf>();
        player.MoneyChange += RefreshMoney;   // �÷��̾��� Money�� ����Ǵ� ����Ǵ� ��������Ʈ�� �Լ� ���
        RefreshMoney();             // ù ����

        Close();    // ������ �� ������ �ݱ�
    }

    public void InitializeInventory(Inventory newInven)
    {
        inven = newInven;   // �ٷ� �Ҵ�
        if( Inventory.Default_Inventory_Size != newInven.SlotCount)
        {
            // �⺻ ����UI ���� ����
            ItemSlotUI[] slots = GetComponentsInChildren<ItemSlotUI>();
            foreach(var slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // ���� �����
            slotUIs = new ItemSlotUI[inven.SlotCount];
            for (int i=0; i<inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);
                obj.name = $"{slotPrefab.name}_{i}";            // �̸� �����ְ�
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();
                slotUIs[i].Initialize((uint)i, inven[i]);       // �� ����UI�鵵 �ʱ�ȭ
            }

        }
        else
        {
            // ũ�Ⱑ ���� ��� ����UI���� �ʱ�ȭ�� ����
            slotUIs = slotParent.GetComponentsInChildren<ItemSlotUI>();
            for (int i = 0; i < inven.SlotCount; i++)
            {
                slotUIs[i].Initialize((uint)i, inven[i]);
            }
        }

        // TempSlot �ʱ�ȭ
        tempItemSlotUI.Initialize(Inventory.TempSlotID, inven.TempSlot);    // TempItemSlotUI�� TempSlot ����
        tempItemSlotUI.Close(); // tempItemSlotUI ����ä�� �����ϱ�
        //inputActions.Inventory.ItemDrop.canceled += tempItemSlotUI.OnDrop;

        // ItemSpliterUI �ʱ�ȭ(������ ���� ��� ����)
        itemSpliterUI.Initialize();
        itemSpliterUI.OnOkClick += OnSpliteOK;  // itemSpliterUI�� OK ��ư�� �������� �� ������ �Լ� ���

        RefreshAllSlots();  // ��ü ����UI ����

    }

    /// <summary>
    /// ��� ������ Icon�̹����� ����
    /// </summary>
    private void RefreshAllSlots()
    {
        foreach(var slotUI in slotUIs)
        {
            slotUI.Refresh();
        }
    }

    /// <summary>
    /// �÷��̾ ���� ���� ����
    /// </summary>
    /// <param name="money">ǥ�õ� �ݾ�</param>
    private void RefreshMoney()
    {
        goldText.text = $"{player.MONEY:N0}";  // Money�� ����� �� ����� �Լ�
    }

    /// <summary>
    /// �κ��丮 ���� �ݱ�
    /// </summary>
    public void InventoryOnOffSwitch()
    {
        if (canvasGroup.blocksRaycasts)  // ĵ���� �׷��� blocksRaycasts�� �������� ó��
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void ClearAllEquipMark()
    {
        foreach (var slot in slotUIs)
        {
            slot.ClearEquipMark();
        }
    }

    // ��������Ʈ�� �Լ� ---------------------------------------------------------------------------
    /// <summary>
    /// SpliterUI�� OK���� �� ����� �Լ�
    /// </summary>
    /// <param name="slotID">�������� ������ ID</param>
    /// <param name="count">���� ����</param>
    private void OnSpliteOK(uint slotID, uint count)
    {
        inven.TempRemoveItem(slotID, count);    // slotID���� count��ŭ ����� TempSlot�� �ű��
        tempItemSlotUI.Open();  // tempItemSlotUI ��� �����ֱ�
    }

    // �̺�Ʈ �ý����� �������̽� �Լ��� -------------------------------------------------------------

    /// <summary>
    /// �巡�� �߿� ����(OnBeginDrag, OnEndDrag�� ����Ϸ��� �ݵ�� �ʿ��ؼ� ���� ��)
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // OnBeginDrag, OnEndDrag�� ����Ϸ��� �ݵ�� �ʿ��ؼ� ���� ��

        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    tempItemSlotUI.transform.position = eventData.position;
        //}
    }

    /// <summary>
    /// �巡�� ���۽� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // ��Ŭ���� ���� ó��
        {
            // �ӽ� ���Կ� �������� ���� ���ø���UI�� �ȿ����� ��쿡�� ����(�������� ����� ��� �ִ� ��Ȳ)
            if (TempSlotUI.IsEmpty() && !SpliterUI.isActiveAndEnabled)
            {
                GameObject startObj = eventData.pointerCurrentRaycast.gameObject;   // �巡�� ������ ��ġ�� �ִ� ���� ������Ʈ ��������
                if (startObj != null)
                {
                    // �巡�� ������ ��ġ�� ���� ������Ʈ�� ������
                    //Debug.Log(startObj.name);
                    ItemSlotUI slotUI = startObj.GetComponent<ItemSlotUI>();    // ItemSlotUI ������Ʈ ��������
                    if (slotUI != null)
                    {
                        // ItemSlotUI ������Ʈ�� ������ ID ����� ����
                        //Debug.Log($"Start SlotID : {slotUI.ID}");
                        dragStartID = slotUI.ID;
                        inven.TempRemoveItem(dragStartID, slotUI.ItemSlot.ItemCount, slotUI.ItemSlot.ItemEquiped);   // �巡�� ������ ��ġ�� �������� TempSlot���� �ű�
                        tempItemSlotUI.Open();  // �巡�� ������ �� TempSlot ����
                        detail.Close();         // ������â �ݱ�
                        detail.IsPause = true;  // ������â �ȿ����� �ϱ�
                    }
                }
            }
        }
    }

    /// <summary>
    /// �巡�װ� ������ �� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // ��Ŭ���� ���� ó��
        {
            if (dragStartID != InvalideID)  // �巡�װ� ���������� ���۵Ǿ��� ���� ó��
            {
                GameObject endObj = eventData.pointerCurrentRaycast.gameObject; // �巡�� ���� ��ġ�� �ִ� ���� ������Ʈ ��������
                if (endObj != null)
                {
                    // �巡�� ���� ��ġ�� ���� ������Ʈ�� ������
                    //Debug.Log(endObj.name);
                    ItemSlotUI slotUI = endObj.GetComponent<ItemSlotUI>();  // ItemSlotUI ������Ʈ ��������
                    if (slotUI != null)
                    {
                        // ItemSlotUI ������Ʈ�� ������ Inventory.MoveItem() �����Ű��
                        //Debug.Log($"End SlotID : {slotUI.ID}");

                        // TempSlot�� �������� �巡�װ� ���� ���Կ� �ű��.
                        // ���� �巡�װ� ���� ���Կ� �������� �־��� ��� �� �������� TempSlot�� �̵�
                        inven.MoveItem(Inventory.TempSlotID, slotUI.ID);

                        // �巡�װ� ���� ���Կ� �ִ� �������� dragStartID �������� �ű��
                        inven.MoveItem(Inventory.TempSlotID, dragStartID);

                        detail.IsPause = false;                         // ������â �ٽ� ���� �� �ְ� �ϱ�
                        detail.Open(slotUI.ItemSlot.SlotItemData);      // ������â ����
                        dragStartID = InvalideID;                       // �巡�� ���� id�� �� �� ���� ������ ����(�巡�װ� �������� ǥ��)
                    }
                }

                if (tempItemSlotUI.IsEmpty())
                {
                    tempItemSlotUI.Close(); // �巡�׸� ������ tempSlot�� ������� �ݱ�
                }
            }
        }
    }

}
