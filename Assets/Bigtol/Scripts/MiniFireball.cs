using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFireball : MonoBehaviour
{
    float t;        //타이머
    float angle;    //생성 각도

    Vector3 Pos;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        Pos = transform.position;   //생성 초기위치
        angle = Random.Range(0, 180);   //바닥 윗 부분 각도에 대해서만 생성 각도 설정

        float x = Mathf.Cos(angle * Mathf.PI / 180.0f) * 8 + Pos.x;
        float y = Mathf.Sin(angle * Mathf.PI / 180.0f) * 8 + Pos.y;

        transform.position = new Vector3(x, y, Pos.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //각도에 따라 발사되는 방향 설정
        if (angle < 90)
            transform.position += new Vector3(0.1f, 0.1f, 0) * t; //북동쪽
        if (angle == 90)
            transform.position += Vector3.up * 0.1f * t; //북쪽
        if (angle > 90)
            transform.position += new Vector3(-0.1f, 0.1f, 0) * t;  //북서쪽

        //벽에 부딪힌 경우 튕기기 구현!!

        if (Mathf.Abs(transform.position.x) > 60 || transform.position.y > 69)  //벽 경계 벗어날 시 소멸
        {
            Destroy(gameObject);
        }

        t += 0.09f;

    }
}
