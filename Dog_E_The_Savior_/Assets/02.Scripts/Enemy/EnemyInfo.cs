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

    [Header("- Enemy Up Down State")]
    [SerializeField]
    protected float updownSpeed; // ���� �̵� �ӵ�
    [SerializeField]
    protected float updownTime; // ���� �̵� ���� ��� �ð�
    [SerializeField]
    protected float updownDelay; // ���� �̵� ���� ���� ������
    [SerializeField]
    protected Vector3 updownState; // ���� �̵� ����


    protected Vector3 moveDir;    // �� ĳ���� �̵� ����
    [Header("- Enemy Bullet")]
    [SerializeField]
    protected GameObject bullet;    // �� ĳ���� �Ѿ�
    [SerializeField]
    protected float fireDelay; // �� ĳ���� �Ѿ� �߻� ������
    protected float fireTime; // �������� �Ѿ� �߻� �� ��� �ð�.





}
