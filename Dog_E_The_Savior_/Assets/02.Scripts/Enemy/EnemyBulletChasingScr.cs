using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletChasingScr : MonoBehaviour
{
    Transform tr; // 총알 오브젝트 위치
    Rigidbody2D rb2d; // 오브젝트의 rigidbody2d


    public float speed; // 총알 오브젝트 속도
    string targetTag; // 총알의 타겟 태그
    Transform targetTr; // 총알의 타겟의 Transform
    Vector3 targetPos; // 총알의 타겟의 위치

    Vector3 moveDir; // 총알의 이동 방향

    float removeTime; // 총알의 제거 시간

    // Start is called before the first frame update
    void Start()
    {
        // 총알 오브젝트 위치
        tr = gameObject.GetComponent<Transform>();
        // 총알 오브젝트 리지드바디
        rb2d = tr.GetComponent<Rigidbody2D>();
        // 타겟 태그
        targetTag = "PLAYER";
        // 플레이어 위치
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);


        // 플레이어가 있을 경우
        if (player != null)
        {
            // 타겟의 위치에 플레이어 위치 설정
            targetTr = player.GetComponent<Transform>();
            targetPos = targetTr.position;
        }
        else
        {
            
            // 플레이어가 없으므로 왼쪽으로 날아갈 수 있게 타겟 설정.
            Vector3 pos = new Vector3(tr.position.x - 20f, tr.position.y, tr.position.z);
            targetPos = pos;
        }

        //Debug.Log(targetPos);
        

        // 제거 시간 설정
        removeTime = 3f;
        // 이동 속도 설정
        speed = 2f;

        // 일정 시간 후에 제거됨.
        Destroy(this.gameObject, removeTime);
    }


    // Update is called once per frame
    void Update()
    {
        if (targetTr != null)
        {
            targetPos = targetTr.position;
        }

        moveDir = targetPos - tr.position;
        moveDir.Normalize();



        rb2d.AddForce(moveDir * speed, ForceMode2D.Force);
    }
}
