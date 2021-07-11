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
    protected enum DiceCount // �� ĳ���� ü�� ���� �� �ֻ��� ������ Ƚ��
    {
        normal = 2, middleBoss = 5, Boss = 6,
    }
    protected enum MaxHPperDice // �� ĳ���� ü�� ���� �� �ֻ��� �� ���� �ִ� ü��
    {
        normal = 10, middleBoss = 200, Boss = 500,
    }


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
    protected float[] incDamageList = { 0.5f, 1.25f, 2.5f };
    protected enum EnemyType
    {
        normal = 0, middleBoss, Boss,
    }
    [SerializeField]
    protected EnemyType enemyType;


    protected GameObject expAnim; // ���� �ִϸ��̼�
    protected Vector3 expAnimScale; // ���� �ִϸ��̼��� ũ��,
                                    // ������ ���� �ִϸ��̼��� ����ϴµ�
                                    // ���� ������ ���� ���� �ִϸ��̼��� ũ�⸦ �ٸ��� ����Ѵ�.


    // AtkSpeedUp / BoomItem / DamageUp / ScoreUp / ShieldItem
    [SerializeField]
    protected GameObject[] items; // �����۵��� �����صδ� ����Ʈ ����
    protected enum ItemType
    {
        AtkSpeedUp = 0, BoomItem, DamageUp, ScoreUp, ShieldItem,
    }


    protected void Awake()
    {
        // �� ���� ������Ʈ�� ��ġ ����
        tr = gameObject.GetComponent<Transform>();
        // ���� �Ŵ��� ����.
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

    }

    // �� ĳ���� ü�� ������ ���� �ֻ���.
    protected float HpDice(int count, float maxHp)
    {
        float hp = 0f;

        for(int i = 0; i < count; i++)
            hp += Random.Range(0f, maxHp);
        

        return hp;
    }

    abstract protected void EnemyMove(); // �� ĳ������ �̵�
    abstract protected void updownMove(); // �� ĳ������ ���� �̵�
    abstract protected void horiMove(); // �� ĳ������ �¿� �̵�
    abstract protected void MakeItem(float num); // �� ĳ���� ����� ������ ����



}
