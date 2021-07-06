using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScr : MonoBehaviour
{
    Transform tr; // 총알 오브젝의 위치

    public float speed; // 총알의 이동 속도
    string targetTag; // 총알의 타겟의 태그

    Vector3 moveDir; // 총알의 이동 방향

    [SerializeField]
    float removeTime; // 총알의 제거 시간

    public float bullet_damage; // 총알의 데미지

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        // 총알의 이동 속도 3f
        speed = 12f;
        // 총알의 타겟 태그 ENEMY
        targetTag = "ENEMY";

        // 총알이 이동하는 방향 오른쪽
        moveDir = Vector3.right;

        removeTime = 5f;

        // 총알이 생성되고 일정 시간이 지나면 제거됨.
        Destroy(this.gameObject, removeTime);

    }

    // Update is called once per frame
    void Update()
    {
        // 총알이 오른쪽으로 이동함.
        tr.position += moveDir * Time.deltaTime * speed;

    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.CompareTag(targetTag))
    //    {


    //        // Enemy와 부딪히면 제거됨.
    //        Destroy(this.gameObject);
    //    }

    //}


}
