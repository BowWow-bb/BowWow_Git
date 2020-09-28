using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSballMove : MonoBehaviour
{
    //float tmp_x;    //이동 랜덤 난수
    //float tmp_y;    //이동 랜덤 난수
    float v;        //이동 속도
    //Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        //pos = transform.position;
        v = 12.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //tmp_x= Random.Range(-10.0f,10.0f);   //-10.0 ~ 10.0 랜덤 난수 발생
        //tmp_y= Random.Range(-10.0f, 10.0f);   //-10.0 ~ 10.0 랜덤 난수 발생
        //Vector3 dir = new Vector3(transform.position.x+tmp_x,transform.position.y+tmp_y,transform.position.z);
        //transform.position = (transform.forward * Time.deltaTime * v) + pos;
        transform.position += transform.forward * Time.deltaTime * v;
    }
}
