using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScr : MonoBehaviour
{
    Transform tr; // �Ѿ� ������Ʈ ��ġ

    public float speed; // �Ѿ� ������Ʈ �ӵ�
    string targetTag; // �Ѿ��� Ÿ�� �±�

    Vector3 moveDir; // �Ѿ��� �̵� ����

    [SerializeField]
    float removeTime; // �Ѿ� ���� �ð�




    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        // �Ѿ��� �̵� �ӵ�
        speed = 3f;
        // �Ѿ��� Ÿ�� �±�
        targetTag = "PLAYER";

        // �Ѿ��� �̵� ����
        moveDir = Vector3.left;

        removeTime = 5f;

        // �Ѿ� ���� ���� ���ŵǴ� �ð�
        Destroy(gameObject, removeTime);

   }

    // Update is called once per frame
    void Update()
    {
        // �Ѿ��� �̵��Ѵ�.
        tr.position += moveDir * speed * Time.deltaTime;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag(targetTag))
    //    {

    //        collision.GetComponent<PlayerCtrl>();

    //    }
    //}

}