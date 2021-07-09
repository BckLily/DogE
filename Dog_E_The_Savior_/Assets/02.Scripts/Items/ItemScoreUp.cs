using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScoreUp : ItemInfo
{
    private float inc_Score; // ������ ����



    // Start is called before the first frame update
    void Start()
    {
        // �������� �����̴� �ӵ�
        speed = 3f;


        // �������� ���ư� ���� ����
        moveDir = playerTr.position - tr.position;
        moveDir.y -= 2f;
        moveDir.Normalize();

        inc_Score = 50f;


        //StartCoroutine(CheckPos());

        // �������� ����� �ð�
        removeTime = 10f;
        // ���� �ð� ���� ������� ����
        Destroy(this.gameObject, removeTime);

    }

    // Update is called once per frame
    void Update()
    {
        itemMove();
    }
    
    protected override void GetItem()
    {
        gameMgr.GameScoreUp(inc_Score);

        //throw new System.NotImplementedException();
    }



}
