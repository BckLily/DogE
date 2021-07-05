using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStelarScr : EnemyInfo
{

    // Start is called before the first frame update
    void Start()
    {
        // Stelar Enemy 위치 저장
        tr = GetComponent<Transform>();

        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // Stelar Enemy 이동 속도
        speed = 3.5f;
        // Stelar Enemy HP
        hp = 10 + Random.Range(1, 11) + Random.Range(1, 11);
        // hp *= stageCorr; // 스테이지의 진행에 따라 hp에 대한 보정이 들어간다. correction

        // Stelar Enemy가 기본적으로 움직이는 방향
        moveDir = Vector3.left;

        // Stelar Enemy가 발사할 총알
        string _bulletPath = "Prefabs/Bullets/Bullet_Stelar";
        bullet = Resources.Load<GameObject>(_bulletPath);

        // 총알 발사 딜레이
        fireDelay = 1.25f;
        // 총알 발사 경과 시간
        fireTime = 0f;

        // 상하 이동 속도
        updownSpeed = 1f;
        // 상하 이동 경과 시간
        updownTime = 0f;
        // 상하 이동 상태 변경 딜레이
        updownDelay = 1f;
        // 상하 이동 상태
        updownState = Vector3.up;


    }

    // Update is called once per frame
    void Update()
    {
        tr.position += moveDir * speed * Time.deltaTime;

        Fire();

        updownMove();

    }

    void Fire()
    {
        fireTime += Time.deltaTime;

        if (fireTime >= fireDelay)
        {
            fireTime -= fireDelay;

            Vector3 _bulletPos = tr.position;
            _bulletPos.z += 1;

            Instantiate(bullet, _bulletPos, Quaternion.identity);
        }
    }


    void updownMove()
    {
        updownTime += Time.deltaTime;

        tr.position += updownState * updownSpeed * Time.deltaTime;

        if (updownTime >= updownDelay)
        {
            updownTime -= updownDelay;

            updownState *= -1f;    

        }


    }


    IEnumerator upState()
    {
        updownTime += Time.deltaTime;
        yield return new WaitForSeconds(Time.deltaTime);

        tr.position += Vector3.up * updownSpeed * Time.deltaTime;

        if (updownTime >= updownDelay)
        {
            updownTime -= updownDelay;

            StartCoroutine(downState());
            StopCoroutine(upState());
        }
    }

    IEnumerator downState()
    {
        updownTime += Time.deltaTime;
        yield return new WaitForSeconds(Time.deltaTime);

        tr.position += Vector3.down * updownSpeed * Time.deltaTime;

        if (updownTime >= updownDelay)
        {
            updownTime -= updownDelay;

            StartCoroutine(upState());
            StopCoroutine(downState());
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어의 총알과 부딪혔을 경우
        if (collision.CompareTag("BULLET"))
        {
            // 체력을 플레이어의 공격력 만큼 감소시킨다.
            hp -= gameMgr.GetBulletDamage();

            // Stelar의 체력이 0이하일 경우
            if (hp <= 0)
            {
                // 제거한다.    
                Destroy(gameObject);
            }
        }

    }

}
