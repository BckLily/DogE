using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMakerScr : MonoBehaviour
{
    Transform tr; // ��ġ
    GameManager gameMgr; // ���� �Ŵ��� ��ġ
    GameObject[] enemies; // ������ Enemy (���߿��� ����Ʈ)

    enum Enemy
    {
        Stelar=1, Beet=0, 
    }

    //[Header("- Enemy Pos")]
    //[SerializeField]
    float maxHeight; // ������ �� �ִ� �ְ� ����
    //[SerializeField]
    float minHeight; // ������ �� �ִ� �ּ� ����
    float rightPos; // ������ ��ġ (������ ȭ�� ��)

    // �� ���� ������ �ִ� �� set
    Enemy makeEnemyNumber;

    //[Header("- Enemy Make Data")]
    //[SerializeField]
    float enemyMakeDelay; // �� ���� ������
    //[SerializeField]
    float enemyMakeTime; //  �ټ� �� ������ ����                   �� ���� �� ���� �ð�

    // �������� ���� ����
    // ������ �� �� �����ϸ� ���� ���������� ���۵ǰ� �Ѵ�.
    [SerializeField]
    float stageStartScore;
    // ���� ���������� �����ϰ� �� ����
    float bossStartScore;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();

        // �� �������� �ִ� ����
        string loadPath = "Prefabs/Enemies/";
        enemies = Resources.LoadAll<GameObject>(loadPath);

        // �ִ� ���� ���� �� ������ x��ǥ
        maxHeight = 3.5f;
        minHeight = 1f;
        rightPos = 12f;

        // �� ��Ʈ ����
        makeEnemyNumber = Enemy.Stelar;

        // �� �����ϴ� �ð�
        enemyMakeDelay = 4f;
        enemyMakeTime = 2f;

        stageStartScore = gameMgr.GetScore;
        bossStartScore = 200f;

    }



    // Update is called once per frame
    void Update()
    {
        // ���� ���� �������� Ȯ��
        ScoreCheck();
        // �� ����
        MakeEnemy();

    }

    void ScoreCheck()
    {
        if (gameMgr.GetScore - stageStartScore >= bossStartScore && gameMgr.isBossStart == false)
        {
            makeEnemyNumber = Enemy.Beet;
            // �����ϸ� ������ ������ true�� �����
            // ������ ���� �� ������ ������ false�� �����.
            // ������ ���� �� stageStartScore�� �ٽ� �����Ѵ�. (���� ��� �κп��� ó���ϸ� �� �� ����.)
            gameMgr.isBossStart = true;
        }
    }



    // �� ���� �Լ�
    void MakeEnemy()
    {
        // ������ �� ĳ���Ͱ� Stelar�� ��
        if (makeEnemyNumber == Enemy.Stelar)
        {
            enemyMakeTime += Time.deltaTime;

            if (enemyMakeTime >= enemyMakeDelay)
            {
                enemyMakeTime -= enemyMakeDelay;

                // ������ ���̸� �������� ����.
                float heightPos = Random.Range(minHeight, maxHeight);
                Vector3 makePos = new Vector3(rightPos, heightPos, 0);

                var enemy1 = Instantiate(enemies[(int)makeEnemyNumber], makePos, Quaternion.identity);

                makePos.y = (makePos.y * -1);
                var enemy2 = Instantiate(enemies[(int)makeEnemyNumber], makePos, Quaternion.identity);

                //makePos.x += 2;
            }

        }
        // ������ �� ĳ���Ͱ� Beet�� ��.
        else if (makeEnemyNumber == Enemy.Beet)
        {
            // ������ �����ǰ� �ٷ� �Ϲ� ���Ͱ� �������� �ʰ� Time�� �ʱ�ȭ.
            enemyMakeTime = 0f;
            // ���� �������� �Ϲ� ������ ���� �����̸� �÷��ش�.
            enemyMakeDelay *= 2f;

            // ���� ĳ���͸� ������ ��ġ�� �����ϰ� �����Ѵ�.
            Vector3 makePos = new Vector3(rightPos, 0, 0);
            Instantiate(enemies[(int)makeEnemyNumber], makePos, Quaternion.identity);

            // ������ ���� Stelar�� �����.
            makeEnemyNumber = Enemy.Stelar;



        }


    }


}
