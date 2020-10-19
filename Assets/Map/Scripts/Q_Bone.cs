using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_Bone : MonoBehaviour
{
    public bool isThere;   //존재 유뮤 파악
    Vector3 origin_pos;
    Vector3 Pos;
    Move DD;
    // Start is called before the first frame update
    void Start()
    {
        origin_pos = gameObject.transform.position;
        Pos = new Vector3(-47.4f, 6.3f, -18.0f);     
        isThere = false;

        DD = GameObject.FindWithTag("DDaeng").GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isThere)    //스킬 습득한 경우
        {
            gameObject.transform.position = Pos;    //스킬 창 활성화
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DD.BoneActive = true;
                isThere = false;
            }
        }
        else
        {
            gameObject.transform.position = origin_pos;
        }
    }
}
