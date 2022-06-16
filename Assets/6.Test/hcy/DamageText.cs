using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    public TextMeshProUGUI text;
    Color alpha;
    private Color criAlpha;
    public string damage;
    public Image criUI;
    public bool isCri;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 0.5f;
        text.text = damage;

        criAlpha = criUI.color;
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치

        if (isCri == true)
        {
            criAlpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
            criUI.color = criAlpha;
        }
        
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        
        text.color = alpha;
        
    }

    private void DestroyObject()
    {
        if (parent.transform.tag == "Enemy")
        {
            parent.transform.GetComponent<Enemy>().nowDamagePos -= 1;
            parent.transform.GetComponent<Enemy>().damageTexts.RemoveAt(0);
        }
        else if (parent.transform.tag == "Unit")
        {
            parent.transform.GetComponent<UnitMove>().nowDamagePos -= 1;
            parent.transform.GetComponent<UnitMove>().damageTexts.RemoveAt(0);
        }
        else if (parent.transform.tag == "Player")
        {
            parent.transform.GetComponent<Player>().nowDamagePos -= 1;
            parent.transform.GetComponent<Player>().damageTexts.RemoveAt(0);
        }
        else if (parent.transform.tag == "Tower")
        {
            parent.transform.GetComponent<Tower>().nowDamagePos -= 1;
            parent.transform.GetComponent<Tower>().damageTexts.RemoveAt(0);
        }
        Destroy(gameObject);
    }
}
