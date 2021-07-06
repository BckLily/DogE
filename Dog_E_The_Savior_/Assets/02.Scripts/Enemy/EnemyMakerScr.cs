using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMakerScr : MonoBehaviour
{
    // ��ġ
    Transform tr;

    GameObject[] enemies; // ������ Enemy (���߿��� ����Ʈ)


    [Header("- Enemy Pos")]
    [SerializeField]
    float maxHeight; // ������ �� �ִ� �ְ� ����
    [SerializeField]
    float minHeight; // ������ �� �ִ� �ּ� ����
    float rightPos; // ������ ��ġ (������ ȭ�� ��)

    // �� ���� ������ �ִ� �� set
    int makeEnemyNumber;

    [Header("- Enemy Make Data")]
    [SerializeField]
    float enemyMakeDelay; // �� ���� ������
    [SerializeField]
    float enemyMakeTime; //  �ټ� �� ������ ����                   �� ���� �� ���� �ð�



    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        // �� �������� �ִ� ����
        string loadPath = "Prefabs/Enemies/";
        enemies = Resources.LoadAll<GameObject>(loadPath);

        // �ִ� ���� ���� �� ������ x��ǥ
        maxHeight = 3.5f;
        minHeight = 1.5f;
        rightPos = 12f;

        // �� ��Ʈ ����
        makeEnemyNumber = 1;

        // �� �����ϴ� �ð�
        enemyMakeDelay = 6f;
        enemyMakeTime = 1f;

    }



    // Update is called once per frame
    void Update()
    {
        // �� ����
        MakeEnemy();

    }


    // �� ���� �Լ�
    void MakeEnemy()
    {
        enemyMakeTime += Time.deltaTime;

        if (enemyMakeTime >= enemyMakeDelay)
        {
            enemyMakeTime -= enemyMakeDelay;

            // ������ ���̸� �������� ����.
            float heightPos = Random.Range(minHeight, maxHeight);
            Vector3 makePos = new Vector3(rightPos, heightPos, 0);

            var enemy1 = Instantiate(enemies[0], makePos, Quaternion.identity);

            makePos.y = (makePos.y * -1);
            var enemy2 = Instantiate(enemies[0], makePos, Quaternion.identity);

            //makePos.x += 2;

        }

    }


}
