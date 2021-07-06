using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAtkSpeedUpScr : ItemInfo
{

    private float atkSpeedUp;

    // Start is called before the first frame update
    void Start()
    {
        //gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        //tr = GetComponent<Transform>();
        //playerTr = GameObject.Find()

        speed = 6.5f;

        tr = GetComponent<Transform>();
        moveDir = playerTr.position - tr.position;
        moveDir.y += 2f;
        moveDir.Normalize();

        atkSpeedUp = 0.01f;

        removeTime = 8f;

        Destroy(this.gameObject, removeTime);

    }


    // Update is called once per frame
    void Update()
    {
        itemMove();
    }
    
    protected override void GetItem()
    {
        playerTr.gameObject.GetComponent<PlayerCtrl>().atkSpeedUp += atkSpeedUp;


    }

}
