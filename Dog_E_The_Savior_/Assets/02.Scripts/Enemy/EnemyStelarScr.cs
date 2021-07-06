using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStelarScr : EnemyInfo
{

    float enemyMaxLeftPos;

    // AtkSpeedUp / BoomItem / DamageUp / ScoreUp / ShieldItem
    [SerializeField]
    GameObject[] items; // �����۵��� �����صδ� ����Ʈ ����
    enum ItemType
    {
        AtkSpeedUp = 0, BoomItem, DamageUp, ScoreUp, ShieldItem,
    }


    // Start is called before the first frame update
    void Start()
    {
        // Stelar Enemy ��ġ ����
        tr = GetComponent<Transform>();

        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // Stelar Enemy �̵� �ӵ�
        speed = 4f;
        // Stelar Enemy HP
        hp = 10 + Random.Range(1, 11) + Random.Range(1, 11);
        // hp *= stageCorr; // ���������� ���࿡ ���� hp�� ���� ������ ����. correction

        // Stelar Enemy�� �⺻������ �����̴� ����
        moveDir = Vector3.left;

        // Stelar Enemy�� �߻��� �Ѿ�
        string _bulletPath = "Prefabs/Bullets/Bullet_Stelar";
        bullet = Resources.Load<GameObject>(_bulletPath);

        string _itemsPath = "Prefabs/Items/";
        items = Resources.LoadAll<GameObject>(_itemsPath);



        // �Ѿ� �߻� ������
        fireDelay = 1.25f;
        // �Ѿ� �߻� ��� �ð�
        fireTime = 0f;

        // ���� �̵� �ӵ�
        updownSpeed = 1.75f;
        // ���� �̵� ��� �ð�
        updownTime = 0f;
        // ���� �̵� ���� ���� ������
        updownDelay = 0.55f;

        if (tr.position.y >= 0f)
        {
            // ���� �̵� ����
            updownState = Vector3.up;
        }
        else
        {
            updownState = Vector3.down;
        }


        // Enemy�� Ÿ���� �븻(�⺻ ��)���� ����
        enemyType = EnemyType.normal;


        // ���� �̵��� �� �ִ� �ִ� ���� ��ǥ
        enemyMaxLeftPos = -12f;
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy�� ������ �Լ�
        EnemyMove();

        // Enemy ����
        Fire();


    }

    void Fire()
    {
        // ������ �߻� �ð��� ������Ų��.
        fireTime += Time.deltaTime;

        // ������ �߻� �ð��� ������ �ð����� Ŀ����
        if (fireTime >= fireDelay)
        {
            // ������ �ð���ŭ ����
            fireTime -= fireDelay;

            // �Ѿ��� ���� ��ġ�� Enemy�� ��ġ�� �����ϰ� �ϰ�
            // Z���� 1 ������ Enemy�� �Ѿ˿� �������� �ʰ� �Ѵ�.
            Vector3 _bulletPos = tr.position;
            _bulletPos.z += 1;

            Instantiate(bullet, _bulletPos, Quaternion.identity);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾��� �Ѿ˰� �ε����� ���
        if (collision.CompareTag("BULLET"))
        {
            // ü���� �÷��̾��� ���ݷ� ��ŭ ���ҽ�Ų��.
            hp -= gameMgr.GetBulletDamage();

            Destroy(collision.gameObject);

            // Stelar�� ü���� 0������ ���
            if (hp <= 0)
            {
                gameMgr.PlayerDamageUp(incDamageList[(int)enemyType]);

                float rand = Random.Range(0.0f, 10.0f);
                MakeItem(rand);

                // �����Ѵ�.    
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("PLAYER"))
        {
            // �÷��̾�� �浹�ߴٴ� ���� �˷��ش�.
            gameMgr.PlayerHit();
        }

    }

    void MakeItem(float num)
    {
        GameObject item = new GameObject();
        Debug.Log("Random: " + num);


        if (num >= 9.5f)
        {
            item = items[(int)ItemType.BoomItem];
        }
        else if (num >= 8.5f)
        {
            item = items[(int)ItemType.ShieldItem];
        }
        else if (num >= 7f)
        {
            item = items[(int)ItemType.AtkSpeedUp];
        }
        else if (num >= 4f)
        {
            item = items[(int)ItemType.DamageUp];
        }
        else if(num >= 0f)
        {
            item = items[(int)ItemType.ScoreUp];
        }

        Debug.Log(item.name);

        Instantiate(item, tr.position, Quaternion.identity);
    }


    // Up Down State ���� ���� �� �Լ�
#if false

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
#endif


    // Enemy�� �����̴� �Լ�
    protected override void EnemyMove()
    {
        horiMove();
        updownMove();

        if (tr.position.x <= enemyMaxLeftPos)
        {
            Destroy(this.gameObject);
        }
    }

    // �������� �̵��ϴ� �Լ�.
    protected override void horiMove()
    {
        // Enemy ��ġ �̵�
        tr.position += moveDir * speed * Time.deltaTime;
    }

    // Enemy�� ���Ʒ��� �����̴� �Լ�
    protected override void updownMove()
    {
        updownTime += Time.deltaTime;

        tr.position += updownState * updownSpeed * Time.deltaTime;

        // ������ �������� �����̱������ �ð���
        // ������ �ð�(���� �ð�)���� Ŀ���� �����̴� ���� ����
        if (updownTime >= updownDelay)
        {
            updownTime -= updownDelay;

            updownState *= -1f;

        }

    }



}
