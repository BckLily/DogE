using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMakerScr : MonoBehaviour
{
    // Item Maker는 DamageUp Item만 생성할 것이다.
    GameObject damageItem;

    Transform tr; // 오브젝트 위치
    Transform playerTr; // 플레이어의 위치로 날아갈 것이다.

    float maxHeight; // 최대 생성 높이
    float minHeight; // 최소 생성 높이
    float maxRight; // 생성할 최대 오른쪽 위치.

    float makeTime; // 생성한 후 지난 시간
    float makeDelay; // 생성하는 주기

    // Start is called before the first frame update
    void Start()
    {
        // 오브젝트 위치
        tr = gameObject.GetComponent<Transform>();

        // 생성할 Damage Item
        damageItem = Resources.Load<GameObject>("Prefabs/Items/DamageUp");
        // 플레이어의 위치
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();

        // 최대 및 최소 생성 위치(y)
        maxHeight = 10f;
        minHeight = -maxHeight;
        // 생성되는 위치(x)
        maxRight = 12f;

        // 생성 딜레이를 준다.
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

            // 생성할 y축
            float yPos = Random.Range(minHeight, maxHeight);
            // 생성할 위치
            Vector3 pos = new Vector3(maxRight, yPos, 0f);
            // 생성할 방향
            Vector3 dir = playerTr.position - tr.position;
            // 방향을 오일러 값으로 변경함.
            //Quaternion rot = Quaternion.Euler(playerTr.position - tr.position);
            Quaternion rot = Quaternion.Euler(dir);

            //Debug.Log("ITEM MAKE");
            Instantiate(damageItem, pos, rot);
        }

    }
}
