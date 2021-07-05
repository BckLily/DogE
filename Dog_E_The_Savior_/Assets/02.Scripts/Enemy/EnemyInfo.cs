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

    [Header("- Enemy Up Down State")]
    [SerializeField]
    protected float updownSpeed; // 상하 이동 속도
    [SerializeField]
    protected float updownTime; // 상하 이동 동작 경과 시간
    [SerializeField]
    protected float updownDelay; // 상하 이동 동작 변경 딜레이
    [SerializeField]
    protected Vector3 updownState; // 상하 이동 상태


    protected Vector3 moveDir;    // 적 캐릭터 이동 방향
    [Header("- Enemy Bullet")]
    [SerializeField]
    protected GameObject bullet;    // 적 캐릭터 총알
    [SerializeField]
    protected float fireDelay; // 적 캐릭터 총알 발사 딜레이
    protected float fireTime; // 마지막에 총알 발사 후 경과 시간.





}
