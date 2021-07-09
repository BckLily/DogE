using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoomScr : ItemInfo
{


    // Start is called before the first frame update
    void Start()
    {
        // ������ �̵��ӵ�
        speed = 10f;

        // �������� �̵� ����
        // �÷��̾������� X���� ���� ���� �̵��Ѵ�
        moveDir = playerTr.position - tr.position;
        moveDir.x *= 2f;
        moveDir.Normalize();

        //StartCoroutine(CheckPos());

        // �������� ���ŵǴ� �ð�
        removeTime = 6f;
        Destroy(gameObject, removeTime);
    }

    // Update is called once per frame
    void Update()
    {
        itemMove();
    }

    protected override void GetItem()
    {
        gameMgr.GetBoomItem();
    }


}
