using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour
{
   public enum ColorType
    {
        PURPLE,
        YELLOW,
        PINK,
        GREEN,
        ORANGE,
        ANY, 
        COUNT
    };
    [System.Serializable]public struct ColorSprite
    {
        public ColorType color;
        public Sprite sprite;
        
    }
    public ColorSprite[] colorSprites;

    private ColorType color;
    public ColorType Color
    {
        get { return color; }
        set { SetColor(value); }
    }
    public int NumColors
    {
        get { return colorSprites.Length; }
    }
    
    private SpriteRenderer sprite;
    private Dictionary<ColorType, Sprite> colorSpriteDict;

    void Awake()
    {
        sprite = transform.Find("roupa").GetComponent<SpriteRenderer> ();

        colorSpriteDict = new Dictionary<ColorType, Sprite> ();

        for (int i=0; i <colorSprites.Length; i++)
        {
            if (!colorSpriteDict.ContainsKey(colorSprites [i].color))
            {
                colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(ColorType newColor)
    {
        color = newColor;

        if(colorSpriteDict.ContainsKey(newColor))
        {
            sprite.sprite = colorSpriteDict [newColor];
        }   
    
    }
}
