using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{

    public float moveSpeed;//텍스트 이동속도
    public float alphaSpeed;//투명도 변환 속도
    public float destroyTime;//사라지는 속도
    public int damage;
    TextMeshPro text;
    Color alpha;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyObject",destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
