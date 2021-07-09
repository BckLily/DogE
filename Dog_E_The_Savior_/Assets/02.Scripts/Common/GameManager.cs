using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("- Player Damage")]
    [SerializeField]
    private float damage; // �÷��̾� ���ݷ�
    public float damageUp; // ������ ���ݷ�
    public int boomCount; // �÷��̾ ������ �ִ� ��ź�� ����

    Transform uiCanvas; // UI�� �����ϰ� �ִ� Canvas
    Text atkDamageText; // �÷��̾� ���ݷ��� ǥ���� �ؽ�Ʈ
    string atkBaseText; // ���ݷ� �⺻ ���ڿ�
    Text scoreText; // ������ ǥ���� �ؽ�Ʈ
    Text boomText; // ��ź�� ������ ǥ���� �ؽ�Ʈ
    string boomBaseText; // ��ź �⺻ ���ڿ�
    public Transform bossCanvas; // ���� ���� UI�� ������ �ִ� Cavnas
    Text bossNameText; // ���� �̸��� ǥ������ Text
    string bossName; // ���� �̸� ����...?
    public string SetBossName
    {
        set
        {
            bossName = value;
        }
    }
    Transform bossHPBar; // ������ HP�ٰ� �ִ� ��ġ.
    [SerializeField]
    Image[] bossHPImages; // ���� hp�� �̹�����.
    enum BossHPImage
    {
        head = 0, middle, tail,
    }
    Text bossHPText; // ������ ���� ���� HP�� ǥ������ �ؽ�Ʈ
    float bossHP; // ������ ���� HP
    public float SetBossHP
    {
        set
        {
            bossHP = value;
        }
    }


    //[Header("- Game Info")]
    //[SerializeField]
    private float score; // ������ ����

    // �������� ���۵Ǿ��°� Ȯ��.
    public bool isBossStart;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        damage = 10f;
        damageUp = 0f;
        boomCount = 1;

        // UI Canvas�� atkDamageText�� ã�Ƽ� ����.
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Transform>();
        atkDamageText = uiCanvas.Find("AtkDamage").Find("AtkDamageText").GetComponent<Text>();
        scoreText = uiCanvas.Find("GameScore").Find("GameScoreText").GetComponent<Text>();
        boomText = uiCanvas.Find("BoomCanvas").Find("BoomCountText").GetComponent<Text>();

        // ������ ���� UI�� ã�Ƽ� ����
        bossCanvas = uiCanvas.Find("BossCanvas").GetComponent<Transform>();
        bossNameText = bossCanvas.Find("BossName").GetComponent<Text>();
        bossHPBar = bossCanvas.Find("BossHP").GetComponent<Transform>();
        bossHPImages = bossHPBar.GetComponentsInChildren<Image>();
        //Debug.Log("�̹��� ����: " + bossHPImages.Length);
        bossHPText = bossHPBar.Find("BossHPFloat").GetComponent<Text>();

        // ������ �����ϸ� ���̸� �Ǳ� ������ Canvas�� Active false�� �Ѵ�.
        // ������ �� ������ �ٽ� ��Ȱ��ȭ �ϰ� �Ѵ�.
        bossCanvas.gameObject.SetActive(false);

        // ���ݷ� ǥ��
        atkBaseText = "���ݷ�: ";
        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

        // ���� ǥ��
        score = 0f;
        // ���߿��� ������ �����Ϳ��� �̾��ϱ� ���� �������.
        scoreText.text = (int)score + "";

        // ��ź ���� ǥ��
        boomBaseText = "X ";
        boomText.text = boomBaseText + boomCount;

        isBossStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime;
        GameScoreUp(0f);
    }

    public float GetBulletDamage()
    {
        // �Ѿ��� ���ݷ��� ��ȯ
        return (damage + damageUp);
    }


    // �÷��̾ �ǰ� ���� ���
    // PlayerHit()
    public void PlayerHit()
    {
        // ���ҽ�ų ���ݷ� %���� (5 ~ 20)%
        var per = Random.Range(0.05f, 0.2f);
        // ���� ������ ���ݷ�
        // �ּ� 0.25�� �����ϰ� %�� �߰� ����.
        float dec_bulletDamage = 0.15f + GetBulletDamage() * per;

#if UNITY_EDITOR

        //Debug.Log("Player Hit");


#endif


        damageUp -= dec_bulletDamage;

        if (damageUp < 0f)
        {
            damage += damageUp;
            damageUp = 0f;
        }

        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

        if (damage <= 0)
        {
            // �÷��̾� ���

            //Debug.Log("Player Die");
        }
    }


    // ���� �����߷��� �� �����ϴ� �÷��̾� ������
    public void PlayerDamageUp(float inc_damage)
    {
        damageUp += inc_damage;

        GameScoreUp(inc_damage * 10f);

        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

#if UNITY_EDITOR

        //Debug.Log("Player Damage Increasesd");

#endif 

    }

    // ������ �����ִ� �Լ�
    public float GetScore
    {
        get
        {
            return score;
        }

    }


    // ���� ���� �Լ�.
    public void GameScoreUp(float inc_Score)
    {
        score += inc_Score;

        scoreText.text = (int)score + "";
    }

    public void GetBoomItem()
    {
        boomCount += 1;
        boomText.text = boomBaseText + boomCount;
    }

    public void UseBoomItem()
    {
        boomCount -= 1;
        boomText.text = boomBaseText + boomCount;
    }

    public IEnumerator BossStart()
    {
        // ���� ���� ������ �����ִ� Canvas�� On���� �Ѵ�.
        bossCanvas.gameObject.SetActive(true);
        // ���� �̸� ����
        bossNameText.text = bossName;

        // ������ ���� ���� ����
        while (isBossStart)
        {
            //Debug.Log("Boss HP Setting");
            yield return new WaitForSeconds(0.1f);

            // ���� ü�� ����. ������ ü���� ���϶����� �������� SetBossHP�� �� �����ָ� ���� ������.
            bossHPText.text = string.Format("{0:F2}", bossHP);
        }

    }



}
