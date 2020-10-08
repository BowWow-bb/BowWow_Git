using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_toll_stage1 : MonoBehaviour
{
    public GameObject DDaeng;//땡이

    Vector3 target;//땡이 위치
    Vector3 me;//스몰톨 위치 

    public float d = 20f;//범위 거리 설정  
    float movePower = 5f;//움직이는 속력

    int movementFlag = 0;//0: 정지, 1: 왼쪽, 2: 오른쪽
    string dist = "";//이동 방향 

    bool isTracing = false;//거리 내에 들어와서 유지 중인 상태 
    bool Enter = false;//거리 내에 들어오면 (처음)

    // Start is called before the first frame update
    void Start()
    {
        DDaeng = GameObject.Find("DDaeng");

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(1, 3);//움직임 설정 랜덤 

        yield return new WaitForSeconds(3f);//3초동안 실행 

        StartCoroutine("ChangeMovement");//다른 움직임 또 하게 호출 
    }

    //스몰톨이 카메라 벗어나지 않게 제한 
    IEnumerator ClipMovementleft()//왼쪽으로 가는 코루틴 실행
    {
        movementFlag = 1;
        Debug.Log("코루틴 left");

        yield return new WaitForSeconds(3f);

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ClipMovementright()//오른쪽으로 가는 코루틴 실행 
    {
        movementFlag = 2;
        Debug.Log("코루틴 right");

        yield return new WaitForSeconds(3f);

        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Distance();
        Move();
    }

    void Distance()//거리 파악. 트리거 대신 
    {
        target = DDaeng.transform.position;
        float distance = Vector3.Distance(target, transform.position);//거리 구하는 함수 

        Debug.Log("땡이랑 거리: " + distance);

        if (distance <= d)//범위 내에 처음 들어오면
        {
            Enter = true;
            Debug.Log("범위 내에 들어옴");
            StopCoroutine("ChangeMovement");//이동하던 거 멈추고 추적 시작 
        }

        if (Enter == true && distance <= d)//들어 온 상태이고 범위 내에 계속 있으면 
        {
            isTracing = true;
        }

        if (isTracing == true && distance > d)//거리 벗어나면 
        {
            Enter = false;
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
    }

    void Move()
    {
        me = transform.position;

        Vector3 moveVelocity = Vector3.zero;

        if (isTracing)//일정 거리 내이면 추적 
        {
            movePower = 12;//추적 시에 속도 빠르게

            if (target.x < me.x)//땡이가 왼쪽이면
            {
                dist = "Left";
            }

            else if (target.x > me.x)//땡이가 오른쪽이면
            {
                dist = "Right";
            }

        }
        else//거리 밖이면 (평소)
        {
            movePower = 5;

            if (me.x >= 40)
            {
                StopCoroutine("ChangeMovement");
                StartCoroutine("ClipMovementleft");
            }
            else if (me.x <= -40)
            {
                StopCoroutine("ChangeMovement");
                StartCoroutine("ClipMovementright");
            }

            if (movementFlag == 1)
                dist = "Left";
            else if (movementFlag == 2)
                dist = "Right";

        }
        //좌우 이동 
        if (dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

}