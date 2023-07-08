using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameGriddddd : MonoBehaviour
{

    private GameObject[] candies;

    private GridItem[,] itemsFirstGrid;
    private GridItem[,] itemSecondGrid;
    private GridItem _currentlySelectedItem;


    private int x, y, i;
    public static int minItemsForMatch = 3;

    [SerializeField] private int xSize, ySize;
    void Start()
    {
        itemsFirstGrid = new GridItem[xSize, ySize];
        itemSecondGrid = new GridItem[xSize, ySize];
        GetCandies();
        FillGrid(-7, itemsFirstGrid);
        FillGrid(5, itemSecondGrid);
        GridItem.OnMouseOverItemEventHandler += OnMouseOverItem;

    }

    void OnDisable()
    {
        GridItem.OnMouseOverItemEventHandler -= OnMouseOverItem;
    }
    void FillGrid(int startPoint, GridItem[,] grid)
    {
        //grid = new GridItem [xSize, ySize];

        for (x = startPoint; x < (xSize + startPoint); x++)
        {
            for (y = 0; y < ySize; y++)
            {
                grid[x - startPoint, y] = InstantiateCandy(x, y);
            }
        }
    }

    GridItem InstantiateCandy(int x, int y)
    {
        GameObject randomCandy = candies[Random.Range(0, candies.Length)];
        GridItem newCandy = ((GameObject)Instantiate(randomCandy, new Vector3(x, y), Quaternion.identity)).GetComponent<GridItem>();
        newCandy.OnItemPositionChanged(x, y);
        return newCandy;
    }

    void OnMouseOverItem(GridItem item)
    {
        if (_currentlySelectedItem == null)
        {
            _currentlySelectedItem = item;
        }
        else
        {
            float xDiff = Mathf.Abs(item.x - _currentlySelectedItem.x);
            float yDiff = Mathf.Abs(item.y - _currentlySelectedItem.y);
            if (xDiff + yDiff == 1)
            {

                StartCoroutine(TryMatch(_currentlySelectedItem, item));
                _currentlySelectedItem = null;

            }
            else
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
        float movDuration = 0.1f;
        StartCoroutine(a.transform.Move(b.transform.position, movDuration));
        StartCoroutine(b.transform.Move(aPosition, movDuration));
        yield return new WaitForSeconds(movDuration);
        if (itemsFirstGrid[a.x, a.y])
        {
            SwapIndices(a, b);
        }
        else
        {
            SwapIndicesSF(a, b);
        }
        ChangeRigidbodyStatusFG(true);
    }

    void SwapIndices(GridItem a, GridItem b)
    {
        GridItem tempA = itemsFirstGrid[a.x, a.y];
        itemsFirstGrid[a.x, a.y] = b;
        itemsFirstGrid[b.x, b.y] = tempA;
        int bx = b.x; int by = b.y;
        b.OnItemPositionChanged(a.x, a.y);
        a.OnItemPositionChanged(bx, by);
    }
    void SwapIndicesSF(GridItem a, GridItem b)
    {
        GridItem tempA = itemSecondGrid[a.x, a.y];
        itemSecondGrid[a.x, a.y] = b;
        itemSecondGrid[b.x, b.y] = tempA;
        int bx = b.x; int by = b.y;
        b.OnItemPositionChanged(a.x, a.y);
        a.OnItemPositionChanged(bx, by);
    }

    List<GridItem> SearchHorizontally(GridItem item)
    {
        List<GridItem> hItem = new List<GridItem> { item };
        int left = item.x - 1;
        int right = item.x + 1;
        while (left >= 0 && itemsFirstGrid[left, item.y].id == item.id)
        {
            hItem.Add(itemsFirstGrid[left, item.y]);
            left--;
        }
        while (right < xSize && itemsFirstGrid[right, item.y].id == item.id)
        {
            hItem.Add(itemsFirstGrid[right, item.y]);
            right++;
        }
        while (left >= 0 && itemSecondGrid[left, item.y].id == item.id)
        {
            hItem.Add(itemsFirstGrid[left, item.y]);
            left--;
        }
        while (right < xSize && itemSecondGrid[right, item.y].id == item.id)
        {
            hItem.Add(itemSecondGrid[right, item.y]);
            right++;
        }
        return hItem;
    }
    List<GridItem> SearchVertically(GridItem item)
    {
        List<GridItem> vItem = new List<GridItem> { item };
        int lower = item.y - 1;
        int upper = item.y + 1;
        while (lower >= 0 && itemsFirstGrid[item.x, lower].id == item.id)
        {
            vItem.Add(itemsFirstGrid[item.x, lower]);
            lower--;
        }
        while (upper < ySize && itemsFirstGrid[item.x, upper].id == item.id)
        {
            vItem.Add(itemsFirstGrid[item.x, upper]);
            upper++;
        }
        while (lower >= 0 && itemSecondGrid[item.x, lower].id == item.id)
        {
            vItem.Add(itemsFirstGrid[item.x, lower]);
            lower--;
        }
        while (upper < ySize && itemSecondGrid[item.x, upper].id == item.id)
        {
            vItem.Add(itemsFirstGrid[item.x, upper]);
            upper++;
        }
        return vItem;
    }
    void GetCandies()
    {
        candies = Resources.LoadAll<GameObject>("PreFabs");
        for (i = 0; i < candies.Length; i++)
        {
            candies[i].GetComponent<GridItem>().id = i;
        }
    }
    void ChangeRigidbodyStatusFG(bool status)
    {
        foreach (GridItem g in itemsFirstGrid)
        {
            g.GetComponent<Rigidbody2D>().isKinematic = !status;
        }
        foreach (GridItem g in itemSecondGrid)
        {
            g.GetComponent<Rigidbody2D>().isKinematic = !status;
        }
    }

    Matchinfo GetMatchInfo(GridItem item)
    {
        Matchinfo n = new Matchinfo();
        n.match = null;
        List<GridItem> hMatch = SearchHorizontally(item);
        List<GridItem> vMatch = SearchVertically(item);
        if (hMatch.Count >= minItemsForMatch && hMatch.Count > vMatch.Count)
        {
            n.matchStartingX = GetMinnuMx(hMatch);
            n.matchEndingX = GetMaxnuMx(hMatch);
            n.matchStartingY = n.matchEndingY = hMatch[0].y;
            n.match = hMatch;

        }
        else if (vMatch.Count >= minItemsForMatch)
        {

            n.matchStartingY = GetMinnuMy(vMatch);
            n.matchEndingY= GetMaxnuMy(vMatch);
            n.matchStartingX = n.matchEndingX = hMatch[0].x;
            n.match = vMatch;
        }
        return n;

    }
    int GetMinnuMx (List<GridItem> items)
    {
        float[] indices = new float [items.Count];
        for(int i=0; i<indices.Length; i++)
        {
            indices[i] = items[i].x;
        }
        return (int)Mathf.Min (indices);
    }
    int GetMaxnuMx(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].x;
        }
        return (int) Mathf.Max(indices);
    }
    int GetMinnuMy(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;
        }
        return (int)Mathf.Min(indices);
    }
    int GetMaxnuMy(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;
        }
        return (int)Mathf.Max(indices);
    }
    IEnumerator DestroyItems(List<GridItem> items)
    {
        foreach (GridItem i in items)
        {
            yield return StartCoroutine(i.transform.Scale(Vector3.zero, 0.05f));
            Destroy(i.gameObject);
        }
      
    }

    IEnumerator TryMatch(GridItem a, GridItem b)
    {
        yield return StartCoroutine(SwapFG(a, b));
        Matchinfo matchA = GetMatchInfo (a);
        print(matchA.validMatch);
        Matchinfo matchB = GetMatchInfo (b);
        print(matchB.validMatch);
        if (!matchA.validMatch && !matchB.validMatch)
        {
            yield return StartCoroutine(SwapFG(a, b));
            yield break;

        }if (matchA.validMatch)
        {
            yield return StartCoroutine(DestroyItems(matchA.match));

        }else if(matchB.validMatch)
            {
            yield return StartCoroutine (DestroyItems(matchB.match));
        }

    }

}