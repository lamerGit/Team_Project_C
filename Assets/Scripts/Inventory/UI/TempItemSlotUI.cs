using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// �ӽ÷� �ѹ����� ���̴� ����
/// </summary>
public class TempItemSlotUI : ItemSlotUI
{
    PointerEventData eventData;

    /// <summary>
    /// Awake�� override�ؼ� �θ��� Awake ����ȵǰ� �����(base.Awake ����)
    /// </summary>
    protected override void Awake()
    {
        itemImage = GetComponent<Image>();  // �̹��� ã�ƿ���
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        equipMark = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        eventData = new PointerEventData(EventSystem.current);
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();    // ���콺 ��ġ�� ���缭 �ӽ� ���� �̵�        
    }

    /// <summary>
    /// �ӽ� ������ ���̵��� ����
    /// </summary>
    public void Open()
    {
        if (!ItemSlot.IsEmpty())    // ���Կ� �������� ������� ���� ����
        {
            transform.position = Mouse.current.position.ReadValue();    // ���̱� ���� ��ġ ����
            gameObject.SetActive(true); // ������ ���̰� �����(Ȱ��ȭ��Ű��)
        }
    }

    /// <summary>
    /// �ӽ� ������ ������ �ʰ� �ݱ�
    /// </summary>
    public void Close()
    {
        itemSlot.ClearSlotItem();       // ���Կ� ����ִ� �����۰� ���� ����
        gameObject.SetActive(false);    // ������ ������ �ʰ� �����(��Ȱ��ȭ��Ű��)
    }

    /// <summary>
    /// ������ ������� Ȯ��
    /// </summary>
    /// <returns>true�� ������ ����ִ�.</returns>
    public bool IsEmpty() => itemSlot.IsEmpty();

    /// <summary>
    /// �κ��丮 �ٱ��ʿ� �������� ������ �Լ�. �ӽ� ���Կ� �������� ����ְ� ���콺 ���� ��ư�� ������ �� ����.
    /// </summary>
    /// <param name="obj"></param>
    //public void OnDrop(InputAction.CallbackContext obj)
    //{
    //    Vector2 mousePos = Mouse.current.position.ReadValue();      // ���콺 ��ġ �޾ƿ���
    //    eventData.position = mousePos;
    //    List<RaycastResult> results = new();
    //    EventSystem.current.RaycastAll(eventData, results);         // �̺�Ʈ �ý����� �̿��� UI�� ����ĳ����
    //    if (results.Count < 1 && !IsEmpty())    // UI �߿� ����ĳ���õ� UI�� ���� �ӽ� ���Կ� �������� ����ִ�.
    //    {
    //        //Debug.Log("UI �ٱ��� ���");

    //        Ray ray = Camera.main.ScreenPointToRay(mousePos);
    //        // Ground ���̾ ����ִ� ������Ʈ�� ��ŷ(����ĳ����)�Ǿ����� Ȯ��
    //        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")))
    //        {
    //            //Debug.Log("�� ����ĳ��Ʈ ����");
    //            Vector3 pos = GameManager.INSTANCE.MainPlayer.ItemDropPosition(hit.point);      // ������ ����� ��ġ ���
    //            ItemFactory.MakeItems(ItemSlot.SlotItemData.id, pos, ItemSlot.ItemCount);   // �ӽ� ���Կ� ����ִ� ��� �������� ����

    //            if (itemSlot.ItemEquiped)  // ������� �������� ������ ��Ȳ�̸� ��� ����
    //            {
    //                GameManager.INSTANCE.MainPlayer.UnEquipWeapon();
    //            }

    //            Close();    // �ӽý���UI �ݰ� Ŭ�����ϱ�
    //        }
    //    }
    //}
}
