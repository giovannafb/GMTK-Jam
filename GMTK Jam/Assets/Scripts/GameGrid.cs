using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    private GameObject [] candies;

    private GridItem [,] itemsFirstGrid;
    private GridItem [,] itemSecondGrid;

    private int x, y, i;

    [SerializeField] private int xSize, ySize;
    void Start()
    {
        itemsFirstGrid = new GridItem [xSize, ySize];
        itemSecondGrid = new GridItem [xSize, ySize];
        GetCandies();
        FillGrid(-9, itemsFirstGrid);
        FillGrid(5, itemSecondGrid);
    }

    void FillGrid(int startPoint, GridItem [,] grid)
    {
        for(x = startPoint; x < (xSize + startPoint); x++)
        {
            for(y = 0; y < ySize; y++)
            {
                grid[x - startPoint, y] = InstantiateCandy(x, y);
            }
        }
    }

    GridItem InstantiateCandy(int x, int y)
    {
        GameObject randomCandy = candies[Random.Range(0, candies.Length)];
        GridItem newCandy = ((GameObject) Instantiate(randomCandy, new Vector3(x, y), Quaternion.identity)).GetComponent<GridItem>();
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
