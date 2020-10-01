using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_fireball : MonoBehaviour
{
    //파이어볼 움직임
    public GameObject smalltall;//스몰톨 가져오기
    public GameObject DDaeng;//플레이어 위치

    Vector3 ball;
    Vector3 me;
    Vector3 target;

    public float movePower = 5f;
    float gravity = 9.8f;
    float accel = 0f;//가속도
    float c = 0.7f;//탄성계수 

    // Start is called before the first frame update
    void Start()
    {
        smalltall = GameObject.Find("Smalltol");
        DDaeng = GameObject.Find("DDaeng");

        target = DDaeng.transform.position;//생성 당시 땡이의 위치
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        ball = transform.position;//파이어볼의 위치
        me = smalltall.transform.position;//스몰톨의 위치 
        
     
        //Debug.Log("player위치: " + target.x);//파이어볼이 생성될 때 플레이어의 위치

        Vector3 moveVelocity = Vector3.zero;

        //땡이가 왼쪽
        if (target.x < me.x)
        {
            moveVelocity = Vector3.left;
        }
        //땡이가 오른쪽
        if (target.x > me.x)
        {
            moveVelocity = Vector3.right;
        }
        transform.position += moveVelocity * movePower * Time.deltaTime*ball.y;
    }
}
