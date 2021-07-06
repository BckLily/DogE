using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    GameObject player; // �÷��̾� ���� ������Ʈ
    Transform tr; // �÷��̾� ������Ʈ ��ġ
    //SpriteRenderer playerImage; // �÷��̾� ������Ʈ �̹���

    GameManager gameMgr; // ���� �Ŵ��� ������Ʈ

    [Header("- Player Speed")]
    [SerializeField]
    float speed; // �÷��̾��� �̵� ��
    float speed_fast; // �÷��̾��� ���� �̵� �ӵ�
    float speed_slow; // �÷��̾��� ���� �̵� �ӵ�

    // �÷��̾��� �⺻ �̵� �ӵ����� ����� ����ؼ� ���� �̵��� ���� �̵��� ����� ���� �ֱ� ������
    // �������� ������� �ƴϸ� ����� ������� ����.


    private float atkDamage; // �÷��̾� ���ݷ�
    private float atkSpeed; // �÷��̾� ���� �ӵ� / ���� ������
    private float lastAtkTime; // �÷��̾ �������� �����ϰ� ���� �ð�

    [Header("- Player Attack")]
    public float atkDamageUp; // �÷��̾� ���ݷ� ����.(��ȭ ��� ����)
    public float atkSpeedUp; // �÷��̾� ���ݼӵ� ����.(������ ����)


    public int boomCount;  // ��ź�� ����


    bool canDamaged; // ���� ���� �� �ִ� �����ΰ�?
    private float damagedDelay; // ���� ���� �ð� / ���� �޴� �ð� ������

    GameObject bullet; // �÷��̾ ���ݽ� ������ bullet

    GameObject shield; // �÷��̾��� �ǵ� ������Ʈ
    public bool isShieldActive; // �÷��̾��� �ǵ尡 Ȱ��ȭ �������� üũ
    public float shieldActiveTime; // �ǵ��� ���ӽð�


    List<GameObject> enemies; // ��ź ���� ���ŵ� ENEMY, ENEMTBULLET ��.

    bool isBoom; // ���� ���ΰ�

    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾� ��ġ ����
        tr = GetComponent<Transform>();
        // �÷��̾� ������Ʈ ����
        player = gameObject;
        // �÷��̾� ������Ʈ �̹��� ������ ����
        // ���� ������ �� Active ���� ����
        //playerImage = GetComponent<SpriteRenderer>();

        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // �÷��̾� �Ѿ� ������Ʈ ��ġ
        var _bullet_Path = "Prefabs/Bullets/Bullet";
        bullet = Resources.Load<GameObject>(_bullet_Path);

        enemies = new List<GameObject>();

        // �÷��̾��� �̵��ӵ�
        // �̵��ӵ��� 3f ���� �̵��� �⺻ �̼� 1.3 ���� �̵��� �⺻ �̼� 0.7
        speed = 8f;
        speed_fast = speed * 1.5f;
        speed_slow = speed * 0.4f;

        // �÷��̾��� �⺻ ���ݷ� 10
        atkDamage = 10f;
        // �÷��̾��� ���� �ӵ� �ִ� 1�ʿ� 20��
        atkSpeed = 0.25f;
        lastAtkTime = atkSpeed;

        // ��ȭ�� ���ݷ� �� ���� �ӵ� 0
        atkDamageUp = 0f;
        atkSpeedUp = 0f;

        // ������ �ִ� ��ź�� ����
        boomCount = 1;
        isBoom = false;

        // �ǰ� ������ �����ΰ�
        canDamaged = true;
        // ���� �ð� ���� 3��
        damagedDelay = 3f;

        // �÷��̾��� child�� shield�� ã�´�.
        shield = tr.Find("Shield").gameObject;
        // �ʱ� Ȱ��ȭ ���´� ��Ȱ��ȭ
        isShieldActive = true;
        shield.SetActive(isShieldActive);
        // �ǵ��� ���ӽð��� 3�ʷ� �����Ѵ�.
        shieldActiveTime = 3f;


    }

    private void FixedUpdate()
    {
        // �̵� ������ ������ ����
        var moveDir = Vector3.zero;
        var moveSpeed = speed;

        // �÷��̾��� �� �Ʒ� �̵��� ����
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDir += Vector3.down;
        }
        // �÷��̾��� �� �� �̵��� ����
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir += Vector3.right;
        }

        // ���� Shift�� ������ ������ �̵�.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = speed_fast;
        }
        // Space Bar�� ������ õõ�� �̵�
        else if (Input.GetKey(KeyCode.Space))
        {
            moveSpeed = speed_slow;
        }


        // �÷��̾��� ��ġ �̵�
        tr.position += moveDir.normalized * moveSpeed * Time.deltaTime;

    }

    // Update is called once per frame
    void Update()
    {
        // ���� �����̸� ���� �ӵ����� ������ ���ݼӵ� ��ŭ ��.
        var atkDelay = atkSpeed - atkSpeedUp;

        //Debug.Log("Attack Delay: " + atkDelay);

        // ���� ��ư�� JŰ�� ������
        if (Input.GetKey(KeyCode.J))
        {
            // �������� ������ �ð��� �����ϰ�
            lastAtkTime += Time.deltaTime;
            // ���� �����̸� �ʰ��ϸ�
            if(lastAtkTime >= atkDelay)
            {
                lastAtkTime -= atkDelay;
                // �߻�.
                Fire();
            }
        }
        // JŰ�� ����
        if (Input.GetKeyUp(KeyCode.J))
        {
            // ������ Ű�� ������ �� �ٷ� ������ �� �ְ� ������.
            lastAtkTime = atkDelay;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            UseBoom();
        }

        ActiveShield();

        if (isBoom)
        {
            RemoveEnemy();
        }
        //else
        //{
        //    StopCoroutine(RemoveEnemy());
        //}


    }

    // �Ѿ��� �߻��ϴ� �Լ�
    void Fire()
    {
        // ������ �Ѿ��� ��ġ
        Vector3 bulletPos = tr.position;
        // z���� 1 ���ϴ� ������ �÷��̾� ĳ���ͺ��� �ڿ� �����ǰ��Ͽ� �Ѿ˿� �÷��̾� ĳ���Ͱ� �������� �ʰ� �Ѵ�.
        bulletPos.z += 1;

        // �Ѿ��� �������� ����.
        // �⺻ ���ݷ� + �߰��� ���ݷ�.
        //var b_damage = atkDamage + atkDamageUp;
        float b_damage = gameMgr.GetBulletDamage();

        // �Ѿ��� �����ϰ� ������ �����Ͽ� ����� �� �ְ� ��.
        var _bullet = Instantiate(bullet, bulletPos, Quaternion.identity);
        // �Ѿ��� ���ݷ��� ����.
        _bullet.GetComponent<BulletScr>().bullet_damage = b_damage;

    }

    // �ǵ� ���¸� �����ϴ� �Լ�
    public void ActiveShield()
    {
        // �ǵ� Ȱ��ȭ �ð��� 0���� ũ��
        if (shieldActiveTime > 0)
        {
            // �ǵ带 Ȱ��ȭ ��Ų��.
            shield.SetActive(isShieldActive);
            // �ǵ� Ȱ��ȭ �ð��� ���ҽ�Ų��.
            shieldActiveTime -= Time.deltaTime;
        }
        // �ǵ尡 Ȱ��ȭ �Ǿ��ְ� Ȱ��ȭ �ð��� 0���� �۰ų� ������
        else if (isShieldActive == true)
        {
            // Ȱ��ȭ �ð��� 0���� �����.
            shieldActiveTime = 0f;
            // �ǵ� Ȱ��ȭ ���¸� ��Ȱ��ȭ�� �����.
            isShieldActive = false;
            shield.SetActive(isShieldActive);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �Ѿ˰� �浹
        if (collision.CompareTag("E_BULLET"))
        {
            // ���� �Ѿ��� ���ش�.
            Destroy(collision.gameObject);

            // �ǵ尡 ��Ȱ��ȭ�� ���
            if (shield.activeSelf == false)
            {
                // �÷��̾ �¾Ҵٴ� ���� �˷��ش�.
                gameMgr.PlayerHit();
            }
        }
    }

    void UseBoom()
    {
        // ���� �ִϸ��̼� ������ ���� ��.

        // ������ �ִ� ��ź�� 1�� �̻��� ���
        if (gameMgr.boomCount > 0)
        {
            // ������ ��ź�� ������ 1�� ���δ�.
            gameMgr.UseBoomItem();
            //Debug.Log("USE BOOM");

            // ENEMY Tag�� E_BULLET Tag�� ���� ��� GameObject�� �����Ѵ�.
            GameObject[] enemyTags = GameObject.FindGameObjectsWithTag("ENEMY");
            GameObject[] eBulletTags = GameObject.FindGameObjectsWithTag("E_BULLET");

            // ����� �͵��� enemies ����Ʈ�� �߰��Ѵ�.
            enemies.AddRange(enemyTags);
            enemies.AddRange(eBulletTags);


            // ���� �����Ѵ�.
            isBoom = true;

        }
    }

    void RemoveEnemy()
    {
        int enemyCount = enemies.Count;

        if (enemyCount == 0)
        {
            isBoom = false;
        }

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        // �ڷ�ƾ ó���ؼ� ���� ���� �����ϴ� ����� ����...

        enemies.Clear();
    }

    
    //IEnumerator RemoveEnemy()
    //{
    //    yield return new WaitForSeconds(Time.deltaTime);
    //    int enemyCount = enemies.Count;
    //    if (enemyCount == 0)
    //    {
    //        isBoom = false;
    //        StopCoroutine(RemoveEnemy());
    //        yield return null;
    //    }
    //    Destroy(enemies[enemyCount - 1]);
    //    enemies.RemoveAt(enemyCount - 1);
    //}


}
