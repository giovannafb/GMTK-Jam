using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderDisplay : MonoBehaviour
{
    public Order _order;

    

    [SerializeField] TextMeshProUGUI qtdFood;

    public Image foodImage;

    void Start()
    {
        int _random = Random.Range(1,6);
        foodImage.sprite = _order.Food;
        qtdFood.text = _random.ToString();
    }
}
