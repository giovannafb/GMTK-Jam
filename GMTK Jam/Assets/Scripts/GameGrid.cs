using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    private GameObject [] candies;

    private GridItem [,] items;

    private int x, y, i;

    [SerializeField] private int xSize, ySize;
    void Start()
    {
        GetCandies();
        FillGrid();
    }

    void FillGrid()
    {
        items = new GridItem [xSize, ySize];
        for(x = 0; x < xSize; x++)
        {
            for(y = 0; y < ySize; y++)
            {
                items[x, y] = InstantiateCandy(x, y);
            }
        }

    }

    GridItem InstantiateCandy(int x, int y)
    {
        GameObject randomCandie = candies[Random.Range(0, candies.Length)];
        GridItem newCandy = ((GameObject) Instantiate(randomCandie, new Vector3(x, y), Quaternion.identity)).GetComponent<GridItem>();
        newCandy.OnItemPositionChanged(x, y);
        return newCandy;
    }

    void GetCandies()
    {
        candies = Resources.LoadAll<GameObject> ("PreFabs");
        for(i = 0; i < candies.Length; i++)
        {
            candies[i].GetComponent<GridItem>().id = i;
        }
    }

}
