using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    
    [SerializeField] ThoughtBubble thoughtBubble;
    [SerializeField] Image displayedSymbol;
    [SerializeField] Sprite circleSprite;
    [SerializeField] Sprite squareSprite;
    [SerializeField] Sprite triangleSprite;
    [SerializeField] Sprite xSprite;
    [SerializeField] Sprite circleSquareSprite;
    [SerializeField] Sprite circleTriangleSprite;
    [SerializeField] Sprite circleXSprite;
    [SerializeField] Sprite squareTriangleSprite;
    [SerializeField] Sprite squareXSprite;
    [SerializeField] Sprite triangleXSprite;
    int[] symbolArray = new int[] {(int)Symbols.empty, (int)Symbols.empty};

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    // called by other objects (thought bubble for the player) to draw the symbols
    public void updateSymbolList(Symbols s1, Symbols s2)
    {
        symbolArray[0] = (int)s1;
        symbolArray[1] = (int)s2;
        drawSymbol();
    }

    void drawSymbol()
    {
        int symbolIndex = symbolArray[0] * symbolArray[1];
        
        if (symbolIndex == 1)
        {
            displayedSymbol.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            return;
        }
        displayedSymbol.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        if (symbolIndex == (int)Symbols.circle)
        {
            displayedSymbol.sprite = circleSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.square)
        {
            displayedSymbol.sprite = squareSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.triangle)
        {
            displayedSymbol.sprite = triangleSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.ex)
        {
            displayedSymbol.sprite = xSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.squareCirc)
        {
            displayedSymbol.sprite = circleSquareSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.squareTri)
        {
            displayedSymbol.sprite = squareTriangleSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.squareX)
        {
            displayedSymbol.sprite = squareXSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.circleTri)
        {
            displayedSymbol.sprite = circleTriangleSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.circleX)
        {
            displayedSymbol.sprite = circleXSprite;
            return;
        }
        if (symbolIndex == (int)Symbols.triangleX)
        {
            displayedSymbol.sprite = triangleXSprite;
            return;
        }
    }
}