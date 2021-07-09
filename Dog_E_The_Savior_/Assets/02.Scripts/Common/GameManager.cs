using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("- Player Damage")]
    [SerializeField]
    private float damage; // 플레이어 공격력
    public float damageUp; // 증가한 공격력
    public int boomCount; // 플레이어가 가지고 있는 폭탄의 개수

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
    Image[] bossHPImages; // 보스 hp바 이미지들.
    enum BossHPImage
    {
        head = 0, middle, tail,
    }
    Text bossHPText; // 보스의 현재 남은 HP를 표시해줄 텍스트
    float bossHP; // 보스의 현재 HP
    public float SetBossHP
    {
        set
        {
            bossHP = value;
        }
    }


    //[Header("- Game Info")]
    //[SerializeField]
    private float score; // 게임의 점수

    // 보스전이 시작되었는가 확인.
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

        // UI Canvas와 atkDamageText를 찾아서 저장.
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Transform>();
        atkDamageText = uiCanvas.Find("AtkDamage").Find("AtkDamageText").GetComponent<Text>();
        scoreText = uiCanvas.Find("GameScore").Find("GameScoreText").GetComponent<Text>();
        boomText = uiCanvas.Find("BoomCanvas").Find("BoomCountText").GetComponent<Text>();

        // 보스전 관련 UI를 찾아서 저장
        bossCanvas = uiCanvas.Find("BossCanvas").GetComponent<Transform>();
        bossNameText = bossCanvas.Find("BossName").GetComponent<Text>();
        bossHPBar = bossCanvas.Find("BossHP").GetComponent<Transform>();
        bossHPImages = bossHPBar.GetComponentsInChildren<Image>();
        //Debug.Log("이미지 길이: " + bossHPImages.Length);
        bossHPText = bossHPBar.Find("BossHPFloat").GetComponent<Text>();

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
        // 최소 0.25이 감소하고 %로 추가 감소.
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
            // 플레이어 사망

            //Debug.Log("Player Die");
        }
    }


    // 적을 쓰러뜨렸을 때 증가하는 플레이어 데미지
    public void PlayerDamageUp(float inc_damage)
    {
        damageUp += inc_damage;

        GameScoreUp(inc_damage * 10f);

        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

#if UNITY_EDITOR

        //Debug.Log("Player Damage Increasesd");

#endif 

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
        // 보스 이름 설정
        bossNameText.text = bossName;

        // 보스전 진행 중일 때만
        while (isBossStart)
        {
            //Debug.Log("Boss HP Setting");
            yield return new WaitForSeconds(0.1f);

            // 보스 체력 설정. 보스의 체력이 깎일때마다 보스에서 SetBossHP로 값 보내주면 되지 않을까.
            bossHPText.text = string.Format("{0:F2}", bossHP);
        }

    }



}
