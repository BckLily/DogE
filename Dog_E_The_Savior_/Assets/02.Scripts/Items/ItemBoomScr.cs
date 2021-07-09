using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoomScr : ItemInfo
{


    // Start is called before the first frame update
    void Start()
    {
        // 아이템 이동속도
        speed = 10f;

        // 아이템의 이동 방향
        // 플레이어쪽으로 X축이 아주 조금 이동한다
        moveDir = playerTr.position - tr.position;
        moveDir.x *= 2f;
        moveDir.Normalize();

        //StartCoroutine(CheckPos());

        // 아이템이 제거되는 시간
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
