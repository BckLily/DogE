using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private float damage; // �÷��̾� ���ݷ�
    public float damageUp; // ������ ���ݷ�


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        damage = 10f;
        damageUp = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetBulletDamage()
    {
        // �Ѿ��� ���ݷ��� ��ȯ
        return (damage + damageUp);
    }

    // �÷��̾ �ǰ� ���� ���
    // PlayerHit(������ ���ݷ�)
    public void PlayerHit(float dec_bulletDamage)
    {
        damageUp -= dec_bulletDamage;
        if(damageUp < 0f)
        {
            damage += damageUp;
            damageUp = 0f;
        }

        if (damage <= 0)
        {
            // �÷��̾� ���
        }
    }
}
