using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDamageUpScr : ItemInfo
{
    private float incDamage; // 증가할 플레이어 공격력

    // Start is called before the first frame update
    void Start()
    {
        //// 게임 오브젝트
        //gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        //// 아이템의 위치와 플레이어의 위치 설정
        //tr = GetComponent<Transform>();
        //playerTr = GameObject.Find("DogE").GetComponent<Transform>();

        // 아이템이 움직이는 속도
        speed = 5f;

        // 아이템이 날아갈 방향 설정
        moveDir = playerTr.position - tr.position;
        moveDir.Normalize();

        incDamage = 2f;

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
        gameMgr.PlayerDamageUp(incDamage);
    }


}
