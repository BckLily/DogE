using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStelarScr : EnemyInfo
{

    float enemyMaxLeftPos;

    //// AtkSpeedUp / BoomItem / DamageUp / ScoreUp / ShieldItem
    //[SerializeField]
    //GameObject[] items; // �����۵��� �����صδ� ����Ʈ ����
    //enum ItemType
    //{
    //    AtkSpeedUp = 0, BoomItem, DamageUp, ScoreUp, ShieldItem,
    //}


    // Start is called before the first frame update
    void Start()
    {
        //// Stelar Enemy ��ġ ����
        //tr = gameObject.GetComponent<Transform>();

        //gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();


        // Stelar Enemy �̵� �ӵ�
        speed = 3.5f;
        // Stelar Enemy HP
        hp = 10 + HpDice((int)DiceCount.normal, (float)MaxHPperDice.normal);
        // hp *= stageCorr; // ���������� ���࿡ ���� hp�� ���� ������ ����. correction
        //Debug.Log("Stelar HP: " + hp);


        // Stelar Enemy�� �⺻������ �����̴� ����
        moveDir = Vector3.left;

        // Stelar Enemy�� �߻��� �Ѿ�
        string _bulletPath = "Prefabs/Bullets/Stelar/Bullet_Stelar";
        bullet = Resources.Load<GameObject>(_bulletPath);
        // Stelar Enemy�� ������ ������ ������ ��
        string _itemsPath = "Prefabs/Items/";
        items = Resources.LoadAll<GameObject>(_itemsPath);
        // Stelar Enemy�� ������ �߻��� �ִϸ��̼�
        string expAnimPath = "Prefabs/Animations/ExpAnim";
        expAnim = Resources.Load<GameObject>(expAnimPath);
        expAnimScale = Vector3.one;



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

    // �� ĳ������ ����
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
                // �÷��̾��� ���ݷ��� ���� Ÿ�Կ� ���� ������Ű��
                gameMgr.PlayerDamageUp(incDamageList[(int)enemyType]);

                // ������ ���� �����Ͽ� � �������� ������ �����Ѵ�.
                float rand = Random.Range(-5f, 10.0f);
                MakeItem(rand);

                // �� ĳ���Ͱ� ������� �� ���� �ִϸ��̼��� ����� ��ġ�� �����ؼ� ǥ�����ش�.
                GameObject exp = Instantiate(expAnim, tr.position, Quaternion.identity);
                exp.transform.localScale = expAnimScale;
                // �ִϸ��̼��� �ð��� 1�� �����̹Ƿ� ���� ������ �ΰ� �����Ѵ�.
                Destroy(exp.gameObject, 1.15f);

                
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

    // ����� �������� ����
    protected override void MakeItem(float num)
    {
        GameObject item;
        //Debug.Log("Random: " + num);


        if (num >= 9.5f)
        {
            item = items[(int)ItemType.BoomItem];
            //Instantiate(items[(int)ItemType.BoomItem], tr.position, Quaternion.identity);
        }
        else if (num >= 8.5f)
        {
            item = items[(int)ItemType.ShieldItem];
            //Instantiate(items[(int)ItemType.ShieldItem], tr.position, Quaternion.identity);
        }
        else if (num >= 7f)
        {
            item = items[(int)ItemType.AtkSpeedUp];
            //Instantiate(items[(int)ItemType.AtkSpeedUp], tr.position, Quaternion.identity);
        }
        else if (num >= 4f)
        {
            item = items[(int)ItemType.DamageUp];
            //Instantiate(items[(int)ItemType.DamageUp], tr.position, Quaternion.identity);
        }
        else if (num >= 0f)
        {
            item = items[(int)ItemType.ScoreUp];
            //Instantiate(items[(int)ItemType.ScoreUp], tr.position, Quaternion.identity);
        }
        else
        {
            return;
        }


        //Debug.Log(item.name);
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
