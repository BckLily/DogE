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
    private float damage; // 플레이어 공격력
    public float damageUp; // 증가한 공격력
    public int boomCount; // 플레이어가 가지고 있는 폭탄의 개수
    GameObject playerExp; // 플레이어가 사망시 나타날 폭발 이펙트
    

    Transform uiCanvas; // UI를 포함하고 있는 Canvas
    Text atkDamageText; // 플레이어 공격력을 표시할 텍스트
    string atkBaseText; // 공격력 기본 문자열
    Text scoreText; // 점수를 표시할 텍스트
    Text boomText; // 폭탄의 개수를 표시할 텍스트
    string boomBaseText; // 폭탄 기본 문자열
    public Transform bossCanvas; // 보스 관련 UI를 가지고 있는 Cavnas
    Text bossNameText; // 보스 이름을 표시해줄 Text
    string bossName; // 보스 이름 저장...?
    public string SetBossName
    {
        set
        {
            bossName = value;
        }
    }
    Transform bossHPBar; // 보스의 HP바가 있는 위치.
    [SerializeField]
    Image[] bossHPImages; // 보스 hp바 이미지들. 아마 사용 안할 듯.
    enum BossHPImage
    {
        head = 0, middle, tail,
    } // 아마 사용 안할 듯
    Text bossHPText; // 보스의 현재 남은 HP를 표시해줄 텍스트
    [SerializeField]
    float bossMaxHp; // 보스의 최대 HP
    public float SetBossMaxHp
    {
        set { bossMaxHp = value; }
    }
    float bossHP; // 보스의 현재 HP
    public float SetBossHP
    {
        set
        {
            bossHP = value;
        }
    }
    Image bossHPShadow; // 보스 체력 감소를 표시해줄 그림자.


    //[Header("- Game Info")]
    //[SerializeField]
    private float score; // 게임의 점수

    // 보스전이 시작되었는가 확인.
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

        // UI Canvas와 atkDamageText를 찾아서 저장.
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Transform>();
        atkDamageText = uiCanvas.Find("AtkDamage").Find("AtkDamageText").GetComponent<Text>();
        scoreText = uiCanvas.Find("GameScore").Find("GameScoreText").GetComponent<Text>();
        boomText = uiCanvas.Find("BoomCanvas").Find("BoomCountText").GetComponent<Text>();

        // 보스전 관련 UI를 찾아서 저장
        bossCanvas = uiCanvas.Find("BossCanvas").GetComponent<Transform>();
        bossNameText = bossCanvas.Find("BossName").GetComponent<Text>();
        bossHPBar = bossCanvas.Find("BossHP").GetComponent<Transform>();
        
        bossHPImages = bossHPBar.GetComponentsInChildren<Image>(); // 이 값은 사용 안할 것 같다.
        //Debug.Log("이미지 길이: " + bossHPImages.Length);
        
        bossHPText = bossHPBar.Find("BossHPFloat").GetComponent<Text>();
        bossHPShadow = bossHPBar.Find("HPShadow").GetComponent<Image>();


        // 보스전 시작하면 보이면 되기 때문에 Canvas를 Active false로 한다.
        // 보스전 이 끝나면 다시 비활성화 하게 한다.
        bossCanvas.gameObject.SetActive(false);

        // 공격력 표시
        atkBaseText = "공격력: ";
        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

        // 점수 표시
        score = 0f;
        // 나중에는 저장한 데이터에서 이어하기 점수 가져오등가.
        scoreText.text = (int)score + "";

        // 폭탄 개수 표시
        boomBaseText = "X ";
        boomText.text = boomBaseText + boomCount;

        isBossStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime;
        GameScoreUp(0f);


        // W D J 를 같이 누르면 셋 중 하나의 키가 동작하지 않는 문제가 있다...?
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
        // 총알의 공격력을 반환
        return (damage + damageUp);
    }


    // 플레이어가 피격 당할 경우
    // PlayerHit()
    public void PlayerHit()
    {
        // 감소시킬 공격력 %설정 (5 ~ 20)%
        var per = Random.Range(0.05f, 0.2f);
        // 최종 감소할 공격력
        // 최소 0.15이 감소하고 %로 추가 감소.
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

            // 플레이어 사망
            GameObject player = GameObject.FindGameObjectWithTag("PLAYER");
            
            // 폭발 이펙트 생성.
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

        // 플레이어 공격력을 화면에 표시
        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

    }


    // 적 생성과 아이템 생성을 멈춰서 Enemy를 찾는 것을 멈추게 해버리자.
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
        // 플레이어 사망 이펙트가 생성되는 시간을 주어야 한다.
        yield return new WaitForSeconds(1.2f);

        Time.timeScale = 0f;
        //Debug.Log("Stop Time");

        while (true)
        {
            // Time.timeScale을 0으로 하면 게임 시간이 흐르지 않게 된다.

            //Debug.Log("Loop");


            // Enter키를 누르면 timeScale을 1로 복귀하고, 씬을 다시 불러온다...?
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Time.timeScale = 1f;
                //Debug.Log("Start Time");
                break;
            }
            // 추가할 동작은 씬 오버라이트나 UI로 새로 하겠냐는 문구를 띄운 다음에 
            // 새로 하겠다고하면 새로운 씬을 호출한다.
            // 아니라고 하면 메인 화면으로 돌아간다.
            // 메인 화면을 만들어야 한다.
            // 즉 여기서는 타임 스케일만 멈추는 동작으로 바꾸면 된다.


            yield return null;
        }

        //Debug.Log("Restart Scene");

        SceneManager.LoadScene("SampleScene");
        yield break;

    }




    // 적을 쓰러뜨렸을 때 증가하는 플레이어 데미지
    public void PlayerDamageUp(float inc_damage)
    {
        damageUp += inc_damage;

        // 공격력이 증가하면 점수도 같이 증가하는 시스템
        GameScoreUp(inc_damage * 10f);

        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());


    }

    // 점수를 보내주는 함수
    public float GetScore
    {
        get
        {
            return score;
        }

    }


    // 점수 증가 함수.
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
        // 보스 관련 정보를 보여주는 Canvas를 On으로 한다.
        bossCanvas.gameObject.SetActive(true);

        //// 보스 HP Image가 비활성화 되있을 수 있으므로 활성화 시킨다.
        //for (int i = 0; i < bossHPImages.Length; i++)
        //{
        //    if (bossHPImages[i].gameObject.activeSelf == false)
        //    {
        //        bossHPImages[i].gameObject.SetActive(true);
        //    }
        //}

        // 보스 이름 설정
        bossNameText.text = bossName;

        bossHPShadow.fillAmount = 0;

        // 최초 입력 받은 HP 값을 최대 HP라고 본다.
        // 그런데 코루틴이 먼저 시작하고 보스가  생성되므로 정상적인 값이 들어오지 않는다.
        // 보스 생성할 때 새로 받아야 할 듯.
        //bossMaxHp = bossHP;

        // 보스전 진행 중일 때만
        while (isBossStart)
        {
            BossHPImageChange();
            //Debug.Log("Boss HP Setting");
            yield return new WaitForSeconds(0.1f);

            // 보스 체력 설정. 보스의 체력이 깎일때마다 보스에서 SetBossHP로 값 보내주면 되지 않을까.
            bossHPText.text = string.Format("{0:F2}", bossHP);
        }

    }


    void BossHPImageChange()
    {
        // 보스 체력의 %. 인데 사용을 0~1 값으로 할 것이라 100을 곱하지 않았다.
        float percent = bossHP / bossMaxHp;
        // 이 값은 아마 사용 안할 듯 하다.
        float one_percent = bossMaxHp / 100;

        //// 퍼센트가 99% 이상일 때
        //if (percent >= 99)
        //{
        //    // image의 fill 값은 0~1이다.
        //    // percent 값이 100 ~ 99 값으로 들어올 것이므로
        //    // percent - 99의 값을 fill에 넣으면 될 것이다.
        //    bossHPImages[(int)BossHPImage.tail].fillAmount = percent - 99f;
        //}
        //// 퍼센트가 1% 이상일 때
        //else if (percent >= 1)
        //{
        //    // image의 fill 값은 0~1이다.
        //    // percent 값이 1~99의 값으로 들어올 것이므로
        //    // 
        //}
        //// 그 이하일 때

        // 그냥 HP Bar에 그림자를 씌워서 체력이 깎이는 것처럼 보이게 해보자.
        // 그림자는 0~1로 갈 수록 생긴다.
        // 그러므로 1에서 percent 값을 빼주면 채워질 것이다.
        bossHPShadow.fillAmount = 1 - percent;

    }



}
