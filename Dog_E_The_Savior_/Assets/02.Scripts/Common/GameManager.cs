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

    [Header("- Game Info")]
    [SerializeField]
    private float score; // 게임의 점수




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
        // 감소시킬 공격력 %설정 (10 ~ 30)%
        var per = Random.Range(0.1f, 0.3f);
        // 최종 감소할 공격력
        // 최소 0.25이 감소하고 %로 추가 감소.
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
            // 플레이어 사망

            Debug.Log("Player Die");
        }
    }


    // 적을 쓰러뜨렸을 때 증가하는 플레이어 데미지
    public void PlayerDamageUp(float inc_damage)
    {
        damageUp += inc_damage;

        GameScoreUp(inc_damage * 10f);

        atkDamageText.text = atkBaseText + string.Format("{0:f2}", GetBulletDamage());

#if UNITY_EDITOR

        Debug.Log("Player Damage Increasesd");

#endif 

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

}
