using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamageUpScr : ItemInfo
{
    private float incDamage; // ������ �÷��̾� ���ݷ�

    // Start is called before the first frame update
    void Start()
    {
        //// ���� ������Ʈ
        //gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        //// �������� ��ġ�� �÷��̾��� ��ġ ����
        //tr = GetComponent<Transform>();
        //playerTr = GameObject.Find("DogE").GetComponent<Transform>();

        // �������� �����̴� �ӵ�
        speed = 5f;

        // �������� ���ư� ���� ����
        moveDir = playerTr.position - tr.position;
        moveDir.Normalize();

        incDamage = 2f;

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
        gameMgr.PlayerDamageUp(incDamage);
    }


}
