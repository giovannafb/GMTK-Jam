using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Order", menuName = "Orders")]
public class Orders : ScriptableObject
{
   public new string name;

    public int foodID;

    public int quantity;

    public Sprite typeofFood;
}
