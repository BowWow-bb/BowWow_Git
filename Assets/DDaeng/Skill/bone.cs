using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bone : MonoBehaviour
{
    bool left;
    bool arrival;
    GameObject DD;
    Vector3 position;
    float check;
    float rotate;
    // Start is called before the first frame update
    void Start()
    {
        //-45 45
        rotate = 0;
        check = gameObject.transform.position.x;
        position = gameObject.transform.position;
        arrival = false;

        DD = GameObject.Find("DDaeng");


        if (DD.transform.position.x > position.x) // 음파가 왼쪽일때
            left = true;
        else
            left = false;
    }

    // Update is called once per frame
    void Update()
    {
        rotate += 5f;
        if (!arrival)
        {
            if (left && gameObject.transform.position.x > -45f)
            {
                gameObject.transform.position = new Vector3(check, (position.y + (6f) * Mathf.Sin(3.14f * (check - position.x) / (-46 - position.x))), 0);
                check -= 0.1f;
            }
            else if (!left && gameObject.transform.position.x < 45f)
            {
                gameObject.transform.position = new Vector3(check, position.y + (6f) * Mathf.Sin(3.14f * (check - position.x) / (46 - position.x)), 0);
                check += 0.1f;
            }
            else
            {
                position = gameObject.transform.position;
                arrival = true;
                check = 0;
            }
        }
        else
        {
            if (check <= 20f)
            {
                gameObject.transform.position = new Vector3(position.x + 3 * Mathf.Cos(rotate), position.y + 3 * Mathf.Sin(rotate), 0);
                check += 0.1f;
            }
            else if (check == 20f)
                check = 0;
            else
            {
                position = gameObject.transform.position;
                check += 0.1f;
               // gameObject.transform.position = new Vector3(DD.transform.position.x);
            }
        }

        gameObject.transform.rotation = Quaternion.Euler(rotate, rotate, 0);
    }
}
