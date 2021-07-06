using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMakerScr : MonoBehaviour
{
    // 위치
    Transform tr;

    GameObject[] enemies; // 생성할 Enemy (나중에는 리스트)


    [Header("- Enemy Pos")]
    [SerializeField]
    float maxHeight; // 생성할 수 있는 최고 높이
    [SerializeField]
    float minHeight; // 생성할 수 있는 최소 높이
    float rightPos; // 생성될 위치 (오른쪽 화면 밖)

    // 한 번에 생성할 최대 적 set
    int makeEnemyNumber;

    [Header("- Enemy Make Data")]
    [SerializeField]
    float enemyMakeDelay; // 적 생성 딜레이
    [SerializeField]
    float enemyMakeTime; //  다수 적 생성시 간격                   적 생성 후 지난 시간



    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        // 적 프리팹이 있는 폴더
        string loadPath = "Prefabs/Enemies/";
        enemies = Resources.LoadAll<GameObject>(loadPath);

        // 최대 생성 높이 및 생성될 x좌표
        maxHeight = 3.5f;
        minHeight = 1.5f;
        rightPos = 12f;

        // 한 세트 생성
        makeEnemyNumber = 1;

        // 적 생성하는 시간
        enemyMakeDelay = 6f;
        enemyMakeTime = 1f;

    }



    // Update is called once per frame
    void Update()
    {
        // 적 생성
        MakeEnemy();

    }


    // 적 생성 함수
    void MakeEnemy()
    {
        enemyMakeTime += Time.deltaTime;

        if (enemyMakeTime >= enemyMakeDelay)
        {
            enemyMakeTime -= enemyMakeDelay;

            // 생성될 높이를 랜덤으로 설정.
            float heightPos = Random.Range(minHeight, maxHeight);
            Vector3 makePos = new Vector3(rightPos, heightPos, 0);

            var enemy1 = Instantiate(enemies[0], makePos, Quaternion.identity);

            makePos.y = (makePos.y * -1);
            var enemy2 = Instantiate(enemies[0], makePos, Quaternion.identity);

            //makePos.x += 2;

        }

    }


}
