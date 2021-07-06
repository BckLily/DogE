using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScr : MonoBehaviour
{
    Transform tr; // �Ѿ� �������� ��ġ

    public float speed; // �Ѿ��� �̵� �ӵ�
    string targetTag; // �Ѿ��� Ÿ���� �±�

    Vector3 moveDir; // �Ѿ��� �̵� ����

    [SerializeField]
    float removeTime; // �Ѿ��� ���� �ð�

    public float bullet_damage; // �Ѿ��� ������

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        // �Ѿ��� �̵� �ӵ� 3f
        speed = 12f;
        // �Ѿ��� Ÿ�� �±� ENEMY
        targetTag = "ENEMY";

        // �Ѿ��� �̵��ϴ� ���� ������
        moveDir = Vector3.right;

        removeTime = 5f;

        // �Ѿ��� �����ǰ� ���� �ð��� ������ ���ŵ�.
        Destroy(this.gameObject, removeTime);

    }

    // Update is called once per frame
    void Update()
    {
        // �Ѿ��� ���������� �̵���.
        tr.position += moveDir * Time.deltaTime * speed;

    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.CompareTag(targetTag))
    //    {


    //        // Enemy�� �ε����� ���ŵ�.
    //        Destroy(this.gameObject);
    //    }

    //}


}
