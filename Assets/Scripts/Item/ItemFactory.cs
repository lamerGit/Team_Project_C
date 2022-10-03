using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������ Ŭ����(������)
/// </summary>
public class ItemFactory
{
    static int itemCount = 0;   // �̶����� ������ �� ������ ����. (�� �����ۺ� ���� ���̵� �뵵�� ���)

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="code">������ �������� ����</param>
    /// <returns>������ ���ӿ�����Ʈ</returns>
    public static GameObject MakeItem(ItemIDCode code)
    {
        GameObject obj = new GameObject();              // �� ������Ʈ ����� ((0,0,0)�� ������)
        Item item = obj.AddComponent<Item>();           // Item ������Ʈ �߰�

        item.data = GameManager.INSTANCE.ItemData[code];    // ItemData ����
        string[] itemName = item.data.name.Split("_");  // ���� �����ϴ� ������ �°� �̸� ����
        obj.name = $"{itemName[0]}_{itemCount}";        // ���� ���̵� �߰�
        obj.layer = LayerMask.NameToLayer("Item");      // ���̾� ����
        SphereCollider col = obj.AddComponent<SphereCollider>();    // �ڵ�� �ö��̴� �߰�
        col.radius = 0.5f;
        col.isTrigger = true;
        itemCount++;    // ������ ������ ���� �������Ѽ� �ߺ��� ������ ó��

        GameObject indicator = Resources.Load<GameObject>("MinimapItemIndicator_Item"); // ���ҽý� �������� �������� �ε�
        if (indicator != null)  // �ε��� �ȵǾ��� ���� ����ؼ� �߰�
        {
            // ���带 ������ ���� 90�� ȸ��
            GameObject.Instantiate(indicator,
                obj.transform.position, obj.transform.rotation * Quaternion.Euler(90, 0, 0), obj.transform);
        }

        return obj;     // �����Ϸ�� �� ����
    }

    /// <summary>
    /// ������ ���� ��ġ�� ��¦ ������Ű ���� �Լ�
    /// </summary>
    /// <param name="code">������ ������</param>
    /// <param name="position">������ ��ġ</param>
    /// <param name="randomNoise">false�� ��Ȯ�� position�� ����. true�� position���� ��¦ ��ġ ����</param>
    /// <returns></returns>
    public static GameObject MakeItem(ItemIDCode code, Vector3 position, bool randomNoise = false)
    {
        GameObject obj = MakeItem(code);
        if (randomNoise)
        {
            Vector2 noise = Random.insideUnitCircle * 0.5f;
            position.x += noise.x;
            position.z += noise.y;
        }
        obj.transform.position = position;

        return obj;
    }

    /// <summary>
    /// �������� ������ �����ϱ� ���� �Լ�
    /// </summary>
    /// <param name="code">������ ������</param>
    /// <param name="position">������ �������� ��ġ</param>
    /// <param name="count">������ ����</param>
    public static void MakeItems(ItemIDCode code, Vector3 position, uint count)
    {
        for (int i = 0; i < count; i++)
        {
            MakeItem(code, position, true);
        }
    }

    public static GameObject MakeItem(uint id)
    {
        return MakeItem((ItemIDCode)id);
    }

    public static GameObject MakeItem(uint id, Vector3 position, bool randomNoise = false)
    {
        return MakeItem((ItemIDCode)id, position, randomNoise);
    }

    public static void MakeItems(uint id, Vector3 position, uint count)
    {
        MakeItems((ItemIDCode)id, position, count);
    }
}
