using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private float damage; // 플레이어 공격력
    public float damageUp; // 증가한 공격력


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
        // 총알의 공격력을 반환
        return (damage + damageUp);
    }

    // 플레이어가 피격 당할 경우
    // PlayerHit(감소할 공격력)
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
            // 플레이어 사망
        }
    }
}
