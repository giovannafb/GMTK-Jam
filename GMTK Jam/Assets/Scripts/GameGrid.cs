using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GameGrid : MonoBehaviour
{

    private GameObject [] candies;

    private GridItem [,] items;

    private GridItem _currentlySelectedItem;

    private int x, y, i;


    [SerializeField] private int xSize, ySize;
    void Start()
    {
        GetCandies();
        FillGrid();
        GridItem.OnMouseOverItemEventHandler += OnMouseOverItem;

    }
    void OnDisable()
    {
        GridItem.OnMouseOverItemEventHandler -= OnMouseOverItem;

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
    void OnMouseOverItem (GridItem item)
    {
        if (_currentlySelectedItem == item)
        {
            return;
        }

        if(_currentlySelectedItem == null)
        {
            _currentlySelectedItem = item;
        }
        else
        {
            float xDiff = Mathf.Abs(item.x - _currentlySelectedItem.x);
            float yDiff = Mathf.Abs(item.y - _currentlySelectedItem.y);
            if (xDiff + yDiff == 1) 
            {
                StartCoroutine(Swap(_currentlySelectedItem, item));

            }
            else
            {
                Debug.LogError("muiito longe");
            }

            _currentlySelectedItem = null;
        }
    }

    IEnumerator Swap (GridItem a, GridItem b)
    {
        ChangeRigidbodyStatus(false);
        float movDuration = 0.1f;
        Vector3 aPosition = a.transform.position;
        StartCoroutine(a.transform.Move (b.transform.position, movDuration));
        StartCoroutine(b.transform.Move(aPosition, movDuration));
        yield return new WaitForSeconds (movDuration);
        SwapIndices(a, b);
        ChangeRigidbodyStatus(true);

    }
    void SwapIndices (GridItem a, GridItem b)
    {
        GridItem tempA = items [a.x, a.y];
        items [a.x, a.y] = b;
        items [b.x, b.y] = tempA;
        int bOldX = b.x; int bOldY = b.y;
        b.OnItemPositionChanged(a.x, a.y);
        a.OnItemPositionChanged(bOldX, bOldY);
    }
    void GetCandies()
    {
        candies = Resources.LoadAll<GameObject> ("PreFabs");
        for(i = 0; i < candies.Length; i++)
        {
            candies[i].GetComponent<GridItem>().id = i;
        }
    }

    void ChangeRigidbodyStatus (bool status)
    {
        foreach (GridItem g in items)
        {
            g.GetComponent<Rigidbody2D>().isKinematic = !status;
        }
    }
}
