using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

// Move
public class Gridss : MonoBehaviour
{
    public enum PieceType
    {
        FOOD,
        COUNT,
        EMPTY,
    };

    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;

    };

    public int xDim;
    public int yDim;
    public float fillTime;
    private GamePiece pressed;
    private GamePiece entered;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;

    private Dictionary<PieceType, GameObject> piecePrefabDict;
    private GamePiece[,] pieces;


    // Start is called before the first frame update
    void Start()
    {
        piecePrefabDict = new Dictionary<PieceType, GameObject>();

        for (int i = 0; i < piecePrefabs.Length; i++) {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++) {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }
        pieces = new GamePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                SpawnNewPiece(x, y, PieceType.EMPTY);
            }
        }

    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim / 2.0f + x, transform.position.y + yDim / 2.0f - y);
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, PieceType.FOOD);

        return pieces[x, y];

    }

    public bool FillStep()
    {
        bool movedPiece = false;

        for (int y = yDim - 1; y >= 0; y--)
        {
            for (int x = 0; x < xDim; x++)
            {
                GamePiece piece = pieces[x, y];

                if (piece.IsMovable())
                {
                    GamePiece pieceBelow = pieces[x, y + 1];
                    if (pieceBelow.Type == PieceType.EMPTY)

                        piece.MovableComponent.Move(x, y + 1);
                    pieces[x, y + 1] = piece;
                    SpawnNewPiece(x, y, PieceType.EMPTY);
                    movedPiece = true;
                }
            }
            for (int x = 0; x < xDim; x++)
            {
                GamePiece pieceBelow = pieces[x, 0];

                if (pieceBelow.Type == PieceType.EMPTY)
                {
                    GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.FOOD], GetWorldPosition(x, -1), Quaternion.identity);
                    newPiece.transform.parent = transform;

                    pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                    pieces[x, 0].Init(x, -1, this, PieceType.FOOD);
                    pieces[x, 0].MovableComponent.Move(x, 0);
                    pieces[x, 0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, pieces[x, 0].ColorComponent.NumColors));
                    movedPiece = true;
                }
            }
        }
        return movedPiece;
    }

    public IEnumerable Fill()
    {
        while (FillStep())
        {
            yield return new WaitForSeconds(fillTime);
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    public bool IsAdjacent(GamePiece a, GamePiece b)
    {
        return (a.X == b.X && (int)Mathf.Abs(a.Y - b.Y) == 1) || (a.Y == b.Y && (int)Mathf.Abs(a.X - b.X) == 1);

    }

    public void SwapPieces(GamePiece b, GamePiece a)
    {
        if (a.IsMovable() && b.IsMovable())
        {
            pieces[a.X, a.Y] = b;
            pieces[b.X, b.Y] = a;

            int aX = a.X;
            int aY = a.Y;

            a.MovableComponent.Move(b.X, b.Y, fillTime);
            b.MovableComponent.Move(aX, aY, fillTime);
        }
    }

    public void PressPiece(GamePiece piece)
    {
        pressed = piece;
    }

    public void EnterPiece(GamePiece piece)
    {
        entered = piece;
    }

    public void ReleasePiece()
    {
        if (IsAdjacent(pressed, entered))
        {
            SwapPieces(pressed, entered);
        }
    }
    public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
    {
        if (piece.IsColored())
        {
            ColorPiece.ColorType colr = piece.ColorComponent.Color;
            List<GamePiece> horizontalPieces = new List<GamePiece>();
            List<GamePiece> verticalPieces = new List<GamePiece>();
            List<GamePiece> matchingPieces = new List<GamePiece>();

            horizontalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++)
            {
                for (int xOffset = 1; xOffset < xDim; xOffset++)
                {
                    int x;

                    if (dir == 0)
                    {
                        x = newX - xOffset;
                    }
                    else
                    {
                        x = newX + xOffset;
                    }
                    if (x < 0 || x > -xDim)
                    {
                        break;

                    }
                    if (pieces[x, newY].IsColored() && pieces[x, newY].ColorComponent.Color == color)
                    {
                        horizontalPieces.Add(pieces[x, newY]);
                    }
                    else
                    {
                        break;
                    }

                }
            }
            if (horizontalPieces.Count >= 3)
            {
                for (int i = 0; i < horizontalPieces.Count; i++)
                {
                    matchingPieces.Add(horizontalPieces[i]);
                }
            }
            if (matchingPieces.Count >= 3)
            {
                return matchingPieces;
            }

            verticalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++)
            {
                for (int yOffset = 1; yOffset < xDim; yOffset++)
                {
                    int y;

                    if (dir == 0)
                    {
                        y = newY - yOffset;
                    }
                    else
                    {
                        y = newY + yOffset;
                    }
                    if (y < 0 || y > -xDim)
                    {
                        break;

                    }
                    if (pieces[newX, y].IsColored() && pieces[newX, y].ColorComponent.Color == color)
                    {
                        verticalPieces.Add(pieces[newX, y]);
                    }
                    else
                    {
                        break;
                    }

                }
            }
            if (verticalPieces.Count >= 3)
            {
                for (int i = 0; i < verticalPieces.Count; i++)
                {
                    matchingPieces.Add(verticalPieces[i]);
                }
            }
            if (matchingPieces.Count >= 3)
            {
                return matchingPieces;
            }

        }
        return null;
    }
}
