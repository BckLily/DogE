using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ItemInfo : MonoBehaviour
{
    protected GameManager gameMgr; // 게임 매니저

    protected Transform tr; // 아이템의 위치
    protected Transform playerTr; // 플레이어의 위치

    protected CircleCollider2D col2D; //  콜라이더 활성화 비활성화

    protected float speed; // 아이템 이동 속도

    protected Vector3 moveDir; // 아이템이 이동하는 방향

    protected float removeTime; // 아이템 생성 후 사라지는 시간

    protected void itemMove()
    {
        tr.position += moveDir * speed * Time.deltaTime;
    }

    protected void Awake()
    {
        // 게임 오브젝트
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // 아이템의 위치와 플레이어의 위치 설정
        tr = this.gameObject.GetComponent<Transform>();

        GameObject player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        else
        {
            Vector3 pos = new Vector3(tr.position.x - 16f, tr.position.y, tr.position.z);
            playerTr.position = pos;
        }
        
        // 화면 밖에서 아이템이 생성되더라도 맵 안으로 들어갈 수 있게 콜라이더 껐다가
        // 나중에 다시 켠다.
        col2D = gameObject.GetComponent<CircleCollider2D>();
        col2D.enabled = false;
        StartCoroutine(CheckPos());

    }


    protected IEnumerator CheckPos()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            //Debug.Log(tr.position.x);
            //Debug.Log(tr.position.y);

            if (Mathf.Abs(tr.position.x) <= 7.5f && Mathf.Abs(tr.position.y) <= 4f)
            {
                //Debug.Log("GET IN");
                col2D.enabled = true;
                yield break;
            }
            else
            {
                //Debug.Log("GET OUT");
            }
        }
    }


    // 플레이어가 아이템을 얻었을 때 시행할 동작을 각각 스크립트 별로 작성.
    abstract protected void GetItem();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // 벽면 충돌 시 아이템 이동 방향 변경
        if (collision.CompareTag("WALL"))
        {
            if(collision.gameObject.name == "LeftWall" || collision.gameObject.name == "RightWall")
            {
                moveDir.x *= -1;
            }
            else
            {
                moveDir.y *= -1;
            }
        }

        if (collision.CompareTag("PLAYER"))
        {
            GetItem();

            //Debug.Log("Destroy this Game Object: " + this.gameObject.name);

            Destroy(this.gameObject);
        }
    }


}
