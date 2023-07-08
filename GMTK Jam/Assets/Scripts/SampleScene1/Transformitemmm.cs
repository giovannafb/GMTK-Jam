using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Transformitemmm 
{
    public static IEnumerator Move (this Transform t, Vector3 target, float duration)
    {
    Vector3 diffVector = (target - t.position);
    float diffLenght = diffVector.magnitude;
    diffVector.Normalize ();
    float counter = 0;
    while(counter < duration)
    {
        float movAmount= (Time.deltaTime* diffLenght)/duration;
        t.position += diffVector * movAmount;
        counter += Time.deltaTime;
        yield return null;
    }
        t.position = target;
    }
    public static IEnumerator Scale (this Transform t, Vector3 target, float duration)
    {
        Vector3 diffVector = (target - t.localScale);
        float diffLenght = diffVector.magnitude;
        diffVector.Normalize ();
        float counter = 0;
        while (counter < duration)
        {
            float movAmount = (Time.deltaTime * diffLenght)/duration;
            t.localScale += -diffVector * movAmount;
            counter+= Time.deltaTime;
            yield return null;
        }
        t.localScale = target;
    }
}
