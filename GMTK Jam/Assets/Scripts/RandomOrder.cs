using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOrder : MonoBehaviour
{
    private OrderDisplay [] allOrders;

    [SerializeField] private float maxTime = 10f;
    private float currentTime;

    void Start()
    {
        currentTime = maxTime;
        GetOrders();
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        Debug.Log(currentTime);
        UptadeTimerUI();
        Debug.Log(allOrders[0].qtdFood.text);
    }

    void UptadeTimerUI()
    {
       if(currentTime <= 0)
       {
        Debug.Log("Acabou o tempo");
        currentTime = maxTime;

       }
    }

    void GetInfo()
    {
        
    }

    void GetOrders()
    {
        int i = 0;
        for(i = 0; i < 5; i++)
        {
            allOrders[i] = FindAnyObjectByType<OrderDisplay>();
        }
    }
}
