using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_toll : MonoBehaviour
{
    public GameObject fireballPrefab;//파이어볼 프리팹
    public GameObject DDaeng;//땡이

    Vector3 target;//땡이 위치

    public float d = 20f;//범위 거리 설정  
    public float movePower = 1f;//움직이는 속력
    public float RateMin = 0.5f;//최소 생성 주기 
    public float RateMax = 3f;//최대 생성 주기
    public float Rate;

    private float timeAfter;//발사 후 지난 시간 

    int movementFlag = 0;//0: 정지, 1: 왼쪽, 2: 오른쪽
    
    bool isTracing = false;//거리 내에 들어와서 유지 중인 상태 
    bool Enter = false;//거리 내에 들어오면 (처음)


    // Start is called before the first frame update
    void Start()
    {
        timeAfter = 0f;
        Rate = Random.Range(RateMin, RateMax);

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);//움직임 설정
            
        yield return new WaitForSeconds(3f);//3초 기다리기 

        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeAfter += Time.deltaTime;//시간 갱신
     
        Distance();
        Move();
    }

    void Distance()//거리 파악 
    {
        target = DDaeng.transform.position;
        float distance = Vector3.Distance(target, transform.position);

        Debug.Log("땡이랑 거리: "+distance);

        if (distance<= d)//범위 내에 처음 들어오면
        {
            Enter = true;
            Debug.Log("범위 내에 들어옴");
            StopCoroutine("ChangeMovement");
        }

        if (Enter == true && distance <= d)//들어 온 상태이고 범위 내에 계속 있으면 
        {
            isTracing = true;
        }
        
        if(isTracing == true && distance > d)//거리 벗어나면 
        {
            Enter = false;
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if(isTracing)//일정 거리 내이면 추적 
        {
            if (target.x < transform.position.x)//땡이가 왼쪽이면 
                dist = "Left";
            else if (target.x > transform.position.x)//땡이가 오른쪽이면 
                dist = "Right";
        }
        else//거리 밖이면 
        {
            if (movementFlag == 1)
                dist = "Left";
            else if (movementFlag == 2)
                dist = "Right";
            else//멈춘 상태이면 
            {
                if (timeAfter >= Rate)
                {
                    timeAfter = 0f;
                    FireballMake();
                }
            }
        }

        if(dist =="Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
    void FireballMake()
    {
        GameObject ball = GameObject.Instantiate(fireballPrefab); //파이어볼 생성
        ball.transform.position = new Vector3(transform.position.x,transform.position.y+0.5f, 10f);//파이어볼 초기 위치 
        ball.transform.parent = null;

        Rate = Random.Range(RateMin, RateMax);//다음 번 파이어볼 생성 주기 설정 
    }
}
