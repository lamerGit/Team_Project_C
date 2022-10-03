using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse_Control : MonoBehaviour
{
    //���콺�� �Ѿƴٴϸ鼭 ������ Ÿ���� ��ġ���ִ� ��ũ��Ʈ

    Vector3 mousePos;
    
    public GameObject Tower = null;
    public GameObject Tower2 = null;

    GameObject[] ChildObejct;

    int TowerNumber = 0;

    public Camera MainCamera;

    PlayerWolf player;

    float mouseHeight = 1.5f;

    CheckTowerPosition[] checkTowerPosition=new CheckTowerPosition[4];

    int[] towerPrice=new int[2] {100,200};


    CircleLine circleLine;
    private void Awake()
    {
        ChildObejct = new GameObject[transform.childCount];
        for(int i = 0; i < ChildObejct.Length; i++)
        {
            ChildObejct[i] = transform.GetChild(i).gameObject;
        }

        int index = 0;
        for (int i = ChildObejct.Length - 1; i >= ChildObejct.Length - 4; i--)
        {
            checkTowerPosition[index] = ChildObejct[i].GetComponent<CheckTowerPosition>();
            index++;
        }

        circleLine = transform.Find("Tower_Line").GetComponent<CircleLine>();
        circleLine.radius = 4.0f*2.0f;
        circleLine.CirclePoint();

    }

    private void Start()
    {
        player = GameManager.INSTANCE.PLAYER.GetComponent<PlayerWolf>();
    }

    /// <summary>
    /// ���콺�� ��ġ�� �޾ƿ��� aŰ�� �������� ����+�ٸ�Ÿ���� ��ó�� ���� ���� ������ ��ġ
    /// </summary>
    void Update()
    {

        mousePos = Mouse.current.position.ReadValue();

        mousePos.z = Camera.main.farClipPlane;

        Keyboard k = Keyboard.current;



        Ray cameraRay = MainCamera.ScreenPointToRay(mousePos);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            pointTolook.y = mouseHeight;
            transform.position = pointTolook;

        }

        if (k.aKey.wasPressedThisFrame && WallCheck() && !TowerCheck())
        {


            if (TowerNumber == 0 && player.MONEY >= towerPrice[TowerNumber])
            {
                GameObject T = Instantiate(Tower);
                T.transform.position = new Vector3(transform.position.x, mouseHeight + 3.5f, transform.position.z);
                player.MONEY -= towerPrice[TowerNumber];
            }
            else if (TowerNumber == 1 && player.MONEY >= towerPrice[TowerNumber])
            {
                GameObject T = Instantiate(Tower2);
                T.transform.position = new Vector3(transform.position.x, mouseHeight + 3.5f, transform.position.z);
                player.MONEY -= towerPrice[TowerNumber];
            }
        }

        CheckColor();

    }

    /// <summary>
    /// Ÿ���� ��ġ�� �� ������ �ʷϻ� �ƴϸ� �������� ǥ�����ִ� �Լ�
    /// </summary>
    private void CheckColor()
    {
        if (WallCheck() && !TowerCheck() && player.MONEY >= towerPrice[TowerNumber])
        {

            ChildObejct[TowerNumber].transform.Find("Tower_Quad").GetComponent<Renderer>().material.SetColor("_Color", Color.green);

        }
        else
        {
            ChildObejct[TowerNumber].transform.Find("Tower_Quad").GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }
    }

    /// <summary>
    /// Ÿ���� ground���� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns>���� ������ true �ƴϸ� false</returns>
    bool WallCheck()
    {
        bool result = false;
        if (checkTowerPosition[0].WallState && checkTowerPosition[1].WallState && checkTowerPosition[2].WallState && checkTowerPosition[3].WallState)
        {
            result = true;
        }

        return result;
    }
    /// <summary>
    /// ��ó�� Ÿ���� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns>��ó�� ������ true �ƴϸ� false</returns>
    bool TowerCheck()
    {
        bool result = false;
        if (checkTowerPosition[0].TowerZone || checkTowerPosition[1].TowerZone || checkTowerPosition[2].TowerZone || checkTowerPosition[3].TowerZone)
        {
            result = true;
        }

        return result;
    }

    

    /// <summary>
    /// ��ư�� ������ ��� ������ ��ư�� �����ϰ� �������� ��Ȱ��ȭ �ؼ� ������ ��ư�� ������Ʈ�� ���̰��ϴ� �Լ�
    /// </summary>
    /// <param name="number">Ȱ��ȭ �ؾ��ϴ� Ÿ���� ��ȣ</param>
    public void ObjectSwap(int number)
    {
        for (int i = 0; i < ChildObejct.Length; i++)
        {
            ChildObejct[i].SetActive(false);
        }
        TowerNumber = number;
        ChildObejct[number].SetActive(true);


        for(int i=ChildObejct.Length-1; i>=ChildObejct.Length-5; i--)
        {
            ChildObejct[i].SetActive(true);
        }

        if(TowerNumber==0)
        {
            circleLine.radius = 4.0f*2.0f;
            circleLine.CirclePoint();
        }else if(TowerNumber==1)
        {
            circleLine.radius = 7.0f*2.0f;
            circleLine.CirclePoint();
        }
    }
}
