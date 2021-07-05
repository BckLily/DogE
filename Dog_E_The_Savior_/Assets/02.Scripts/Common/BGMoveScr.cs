using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMoveScr : MonoBehaviour
{
    public float speed; // ����� �̵� �ӵ�
    public float scale; // ����� ũ�� ���� >> bg���� ������� �ʰ� cloud ���� �Ϳ� ���

    Vector3 moveDir;    // ����� �̵� ����

    float maxLeft;

    Transform tr;       // ������Ʈ�� ��ġ


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        speed = 2f; // �̵� �ӵ� 3f
        scale = 1; // ũ�� 1
        moveDir = Vector3.left; // �̵� ������ ����
        maxLeft = 19.47f; // �ִ� �������� �� �� �ִ� �Ÿ� >> 19.47

        

        
    }

    private void FixedUpdate()
    {
        // ����� moveDir �������� speed�� ��ŭ ������
        tr.position += moveDir * speed * Time.deltaTime;

        
        // ���� �������� ���� ��
        if (tr.position.x <= -maxLeft)
        {
            // Tag�� ����̸�
            if (this.gameObject.CompareTag("BACKGROUND"))
            {

                // ���� ��ġ���� �ִ� ���� ���� 2�� �����־
                Vector3 movePos = tr.position;
                movePos.x += maxLeft * 2;

                // ���ο� ��ġ�� �����ؼ� �̵���Ų��.
                // ����� ����Ǽ� ���̰� �ȴ�.
                tr.position = movePos;
            }
            // Tag�� �����̸�
            else if (this.gameObject.CompareTag("CLOUD"))
            {
                // ������Ʈ�� �����Ѵ�.
                Destroy(this.gameObject);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
