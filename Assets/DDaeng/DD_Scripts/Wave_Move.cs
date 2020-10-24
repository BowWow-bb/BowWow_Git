using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Move : MonoBehaviour
{
    GameObject DD; // 음파를 발사한 주인공(땡이) 오브젝트
    bool left;  // 음파가 땡이 보다 왼쪽인지 판단
    Vector3 position; // 음파의 좌표
    float rotate = 0f; // 음파가 자전하는 rotate 각 
    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position; // 시작좌표 저장
        DD = GameObject.Find("DDaeng"); // 땡이 오브젝트를 찾아서 저장
        if (DD.transform.position.x > position.x) // 음파가 땡이보다 왼쪽일때, 오른쪽일 때 구분
            left = true;
        else
            left = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(DD.transform.position.x - gameObject.transform.position.x) > 50) // 음파와 땡이사이의 거리가 일정 이상이면 음파 파괴
        {
            Destroy(gameObject);
        }
        if (left) // 음파가 왼쪽일때
        {
            // 왼쪽으로 전진             
            gameObject.transform.position = new Vector3(position.x - 1f, position.y, position.z);
        }
        else // 오른쪽 일때
        {
            // 오른쪽으로 전진
            gameObject.transform.position = new Vector3(position.x +1f, position.y, position.z);
        }
        position = gameObject.transform.position; // 좌표 저장
        rotate += 3f; // 음파가 꾸준히 자전하도록 증가
        gameObject.transform.localRotation = Quaternion.Euler(rotate, rotate, 0); // 자전 적용
    } 

    void OnTriggerEnter(Collider other)//몬스터 때리기 
    {
        if (other.gameObject.GetComponent<small_toll>() != null)    //스테이지2 몬스터와 충돌한 경우
        {
            small_toll monster = other.GetComponent<small_toll>();
            if(monster.transform.position.x> DD.transform.position.x)//땡이가 왼쪽이면 
            {
                monster.head.position = monster.headright.position;
            }
            else//땡이가 오른쪽이면 
            {
                monster.head.position = monster.headleft.position;
            }
            monster.TakeDamage(10);//공격         
            monster.hpMove(10);
        }
        if(other.gameObject.GetComponent<small_toll_stage1>() != null)  //스테이지1 몬스터와 충돌한 경우
        {
            small_toll_stage1 monster = other.GetComponent<small_toll_stage1>();
            if (monster.transform.position.x > DD.transform.position.x)//땡이가 왼쪽이면 
            {
                monster.head.position = monster.headright.position;
            }
            else//땡이가 오른쪽이면 
            {
                monster.head.position = monster.headleft.position;
            }
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
