using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ItemSpliterUI : MonoBehaviour
{
    /// <summary>
    /// ������ ���� ������ ����
    /// </summary>
    uint itemSplitCount = 1;

    /// <summary>
    /// �������� ������ ����UI
    /// </summary>
    ItemSlotUI targetSlotUI;

    /// <summary>
    /// ���� �Է� �� ǥ�ø� ���� UI(Input Field)
    /// </summary>
    TMP_InputField inputField;

    /// <summary>
    /// OK ��ư�� ������ �� ����� ��������Ʈ
    /// </summary>
    public Action<uint, uint> OnOkClick;

    /// <summary>
    /// ������ ���� ������ ������Ƽ(private)
    /// </summary>
    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            // ���� �Էµ� �� �ּҰ��� 1, �ִ밪�� (��󽽷��� ������ �ִ� ������ ���� - 1)�� �����ϴ� �ڵ�
            if (itemSplitCount != value)
            {
                itemSplitCount = value;
                itemSplitCount = (uint)Mathf.Max(1, itemSplitCount);    // 1�� �ּҰ�
                                                                        //(targetSlotUI.ItemSlot.ItemCount - 1)�� �ִ� ��.
                if (targetSlotUI != null)
                {
                    itemSplitCount = (uint)Mathf.Min(itemSplitCount, targetSlotUI.ItemSlot.ItemCount - 1);
                }
                inputField.text = itemSplitCount.ToString();
            }
        }
    }

    /// <summary>
    /// �ʱ�ȭ �Լ�(������Ʈ ã�� UI�� �Լ� ����)
    /// </summary>
    public void Initialize()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnInputChange);
        inputField.text = itemSplitCount.ToString();

        Button increase = transform.Find("IncreaseButton").GetComponent<Button>();
        increase.onClick.AddListener(OnIncrease);
        Button decrease = transform.Find("DecreaseButton").GetComponent<Button>();
        decrease.onClick.AddListener(OnDecrease);
        Button ok = transform.Find("OK_Button").GetComponent<Button>();
        ok.onClick.AddListener(OnOK);
        Button cancel = transform.Find("Cancel_Button").GetComponent<Button>();
        cancel.onClick.AddListener(OnCancelClick);

        Close();
    }

    /// <summary>
    /// ������ ����â ����.
    /// </summary>
    /// <param name="target">�������� ������ ��� ����</param>
    public void Open(ItemSlotUI target)
    {
        if (target.ItemSlot.ItemCount > 1)  // ��� ���Կ� �������� 1���� �ʰ��� ���� ����
        {
            targetSlotUI = target;  // ��� �������� ����
            ItemSplitCount = 1;     // �⺻ ������ 1 ����
            transform.position = target.transform.position; // �������� ���� ������ ��ġ�� ���ø���UI �ű��
            gameObject.SetActive(true); // �����ֱ�
        }
    }

    /// <summary>
    /// ������ ����â �ݱ�
    /// </summary>
    public void Close() => gameObject.SetActive(false);

    /// <summary>
    /// ���� ������Ű�� ��ư ������ �� ����� �Լ�
    /// </summary>
    private void OnIncrease()
    {
        Debug.Log("OnIncrease");
        ItemSplitCount++;
    }

    /// <summary>
    /// ���� ���ҽ�Ű�� ��ư ������ �� ����� �Լ�
    /// </summary>
    private void OnDecrease()
    {
        Debug.Log("OnDecrease");
        ItemSplitCount--;
    }

    /// <summary>
    /// OK ��ư ������ �� ����� �Լ�
    /// </summary>
    private void OnOK()
    {
        //Debug.Log("OnOK");

        OnOkClick?.Invoke(targetSlotUI.ID, ItemSplitCount); // ��������Ʈ�� ����� �Լ��� ����.(InventoryUI���� SpliterOK �Լ� ����)

        Close();    // �ݱ�
    }

    /// <summary>
    /// Cancel ��ư ������ �� ����� �Լ�
    /// </summary>
    private void OnCancelClick()
    {
        //Debug.Log("OnCancelClick");
        targetSlotUI = null;    // ������ �ʱ�ȭ�ϰ�
        Close();                // �ݱ�
    }

    /// <summary>
    /// InputField���� ���� ����� �� ����� �Լ�
    /// </summary>
    /// <param name="input">����� ��</param>
    private void OnInputChange(string input)
    {
        //Debug.Log($"OnInputChange : {input}");
        if (input.Length == 0)
        {
            ItemSplitCount = 0; // ""�� ��� 0���� ó��
        }
        else
        {
            ItemSplitCount = uint.Parse(input); // uint �Ľ��ؼ� ItemSplitCount�� ����
        }
    }
}
