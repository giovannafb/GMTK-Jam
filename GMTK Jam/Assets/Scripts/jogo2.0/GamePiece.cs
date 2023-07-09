using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    private int x;
    private int y;

    public int X
    {  get { return x; }
        set
        {
            if (IsMovable())
            {
                x = value;
            }
        }  
    }

    public int Y
    { get { return y; }
        set
        {
            if (IsMovable())
            {
                y = value;
            }
        }
    }

    private Gridss.PieceType type;

    public Gridss.PieceType Type
    {
        get { return type; }
    }

    private Gridss grid;

    public Gridss GridRef
    {
        get { return grid; }
    }

    private Movable movableComponent;

    public Movable MovableComponent
    {
        get { return movableComponent; }
    }
    private ColorPiece colorComponent;

    public ColorPiece ColorComponent
    {
        get { return colorComponent; }
    }
    void Awake()
    {
        movableComponent = GetComponent<Movable>();
        colorComponent = GetComponent<ColorPiece>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(int _x, int _y, Gridss _grid, Gridss.PieceType _type)
    {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;

    }
    public bool IsMovable()
        {
        return movableComponent != null;
    }
    public bool IsColored()
    {
        return colorComponent != null;
    }
    private void OnMouseEnter()
    {
        grid.EnterPiece(this);
    }
    private void OnMouseDown()
    {
        grid.PressPiece (this);
    }
    private void OnMouseUp()
    {
        grid.ReleasePiece();
    }
}
