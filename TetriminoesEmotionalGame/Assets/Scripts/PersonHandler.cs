using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class specialCases{
    public Symbols input;
    public Symbols output;
}

public abstract class person{
    public string name;
    public int affection; // caps at -1 and 100
    public Image[] spriteSheet;
    public bool leaves;
    public bool waits;
    public int proximity;
    public specialCases[] cases;
}

public class PersonHandler : MonoBehaviour
{
    private person character;



    void Awake(){
        character.affection = 0;
        character.leaves = true;
        character.waits = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(character.name == "Stalker"){

        }
    }
    public void clear(){

    }

    public void importJSON(int i){

    }
}
