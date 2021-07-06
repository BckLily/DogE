using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    GameObject player; // 플레이어 게임 오브젝트
    Transform tr; // 플레이어 오브젝트 위치
    //SpriteRenderer playerImage; // 플레이어 오브젝트 이미지

    GameManager gameMgr; // 게임 매니저 오브젝트

    [Header("- Player Speed")]
    [SerializeField]
    float speed; // 플레이어의 이동 속
    float speed_fast; // 플레이어의 빠른 이동 속도
    float speed_slow; // 플레이어의 느린 이동 속도

    // 플레이어의 기본 이동 속도에서 배수를 사용해서 빠른 이동과 느린 이동을 사용할 수도 있기 때문에
    // 고정값을 사용할지 아니면 배수를 사용할지 생각.


    private float atkDamage; // 플레이어 공격력
    private float atkSpeed; // 플레이어 공격 속도 / 공격 딜레이
    private float lastAtkTime; // 플레이어가 마지막에 공격하고 지난 시간

    [Header("- Player Attack")]
    public float atkDamageUp; // 플레이어 공격력 증가.(변화 요소 영향)
    public float atkSpeedUp; // 플레이어 공격속도 증가.(아이템 영향)


    public int boomCount;  // 폭탄의 개수


    bool canDamaged; // 공격 받을 수 있는 상태인가?
    private float damagedDelay; // 무적 지속 시간 / 공격 받는 시간 딜레이

    GameObject bullet; // 플레이어가 공격시 생성할 bullet

    GameObject shield; // 플레이어의 실드 오브젝트
    public bool isShieldActive; // 플레이어의 실드가 활성화 상태인지 체크
    public float shieldActiveTime; // 실드의 지속시간


    List<GameObject> enemies; // 폭탄 사용시 제거될 ENEMY, ENEMTBULLET 들.

    bool isBoom; // 폭발 중인가

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 위치 저장
        tr = GetComponent<Transform>();
        // 플레이어 오브젝트 저장
        player = gameObject;
        // 플레이어 오브젝트 이미지 렌더러 저장
        // 무적 상태일 때 Active 상태 조절
        //playerImage = GetComponent<SpriteRenderer>();

        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // 플레이어 총알 오브젝트 위치
        var _bullet_Path = "Prefabs/Bullets/Bullet";
        bullet = Resources.Load<GameObject>(_bullet_Path);

        enemies = new List<GameObject>();

        // 플레이어의 이동속도
        // 이동속도는 3f 빠른 이동은 기본 이속 1.3 느린 이동은 기본 이속 0.7
        speed = 8f;
        speed_fast = speed * 1.5f;
        speed_slow = speed * 0.4f;

        // 플레이어의 기본 공격력 10
        atkDamage = 10f;
        // 플레이어의 공격 속도 최대 1초에 20번
        atkSpeed = 0.25f;
        lastAtkTime = atkSpeed;

        // 변화된 공격력 및 공격 속도 0
        atkDamageUp = 0f;
        atkSpeedUp = 0f;

        // 가지고 있는 폭탄의 개수
        boomCount = 1;
        isBoom = false;

        // 피격 가능한 상태인가
        canDamaged = true;
        // 무적 시간 설정 3초
        damagedDelay = 3f;

        // 플레이어의 child인 shield를 찾는다.
        shield = tr.Find("Shield").gameObject;
        // 초기 활성화 상태는 비활성화
        isShieldActive = true;
        shield.SetActive(isShieldActive);
        // 실드의 지속시간은 3초로 설정한다.
        shieldActiveTime = 3f;


    }

    private void FixedUpdate()
    {
        // 이동 방향을 결정할 변수
        var moveDir = Vector3.zero;
        var moveSpeed = speed;

        // 플레이어의 위 아래 이동을 결정
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector3.down;
        }
        // 플레이어의 앞 뒤 이동을 결정
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector3.right;
        }

        // 왼쪽 Shift를 누르면 빠르게 이동.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = speed_fast;
        }
        // Space Bar를 누르면 천천히 이동
        else if (Input.GetKey(KeyCode.Space))
        {
            moveSpeed = speed_slow;
        }


        // 플레이어의 위치 이동
        tr.position += moveDir.normalized * moveSpeed * Time.deltaTime;

    }

    // Update is called once per frame
    void Update()
    {
        // 공격 딜레이를 공격 속도에서 증가한 공격속도 만큼 뺌.
        var atkDelay = atkSpeed - atkSpeedUp;

        //Debug.Log("Attack Delay: " + atkDelay);

        // 공격 버튼인 J키를 누르면
        if (Input.GetKey(KeyCode.J))
        {
            // 마지막에 공격한 시간을 측정하고
            lastAtkTime += Time.deltaTime;
            // 공격 딜레이를 초과하면
            if(lastAtkTime >= atkDelay)
            {
                lastAtkTime -= atkDelay;
                // 발사.
                Fire();
            }
        }
        // J키를 떼면
        if (Input.GetKeyUp(KeyCode.J))
        {
            // 다음에 키를 눌렀을 때 바로 공격할 수 있게 설정함.
            lastAtkTime = atkDelay;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            UseBoom();
        }

        ActiveShield();

        if (isBoom)
        {
            RemoveEnemy();
        }
        //else
        //{
        //    StopCoroutine(RemoveEnemy());
        //}


    }

    // 총알을 발사하는 함수
    void Fire()
    {
        // 생성될 총알의 위치
        Vector3 bulletPos = tr.position;
        // z축을 1 더하는 것으로 플레이어 캐릭터보다 뒤에 생성되게하여 총알에 플레이어 캐릭터가 가려지지 않게 한다.
        bulletPos.z += 1;

        // 총알의 데미지를 설정.
        // 기본 공격력 + 추가된 공격력.
        //var b_damage = atkDamage + atkDamageUp;
        float b_damage = gameMgr.GetBulletDamage();

        // 총알을 생성하고 변수에 대입하여 사용할 수 있게 함.
        var _bullet = Instantiate(bullet, bulletPos, Quaternion.identity);
        // 총알의 공격력을 설정.
        _bullet.GetComponent<BulletScr>().bullet_damage = b_damage;

    }

    // 실드 상태를 조절하는 함수
    public void ActiveShield()
    {
        // 실드 활성화 시간이 0보다 크면
        if (shieldActiveTime > 0)
        {
            // 실드를 활성화 시킨다.
            shield.SetActive(isShieldActive);
            // 실드 활성화 시간을 감소시킨다.
            shieldActiveTime -= Time.deltaTime;
        }
        // 실드가 활성화 되어있고 활성화 시간이 0보다 작거나 같으면
        else if (isShieldActive == true)
        {
            // 활성화 시간을 0으로 만든다.
            shieldActiveTime = 0f;
            // 실드 활성화 상태를 비활성화로 만든다.
            isShieldActive = false;
            shield.SetActive(isShieldActive);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적의 총알과 충돌
        if (collision.CompareTag("E_BULLET"))
        {
            // 적의 총알을 없앤다.
            Destroy(collision.gameObject);

            // 실드가 비활성화일 경우
            if (shield.activeSelf == false)
            {
                // 플레이어가 맞았다는 것을 알려준다.
                gameMgr.PlayerHit();
            }
        }
    }

    void UseBoom()
    {
        // 폭발 애니메이션 넣으면 좋을 듯.

        // 가지고 있는 폭탄이 1개 이상일 경우
        if (gameMgr.boomCount > 0)
        {
            // 소지한 폭탄의 개수를 1개 줄인다.
            gameMgr.UseBoomItem();
            //Debug.Log("USE BOOM");

            // ENEMY Tag와 E_BULLET Tag를 가진 모든 GameObject를 검출한다.
            GameObject[] enemyTags = GameObject.FindGameObjectsWithTag("ENEMY");
            GameObject[] eBulletTags = GameObject.FindGameObjectsWithTag("E_BULLET");

            // 검출된 것들을 enemies 리스트에 추가한다.
            enemies.AddRange(enemyTags);
            enemies.AddRange(eBulletTags);


            // 전부 제거한다.
            isBoom = true;

        }
    }

    void RemoveEnemy()
    {
        int enemyCount = enemies.Count;

        if (enemyCount == 0)
        {
            isBoom = false;
        }

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        // 코루틴 처리해서 문제 없이 제거하는 방법은 없나...

        enemies.Clear();
    }

    
    //IEnumerator RemoveEnemy()
    //{
    //    yield return new WaitForSeconds(Time.deltaTime);
    //    int enemyCount = enemies.Count;
    //    if (enemyCount == 0)
    //    {
    //        isBoom = false;
    //        StopCoroutine(RemoveEnemy());
    //        yield return null;
    //    }
    //    Destroy(enemies[enemyCount - 1]);
    //    enemies.RemoveAt(enemyCount - 1);
    //}


}
