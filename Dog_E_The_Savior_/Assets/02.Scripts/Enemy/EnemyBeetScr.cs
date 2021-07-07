using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetScr : EnemyInfo
{

    GameObject[] bullets; // 여러 종류의 총알을 사용할 것이기 때문에 새로운 배열을 만들었다.
    Transform firePos; // 총알이 발사될 포인트

    enum BulletType
    {
        normal = 0, chase = 1, 
    }

    bool isStart; // 보스가 원하는 중앙지점으로 이동을 했는가?

    [Header("- Boss Pattern")]
    [SerializeField]
    BossPattern bossPattern; // 보스 패턴 저장할 변수
    //Dictionary<> // 딕셔너리 타입을 사용하는 것이 더 효율적인가?
    enum BossPattern
    {
        stop = 0, shoot, spread , chaser, 
    }

    // stop 패턴은 시간으로 
    // shoot 패턴은 상하 반복 횟수 4번(0지나는 횟수 8번)
    // spread 패턴은 회전 횟수 5회(0 지나는 횟수 10번)
    // chaser 패턴은 회전 횟수 3회(360 지나는 횟수 3번)
    float patternCount; // 패턴의 진행도 확인
    int nextPattern; // 다음 패턴을 설정한다.

    float checkBeforeState; // 이전 Position, Rotation 등의 값을 Vector3로 저장하여 사용.

    Vector3 rotVector; // 회전 상태 설정 변수
    bool rotUp; // 위쪽으로 하느냐?
    float rotSpeed; // 회전 속도


    // Start is called before the first frame update
    void Start()
    {
        firePos = tr.Find("FirePos").GetComponent<Transform>();

        // Beet Enemy 이동 속도
        speed = 15f;
        
        // Beet Enemy 체력
        hp = 3000f + HpDice((int)DiceCount.Boss, (float)MaxHPperDice.Boss);
        // hp *= stageCorr; // 스테이지에 따른 보정.
        Debug.Log("Beet HP: " + hp);

        moveDir = Vector3.left; // 처음엔 맵 밖에서 생성이 되므로 맵 안으로 들어오기 위해서 왼쪽으로 이동한다.
        moveDir.Normalize();

        // Beet의 총알이 저장된 Path
        string _bulletPath = "Prefabs/Bullets/Beet/";
        // Path의 모든 총알을 불러온다.
        bullets = Resources.LoadAll<GameObject>(_bulletPath);
        // bullet을 기본 총알로 설정한다.
        bullet = bullets[(int)BulletType.normal];
        // 죽으면 생성할 아이템들. 여러 개를 생성할 예정.
        string _itemPath = "Prefabs/Items/";
        items = Resources.LoadAll<GameObject>(_itemPath);

        string _expAnimPath = "Prefabs/Animations/ExpAnim";
        expAnim = Resources.Load<GameObject>(_expAnimPath);
        expAnimScale = new Vector3(1f, 1f, 1f);


        fireDelay = 0.2f; // 기본 총알 발사 딜레이 >> IEnumerator로 만들어보자.
        fireTime = 0f; // 총알 발사 경과 시간

        updownDelay = 2f; // 상하 이동 변경 딜레이
        updownSpeed = 3.5f; // 상하 이동 속도
        updownTime = updownDelay / 2; // 상하 이동 경과 시간 >> 처음에 중앙에서 시작하기 때문에
                                      // 상하 이동 변경 딜레이의 절반의 값을 가지고 있어야 정상적으로 왕복하여 움직일 수 있다.

        updownState = Vector3.up; // 상하 이동은 위쪽부터 시작한다.

        // Enemy 타입을 보스로 설정.
        enemyType = EnemyType.Boss;

        rotVector = Vector3.zero; // 회전하지 않는다.
        rotUp = true; // 회전할 때 시계방향으로 회전한다.
        rotSpeed = 55f; // 회전 속도 설정


        isStart = false; // 원하는 장소로 이동하지 않았다.

        nextPattern = 0;
        bossPattern = BossPattern.shoot; // 일반 공격 패턴.
        checkBeforeState = tr.position.y; // 시작할 때 y축을 이전 상태로 저장.



        // 추가요소
        // 보스의 체력바


    }

    // Update is called once per frame
    void Update()
    {


        // 보스가 원하는 장소로 이동을 했을 때
        if (isStart == true)
        {
            BossPatternMove();
        }
        // 아직 원하는 장소로 이동하지 않았을 때
        else
        {
            StartPosSet();
        }

        //Debug.Log("ROT Y: " + tr.rotation.y);
        //Debug.Log("ROT Z: " + tr.rotation.z);


    }


    void BossPatternMove()
    {
        switch (bossPattern)
        {
            case BossPattern.stop:
                StopPattern();

                break;
            case BossPattern.shoot:
                ShootPattern();

                break;
            case BossPattern.spread:
                SpreadPattern();
                
                break;
            case BossPattern.chaser:
                ChaserPattern();

                break;
            default:
                Debug.LogError("Boss Pattern Value is Out of Index");
                break;
        }
    }

    // 이동에 대한 동작을 하기 전에 비교 값을 저장하고
    // 이동이 끝나면 공격 동작을 시행하고
    // 비교 값과 비교를 진행한다.


    void StopPattern()
    {
        patternCount += Time.deltaTime;
        // 2초가 지나면
        if (patternCount >= 2f)
        {
            if (nextPattern == 0)
            {
                // 다음 패턴이 0번 패턴이면 shoot

                moveDir = Vector3.zero; // 좌우 이동은 없다.
                updownState = Vector3.up; // 위아래 이동은 있다.
                updownTime = updownDelay / 2f; // 상하 이동 시간의 절반 값을 넣어 위로 움직이는 시간을 반으로 설정.
                checkBeforeState = tr.position.y; // 패턴을 몇 번 반복했는가를 y축의 변화 값으로 확인한다.
                bullet = bullets[(int)BulletType.normal]; // 총알을 기본 총알로 설정.
                fireDelay = 0.2f; // 총알 공격 속도 변경

                bossPattern = BossPattern.shoot;
            }
            // 다음 패턴이 1번이면 spread
            else if (nextPattern == 1)
            {
                moveDir = Vector3.zero; // 좌우 이동은 없다.
                updownState = Vector3.zero; // 위아래 이동은 있다.
                                            // 패턴을 몇 번 반복했는가를 z축 회전의 변화 값으로 확인한다.
                checkBeforeState = Mathf.Sin(Mathf.Deg2Rad * tr.rotation.eulerAngles.z);
                bullet = bullets[(int)BulletType.normal]; // 총알을 기본 총알로 설정
                fireDelay = 0.1f; // 총알 공격 속도 변경

                bossPattern = BossPattern.spread;
            }
            // 다음 패턴이 2번이면 chaser
            else if (nextPattern == 2)
            {
                moveDir = Vector3.zero;
                updownState = Vector3.zero;
                // 패턴을 몇 번 반복했는가를 y축의 회전의 변화 값으로 확인한다.
                // checkBeforeState를 변수 값으로 사용했다.
                // 변수를 사용해서 진행하기 때문에 현재 값과 이전 값의 비교는 진행되지 않는다.
                checkBeforeState = 0f;
                bullet = bullets[(int)BulletType.chase]; // 총알의 종류를 추적 미사일로 바꾼다 (homing missile)...
                fireDelay = 0.5f;// 총알 공격 속도 변경

                bossPattern = BossPattern.chaser;
            }
            // 다음 패턴이 3번이면 
            else if (nextPattern == 3)
            {

            }
            else if (nextPattern == 4)
            {

            }
            else
            {
                Debug.LogError("Next Pattern Value is Error.");
            }

            patternCount = 0;
        }
    }


    void ShootPattern()
    {
        // 4번 반복했으면(8번 값 바뀜)
        if(patternCount== 8)
        {
            // 혹시라도 y축 오차가 있을 수 있으니까 0으로 맞추어준다.
            Vector3 pos = new Vector3(tr.position.x, 0, tr.position.z);
            tr.position = pos;

            patternCount = 0;
            if (nextPattern == 0)
            {
                nextPattern = 1; // 첫 번째 패턴이 끝났다는 것을 stopPattern에 알려준다.
            }
            //else if (nextPattern == 4) // 추가적으로 원하는 패턴에 사용할 수 있다.

            bossPattern = BossPattern.stop; // 보스 패턴을 stop으로 변경한다.
            return;
        }

        // 현재의 y값을 다음에 비교할 y값으로 저장한다.
        checkBeforeState = tr.position.y;

        // 보스의 움직임 설정.
        EnemyMove();

        // 보스의 공격 설정.
        fireTime += Time.deltaTime;
        if(fireTime >= fireDelay)
        {
            fireTime -= fireDelay;
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);

        }



        // 이전 값과 지금 값이 0보다 작다는 것은 0을 지나서 양수, 음수가 바뀌었다는 뜻이다.
        if (checkBeforeState * tr.position.y <= 0)
        {
            //Debug.Log("Reverse");
            patternCount += 1;
        }
    }

    void SpreadPattern()
    {
        // 6번 반복하면 다음 패턴으로 넘어간다.
        if (patternCount == 8)
        {
            // Enemy의 rotation 값이 틀어졌을 수 있으므로 z축의 회전 값을 0으로 설정하여 기본 값으로 되돌린다.
            // 왜 오일러 각으로 했는지 모르겠네.
            Vector3 rot = new Vector3(tr.rotation.eulerAngles.x, tr.rotation.eulerAngles.y, 0);
            tr.rotation.eulerAngles.Set(rot.x, rot.y, rot.z);

            patternCount = 0;
            // 현재 패턴 두 번째 패턴이면
            if (nextPattern == 1)
            {
                //Debug.Log("Next Pattern 2");
                // 세 번째 패턴으로 넘어간다.
                nextPattern = 2;
            }

            bossPattern = BossPattern.stop; // 보스 패턴을 stop으로 변경한다.
            return;
        }

        // 현재 값과 다음 값을 비교를 위해 저장해 둔다.
        checkBeforeState = Mathf.Sin(Mathf.Deg2Rad * tr.rotation.eulerAngles.z);


        // 시계 방향으로 회전 중이면
        if (rotUp == true)
        {
            // 시계 방향으로 회전시킨다.
            tr.Rotate(Vector3.back * Time.deltaTime * rotSpeed, Space.Self);

            // 현재 회전은 -40~40 에서 이루어지고 있는데
            // 단순하게 tr.rotation.z의 값을 사용하여서 320 > tr.rotation.z를 하면
            // -40~0에서는 정상적으로 작동할 수 있으나
            // 더 진행해야하는 0~40 부분은 정상적으로 작동할 수 없다.
            // 따라서 sine 값을 사용하였다.
            // sine 값은 각도에 의한 y축의 높이 값이다.
            // 즉, -40~0의 sine 값은 음수가 나올 것이고
            // 0~40의 sine 값은 양수일 것이다.
            // 따라서 현재 각도를 sine으로 취한 값이 sine(-40) 즉 sine(320)보다 작아진 경우
            // 시계 방향으로 회전할 수 있는 최대 값인 것이고
            // 현재 각도를 sine으로 취했을 때 sine(40) 보다 커지면 반시계 방향으로 회전할 수 있는 최대 값이다.
            if (Mathf.Sin(tr.rotation.eulerAngles.z * Mathf.Deg2Rad) <= Mathf.Sin(320*Mathf.Deg2Rad))
            {
                // 회전 가능한 최대 값까지 갔으므로 반시계방향으로 만든다.
                rotUp = false;
            }
        }
        else
        {
            // 시계 반대 방향으로 회전시킨다.
            tr.Rotate(Vector3.forward * Time.deltaTime * rotSpeed, Space.Self);
            // 반시계 방향으로 최대 회전 가능한 값을 넘어서면
            if (Mathf.Sin(tr.rotation.eulerAngles.z * Mathf.Deg2Rad) >= Mathf.Sin(40 * Mathf.Deg2Rad))
            {
                // 시계 방향으로 만든다.
                rotUp = true;
            }
        }

        // 적의 기본적인 움직임. 상하 좌우
        EnemyMove();


        // 적 공격
        fireTime += Time.deltaTime;
        if (fireTime >= fireDelay)
        {
            fireTime -= fireDelay;
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);
        }



        // z축 회전 값이 0을 지나게 되면 이전과 지금의 sine 값을 곱하면 음수가 될 것이다.
        // 기본 tr.rotation.eulerAngles.z는 0~360의 양수이므로 그냥 곱하면 계속 양수가 될 것이다.
        if (checkBeforeState * Mathf.Sin(Mathf.Deg2Rad * tr.rotation.eulerAngles.z) <= 0)
        {
            patternCount += 1;
            //Debug.Log("REVERSE " + patternCount);
        }
    }

    void ChaserPattern()
    {
        // spread와는 다르게 변수 값을 사용해서 각도를 변화시키고
        // 특정 각도에 도달하면 횟수를 증가시켜서 원하는 횟수만큼 반복하게 한다.

        if (patternCount == 2)
        {
            Quaternion rot = Quaternion.Euler(new Vector3(tr.rotation.x, 0, tr.rotation.z));
            tr.rotation = rot;

            patternCount = 0;

            // 세 번째 패턴이면
            if (nextPattern == 2)
            {
                // 첫 번째 패턴으로 돌아간다.
                nextPattern = 0;
                // 만약 추가적인 패턴을 만들거나
                // 패턴을 섞어서 진행하고 싶다면 조건을 추가하여 진행할 수 있다.
            }

            // 멈춤 패턴으로 만들고 함수 종료한다.
            bossPattern = BossPattern.stop;
            return;
        }

        // 회전할 각도를 설정한다.
        checkBeforeState += Time.deltaTime * rotSpeed;


        // 새로운 변수에 회전할 각을 설정하여 tr.rotation에 대입한다.
        Quaternion value = Quaternion.Euler(new Vector3(tr.rotation.x, checkBeforeState, tr.rotation.z));
        tr.rotation = value;


        // 적 이동
        EnemyMove();


        // 적 공격
        // 마지막 공격 시간 증가
        fireTime += Time.deltaTime;
        // 공격 시간이 딜레이보다 커지면
        if (fireTime >= fireDelay)
        {
            // 딜레이만큼 다시 줄이고
            fireTime -= fireDelay;
            // bullet 생성.
            GameObject __bullet = Instantiate(bullet, firePos.transform.position, Quaternion.identity);
            // 유도탄이므로 피하기 쉽게 약간 위쪽으로 발사하도록 설정.
            __bullet.GetComponent<Rigidbody2D>().AddForce((-firePos.right + firePos.up).normalized * 2f, ForceMode2D.Impulse);
        }


        // 변수 값이 360을 넘으면 다시 360을 빼서 값을 작게 만든다.
        // 각도의 최대 크기는 360이기 때문
        if (checkBeforeState >= 360f)
        {
            checkBeforeState -= 360f;
            patternCount += 1;
        }
    }


    void StartPosSet()
    {
        // 현재의 위치의 x값이 6보다 크면
        if (tr.position.x > 6f)
        {
            // 왼쪽으로 이동
            tr.position += moveDir * 3f * Time.deltaTime;
        }
        // 6보다 작거나 같아지면
        else
        {
            // 이동 중 생길 수 있는 위치의 이탈을 방지하기 위해서 이동이 끝나면
            // 원하는 위치(x좌표 6)로 다시 보정한다.
            Vector3 pos = new Vector3(6f, tr.position.y, tr.position.z);
            tr.position = pos;
            moveDir = Vector3.zero;

            // 배치가 끝났으므로 공격 패턴이 시작된다.
            isStart = true;
        }
    }


    protected override void EnemyMove()
    {
        // 일반적으로 보스 캐릭터는 좌우 움직임이 없다.
        horiMove();
        // 일반적으로 상하 움직임을 하나, 공격 패턴마다 움직임 없이 회전만 할 수 있다.
        updownMove();

    }

    protected override void horiMove()
    {
        tr.position += moveDir * speed * Time.deltaTime;

    }

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

    // 아이템 생성.
    protected override void MakeItem(float num)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("BULLET"))
        {
            hp -= gameMgr.GetBulletDamage();


            Destroy(collision.gameObject);
            if (hp <= 0)
            {
                Instantiate(expAnim, tr.position, Quaternion.identity);


                Destroy(this.gameObject);
            }
        }

        if (collision.CompareTag("PLAYER"))
        {
            gameMgr.PlayerHit();
        }
    }



}
