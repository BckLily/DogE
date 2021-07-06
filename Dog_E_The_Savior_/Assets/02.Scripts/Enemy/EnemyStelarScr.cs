using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStelarScr : EnemyInfo
{

    float enemyMaxLeftPos;

    // AtkSpeedUp / BoomItem / DamageUp / ScoreUp / ShieldItem
    [SerializeField]
    GameObject[] items; // 아이템들을 저장해두는 리스트 변수
    enum ItemType
    {
        AtkSpeedUp = 0, BoomItem, DamageUp, ScoreUp, ShieldItem,
    }


    // Start is called before the first frame update
    void Start()
    {
        // Stelar Enemy 위치 저장
        tr = GetComponent<Transform>();

        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // Stelar Enemy 이동 속도
        speed = 4f;
        // Stelar Enemy HP
        hp = 10 + Random.Range(1, 11) + Random.Range(1, 11);
        // hp *= stageCorr; // 스테이지의 진행에 따라 hp에 대한 보정이 들어간다. correction

        // Stelar Enemy가 기본적으로 움직이는 방향
        moveDir = Vector3.left;

        // Stelar Enemy가 발사할 총알
        string _bulletPath = "Prefabs/Bullets/Bullet_Stelar";
        bullet = Resources.Load<GameObject>(_bulletPath);

        string _itemsPath = "Prefabs/Items/";
        items = Resources.LoadAll<GameObject>(_itemsPath);



        // 총알 발사 딜레이
        fireDelay = 1.25f;
        // 총알 발사 경과 시간
        fireTime = 0f;

        // 상하 이동 속도
        updownSpeed = 1.75f;
        // 상하 이동 경과 시간
        updownTime = 0f;
        // 상하 이동 상태 변경 딜레이
        updownDelay = 0.55f;

        if (tr.position.y >= 0f)
        {
            // 상하 이동 상태
            updownState = Vector3.up;
        }
        else
        {
            updownState = Vector3.down;
        }


        // Enemy의 타입이 노말(기본 몹)으로 설정
        enemyType = EnemyType.normal;


        // 적이 이동할 수 있는 최대 왼쪽 좌표
        enemyMaxLeftPos = -12f;
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy의 움직임 함수
        EnemyMove();

        // Enemy 공격
        Fire();


    }

    void Fire()
    {
        // 마지막 발사 시간을 증가시킨다.
        fireTime += Time.deltaTime;

        // 마지막 발사 시간이 딜레이 시간보다 커지면
        if (fireTime >= fireDelay)
        {
            // 딜레이 시간만큼 감소
            fireTime -= fireDelay;

            // 총알의 생성 위치를 Enemy의 위치와 동일하게 하고
            // Z축을 1 높여서 Enemy가 총알에 가려지지 않게 한다.
            Vector3 _bulletPos = tr.position;
            _bulletPos.z += 1;

            Instantiate(bullet, _bulletPos, Quaternion.identity);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어의 총알과 부딪혔을 경우
        if (collision.CompareTag("BULLET"))
        {
            // 체력을 플레이어의 공격력 만큼 감소시킨다.
            hp -= gameMgr.GetBulletDamage();

            Destroy(collision.gameObject);

            // Stelar의 체력이 0이하일 경우
            if (hp <= 0)
            {
                gameMgr.PlayerDamageUp(incDamageList[(int)enemyType]);

                float rand = Random.Range(0.0f, 10.0f);
                MakeItem(rand);

                // 제거한다.    
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("PLAYER"))
        {
            // 플레이어와 충돌했다는 것을 알려준다.
            gameMgr.PlayerHit();
        }

    }

    void MakeItem(float num)
    {
        GameObject item = new GameObject();
        Debug.Log("Random: " + num);


        if (num >= 9.5f)
        {
            item = items[(int)ItemType.BoomItem];
        }
        else if (num >= 8.5f)
        {
            item = items[(int)ItemType.ShieldItem];
        }
        else if (num >= 7f)
        {
            item = items[(int)ItemType.AtkSpeedUp];
        }
        else if (num >= 4f)
        {
            item = items[(int)ItemType.DamageUp];
        }
        else if(num >= 0f)
        {
            item = items[(int)ItemType.ScoreUp];
        }

        Debug.Log(item.name);

        Instantiate(item, tr.position, Quaternion.identity);
    }


    // Up Down State 변경 수정 전 함수
#if false

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
#endif


    // Enemy를 움직이는 함수
    protected override void EnemyMove()
    {
        horiMove();
        updownMove();

        if (tr.position.x <= enemyMaxLeftPos)
        {
            Destroy(this.gameObject);
        }
    }

    // 왼쪽으로 이동하는 함수.
    protected override void horiMove()
    {
        // Enemy 위치 이동
        tr.position += moveDir * speed * Time.deltaTime;
    }

    // Enemy가 위아래로 움직이는 함수
    protected override void updownMove()
    {
        updownTime += Time.deltaTime;

        tr.position += updownState * updownSpeed * Time.deltaTime;

        // 현재의 방향으로 움직이기시작한 시간이
        // 딜레이 시간(변경 시간)보다 커지면 움직이는 방향 변경
        if (updownTime >= updownDelay)
        {
            updownTime -= updownDelay;

            updownState *= -1f;

        }

    }



}
