using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ItemInfo : MonoBehaviour
{
    protected GameManager gameMgr; // ���� �Ŵ���

    protected Transform tr; // �������� ��ġ
    protected Transform playerTr; // �÷��̾��� ��ġ

    protected CircleCollider2D col2D; //  �ݶ��̴� Ȱ��ȭ ��Ȱ��ȭ

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
        tr = this.gameObject.GetComponent<Transform>();

        GameObject player = GameObject.FindGameObjectWithTag("PLAYER");
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        else
        {
            Vector3 pos = new Vector3(tr.position.x - 16f, tr.position.y, tr.position.z);
            playerTr.position = pos;
        }
        
        // ȭ�� �ۿ��� �������� �����Ǵ��� �� ������ �� �� �ְ� �ݶ��̴� ���ٰ�
        // ���߿� �ٽ� �Ҵ�.
        col2D = gameObject.GetComponent<CircleCollider2D>();
        col2D.enabled = false;
        StartCoroutine(CheckPos());

    }


    protected IEnumerator CheckPos()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            //Debug.Log(tr.position.x);
            //Debug.Log(tr.position.y);

            if (Mathf.Abs(tr.position.x) <= 7.5f && Mathf.Abs(tr.position.y) <= 4f)
            {
                //Debug.Log("GET IN");
                col2D.enabled = true;
                yield break;
            }
            else
            {
                //Debug.Log("GET OUT");
            }
        }
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

            //Debug.Log("Destroy this Game Object: " + this.gameObject.name);

            Destroy(this.gameObject);
        }
    }


}
