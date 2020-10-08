using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFireball : MonoBehaviour
{
    float t;
    float angle;
    Vector3 Pos;
    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        Pos = transform.position;   //생성 초기위치

        angle = Random.Range(0, 180);

        float x = Mathf.Cos(angle * Mathf.PI / 180.0f) * 8 + Pos.x;
        float y = Mathf.Sin(angle * Mathf.PI / 180.0f) * 8 + Pos.y;

        transform.position = new Vector3(x, y, Pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(angle<90)
            transform.position += new Vector3(0.1f, 0.1f, 0)*t;
        if(angle==90)
            transform.position +=Vector3.up*0.1f*t;
        if (angle>90)
            transform.position += new Vector3(-0.1f, 0.1f, 0)*t;

        if (Mathf.Abs(transform.position.x) > 48 || transform.position.y > 48)  //벽 경계 벗어날 시 소멸
            Destroy(gameObject);

        t += 0.02f;

    }
}
