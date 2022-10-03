using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    //���͸� ���������ִ� ��ũ��Ʈ

    public GameObject Monster = null;
    public GameObject boss = null;
    int xPos;
    int zPos;
    public int monsterCount;
    public int maxMonsterCount = 20; //�ִ�� ��ȯ�� ���ͼ�
    int spawnerSigt;
    float spawnInterval = 1.0f;
    

    /// <summary>
    /// ���ӸŴ������� Ÿ����ġ���� �������� ���� ���� ��ȯ �ڷ�ƾ�� ��������� �Լ�
    /// </summary>
    /// <param name="Swap">true�̸� ��������</param>
    public void StartSpawn(bool Swap)
    {
        if (Swap)
        {
            StartCoroutine(Spawner());
        }
    }
    /// <summary>
    /// ���� �������ִ� IEnumerator
    /// </summary>
    /// <returns>spawnInterval�� ������ �ð��� ���� ��ȯ�� </returns>
    IEnumerator Spawner()
    {
        while (monsterCount<maxMonsterCount-1)
        {
            yield return new WaitForSeconds(spawnInterval);
            spawnerSigt = Random.Range(0, 4);
            if(spawnerSigt== 0)
            {
                GameObject mons = Instantiate(Monster, new Vector3(46, 0, 0), Quaternion.identity);
                monsterCount += 1;
            }
            if (spawnerSigt == 1)
            {
                GameObject mons = Instantiate(Monster, new Vector3(-46, 0, -10), Quaternion.identity);
                monsterCount += 1;
            }
            if (spawnerSigt == 2)
            {
                GameObject mons = Instantiate(Monster, new Vector3(0, 0, 46), Quaternion.identity);
                monsterCount += 1;
            }
            if (spawnerSigt == 3)
            {
                GameObject mons = Instantiate(Monster, new Vector3(0, 0, -46), Quaternion.identity);
                monsterCount += 1;
            }
            Debug.Log("��ȯ");
        }

        if (GameManager.INSTANCE.Wave == GameManager.INSTANCE.MaxWave)
        {

            spawnerSigt = Random.Range(0, 4);
            if (spawnerSigt == 0)
            {
                BossSpawn();
            }
            if (spawnerSigt == 1)
            {
                BossSpawn();
            }
            if (spawnerSigt == 2)
            {
                BossSpawn();
            }
            if (spawnerSigt == 3)
            {
                BossSpawn();
            }
        }
        

    }

    private void BossSpawn()
    {
        StartCoroutine(GameManager.INSTANCE.BossMasageOn());
        GameObject bossmons = Instantiate(boss, new Vector3(0, 0, -46), Quaternion.identity);
        monsterCount += 1;
        Debug.Log("������ȯ");
    }

    //IEnumerator MonsterSpawn()
    //{
    //    spawnerSigt = Random.Range(0, 4);

    //    while ( monsterCount < maxMonsterCount)
    //    {
    //        if (spawnerSigt == 0)
    //        {
    //            xPos = Random.Range(-46, 46);
    //            zPos = Random.Range(-46, -45);
    //            //Instantiate(monster, new Vector3(xPos, 0, zPos), Quaternion.identity);

    //            yield return new WaitForSeconds(spawnInterval);
    //            GameObject mons = Instantiate(Monster, new Vector3(xPos, 0, zPos), Quaternion.identity);
    //            monsterCount += 1;
    //        }
    //        if (spawnerSigt == 1)
    //        {
    //            xPos = Random.Range(-46, -45);
    //            zPos = Random.Range(46, -46);
    //            //Instantiate(monster, new Vector3(xPos, 0, zPos), Quaternion.identity);

    //            yield return new WaitForSeconds(spawnInterval);
    //            GameObject mons = Instantiate(Monster, new Vector3(xPos, 0, zPos), Quaternion.identity);
    //            monsterCount += 1;
    //        }
    //        if (spawnerSigt == 2)
    //        {
    //            xPos = Random.Range(-46, 46);
    //            zPos = Random.Range(46, 45);
    //            //Instantiate(monster, new Vector3(xPos, 0, zPos), Quaternion.identity);

    //            yield return new WaitForSeconds(spawnInterval);
    //            GameObject mons = Instantiate(Monster, new Vector3(xPos, 0, zPos), Quaternion.identity);
    //            monsterCount += 1;
    //        }
    //        if (spawnerSigt == 3)
    //        {
    //            xPos = Random.Range(45, 46);
    //            zPos = Random.Range(46, -46);
    //            //Instantiate(monster, new Vector3(xPos, 0, zPos), Quaternion.identity);

    //            yield return new WaitForSeconds(spawnInterval);
    //            GameObject mons = Instantiate(Monster, new Vector3(xPos, 0, zPos), Quaternion.identity);
    //            monsterCount += 1;
    //        }
    //    }
    //}
}
