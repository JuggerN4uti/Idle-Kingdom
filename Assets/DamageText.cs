using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Text;
    public float expireTime;

    void Start()
    {
        Invoke("Expire", expireTime);
    }

    public void SetText(float value, bool crit) //string hue, float fontSize = 34f)
    {
        if (crit)
        {
            Text.color = new Color(0.686f, 0f, 0f, 1f);
            Text.text = value.ToString("0") + "!";
        }
        else Text.text = value.ToString("0");
    }

    void Expire()
    {
        Destroy(gameObject);
    }
}
