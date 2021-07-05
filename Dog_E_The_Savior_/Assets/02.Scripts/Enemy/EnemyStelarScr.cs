using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStelarScr : EnemyInfo
{

    // Start is called before the first frame update
    void Start()
    {
        // Stelar Enemy ��ġ ����
        tr = GetComponent<Transform>();

        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // Stelar Enemy �̵� �ӵ�
        speed = 3.5f;
        // Stelar Enemy HP
        hp = 10 + Random.Range(1, 11) + Random.Range(1, 11);
        // hp *= stageCorr; // ���������� ���࿡ ���� hp�� ���� ������ ����. correction

        // Stelar Enemy�� �⺻������ �����̴� ����
        moveDir = Vector3.left;

        // Stelar Enemy�� �߻��� �Ѿ�
        string _bulletPath = "Prefabs/Bullets/Bullet_Stelar";
        bullet = Resources.Load<GameObject>(_bulletPath);

        // �Ѿ� �߻� ������
        fireDelay = 1.25f;
        // �Ѿ� �߻� ��� �ð�
        fireTime = 0f;

        // ���� �̵� �ӵ�
        updownSpeed = 1f;
        // ���� �̵� ��� �ð�
        updownTime = 0f;
        // ���� �̵� ���� ���� ������
        updownDelay = 1f;
        // ���� �̵� ����
        updownState = Vector3.up;


    }

    // Update is called once per frame
    void Update()
    {
        tr.position += moveDir * speed * Time.deltaTime;

        Fire();

        updownMove();

    }

    void Fire()
    {
        fireTime += Time.deltaTime;

        if (fireTime >= fireDelay)
        {
            fireTime -= fireDelay;

            Vector3 _bulletPos = tr.position;
            _bulletPos.z += 1;

            Instantiate(bullet, _bulletPos, Quaternion.identity);
        }
    }


    void updownMove()
    {
        updownTime += Time.deltaTime;

        tr.position += updownState * updownSpeed * Time.deltaTime;

        if (updownTime >= updownDelay)
        {
            updownTime -= updownDelay;

            updownState *= -1f;    

        }


    }


    IEnumerator upState()
    {
        updownTime += Time.deltaTime;
        yield return new WaitForSeconds(Time.deltaTime);

        tr.position += Vector3.up * updownSpeed * Time.deltaTime;

        if (updownTime >= updownDelay)
        {
            updownTime -= updownDelay;

            StartCoroutine(downState());
            StopCoroutine(upState());
        }
    }

    IEnumerator downState()
    {
        updownTime += Time.deltaTime;
        yield return new WaitForSeconds(Time.deltaTime);

        tr.position += Vector3.down * updownSpeed * Time.deltaTime;

        if (updownTime >= updownDelay)
        {
            updownTime -= updownDelay;

            StartCoroutine(upState());
            StopCoroutine(downState());
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾��� �Ѿ˰� �ε����� ���
        if (collision.CompareTag("BULLET"))
        {
            // ü���� �÷��̾��� ���ݷ� ��ŭ ���ҽ�Ų��.
            hp -= gameMgr.GetBulletDamage();

            // Stelar�� ü���� 0������ ���
            if (hp <= 0)
            {
                // �����Ѵ�.    
                Destroy(gameObject);
            }
        }

    }

}
