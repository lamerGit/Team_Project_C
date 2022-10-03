using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Virtual : MonoBehaviour
{
    Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = player.position;   //��ġ�� �÷��̾� �߽�
    }


    public void UpDownView(float sideAngle,float upDownAngle)
    {
        transform.eulerAngles = new Vector3(-upDownAngle, sideAngle, 0);    //ī�޶� ȸ��
    }
}
