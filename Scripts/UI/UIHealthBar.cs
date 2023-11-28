using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : GameUnit
{
    [SerializeField] Image imageFill;
    [SerializeField] TextMeshProUGUI hpText;
    Vector3 offset;
    float hp;
    float maxHp;
    Transform target;

    Color[] colors =
    {
        Color.green,
        Color.red,
    };

    private void Awake() 
    {
        poolType = PoolType.Health_Bar;
    }

    private void Update() 
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp/maxHp, Time.deltaTime * 5f);
        TF.position = target.position + offset;
        hpText.text = hp.ToString();
    }

    public void OnInit(float maxHp, Transform target, Vector3 offset, Character character)
    {
        SetColor(character);
        this.offset = offset;
        this.target = target;
        this.maxHp = maxHp;
        hp = maxHp;
        imageFill.fillAmount = 1;
    }

    public void SetNewHP(float hp)
    {
        this.hp = hp;
    }

    public void SetColor(Character character)
    {
        if(character is Player)
        {
            imageFill.color = Color.green;
            hpText.gameObject.SetActive(true);
        }
        else
        {
            imageFill.color = Color.red;
            hpText.gameObject.SetActive(false);
        }
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    
}
