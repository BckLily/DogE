using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    GameObject player; // �÷��̾� ���� ������Ʈ
    Transform tr; // �÷��̾� ������Ʈ ��ġ
    SpriteRenderer playerImage; // �÷��̾� ������Ʈ �̹���

    GameManager GameMgr; // ���� �Ŵ��� ������Ʈ

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



    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾� ��ġ ����
        tr = GetComponent<Transform>();
        // �÷��̾� ������Ʈ ����
        player = gameObject;
        // �÷��̾� ������Ʈ �̹��� ������ ����
        // ���� ������ �� Active ���� ����
        playerImage = GetComponent<SpriteRenderer>();

        GameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();


        var _bullet_Path = "Prefabs/Bullets/Bullet";
        bullet = Resources.Load<GameObject>(_bullet_Path);


        // �÷��̾��� �̵��ӵ�
        // �̵��ӵ��� 3f ���� �̵��� �⺻ �̼� 1.3 ���� �̵��� �⺻ �̼� 0.7
        speed = 7f;
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


        // �ǰ� ������ �����ΰ�
        canDamaged = true;
        // ���� �ð� ���� 3��
        damagedDelay = 3f;


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
        float b_damage = GameMgr.GetBulletDamage();

        // �Ѿ��� �����ϰ� ������ �����Ͽ� ����� �� �ְ� ��.
        var _bullet = Instantiate(bullet, bulletPos, Quaternion.identity);
        // �Ѿ��� ���ݷ��� ����.
        _bullet.GetComponent<BulletScr>().bullet_damage = b_damage;

    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �Ѿ˰� �浹
        if (collision.CompareTag("E_BULLET"))
        {
            // ���ҽ�ų ������ �ۼ�Ʈ
            var per = Random.Range(0.1f, 0.3f);
            // min decrease / max decrease
            // �ּ� ����, �ִ� ����

            // �޾ƿ� ���� ���ݷ¿��� ���ҽ�ų �ۼ�Ʈ�� ���Ѵ�.
            var dmg = GameMgr.GetBulletDamage() * per;

            // �÷��̾ �¾Ҵٴ� ���� �˷��ش�.
            GameMgr.PlayerHit(dmg);
            
        }
    }


}
