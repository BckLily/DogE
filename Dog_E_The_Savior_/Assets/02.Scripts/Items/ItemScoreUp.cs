using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScoreUp : ItemInfo
{
    private float inc_Score; // 증가할 점수



    // Start is called before the first frame update
    void Start()
    {
        // 아이템이 움직이는 속도
        speed = 3f;


        // 아이템이 날아갈 방향 설정
        moveDir = playerTr.position - tr.position;
        moveDir.y -= 2f;
        moveDir.Normalize();

        inc_Score = 50f;


        //StartCoroutine(CheckPos());

        // 아이템이 사라질 시간
        removeTime = 10f;
        // 일정 시간 이후 사라지게 설정
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
