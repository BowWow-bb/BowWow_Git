using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    //오브젝트 다른경우 생각하기!!!!!!!!!!!!!!!!!1 
    public GameObject hp = GameObject.Find("Bigtol").transform.Find("BigHpBar").transform.Find("Hp").gameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        var other = GameObject.Find("Bigtol").GetComponent<Bigtol>();

        Vector3 Pos = hp.transform.position;
        
        if(other.hp < 100.0f)   //감소 발생
        {
            Pos.x -= 0.1f;
            hp.transform.position = Pos;
        }
            

        
    }
}
