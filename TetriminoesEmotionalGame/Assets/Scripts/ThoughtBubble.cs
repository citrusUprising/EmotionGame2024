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

    public List<SymbolKey> selectedKeys;
    public List<SymbolKey> lastSelectedKeys;

    public int numOfAcceptedSymbols = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            addSelectedKey(upKey);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            addSelectedKey(rightKey);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            addSelectedKey(downKey);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            addSelectedKey(leftKey);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            removeSelectedKey(upKey);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            removeSelectedKey(rightKey);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            removeSelectedKey(downKey);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            removeSelectedKey(leftKey);
        }
        highlightKeys();

        if (lastSelectedKeys != selectedKeys)
        {
            speechBubble.updateSymbolList();
        }
        lastSelectedKeys = selectedKeys;
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
}
