using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMakerScr : MonoBehaviour
{
    Transform tr; // 위치
    GameManager gameMgr; // 게임 매니저 위치
    GameObject[] enemies; // 생성할 Enemy (나중에는 리스트)

    enum Enemy
    {
        Stelar=1, Beet=0, 
    }

    //[Header("- Enemy Pos")]
    //[SerializeField]
    float maxHeight; // 생성할 수 있는 최고 높이
    //[SerializeField]
    float minHeight; // 생성할 수 있는 최소 높이
    float rightPos; // 생성될 위치 (오른쪽 화면 밖)

    // 한 번에 생성할 최대 적 set
    Enemy makeEnemyNumber;

    //[Header("- Enemy Make Data")]
    //[SerializeField]
    float enemyMakeDelay; // 적 생성 딜레이
    //[SerializeField]
    float enemyMakeTime; //  다수 적 생성시 간격                   적 생성 후 지난 시간

    // 스테이지 시작 점수
    // 점수가 몇 점 증가하면 보스 스테이지가 시작되게 한다.
    [SerializeField]
    float stageStartScore;
    // 보스 스테이지를 시작하게 할 점수
    float bossStartScore;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // 적 프리팹이 있는 폴더
        string loadPath = "Prefabs/Enemies/";
        enemies = Resources.LoadAll<GameObject>(loadPath);

        // 최대 생성 높이 및 생성될 x좌표
        maxHeight = 3.5f;
        minHeight = 1f;
        rightPos = 12f;

        // 한 세트 생성
        makeEnemyNumber = Enemy.Stelar;

        // 적 생성하는 시간
        enemyMakeDelay = 4f;
        enemyMakeTime = 2f;

        stageStartScore = gameMgr.GetScore;
        bossStartScore = 200f;

    }



    // Update is called once per frame
    void Update()
    {
        // 보스 생성 점수인지 확인
        ScoreCheck();
        // 적 생성
        MakeEnemy();

    }

    void ScoreCheck()
    {
        if (gameMgr.GetScore - stageStartScore >= bossStartScore && gameMgr.isBossStart == false)
        {
            makeEnemyNumber = Enemy.Beet;
            // 생성하면 보스전 시작을 true로 만들고
            // 보스가 죽을 때 보스전 시작을 false로 만든다.
            // 보스가 죽을 때 stageStartScore를 다시 설정한다. (보스 사망 부분에서 처리하면 될 것 같다.)
            gameMgr.isBossStart = true;
        }
    }



    // 적 생성 함수
    void MakeEnemy()
    {
        // 생성할 적 캐릭터가 Stelar일 때
        if (makeEnemyNumber == Enemy.Stelar)
        {
            enemyMakeTime += Time.deltaTime;

            if (enemyMakeTime >= enemyMakeDelay)
            {
                enemyMakeTime -= enemyMakeDelay;

                // 생성될 높이를 랜덤으로 설정.
                float heightPos = Random.Range(minHeight, maxHeight);
                Vector3 makePos = new Vector3(rightPos, heightPos, 0);

                var enemy1 = Instantiate(enemies[(int)makeEnemyNumber], makePos, Quaternion.identity);

                makePos.y = (makePos.y * -1);
                var enemy2 = Instantiate(enemies[(int)makeEnemyNumber], makePos, Quaternion.identity);

                //makePos.x += 2;
            }

        }
        // 생성할 적 캐릭터가 Beet일 때.
        else if (makeEnemyNumber == Enemy.Beet)
        {
            // 보스가 생성되고 바로 일반 몬스터가 생성되지 않게 Time을 초기화.
            enemyMakeTime = 0f;
            // 보스 전에서는 일반 몬스터의 생성 딜레이를 늘려준다.
            enemyMakeDelay *= 2f;

            // 보스 캐릭터를 생성할 위치를 설정하고 생성한다.
            Vector3 makePos = new Vector3(rightPos, 0, 0);
            Instantiate(enemies[(int)makeEnemyNumber], makePos, Quaternion.identity);

            // 생성할 적을 Stelar로 만든다.
            makeEnemyNumber = Enemy.Stelar;



        }


    }


}
