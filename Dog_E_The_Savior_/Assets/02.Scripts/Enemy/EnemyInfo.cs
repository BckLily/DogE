using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyInfo : MonoBehaviour
{
    protected Transform tr; // �� ĳ������ Transform

    protected GameManager gameMgr; // ���� �Ŵ��� ������Ʈ

    [Header("- Enemy State")]
    [SerializeField] 
    protected float speed; // �� ĳ���� �̵� �ӵ�
    [SerializeField]
    protected float hp;    // �� ĳ���� ü��

    //[Header("- Enemy Up Down State")]
    //[SerializeField]
    protected float updownSpeed; // ���� �̵� �ӵ�
    //[SerializeField]
    protected float updownTime; // ���� �̵� ���� ��� �ð�
    //[SerializeField]
    protected float updownDelay; // ���� �̵� ���� ���� ������
    //[SerializeField]
    protected Vector3 updownState; // ���� �̵� ����

    protected Vector3 moveDir;    // �� ĳ���� �̵� ����

    //[Header("- Enemy Bullet")]
    //[SerializeField]
    protected GameObject bullet;    // �� ĳ���� �Ѿ�
    //[SerializeField]
    protected float fireDelay; // �� ĳ���� �Ѿ� �߻� ������
    protected float fireTime; // �������� �Ѿ� �߻� �� ��� �ð�.


    [Header("- Player Increase Damage")]
    [SerializeField]
    protected float[] incDamageList = { 0.5f, 0.75f, 1.0f };
    protected enum EnemyType
    {
        normal = 0, middleBoss, Boss,
    }
    [SerializeField]
    protected EnemyType enemyType;

    abstract protected void EnemyMove();
    abstract protected void updownMove();
    abstract protected void horiMove();




}
