using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ItemInfo : MonoBehaviour
{
    protected GameManager gameMgr; // ���� �Ŵ���

    protected Transform tr; // �������� ��ġ
    protected Transform playerTr; // �÷��̾��� ��ġ

    protected float speed; // ������ �̵� �ӵ�

    protected Vector3 moveDir; // �������� �̵��ϴ� ����

    protected float removeTime; // ������ ���� �� ������� �ð�

    protected void itemMove()
    {
        tr.position += moveDir * speed * Time.deltaTime;
    }

    protected void Awake()
    {
        // ���� ������Ʈ
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // �������� ��ġ�� �÷��̾��� ��ġ ����
        tr = GetComponent<Transform>();
        playerTr = GameObject.Find("DogE").GetComponent<Transform>();

    }


    // �÷��̾ �������� ����� �� ������ ������ ���� ��ũ��Ʈ ���� �ۼ�.
    abstract protected void GetItem();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �浹 �� ������ �̵� ���� ����
        if (collision.CompareTag("WALL"))
        {
            if(collision.gameObject.name == "LeftWall" || collision.gameObject.name == "RightWall")
            {
                moveDir.x *= -1;
            }
            else
            {
                moveDir.y *= -1;
            }
        }

        if (collision.CompareTag("PLAYER"))
        {
            GetItem();

            Destroy(this.gameObject);
        }
    }


}
