using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceBarToolTip : MonoBehaviour
{

    // this is displayed when the player is holding down one or two keys and the space bar is ready to be pressed
    public Color activeColor; 

    // this is displayed when the player is not holdin down any keys
    public Color inactiveColor; 

    bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        setIsActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setIsActive(bool b)
    {
        isActive = b;
        
        if (isActive)
        {
            GetComponent<Image>().color = activeColor;
            GetComponent<Hover>().isHovering = true;
        }
        if (!isActive)
        {
            GetComponent<Image>().color = inactiveColor;
            GetComponent<Hover>().isHovering = false;
        }
    }
}
