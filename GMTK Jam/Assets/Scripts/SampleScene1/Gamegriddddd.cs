using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGriddddd : MonoBehaviour
{

    private GameObject [] candies;

    private GridItem [,] itemsFirstGrid;
    private GridItem [,] itemSecondGrid;
    private GridItem _currentlySelectedItem;
  

    private int x, y, i;

    [SerializeField] private int xSize, ySize;
    void Start()
    {
        itemsFirstGrid = new GridItem [xSize, ySize];
        itemSecondGrid = new GridItem [xSize, ySize];
        GetCandies();
        FillGrid(-7, itemsFirstGrid);
        FillGrid(5, itemSecondGrid);
        GridItem.OnMouseOverItemEventHandler += OnMouseOverItem;
    }

    void OnDisable()
    {
        GridItem.OnMouseOverItemEventHandler -= OnMouseOverItem;
    }
    void FillGrid(int startPoint, GridItem [,] grid)
    {
        //grid = new GridItem [xSize, ySize];

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

    void OnMouseOverItem (GridItem item){
        if(_currentlySelectedItem == null)
        {
            _currentlySelectedItem = item;
        }else
        {
            float xDiff = Mathf.Abs (item.x - _currentlySelectedItem.x);
            float yDiff = Mathf.Abs (item.y - _currentlySelectedItem.y);
            if ( xDiff + yDiff ==1)
            {
                
                StartCoroutine(SwapFG(_currentlySelectedItem, item));
                _currentlySelectedItem = null;
                
            }else
            {
                Debug.LogError("longe");
            }
            _currentlySelectedItem = null;
        }
    }
    IEnumerator SwapFG(GridItem a, GridItem b)
    {
        ChangeRigidbodyStatusFG(false);
        Vector3 aPosition = a.transform.position;
        StartCoroutine (a.transform.Move (b.transform.position, 0.1f));
        StartCoroutine (b.transform.Move (aPosition, 0.1f));
        yield return new WaitForSeconds (0.1f);
        if(itemsFirstGrid[a.x,a.y])
        {
             SwapIndicesSF(a,b);
        }else {
             SwapIndices(a,b); 
        }
        ChangeRigidbodyStatusFG(true);
    }

    void SwapIndices (GridItem a, GridItem b)
    {
        GridItem tempA = itemsFirstGrid[a.x,a.y];
        itemsFirstGrid [a.x,a.y] = b;
        itemsFirstGrid [b.x,b.y] = tempA;
        int bx = b.x; int by = b.y;
        b.OnItemPositionChanged(a.x, a.y);
        a.OnItemPositionChanged (bx,by);
    }
    void SwapIndicesSF (GridItem a, GridItem b)
    {
        GridItem tempA = itemSecondGrid[a.x,a.y];
        itemSecondGrid [a.x,a.y] = b;
        itemSecondGrid[b.x,b.y] = tempA;
        int bx = b.x; int by = b.y;
        b.OnItemPositionChanged(a.x, a.y);
        a.OnItemPositionChanged (bx,by);
    }
    void GetCandies()
    {
        candies = Resources.LoadAll<GameObject> ("PreFabs");
        for(i = 0; i < candies.Length; i++)
        {
            candies[i].GetComponent<GridItem>().id = i;
        }
    }
    void ChangeRigidbodyStatusFG (bool status)
    {
        foreach(GridItem g in itemsFirstGrid)
        {
            g.GetComponent<Rigidbody2D>().isKinematic = !status;
        }
        foreach(GridItem g in itemSecondGrid)
        {
            g.GetComponent<Rigidbody2D>().isKinematic = !status;
        }
    }

}