using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMakerScr : MonoBehaviour
{
    // Item Maker�� DamageUp Item�� ������ ���̴�.
    GameObject damageItem;

    Transform tr; // ������Ʈ ��ġ
    Transform playerTr; // �÷��̾��� ��ġ�� ���ư� ���̴�.

    float maxHeight; // �ִ� ���� ����
    float minHeight; // �ּ� ���� ����
    float maxRight; // ������ �ִ� ������ ��ġ.

    float makeTime; // ������ �� ���� �ð�
    float makeDelay; // �����ϴ� �ֱ�

    // Start is called before the first frame update
    void Start()
    {
        // ������Ʈ ��ġ
        tr = gameObject.GetComponent<Transform>();

        // ������ Damage Item
        damageItem = Resources.Load<GameObject>("Prefabs/Items/DamageUp");
        // �÷��̾��� ��ġ
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();

        // �ִ� �� �ּ� ���� ��ġ(y)
        maxHeight = 10f;
        minHeight = -maxHeight;
        // �����Ǵ� ��ġ(x)
        maxRight = 12f;

        // ���� �����̸� �ش�.
        makeDelay = 12f;
        makeTime = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        ItemMake();
    }

    void ItemMake()
    {
        makeTime += Time.deltaTime;

        if (makeTime >= makeDelay)
        {
            makeTime -= makeDelay;

            // ������ y��
            float yPos = Random.Range(minHeight, maxHeight);
            // ������ ��ġ
            Vector3 pos = new Vector3(maxRight, yPos, 0f);
            // ������ ����
            Vector3 dir = playerTr.position - tr.position;
            // ������ ���Ϸ� ������ ������.
            //Quaternion rot = Quaternion.Euler(playerTr.position - tr.position);
            Quaternion rot = Quaternion.Euler(dir);

            //Debug.Log("ITEM MAKE");
            Instantiate(damageItem, pos, rot);
        }

    }
}
