using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Knife : Tower_Archer
{
    //������Ÿ���� ���� ��ũ��Ʈ �ൿ�� Tower_Archer�� �����ϱ� ������ ��ӹ���

    //���ݼӵ����� ���̸� �ξ���
    private void Start()
    {
        BulletDelayMax = 0.3f;
        GameManager.INSTANCE.towerSwapDelegate += LayerChange;
    }
}
