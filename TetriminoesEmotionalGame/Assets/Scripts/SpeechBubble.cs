using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    
    [SerializeField] ThoughtBubble thoughtBubble;
    int[] symbolArray = new int[] {-1, -1};
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateSymbolList();
    }

    // it would be helpful if there was a "none" value for symbols so i didn't have to cast to int here
    void updateSymbolList()
    {
        symbolArray[0] = -1;
        symbolArray[1] = -1;
        if (thoughtBubble.selectedKeys.Count > 1)
        {
            symbolArray[1] = (int)thoughtBubble.selectedKeys[1].symbol;
        }
        if (thoughtBubble.selectedKeys.Count > 0)
        {
            symbolArray[0] = (int)thoughtBubble.selectedKeys[0].symbol;
        }
    }
}