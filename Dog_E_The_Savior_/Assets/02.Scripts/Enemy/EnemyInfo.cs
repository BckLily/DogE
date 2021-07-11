using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyInfo : MonoBehaviour
{
    protected Transform tr; // 적 캐릭터의 Transform

    protected GameManager gameMgr; // 게임 매니저 오브젝트

    [Header("- Enemy State")]
    [SerializeField] 
    protected float speed; // 적 캐릭터 이동 속도
    [SerializeField]
    protected float hp;    // 적 캐릭터 체력
    protected enum DiceCount // 적 캐릭터 체력 설정 시 주사위 굴리는 횟수
    {
        normal = 2, middleBoss = 5, Boss = 6,
    }
    protected enum MaxHPperDice // 적 캐릭터 체력 설정 시 주사위 한 번당 최대 체력
    {
        normal = 10, middleBoss = 200, Boss = 500,
    }


    //[Header("- Enemy Up Down State")]
    //[SerializeField]
    protected float updownSpeed; // 상하 이동 속도
    //[SerializeField]
    protected float updownTime; // 상하 이동 동작 경과 시간
    //[SerializeField]
    protected float updownDelay; // 상하 이동 동작 변경 딜레이
    //[SerializeField]
    protected Vector3 updownState; // 상하 이동 상태

    protected Vector3 moveDir;    // 적 캐릭터 이동 방향

    //[Header("- Enemy Bullet")]
    //[SerializeField]
    protected GameObject bullet;    // 적 캐릭터 총알
    //[SerializeField]
    protected float fireDelay; // 적 캐릭터 총알 발사 딜레이
    protected float fireTime; // 마지막에 총알 발사 후 경과 시간.


    [Header("- Player Increase Damage")]
    [SerializeField]
    protected float[] incDamageList = { 0.5f, 1.25f, 2.5f };
    protected enum EnemyType
    {
        normal = 0, middleBoss, Boss,
    }
    [SerializeField]
    protected EnemyType enemyType;


    protected GameObject expAnim; // 폭발 애니메이션
    protected Vector3 expAnimScale; // 폭발 애니메이션의 크기,
                                    // 동일한 폭발 애니메이션을 사용하는데
                                    // 적의 종류에 따라 폭발 애니메이션의 크기를 다르게 사용한다.


    // AtkSpeedUp / BoomItem / DamageUp / ScoreUp / ShieldItem
    [SerializeField]
    protected GameObject[] items; // 아이템들을 저장해두는 리스트 변수
    protected enum ItemType
    {
        AtkSpeedUp = 0, BoomItem, DamageUp, ScoreUp, ShieldItem,
    }


    protected void Awake()
    {
        // 적 게임 오브젝트의 위치 저장
        tr = gameObject.GetComponent<Transform>();
        // 게임 매니저 저장.
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

    }

    // 적 캐릭터 체력 설정을 위한 주사위.
    protected float HpDice(int count, float maxHp)
    {
        float hp = 0f;

        for(int i = 0; i < count; i++)
            hp += Random.Range(0f, maxHp);
        

        return hp;
    }

    abstract protected void EnemyMove(); // 적 캐릭터의 이동
    abstract protected void updownMove(); // 적 캐릭터의 상하 이동
    abstract protected void horiMove(); // 적 캐릭터의 좌우 이동
    abstract protected void MakeItem(float num); // 적 캐릭터 사망시 아이템 생성



}
