using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShieldScr : ItemInfo
{

    // Start is called before the first frame update
    void Start()
    {
        // �������� �̵� �ӵ�
        speed = 5f;
        
        // �������� �̵��ϴ� ���� (�÷��̾��� �ݴ� ����)
        moveDir = tr.position - playerTr.position;
        moveDir.Normalize();


        //StartCoroutine(CheckPos());

        // �������� ����� �ð�
        removeTime = 12f;
        // ���� �� ���� �ð��� ������ �������� �������.
        Destroy(this.gameObject, removeTime);


    }

    // Update is called once per frame
    void Update()
    {
        itemMove();   
    }

    protected override void GetItem()
    {
        PlayerCtrl player = playerTr.GetComponent<PlayerCtrl>();
        player.isShieldActive = true;
        player.shieldActiveTime += 3f;
        

    }


}
