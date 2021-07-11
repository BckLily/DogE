using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("- Player Status")]
    [SerializeField]
    private float damage; // �÷��̾� ���ݷ�
    public float damageUp; // ������ ���ݷ�
    public int boomCount; // �÷��̾ ������ �ִ� ��ź�� ����
    GameObject playerExp; // �÷��̾ ����� ��Ÿ�� ���� ����Ʈ
    

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
    Image[] bossHPImages; // ���� hp�� �̹�����. �Ƹ� ��� ���� ��.
    enum BossHPImage
    {
        head = 0, middle, tail,
    } // �Ƹ� ��� ���� ��
    Text bossHPText; // ������ ���� ���� HP�� ǥ������ �ؽ�Ʈ
    [SerializeField]
    float bossMaxHp; // ������ �ִ� HP
    public float SetBossMaxHp
    {
        set { bossMaxHp = value; }
    }
    float bossHP; // ������ ���� HP
    public float SetBossHP
    {
        set
        {
            bossHP = value;
        }
    }
    Image bossHPShadow; // ���� ü�� ���Ҹ� ǥ������ �׸���.


    //[Header("- Game Info")]
    //[SerializeField]
    private float score; // ������ ����

    // �������� ���۵Ǿ��°� Ȯ��.
    public bool isBossStart;


    bool isPlayerDie;


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
        string playerExpPath = "Prefabs/Animations/ExpAnim";
        playerExp = Resources.Load<GameObject>(playerExpPath);

        isPlayerDie = false;

        // UI Canvas�� atkDamageText�� ã�Ƽ� ����.
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Transform>();
        atkDamageText = uiCanvas.Find("AtkDamage").Find("AtkDamageText").GetComponent<Text>();
        scoreText = uiCanvas.Find("GameScore").Find("GameScoreText").GetComponent<Text>();
        boomText = uiCanvas.Find("BoomCanvas").Find("BoomCountText").GetComponent<Text>();

        // ������ ���� UI�� ã�Ƽ� ����
        bossCanvas = uiCanvas.Find("BossCanvas").GetComponent<Transform>();
        bossNameText = bossCanvas.Find("BossName").GetComponent<Text>();
        bossHPBar = bossCanvas.Find("BossHP").GetComponent<Transform>();
        
        bossHPImages = bossHPBar.GetComponentsInChildren<Image>(); // �� ���� ��� ���� �� ����.
        //Debug.Log("�̹��� ����: " + bossHPImages.Length);
        
        bossHPText = bossHPBar.Find("BossHPFloat").GetComponent<Text>();
        bossHPShadow = bossHPBar.Find("HPShadow").GetComponent<Image>();


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


        // W D J �� ���� ������ �� �� �ϳ��� Ű�� �������� �ʴ� ������ �ִ�...?
        //Debug.Log("J: " + (int)KeyCode.J);
        //Debug.Log("W: " + (int)KeyCode.W);
        //Debug.Log("D: " + (int)KeyCode.D);
        //Debug.Log("W | D: " + (int)(KeyCode.W | KeyCode.D));
        //Debug.Log("W & D: " + (int)(KeyCode.W & KeyCode.D));
        //Debug.Log("J | D: " + (int)(KeyCode.J | KeyCode.D));
        //Debug.Log("J & D: " + (int)(KeyCode.J & KeyCode.D));
        //Debug.Log("J | W: " + (int)(KeyCode.J | KeyCode.W));
        //Debug.Log("J & W: " + (int)(KeyCode.J & KeyCode.W));



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
        // �ּ� 0.15�� �����ϰ� %�� �߰� ����.
        float dec_bulletDamage = 0.15f + GetBulletDamage() * per;


        damageUp -= dec_bulletDamage;

        if (damageUp < 0f)
        {
            damage += damageUp;
            damageUp = 0f;
        }


        if (damage <= 0f)
        {
            damage = 0f;

            // �÷��̾� ���
            GameObject player = GameObject.FindGameObjectWithTag("PLAYER");
            
            // ���� ����Ʈ ����.
            GameObject exp = Instantiate(playerExp, player.transform.position, Quaternion.identity);
            Destroy(exp, 1.15f);

            Destroy(player);

            if (isPlayerDie == false)
            {
                StopMakers();
                StartCoroutine(PlayerDie());
                isPlayerDie = true;
            }

            //Debug.Log("Player Die");
        }

        // �÷��̾� ���ݷ��� ȭ�鿡 ǥ��
        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

    }


    // �� ������ ������ ������ ���缭 Enemy�� ã�� ���� ���߰� �ع�����.
    void StopMakers()
    {
        //Debug.Log("Stop Makers");
        GameObject enemyMaker = GameObject.Find("EnemyMaker");
        GameObject itemMaker = GameObject.Find("ItemMaker");

        enemyMaker.SetActive(false);
        itemMaker.SetActive(false);
    }


    IEnumerator PlayerDie()
    {
        // �÷��̾� ��� ����Ʈ�� �����Ǵ� �ð��� �־�� �Ѵ�.
        yield return new WaitForSeconds(1.2f);

        Time.timeScale = 0f;
        //Debug.Log("Stop Time");

        while (true)
        {
            // Time.timeScale�� 0���� �ϸ� ���� �ð��� �帣�� �ʰ� �ȴ�.

            //Debug.Log("Loop");


            // EnterŰ�� ������ timeScale�� 1�� �����ϰ�, ���� �ٽ� �ҷ��´�...?
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Time.timeScale = 1f;
                //Debug.Log("Start Time");
                break;
            }
            // �߰��� ������ �� ��������Ʈ�� UI�� ���� �ϰڳĴ� ������ ��� ������ 
            // ���� �ϰڴٰ��ϸ� ���ο� ���� ȣ���Ѵ�.
            // �ƴ϶�� �ϸ� ���� ȭ������ ���ư���.
            // ���� ȭ���� ������ �Ѵ�.
            // �� ���⼭�� Ÿ�� �����ϸ� ���ߴ� �������� �ٲٸ� �ȴ�.


            yield return null;
        }

        //Debug.Log("Restart Scene");

        SceneManager.LoadScene("SampleScene");
        yield break;

    }




    // ���� �����߷��� �� �����ϴ� �÷��̾� ������
    public void PlayerDamageUp(float inc_damage)
    {
        damageUp += inc_damage;

        // ���ݷ��� �����ϸ� ������ ���� �����ϴ� �ý���
        GameScoreUp(inc_damage * 10f);

        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());


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

        //// ���� HP Image�� ��Ȱ��ȭ ������ �� �����Ƿ� Ȱ��ȭ ��Ų��.
        //for (int i = 0; i < bossHPImages.Length; i++)
        //{
        //    if (bossHPImages[i].gameObject.activeSelf == false)
        //    {
        //        bossHPImages[i].gameObject.SetActive(true);
        //    }
        //}

        // ���� �̸� ����
        bossNameText.text = bossName;

        bossHPShadow.fillAmount = 0;

        // ���� �Է� ���� HP ���� �ִ� HP��� ����.
        // �׷��� �ڷ�ƾ�� ���� �����ϰ� ������  �����ǹǷ� �������� ���� ������ �ʴ´�.
        // ���� ������ �� ���� �޾ƾ� �� ��.
        //bossMaxHp = bossHP;

        // ������ ���� ���� ����
        while (isBossStart)
        {
            BossHPImageChange();
            //Debug.Log("Boss HP Setting");
            yield return new WaitForSeconds(0.1f);

            // ���� ü�� ����. ������ ü���� ���϶����� �������� SetBossHP�� �� �����ָ� ���� ������.
            bossHPText.text = string.Format("{0:F2}", bossHP);
        }

    }


    void BossHPImageChange()
    {
        // ���� ü���� %. �ε� ����� 0~1 ������ �� ���̶� 100�� ������ �ʾҴ�.
        float percent = bossHP / bossMaxHp;
        // �� ���� �Ƹ� ��� ���� �� �ϴ�.
        float one_percent = bossMaxHp / 100;

        //// �ۼ�Ʈ�� 99% �̻��� ��
        //if (percent >= 99)
        //{
        //    // image�� fill ���� 0~1�̴�.
        //    // percent ���� 100 ~ 99 ������ ���� ���̹Ƿ�
        //    // percent - 99�� ���� fill�� ������ �� ���̴�.
        //    bossHPImages[(int)BossHPImage.tail].fillAmount = percent - 99f;
        //}
        //// �ۼ�Ʈ�� 1% �̻��� ��
        //else if (percent >= 1)
        //{
        //    // image�� fill ���� 0~1�̴�.
        //    // percent ���� 1~99�� ������ ���� ���̹Ƿ�
        //    // 
        //}
        //// �� ������ ��

        // �׳� HP Bar�� �׸��ڸ� ������ ü���� ���̴� ��ó�� ���̰� �غ���.
        // �׸��ڴ� 0~1�� �� ���� �����.
        // �׷��Ƿ� 1���� percent ���� ���ָ� ä���� ���̴�.
        bossHPShadow.fillAmount = 1 - percent;

    }



}
