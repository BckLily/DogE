using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScr : MonoBehaviour
{
    Transform tr; // 총알 오브젝트 위치

    public float speed; // 총알 오브젝트 속도
    string targetTag; // 총알의 타겟 태그

    Vector3 moveDir; // 총알의 이동 방향

    //[SerializeField]
    float removeTime; // 총알 제거 시간




    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        // 총알의 이동 속도
        speed = 8f;
        // 총알의 타겟 태그
        targetTag = "PLAYER";

        // 총알의 이동 방향
        moveDir = -tr.right;

        removeTime = 5f;

        // 총알 생성 이후 제거되는 시간
        Destroy(gameObject, removeTime);

   }

    // Update is called once per frame
    void Update()
    {
        // 총알이 이동한다.
        tr.position += moveDir * speed * Time.deltaTime;
    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("SHIELD"))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

}
