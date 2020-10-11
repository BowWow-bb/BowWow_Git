using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_fireball_stage2 : MonoBehaviour
{
    //파이어볼 움직임
    public GameObject smalltall;//스몰톨 가져오기
    public GameObject DDaeng;//플레이어 위치

    Vector3 ball;
    Vector3 me;
    Vector3 target;
    Vector3 moveVelocity;//공이 나갈 방향 (파이어볼 속도)


    float maxh;
    float minh;

    float movePower = 100;//파이어볼 속력 
    float gravity = 9.8f;
    float accel = 0f;//가속도
    //float c = 0.7f;//탄성계수

    bool isUp = false;//처음 생성 시 y 증가 여부 

    // Start is called before the first frame update
    void Start()
    {
        DDaeng = GameObject.Find("DDaeng");

        target = DDaeng.transform.position;//생성 당시 땡이의 위치
        me = transform.position;//생성 당시 스몰톨의 위치
        //Debug.Log("스몰 톨 위치:" + me.x);

        moveVelocity = Vector3.zero;//공이 나갈 방향

        maxh = smalltall.transform.position.y + 5;//파이어볼의 최대 위치 : 스몰톨의 위치를 가지고 파악 
        minh = smalltall.transform.position.y;//파이어볼의 최소 위치 : 바닥에 닿았는지 파악하기 위함
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)
        {
            DDaeng.GetComponent<Move>().TakeDamage(10);//공격
            Move dd = GameObject.Find("DDaeng").GetComponent<Move>();
            dd.hpMove(10.0f);
   
            if (dd.HP == 0)
            {
                Destroy(other.gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        ball = transform.position;//파이어볼의 위치

        //생성 당시에 땡이가 왼쪽
        if (target.x < me.x)
        {
            moveVelocity += Vector3.left * Time.deltaTime * movePower;//왼쪽으로 진행  
        }
        //생성 당시에 땡이가 오른쪽
        if (target.x > me.x)
        {
            moveVelocity += Vector3.right * Time.deltaTime * movePower;//오른쪽으로 진행 
        }

        if (ball.y <= maxh && ball.y >= me.y && isUp == false)//초기 생성 시 파이어볼의 위치 증가 
        {
            ball.y += 5;//공 올려주기 
            transform.position = new Vector3(me.x, ball.y, transform.position.z) + moveVelocity;
            isUp = true;
        }

        accel += gravity * Time.deltaTime * movePower;//내려오는 속도 가속도 적용
        ball.y -= accel * Time.deltaTime;
        transform.position = new Vector3(me.x, ball.y, transform.position.z) + moveVelocity;

        if (ball.y <= minh)//땅바닥에 닿았는지 파악해서 충격량 적용
        {
            //Debug.Log("땅에 닿음");
            ball.y = minh;
            transform.position = new Vector3(me.x, ball.y, transform.position.z) + moveVelocity;

            accel = -1 * Mathf.Abs(accel) + gravity;
            Destroy(gameObject, 0.7f);
        }
    }
}
