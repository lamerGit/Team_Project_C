using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    //������ ������Ʈ�� ���� ��ũ��Ʈ
    float attackPower = 15.0f;
    //Rigidbody rb;
    //Collider col;
    public GameObject ps;

    private void Start()
    {
        //col = GetComponent<Collider>();
        //rb = GetComponent<Rigidbody>();
        StartCoroutine(Del());
    }

    //���� ������ �������� �ָ鼭 ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IBattle battle = other.GetComponent<IBattle>();
            if (battle != null)
            {
                battle.TakeDamage(attackPower);
            }
            //rb.velocity = Vector3.zero;
            //col.enabled = false;
            //transform.parent=other.transform;
            GameObject psObject= Instantiate(ps,transform.position,Quaternion.identity,null);
            psObject.GetComponent<ParticleSystem>().Play();
            
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// isTrigger�� �����߱⶧���� ���� ����Ͽ� ������ ���󰡴� ���� �����ϱ����� 3�ʵ� �ڵ����� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Del()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
