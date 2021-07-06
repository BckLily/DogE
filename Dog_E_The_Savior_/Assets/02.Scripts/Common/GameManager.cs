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

    [Header("- Game Info")]
    [SerializeField]
    private float score; // ������ ����




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
        // ���ҽ�ų ���ݷ� %���� (10 ~ 30)%
        var per = Random.Range(0.1f, 0.3f);
        // ���� ������ ���ݷ�
        // �ּ� 0.25�� �����ϰ� %�� �߰� ����.
        float dec_bulletDamage = 0.25f + GetBulletDamage() * per;

#if UNITY_EDITOR

        Debug.Log("Player Hit");


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

            Debug.Log("Player Die");
        }
    }


    // ���� �����߷��� �� �����ϴ� �÷��̾� ������
    public void PlayerDamageUp(float inc_damage)
    {
        damageUp += inc_damage;

        GameScoreUp(inc_damage * 10f);

        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

#if UNITY_EDITOR

        Debug.Log("Player Damage Increasesd");

#endif 

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

}
