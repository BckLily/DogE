using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMoveScr : MonoBehaviour
{
    public float speed; // 배경의 이동 속도
    public float scale; // 배경의 크기 비율 >> bg에는 사용하지 않고 cloud 같은 것에 사용

    Vector3 moveDir;    // 배경의 이동 방향

    float maxLeft;

    Transform tr;       // 오브젝트의 위치


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        speed = 2f; // 이동 속도 3f
        scale = 1; // 크기 1
        moveDir = Vector3.left; // 이동 방향은 왼쪽
        maxLeft = 19.47f; // 최대 왼쪽으로 갈 수 있는 거리 >> 19.47

        

        
    }

    private void FixedUpdate()
    {
        // 배경을 moveDir 방향으로 speed의 만큼 움직임
        tr.position += moveDir * speed * Time.deltaTime;

        
        // 제일 왼쪽으로 갔을 때
        if (tr.position.x <= -maxLeft)
        {
            // Tag가 배경이면
            if (this.gameObject.CompareTag("BACKGROUND"))
            {

                // 현재 위치에서 최대 왼쪽 값을 2번 더해주어서
                Vector3 movePos = tr.position;
                movePos.x += maxLeft * 2;

                // 새로운 위치를 설정해서 이동시킨다.
                // 배경이 연결되서 보이게 된다.
                tr.position = movePos;
            }
            // Tag가 구름이면
            else if (this.gameObject.CompareTag("CLOUD"))
            {
                // 오브젝트를 제거한다.
                Destroy(this.gameObject);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
