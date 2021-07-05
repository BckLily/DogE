using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRepeatScr : MonoBehaviour
{

    // 배경 화면의 Material
    Material mat;
    
    // 배경 화면의 Offset
    Vector2 offset;

    public float speed; // 배경의 이동 속도
    // Start is called before the first frame update
    void Start()
    {
        // 오브젝트가 화면에 그려지는 방식을 표현해주는
        // 마테리얼을 가져온다.
        mat = GetComponent<SpriteRenderer>().material;

        speed = 0.3f;

    }

    // Update is called once per frame
    void Update()
    {
        // offset의 x를 적절한 속도로 변화시키면서
        // 화면에 보이는 이미지를 슬라이드 시킨다.
        offset.x += Time.deltaTime * speed;

        // offset 0과 1은 동일한 값이므로 offset이 1을 넘으면 -1 처리를 하여 너무 큰 값을 가지지 않게 한다.
        if (offset.x >= 1)
        {
            offset.x -= 1;
        }

        // 변경된 offset 값을 material의 mainTextureOffset에 넣어 적용시킨다.
        mat.mainTextureOffset = offset;
    }
}
