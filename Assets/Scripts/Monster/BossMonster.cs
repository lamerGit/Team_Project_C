using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMonster : Monster
{

    public override void Update()
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

    public override void AttackUpdate()
    {
        if (state == MonsterState.Attack)
        {
            anim.SetTrigger("Attack");
            int randomAttack = Random.Range(0, 10);
            //Debug.Log($"{randomAttack}");
            switch (randomAttack)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    AttackPatternA();   // �Ϲݰ��� 50%
                    break;
                case 5:
                case 6:
                    AttackPatternB();   // 4���� 20%
                    break;
                case 7:
                case 8:
                case 9:
                    AttackPatternC();   // ȸ������(2����) 30%
                    break;
            }
        }
    }

    void AttackPatternA()
    {
        anim.SetInteger("AttackType", 0);
        Attack(attackTarget);
        attackCoolTime = attackSpeed;
        return;
    }

    void AttackPatternB()
    {
        anim.SetInteger("AttackType", 1);
        Attack(attackTarget);
        attackCoolTime = attackSpeed;
        return;
    }

    void AttackPatternC()
    {
        anim.SetInteger("AttackType", 2);
        Attack(attackTarget);
        attackCoolTime = attackSpeed;
        return;
    }
}
