using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_toll : MonoBehaviour
{
    //움직임 
    public float movePower = 1f;
    Vector3 movement;
    int movementFlag = 0;//0: 정지, 1: 왼쪽, 2: 오른쪽 

    //파이어볼 
    public GameObject fireballPrefab;//파이어볼 프리팹
    public float RateMin = 0.5f;//일단 발사 주기 임의로 설정 
    public float RateMax = 3f;

    private Transform target;//플레이어 위치 
    private float Rate;//생성 주기
    private float timeAfter;//최근 생성 시점에서 지난 시간

    // Start is called before the first frame update
    void Start()
    {
        timeAfter = 0f;
        //traget = 플레이어위치;
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
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(movementFlag ==1)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(movementFlag ==2)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
    void FireballMake()
    {
        
    }
}
