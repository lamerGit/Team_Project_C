using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    //�̴ϸ��� �÷��̾ �Ѿƴٴϰ� ���ִ� ��ũ��Ʈ

    //public float cameraSpeed = 5.0f;
    

    public GameObject player;

    /// <summary>
    /// ��� Update���ָ鼭 �÷��̾ ���󰣴�
    /// </summary>
    /// <param name="player">�÷��̾ �Ҵ�</param>
    private void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 80, 0);
    }
}
