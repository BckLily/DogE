using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMakerScr : MonoBehaviour
{
    // ��ġ
    Transform tr;

    GameObject[] enemies; // ������ Enemy (���߿��� ����Ʈ)

    enum Enemy
    {
        Stelar=1, Beet=0, 
    }

    [Header("- Enemy Pos")]
    [SerializeField]
    float maxHeight; // ������ �� �ִ� �ְ� ����
    [SerializeField]
    float minHeight; // ������ �� �ִ� �ּ� ����
    float rightPos; // ������ ��ġ (������ ȭ�� ��)

    // �� ���� ������ �ִ� �� set
    Enemy makeEnemyNumber;

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
        minHeight = 1f;
        rightPos = 12f;

        // �� ��Ʈ ����
        makeEnemyNumber = Enemy.Stelar;

        // �� �����ϴ� �ð�
        enemyMakeDelay = 4f;
        enemyMakeTime = 2f;



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

                var enemy1 = Instantiate(enemies[(int)Enemy.Stelar], makePos, Quaternion.identity);

                makePos.y = (makePos.y * -1);
                var enemy2 = Instantiate(enemies[(int)Enemy.Stelar], makePos, Quaternion.identity);

                //makePos.x += 2;

            }

        }
        // ������ �� ĳ���Ͱ� Beet�� ��.
        else if (makeEnemyNumber == Enemy.Beet)
        {

        }


    }


}
