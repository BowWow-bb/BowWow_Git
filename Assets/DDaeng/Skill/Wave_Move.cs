using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Move : MonoBehaviour
{
    GameObject DD;
    bool left;
    Vector3 position;
    float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
        DD = GameObject.Find("DDaeng");
        if (DD.transform.position.x > position.x) // 음파가 왼쪽일때
            left = true;
        else
            left = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(DD.transform.position.x - gameObject.transform.position.x) > 50)
        {
            Destroy(gameObject);
        }
        gameObject.transform.localRotation = Quaternion.Euler(time, time, 0);
        if (left) // 음파가 왼쪽일때
        {
            gameObject.transform.position = new Vector3(position.x - 0.1f, position.y, position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(position.x + 0.1f, position.y, position.z);
        }
        position = gameObject.transform.position;
        time += 3f;
    } 

    void OnTriggerEnter(Collider other)//몬스터 때리기 
    {
        if (other.gameObject.GetComponent<small_toll>() != null)    //스테이지2 몬스터와 충돌한 경우
        {
            small_toll monster = other.GetComponent<small_toll>();
            monster.TakeDamage(10);//공격         
            monster.hpMove(10);
        }
        if(other.gameObject.GetComponent<small_toll_stage1>() != null)  //스테이지1 몬스터와 충돌한 경우
        {
            small_toll_stage1 monster = other.GetComponent<small_toll_stage1>();
            monster.TakeDamage(10);
            monster.hpMove(10);
        }
        if(other.gameObject.GetComponent<Bigtol>() != null)
        {
            Bigtol monster = other.GetComponent<Bigtol>();
            monster.TakeDamage(10);
            monster.hpMove(10);
        }
    }
}
