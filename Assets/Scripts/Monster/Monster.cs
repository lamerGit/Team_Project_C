using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IBattle, IHealth
{
    //�÷��̾ �߰��ϴ� ���� ��ũ��Ʈ

    GameObject weapon;

    protected NavMeshAgent nav;
    protected Animator anim;

    

    public Transform target;

    public MonsterState state = MonsterState.Chase;
    public MonsterType type = MonsterType.Nomal;
    

    // ���ݿ�
    public float attackSpeed = 1.0f;
    public float attackCoolTime = 1.0f;
    public IBattle attackTarget;

    // HP��

    public float hp = 100.0f;
    float maxHP = 100.0f;

    //�̴ϸʿ�
    protected Vector3 quadPosition;
    protected Transform quad;

    public float HP
    {
        get => hp;
        private set
        {
            hp = Mathf.Clamp(value, 0.0f, maxHP);

            onHealthChange?.Invoke();
        }
    }

    public float MaxHP { get => maxHP; }

    public System.Action onHealthChange { get; set; }

    // ������

    public float attackPower = 10.0f;
    public float defencePower = 5.0f;

    public float AttackPower { get => attackPower; }
    public float DefencePower { get => defencePower; }

    // ���
    bool isDead = false;

    private void Awake()
    {
        weapon = GetComponentInChildren<FindWeapon>().gameObject;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        quad = transform.Find("Goblin_Quad");
    }

    // ���� �� Ÿ�� �÷��̾�� ��ȯ
    protected GameObject Player;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        target = Player.transform;
        if(type==MonsterType.Nomal)
        {
            maxHP = 100;
            HP = maxHP;
        }else if(type==MonsterType.Boss)
        {
            maxHP = 500;
            HP = maxHP;
        }
    }

    public virtual void Update()
    {
        switch (state)
        {
            case MonsterState.Chase:
                ChaseUpdate();
                break;
            case MonsterState.Attack:
                AttackUpdate();
                break;
            case MonsterState.Dead:
            default:
                break;
        }
        quadPosition = new Vector3(quad.position.x, transform.position.y, quad.position.z); //�̴ϸʰ�����
        quad.transform.LookAt(quadPosition); //�̴ϸʰ�����
    }


    /// <summary>
    /// �÷��̾� �߰ݿ� �Լ�
    /// </summary>
    public void ChaseUpdate()
    {
        nav.SetDestination(target.position);
        return;
    }

    /// <summary>
    /// �÷��̾� ���ݿ��Լ�
    /// </summary>
    /// <returns>������Ÿ���� 0.0f���� �۾����� �ִϸ��̼���� ��Ÿ���ʱ�ȭ Attack�Լ� ����</returns>
    public virtual void AttackUpdate()
    {
        if (type != MonsterType.Boss)
        {
            attackCoolTime -= Time.deltaTime;

            if (attackCoolTime < 0.0f)
            {
                anim.SetTrigger("Attack");
                Attack(attackTarget);
                attackCoolTime = attackSpeed;
                return;
            }
        }


    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)    // �׽�Ʈ�Ҹ��� ����
        {
            anim.SetTrigger("TakeDamage");
            return;
        }
        if (other.gameObject.CompareTag("Player"))  // ���Ͱ� �÷��̾ �����ϱ�
        {
            //attackTarget = other.GetComponent<IBattle>();     // �ݶ��̴��� ������ �ٷ� ���ݹߵ�
            ChangeState(MonsterState.Attack);
            return;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ChangeState(MonsterState.Chase);
            return;
        }

    }

    /// <summary>
    /// ���º�ȭ�Լ�
    /// </summary>
    /// <param name="newState">���ο����ǥ��</param>
    public void ChangeState(MonsterState newState)
    {
        if (isDead)
        {
            return;
        }

        switch (state) //���� ���¸� �����鼭 �ؾ�����
        {
            case MonsterState.Chase:
                nav.isStopped = true;
                break;
            case MonsterState.Attack:
                nav.isStopped = true;
                attackTarget = null;
                break;
            case MonsterState.Dead:
                nav.isStopped = true;
                isDead = false;
                break;
            default:
                break;
        }
        switch (newState) // ���ο� ���·� ���鼭 �ؾ�����
        {
            case MonsterState.Chase:
                nav.isStopped = false;
                break;
            case MonsterState.Attack:
                nav.isStopped = true;
                attackCoolTime = attackSpeed;
                break;
            case MonsterState.Dead:
                DiePresent();
                break;
            default:
                break;
        }
        state = newState; //���ο� ���·� ����
        anim.SetInteger("MonsterState", (int)state); // ���ο���¿� ���� �ִϸ��̼� ����
    }

    /// <summary>
    /// �׾����� ���Ǵ� �Լ�
    /// </summary>
    public void DiePresent()
    {
        //gameObject.layer = LayerMask.NameToLayer("Corpse");
        anim.SetBool("Dead", true);
        anim.SetTrigger("Die");
        isDead = true;
        nav.isStopped = true;
        nav.velocity = Vector3.zero;
        HP = 0;
        StartCoroutine(DeadEffect());
    }

    /// <summary>
    /// �������̽� IBattle�� �����ϱ����� �Լ�
    /// </summary>
    /// <param name="target">�����ϱ� ���� Ÿ��</param>
    public void Attack(IBattle target)
    {
        if (target != null)
        {
            float damage = AttackPower;
            target.TakeDamage(damage);
        }
    }
    /// <summary>
    /// �������̽� IBattle�� ���ݹޱ� ���� �Լ�
    /// </summary>
    /// <param name="damage">���� ������</param>
    public void TakeDamage(float damage,int type=0)
    {
        float finalDamage = damage - defencePower;
        if (finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }
        HP -= finalDamage;

        if (HP > 0.0f)
        {
            if (type == 1)
            {
                anim.SetTrigger("TakeDamage");
                attackCoolTime = attackSpeed;
            }
        }
        else
        {
            Die();
        }
        //Debug.Log($"MonsterHP : {hp}");
    }

    /// <summary>
    /// �׾����� ���Ǵ� �Լ�
    /// </summary>
    public void Die()
    {
        if (isDead == false)
        {
            GameManager.INSTANCE.MONSTERLIVECOUNT--;
            ChangeState(MonsterState.Dead);
        }
    }

    /// <summary>
    /// ������ ���Ǵ� �Լ�
    /// </summary>
    /// <returns>1.0f�� �����</returns>
    IEnumerator DeadEffect()
    {
        yield return new WaitForSeconds(1.0f);
        float r = Random.Range(0f, 1.0f);
        if(r<0.1f)
        {
            ItemFactory.MakeItem(ItemIDCode.Trap, new(transform.position.x, 1.0f, transform.position.z), true);
        }
        else if(r<0.2f)
        {
            ItemFactory.MakeItem(ItemIDCode.HP_potion, new(transform.position.x, 1.0f, transform.position.z), true);
        }


        Collider[] colliders = GetComponents<Collider>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        nav.enabled = false;
        gameObject.SetActive(false);
        //Destroy(this.gameObject, 1.0f);
    }
    /// <summary>
    /// �������̽� IHealth�� ȸ���Ҷ� ����� �Լ�
    /// </summary>
    /// <param name="heal">ȸ���� ��</param>
    public void TakeHeal(float heal)
    {
        Debug.Log("���� ����");
    }
}
