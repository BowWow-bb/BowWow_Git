using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_toll1_HP : MonoBehaviour
{
    //오브젝트 다른경우 생각하기!!!!!!!!!!!!!!!!!1 
    public GameObject hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = GameObject.Find("Smalltol_stage1").transform.Find("st1_HpBar").transform.Find("Hp").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        var other = GameObject.Find("Smalltol_stage1").GetComponent<small_toll_stage1>();

        Vector3 Pos = hp.transform.position;

        if (other.HP < 100.0f)   //감소 발생
        {
            Pos.x -= 0.1f;
            hp.transform.position = Pos;
        }



    }
}
