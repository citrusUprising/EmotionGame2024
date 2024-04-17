using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    
    [SerializeField] private SymbolKey upKey;
    [SerializeField] private SymbolKey rightKey;
    [SerializeField] private SymbolKey downKey;
    [SerializeField] private SymbolKey leftKey;

    [SerializeField] private Color selectedColor;
    [SerializeField] private SpeechBubble speechBubble;

    [SerializeField] private SpaceBarToolTip spaceBarToolTip;

    public List<SymbolKey> selectedKeys;

    public int numOfAcceptedSymbols = 2;

    float chooseSymbolPause = 2.0f;
    bool isHidden = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHidden)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            chooseSymbol();
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            addSelectedKey(upKey);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            addSelectedKey(rightKey);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            addSelectedKey(downKey);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            addSelectedKey(leftKey);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            removeSelectedKey(upKey);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            removeSelectedKey(rightKey);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            removeSelectedKey(downKey);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)  || Input.GetKeyUp(KeyCode.A))
        {
            removeSelectedKey(leftKey);
        }

        highlightKeys();

        setSpaceBarActive();

        updateSpeechBubbleSymbols();
    }

    void updateSpeechBubbleSymbols()
    {
        Symbols symbol1 = Symbols.empty;
        Symbols symbol2 = Symbols.empty;

        if (selectedKeys.Count > 1)
            symbol1 = selectedKeys[1].symbol;
        if (selectedKeys.Count > 0)
            symbol2 = selectedKeys[0].symbol;

        speechBubble.updateSymbolList(symbol1, symbol2);
    }

    void addSelectedKey(SymbolKey key)
    {
        if (numOfAcceptedSymbols == 2)
        {
            if (selectedKeys.Count < 2 && !selectedKeys.Contains(key))
            {
                selectedKeys.Add(key);
            }
        }
        if (numOfAcceptedSymbols == 1)
        {
            if (selectedKeys.Count < 1 && !selectedKeys.Contains(key))
            {
                selectedKeys.Add(key);
            }
        }
    }

    void removeSelectedKey(SymbolKey key)
    {
        if (selectedKeys.Count > 1 && selectedKeys[1] == key)
                selectedKeys.RemoveAt(1);
        
        if (selectedKeys.Count > 0 && selectedKeys[0] == key)
                selectedKeys.RemoveAt(0);
    }

    void highlightKeys()
    {
        upKey.GetComponent<Image>().color = Color.white;
        rightKey.GetComponent<Image>().color = Color.white;
        downKey.GetComponent<Image>().color = Color.white;
        leftKey.GetComponent<Image>().color = Color.white;
        
        for (int i = 0; i < selectedKeys.Count; i++)
        {
            selectedKeys[i].GetComponent<Image>().color = selectedColor;
        }
    }

    void setSpaceBarActive()
    {
        if (selectedKeys.Count > 0)
        {
            spaceBarToolTip.setIsActive(true);
        }
        else
        {
            spaceBarToolTip.setIsActive(false);
        }
    }

    void chooseSymbol()
    {
        if (selectedKeys.Count == 0)
            return;
        updateSpeechBubbleSymbols();
        
        StartCoroutine(hideSelf());
    }

    IEnumerator hideSelf()
    {
        isHidden = true;

        GetComponent<CanvasRenderer>().SetAlpha(0);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CanvasRenderer>().SetAlpha(0);
        }
        yield return new WaitForSeconds(chooseSymbolPause);
        GetComponent<CanvasRenderer>().SetAlpha(1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CanvasRenderer>().SetAlpha(1);
        }
        selectedKeys.Clear();
        isHidden = false;
    }
}
