using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Archer : MonoBehaviour
{
    //ȭ�� Ÿ���� ���� ��ũ��Ʈ

    public GameObject bullet = null;
    public float BulletSpeed = 10.0f;

    protected float BulletDelayMax = 1.0f;
    private float BulletDelay = 0.0f;

    public Transform BulletPoint = null;

    private Queue<GameObject> EnemyQueue = new Queue<GameObject>();
    GameObject target = null;

    bool isAttack = false;
    Animator anime;

    //�̴ϸʿ�
    private Vector3 quadPosition;
    Transform quad;


    GameObject towerLine;

    SphereCollider spherCollider;
    float towerRange = 0.0f;
    
    private void Awake()
    {
        anime = GetComponent<Animator>();
        quad = transform.Find("Tower_Quad");
        towerLine = transform.Find("Tower_Line").gameObject;
        spherCollider=GetComponent<SphereCollider>();
        towerRange = spherCollider.radius * 40;
       
    }

    private void Start()
    {
        GameManager.INSTANCE.towerSwapDelegate += LayerChange;
    }
    /// <summary>
    /// Ÿ����ġ��尡 �ƴ� ��������϶��� �ൿ
    /// </summary>
    private void FixedUpdate()
    {


        if (!GameManager.INSTANCE.CAMERASWAP)
        {
            if (BulletDelay < BulletDelayMax && !isAttack) 
            {
                BulletDelay += Time.fixedDeltaTime;
            }

            //EnemyQueue�� enemy�� �ְ� Ÿ���� ���� �� EnemyQueue���� Ÿ�ٿ� �Ҵ�
            if (EnemyQueue.Count > 0 && target == null) 
            {

                target = EnemyQueue.Dequeue();
                if (target.activeInHierarchy == false) //EnemyQueue�� �ִ� enemy�� �̹� �׾��ٸ� Ÿ���� null�� ����
                {
                    target = null;
                }
            }


            //Ÿ���� �����ϸ� Ÿ���� �ٶ�
            if (target != null) 
            {
                Vector3 LookDir = (target.transform.position - transform.position).normalized;
                LookDir.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(LookDir), Time.fixedDeltaTime * 10.0f);

                if (target.activeInHierarchy == false || (target.transform.position - transform.position).sqrMagnitude > towerRange)
                {
                    //Debug.Log((target.transform.position - transform.position).sqrMagnitude);
                    target = null;
                }

            }
            //Ÿ���� �����ϸ鼭 �����̰� ������Max�� �Ѿ�� ���ݾִϸ��̼� Ȱ��
            if (BulletDelay > BulletDelayMax && target != null)
            {
                isAttack = true;
                anime.SetTrigger("Attack");
                BulletDelay = 0.0f;

            }

            quadPosition = new Vector3(quad.position.x, transform.position.y, quad.position.z); //�̴ϸʰ�����
            quad.transform.LookAt(quadPosition); //�̴ϸʰ�����

            
        }
    }

    protected void LayerChange()
    {
        
        if(!GameManager.INSTANCE.CAMERASWAP)
        {
            int temp = LayerMask.NameToLayer("Player_Quad");
            quad.gameObject.layer = temp;
            towerLine.layer = temp;

        }else
        {
            int temp = LayerMask.NameToLayer("Tower_Quad");
            quad.gameObject.layer = temp;
            towerLine.layer = temp;

        }
    }
    /// <summary>
    /// �Ҵ���ִ� �ҷ��� �����Ͽ� Ÿ�ٹ����� �߻��ϴ� �Լ�
    /// </summary>
    public void Attack()
    {

        if (target != null)
        {
            GameObject b = Instantiate(bullet);
            b.transform.position = BulletPoint.transform.position;
            Vector3 dir = (target.transform.position - BulletPoint.transform.position).normalized;
            Rigidbody BulletRigid = b.GetComponent<Rigidbody>();
            BulletRigid.transform.LookAt(target.transform.position);
            dir.y += 0.1f;
            BulletRigid.velocity = dir * BulletSpeed;
        }
        isAttack = false;
    }

    //������ ���� ������ queue�� �ִ´�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            EnemyQueue.Enqueue(other.gameObject);

        }
    }
}
