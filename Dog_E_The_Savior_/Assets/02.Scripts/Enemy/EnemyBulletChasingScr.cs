using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletChasingScr : MonoBehaviour
{
    Transform tr; // �Ѿ� ������Ʈ ��ġ
    Rigidbody2D rb2d; // ������Ʈ�� rigidbody2d


    public float speed; // �Ѿ� ������Ʈ �ӵ�
    string targetTag; // �Ѿ��� Ÿ�� �±�
    Transform targetTr; // �Ѿ��� Ÿ���� Transform
    Vector3 targetPos; // �Ѿ��� Ÿ���� ��ġ

    Vector3 moveDir; // �Ѿ��� �̵� ����

    float removeTime; // �Ѿ��� ���� �ð�

    // Start is called before the first frame update
    void Start()
    {
        // �Ѿ� ������Ʈ ��ġ
        tr = gameObject.GetComponent<Transform>();
        // �Ѿ� ������Ʈ ������ٵ�
        rb2d = tr.GetComponent<Rigidbody2D>();
        // Ÿ�� �±�
        targetTag = "PLAYER";
        // �÷��̾� ��ġ
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);


        // �÷��̾ ���� ���
        if (player != null)
        {
            // Ÿ���� ��ġ�� �÷��̾� ��ġ ����
            targetTr = player.GetComponent<Transform>();
            targetPos = targetTr.position;
        }
        else
        {
            
            // �÷��̾ �����Ƿ� �������� ���ư� �� �ְ� Ÿ�� ����.
            Vector3 pos = new Vector3(tr.position.x - 20f, tr.position.y, tr.position.z);
            targetPos = pos;
        }

        //Debug.Log(targetPos);
        

        // ���� �ð� ����
        removeTime = 3f;
        // �̵� �ӵ� ����
        speed = 2f;

        // ���� �ð� �Ŀ� ���ŵ�.
        Destroy(this.gameObject, removeTime);
    }


    // Update is called once per frame
    void Update()
    {
        if (targetTr != null)
        {
            targetPos = targetTr.position;
        }

        moveDir = targetPos - tr.position;
        moveDir.Normalize();



        rb2d.AddForce(moveDir * speed, ForceMode2D.Force);
    }
}
