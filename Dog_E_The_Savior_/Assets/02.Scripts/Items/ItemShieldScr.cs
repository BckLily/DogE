using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShieldScr : ItemInfo
{

    // Start is called before the first frame update
    void Start()
    {
        // 아이템의 이동 속도
        speed = 5f;
        
        // 아이템이 이동하는 방향 (플레이어의 반대 방향)
        moveDir = tr.position - playerTr.position;
        moveDir.Normalize();


        //StartCoroutine(CheckPos());

        // 아이템이 사라질 시간
        removeTime = 12f;
        // 생성 후 일정 시간이 지나면 아이템이 사라진다.
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
