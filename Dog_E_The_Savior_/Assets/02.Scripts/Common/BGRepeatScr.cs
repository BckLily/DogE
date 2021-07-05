using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRepeatScr : MonoBehaviour
{

    // ��� ȭ���� Material
    Material mat;
    
    // ��� ȭ���� Offset
    Vector2 offset;

    public float speed; // ����� �̵� �ӵ�
    // Start is called before the first frame update
    void Start()
    {
        // ������Ʈ�� ȭ�鿡 �׷����� ����� ǥ�����ִ�
        // ���׸����� �����´�.
        mat = GetComponent<SpriteRenderer>().material;

        speed = 0.3f;

    }

    // Update is called once per frame
    void Update()
    {
        // offset�� x�� ������ �ӵ��� ��ȭ��Ű�鼭
        // ȭ�鿡 ���̴� �̹����� �����̵� ��Ų��.
        offset.x += Time.deltaTime * speed;

        // offset 0�� 1�� ������ ���̹Ƿ� offset�� 1�� ������ -1 ó���� �Ͽ� �ʹ� ū ���� ������ �ʰ� �Ѵ�.
        if (offset.x >= 1)
        {
            offset.x -= 1;
        }

        // ����� offset ���� material�� mainTextureOffset�� �־� �����Ų��.
        mat.mainTextureOffset = offset;
    }
}
