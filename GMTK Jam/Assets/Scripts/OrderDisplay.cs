using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderDisplay : MonoBehaviour
{
    private Sprite [] snacks;

    public TextMeshProUGUI qtdFood;

    public Image foodImage;

   /* void OnEnable()
    {
        GameGrid.OnMatch += GenerateSnacks;
    }

    void OnDisable()
    {
        GameGrid.OnMatch -= GenerateSnacks;
    }*/

    void Start()
    {
        GetSnacks();
        int _random = Random.Range(1,6);
        foodImage.sprite = snacks[Random.Range(0, snacks.Length)];
        qtdFood.text = _random.ToString();
    }

    void GetSnacks()
    {
        snacks = Resources.LoadAll<Sprite>("FoodSprites");
    }

    public void GenerateSnacks()
    {
        int _random = Random.Range(1,6);
        foodImage.sprite = snacks[Random.Range(0, snacks.Length)];
        qtdFood.text = _random.ToString();
    }
}
