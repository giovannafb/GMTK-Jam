using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderDisplay : MonoBehaviour
{
    public Orders order;
    public Text nameText;
    public Text qtd;
    public Image typeFood;

    void Start()
    {
        order = GetComponent<Orders>();
        order.foodID = Random.Range(0, 6);
        order.quantity = Random.Range(0, 6);
        Debug.Log(order.name);
    }
}
