using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoned : MonoBehaviour
{
    public HeroesLibrary HLib;
    public SpriteRenderer HeroModel;
    public SpriteRenderer[] Background;
    public Transform[] BackgroundForm;
    public Vector3 Destination;
    float speed;

    public void SetHero(int ID, int rarity)
    {
        switch (rarity)
        {
            case 0:
                HeroModel.sprite = HLib.CommonHeroes[ID].UnitSprite;
                Background[0].color = new Color(0.855f, 0.855f, 0.855f, 0.4f);
                Background[1].color = new Color(0.855f, 0.855f, 0.855f, 0.4f);
                BackgroundForm[0].localScale = new Vector3(6.5f, 6.5f, 1f);
                BackgroundForm[1].localScale = new Vector3(7.8f, 7.8f, 1f);
                speed = 4.86f;
                break;
            case 1:
                HeroModel.sprite = HLib.UncommonHeroes[ID].UnitSprite;
                Background[0].color = new Color(0.714f, 0.843f, 0.659f, 0.6f);
                Background[1].color = new Color(0.714f, 0.843f, 0.659f, 0.6f);
                BackgroundForm[0].localScale = new Vector3(8f, 8f, 1f);
                BackgroundForm[1].localScale = new Vector3(9.6f, 9.6f, 1f);
                speed = 4.6f;
                break;
        }
        Invoke("Expire", 15.2f / speed);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Destination, speed * Time.deltaTime);
    }

    void Expire()
    {
        Destroy(gameObject);
    }
}
