using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeetScr : EnemyInfo
{

    GameObject[] bullets; // ���� ������ �Ѿ��� ����� ���̱� ������ ���ο� �迭�� �������.
    Transform firePos; // �Ѿ��� �߻�� ����Ʈ

    enum BulletType
    {
        normal = 0, chase = 1, 
    }

    bool isStart; // ������ ���ϴ� �߾��������� �̵��� �ߴ°�?

    [Header("- Boss Pattern")]
    [SerializeField]
    BossPattern bossPattern; // ���� ���� ������ ����
    //Dictionary<> // ��ųʸ� Ÿ���� ����ϴ� ���� �� ȿ�����ΰ�?
    enum BossPattern
    {
        stop = 0, shoot, spread , chaser, 
    }

    // stop ������ �ð����� 
    // shoot ������ ���� �ݺ� Ƚ�� 4��(0������ Ƚ�� 8��)
    // spread ������ ȸ�� Ƚ�� 5ȸ(0 ������ Ƚ�� 10��)
    // chaser ������ ȸ�� Ƚ�� 3ȸ(360 ������ Ƚ�� 3��)
    float patternCount; // ������ ���൵ Ȯ��
    int nextPattern; // ���� ������ �����Ѵ�.

    float checkBeforeState; // ���� Position, Rotation ���� ���� Vector3�� �����Ͽ� ���.

    Vector3 rotVector; // ȸ�� ���� ���� ����
    bool rotUp; // �������� �ϴ���?
    float rotSpeed; // ȸ�� �ӵ�


    // Start is called before the first frame update
    void Start()
    {
        firePos = tr.Find("FirePos").GetComponent<Transform>();

        // Beet Enemy �̵� �ӵ�
        speed = 15f;
        
        // Beet Enemy ü��
        hp = 3000f + HpDice((int)DiceCount.Boss, (float)MaxHPperDice.Boss);
        // hp *= stageCorr; // ���������� ���� ����.
        Debug.Log("Beet HP: " + hp);

        moveDir = Vector3.left; // ó���� �� �ۿ��� ������ �ǹǷ� �� ������ ������ ���ؼ� �������� �̵��Ѵ�.
        moveDir.Normalize();

        // Beet�� �Ѿ��� ����� Path
        string _bulletPath = "Prefabs/Bullets/Beet/";
        // Path�� ��� �Ѿ��� �ҷ��´�.
        bullets = Resources.LoadAll<GameObject>(_bulletPath);
        // bullet�� �⺻ �Ѿ˷� �����Ѵ�.
        bullet = bullets[(int)BulletType.normal];
        // ������ ������ �����۵�. ���� ���� ������ ����.
        string _itemPath = "Prefabs/Items/";
        items = Resources.LoadAll<GameObject>(_itemPath);

        string _expAnimPath = "Prefabs/Animations/ExpAnim";
        expAnim = Resources.Load<GameObject>(_expAnimPath);
        expAnimScale = new Vector3(1f, 1f, 1f);


        fireDelay = 0.2f; // �⺻ �Ѿ� �߻� ������ >> IEnumerator�� ������.
        fireTime = 0f; // �Ѿ� �߻� ��� �ð�

        updownDelay = 2f; // ���� �̵� ���� ������
        updownSpeed = 3.5f; // ���� �̵� �ӵ�
        updownTime = updownDelay / 2; // ���� �̵� ��� �ð� >> ó���� �߾ӿ��� �����ϱ� ������
                                      // ���� �̵� ���� �������� ������ ���� ������ �־�� ���������� �պ��Ͽ� ������ �� �ִ�.

        updownState = Vector3.up; // ���� �̵��� ���ʺ��� �����Ѵ�.

        // Enemy Ÿ���� ������ ����.
        enemyType = EnemyType.Boss;

        rotVector = Vector3.zero; // ȸ������ �ʴ´�.
        rotUp = true; // ȸ���� �� �ð�������� ȸ���Ѵ�.
        rotSpeed = 55f; // ȸ�� �ӵ� ����


        isStart = false; // ���ϴ� ��ҷ� �̵����� �ʾҴ�.

        nextPattern = 0;
        bossPattern = BossPattern.shoot; // �Ϲ� ���� ����.
        checkBeforeState = tr.position.y; // ������ �� y���� ���� ���·� ����.



        // �߰����
        // ������ ü�¹�


    }

    // Update is called once per frame
    void Update()
    {


        // ������ ���ϴ� ��ҷ� �̵��� ���� ��
        if (isStart == true)
        {
            BossPatternMove();
        }
        // ���� ���ϴ� ��ҷ� �̵����� �ʾ��� ��
        else
        {
            StartPosSet();
        }

        //Debug.Log("ROT Y: " + tr.rotation.y);
        //Debug.Log("ROT Z: " + tr.rotation.z);


    }


    void BossPatternMove()
    {
        switch (bossPattern)
        {
            case BossPattern.stop:
                StopPattern();

                break;
            case BossPattern.shoot:
                ShootPattern();

                break;
            case BossPattern.spread:
                SpreadPattern();
                
                break;
            case BossPattern.chaser:
                ChaserPattern();

                break;
            default:
                Debug.LogError("Boss Pattern Value is Out of Index");
                break;
        }
    }

    // �̵��� ���� ������ �ϱ� ���� �� ���� �����ϰ�
    // �̵��� ������ ���� ������ �����ϰ�
    // �� ���� �񱳸� �����Ѵ�.


    void StopPattern()
    {
        patternCount += Time.deltaTime;
        // 2�ʰ� ������
        if (patternCount >= 2f)
        {
            if (nextPattern == 0)
            {
                // ���� ������ 0�� �����̸� shoot

                moveDir = Vector3.zero; // �¿� �̵��� ����.
                updownState = Vector3.up; // ���Ʒ� �̵��� �ִ�.
                updownTime = updownDelay / 2f; // ���� �̵� �ð��� ���� ���� �־� ���� �����̴� �ð��� ������ ����.
                checkBeforeState = tr.position.y; // ������ �� �� �ݺ��ߴ°��� y���� ��ȭ ������ Ȯ���Ѵ�.
                bullet = bullets[(int)BulletType.normal]; // �Ѿ��� �⺻ �Ѿ˷� ����.
                fireDelay = 0.2f; // �Ѿ� ���� �ӵ� ����

                bossPattern = BossPattern.shoot;
            }
            // ���� ������ 1���̸� spread
            else if (nextPattern == 1)
            {
                moveDir = Vector3.zero; // �¿� �̵��� ����.
                updownState = Vector3.zero; // ���Ʒ� �̵��� �ִ�.
                                            // ������ �� �� �ݺ��ߴ°��� z�� ȸ���� ��ȭ ������ Ȯ���Ѵ�.
                checkBeforeState = Mathf.Sin(Mathf.Deg2Rad * tr.rotation.eulerAngles.z);
                bullet = bullets[(int)BulletType.normal]; // �Ѿ��� �⺻ �Ѿ˷� ����
                fireDelay = 0.1f; // �Ѿ� ���� �ӵ� ����

                bossPattern = BossPattern.spread;
            }
            // ���� ������ 2���̸� chaser
            else if (nextPattern == 2)
            {
                moveDir = Vector3.zero;
                updownState = Vector3.zero;
                // ������ �� �� �ݺ��ߴ°��� y���� ȸ���� ��ȭ ������ Ȯ���Ѵ�.
                // checkBeforeState�� ���� ������ ����ߴ�.
                // ������ ����ؼ� �����ϱ� ������ ���� ���� ���� ���� �񱳴� ������� �ʴ´�.
                checkBeforeState = 0f;
                bullet = bullets[(int)BulletType.chase]; // �Ѿ��� ������ ���� �̻��Ϸ� �ٲ۴� (homing missile)...
                fireDelay = 0.5f;// �Ѿ� ���� �ӵ� ����

                bossPattern = BossPattern.chaser;
            }
            // ���� ������ 3���̸� 
            else if (nextPattern == 3)
            {

            }
            else if (nextPattern == 4)
            {

            }
            else
            {
                Debug.LogError("Next Pattern Value is Error.");
            }

            patternCount = 0;
        }
    }


    void ShootPattern()
    {
        // 4�� �ݺ�������(8�� �� �ٲ�)
        if(patternCount== 8)
        {
            // Ȥ�ö� y�� ������ ���� �� �����ϱ� 0���� ���߾��ش�.
            Vector3 pos = new Vector3(tr.position.x, 0, tr.position.z);
            tr.position = pos;

            patternCount = 0;
            if (nextPattern == 0)
            {
                nextPattern = 1; // ù ��° ������ �����ٴ� ���� stopPattern�� �˷��ش�.
            }
            //else if (nextPattern == 4) // �߰������� ���ϴ� ���Ͽ� ����� �� �ִ�.

            bossPattern = BossPattern.stop; // ���� ������ stop���� �����Ѵ�.
            return;
        }

        // ������ y���� ������ ���� y������ �����Ѵ�.
        checkBeforeState = tr.position.y;

        // ������ ������ ����.
        EnemyMove();

        // ������ ���� ����.
        fireTime += Time.deltaTime;
        if(fireTime >= fireDelay)
        {
            fireTime -= fireDelay;
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);

        }



        // ���� ���� ���� ���� 0���� �۴ٴ� ���� 0�� ������ ���, ������ �ٲ���ٴ� ���̴�.
        if (checkBeforeState * tr.position.y <= 0)
        {
            //Debug.Log("Reverse");
            patternCount += 1;
        }
    }

    void SpreadPattern()
    {
        // 6�� �ݺ��ϸ� ���� �������� �Ѿ��.
        if (patternCount == 8)
        {
            // Enemy�� rotation ���� Ʋ������ �� �����Ƿ� z���� ȸ�� ���� 0���� �����Ͽ� �⺻ ������ �ǵ�����.
            // �� ���Ϸ� ������ �ߴ��� �𸣰ڳ�.
            Vector3 rot = new Vector3(tr.rotation.eulerAngles.x, tr.rotation.eulerAngles.y, 0);
            tr.rotation.eulerAngles.Set(rot.x, rot.y, rot.z);

            patternCount = 0;
            // ���� ���� �� ��° �����̸�
            if (nextPattern == 1)
            {
                //Debug.Log("Next Pattern 2");
                // �� ��° �������� �Ѿ��.
                nextPattern = 2;
            }

            bossPattern = BossPattern.stop; // ���� ������ stop���� �����Ѵ�.
            return;
        }

        // ���� ���� ���� ���� �񱳸� ���� ������ �д�.
        checkBeforeState = Mathf.Sin(Mathf.Deg2Rad * tr.rotation.eulerAngles.z);


        // �ð� �������� ȸ�� ���̸�
        if (rotUp == true)
        {
            // �ð� �������� ȸ����Ų��.
            tr.Rotate(Vector3.back * Time.deltaTime * rotSpeed, Space.Self);

            // ���� ȸ���� -40~40 ���� �̷������ �ִµ�
            // �ܼ��ϰ� tr.rotation.z�� ���� ����Ͽ��� 320 > tr.rotation.z�� �ϸ�
            // -40~0������ ���������� �۵��� �� ������
            // �� �����ؾ��ϴ� 0~40 �κ��� ���������� �۵��� �� ����.
            // ���� sine ���� ����Ͽ���.
            // sine ���� ������ ���� y���� ���� ���̴�.
            // ��, -40~0�� sine ���� ������ ���� ���̰�
            // 0~40�� sine ���� ����� ���̴�.
            // ���� ���� ������ sine���� ���� ���� sine(-40) �� sine(320)���� �۾��� ���
            // �ð� �������� ȸ���� �� �ִ� �ִ� ���� ���̰�
            // ���� ������ sine���� ������ �� sine(40) ���� Ŀ���� �ݽð� �������� ȸ���� �� �ִ� �ִ� ���̴�.
            if (Mathf.Sin(tr.rotation.eulerAngles.z * Mathf.Deg2Rad) <= Mathf.Sin(320*Mathf.Deg2Rad))
            {
                // ȸ�� ������ �ִ� ������ �����Ƿ� �ݽð�������� �����.
                rotUp = false;
            }
        }
        else
        {
            // �ð� �ݴ� �������� ȸ����Ų��.
            tr.Rotate(Vector3.forward * Time.deltaTime * rotSpeed, Space.Self);
            // �ݽð� �������� �ִ� ȸ�� ������ ���� �Ѿ��
            if (Mathf.Sin(tr.rotation.eulerAngles.z * Mathf.Deg2Rad) >= Mathf.Sin(40 * Mathf.Deg2Rad))
            {
                // �ð� �������� �����.
                rotUp = true;
            }
        }

        // ���� �⺻���� ������. ���� �¿�
        EnemyMove();


        // �� ����
        fireTime += Time.deltaTime;
        if (fireTime >= fireDelay)
        {
            fireTime -= fireDelay;
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);
        }



        // z�� ȸ�� ���� 0�� ������ �Ǹ� ������ ������ sine ���� ���ϸ� ������ �� ���̴�.
        // �⺻ tr.rotation.eulerAngles.z�� 0~360�� ����̹Ƿ� �׳� ���ϸ� ��� ����� �� ���̴�.
        if (checkBeforeState * Mathf.Sin(Mathf.Deg2Rad * tr.rotation.eulerAngles.z) <= 0)
        {
            patternCount += 1;
            //Debug.Log("REVERSE " + patternCount);
        }
    }

    void ChaserPattern()
    {
        // spread�ʹ� �ٸ��� ���� ���� ����ؼ� ������ ��ȭ��Ű��
        // Ư�� ������ �����ϸ� Ƚ���� �������Ѽ� ���ϴ� Ƚ����ŭ �ݺ��ϰ� �Ѵ�.

        if (patternCount == 2)
        {
            Quaternion rot = Quaternion.Euler(new Vector3(tr.rotation.x, 0, tr.rotation.z));
            tr.rotation = rot;

            patternCount = 0;

            // �� ��° �����̸�
            if (nextPattern == 2)
            {
                // ù ��° �������� ���ư���.
                nextPattern = 0;
                // ���� �߰����� ������ ����ų�
                // ������ ��� �����ϰ� �ʹٸ� ������ �߰��Ͽ� ������ �� �ִ�.
            }

            // ���� �������� ����� �Լ� �����Ѵ�.
            bossPattern = BossPattern.stop;
            return;
        }

        // ȸ���� ������ �����Ѵ�.
        checkBeforeState += Time.deltaTime * rotSpeed;


        // ���ο� ������ ȸ���� ���� �����Ͽ� tr.rotation�� �����Ѵ�.
        Quaternion value = Quaternion.Euler(new Vector3(tr.rotation.x, checkBeforeState, tr.rotation.z));
        tr.rotation = value;


        // �� �̵�
        EnemyMove();


        // �� ����
        // ������ ���� �ð� ����
        fireTime += Time.deltaTime;
        // ���� �ð��� �����̺��� Ŀ����
        if (fireTime >= fireDelay)
        {
            // �����̸�ŭ �ٽ� ���̰�
            fireTime -= fireDelay;
            // bullet ����.
            GameObject __bullet = Instantiate(bullet, firePos.transform.position, Quaternion.identity);
            // ����ź�̹Ƿ� ���ϱ� ���� �ణ �������� �߻��ϵ��� ����.
            __bullet.GetComponent<Rigidbody2D>().AddForce((-firePos.right + firePos.up).normalized * 2f, ForceMode2D.Impulse);
        }


        // ���� ���� 360�� ������ �ٽ� 360�� ���� ���� �۰� �����.
        // ������ �ִ� ũ��� 360�̱� ����
        if (checkBeforeState >= 360f)
        {
            checkBeforeState -= 360f;
            patternCount += 1;
        }
    }


    void StartPosSet()
    {
        // ������ ��ġ�� x���� 6���� ũ��
        if (tr.position.x > 6f)
        {
            // �������� �̵�
            tr.position += moveDir * 3f * Time.deltaTime;
        }
        // 6���� �۰ų� ��������
        else
        {
            // �̵� �� ���� �� �ִ� ��ġ�� ��Ż�� �����ϱ� ���ؼ� �̵��� ������
            // ���ϴ� ��ġ(x��ǥ 6)�� �ٽ� �����Ѵ�.
            Vector3 pos = new Vector3(6f, tr.position.y, tr.position.z);
            tr.position = pos;
            moveDir = Vector3.zero;

            // ��ġ�� �������Ƿ� ���� ������ ���۵ȴ�.
            isStart = true;
        }
    }


    protected override void EnemyMove()
    {
        // �Ϲ������� ���� ĳ���ʹ� �¿� �������� ����.
        horiMove();
        // �Ϲ������� ���� �������� �ϳ�, ���� ���ϸ��� ������ ���� ȸ���� �� �� �ִ�.
        updownMove();

    }

    protected override void horiMove()
    {
        tr.position += moveDir * speed * Time.deltaTime;

    }

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

    // ������ ����.
    protected override void MakeItem(float num)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("BULLET"))
        {
            hp -= gameMgr.GetBulletDamage();


            Destroy(collision.gameObject);
            if (hp <= 0)
            {
                Instantiate(expAnim, tr.position, Quaternion.identity);


                Destroy(this.gameObject);
            }
        }

        if (collision.CompareTag("PLAYER"))
        {
            gameMgr.PlayerHit();
        }
    }



}
