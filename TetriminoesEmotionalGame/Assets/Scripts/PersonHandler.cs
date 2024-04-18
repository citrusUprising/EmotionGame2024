using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class specialCases{
    public Symbols input;
    public Symbols output;
    public int affect;
    public int trigger = 0;
}

[System.Serializable]
public class Person{
    public string name;
    public int affection; // caps at -1 and 100
    public string[] spriteSheet;
    public bool leaves = false;
    public bool waits;
    public bool isActive = false;
    public float proximity;
    public specialCases[] cases;
    public Symbols[] standard;
    public Symbols[] opening;
    public int timeOut;
}

[System.Serializable]
public class AllPeople{
    public Person[] people;
}

public class PersonHandler : MonoBehaviour
{
    //Json
    [SerializeField] private TextAsset manifestJson; //assigned in inspector
    [SerializeField] private AllPeople manifestData;
    [SerializeField] private SpeechBubble toSay;
    private Person character;
    private Vector3 fixedPos;

    //minutae
    private int genAffectMod = 5;
    private int openSpot = 0;
    private int openingLength;
    public float convoDelay = 0;
    public float delayMax = 1;
    private float trainTime = 0;
    private int chatCount = 0;

    private int closestProximity = 10;


    void Awake(){
        character = new Person();
        character.affection = 0;
        character.leaves = true;
        character.waits = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        //manifestData = new AllPeople();// for testing purposes
        Debug.Log ("Importing JSON");
        manifestData = JsonUtility.FromJson<AllPeople>(manifestJson.text);
        Debug.Log ("JSON successfully read, first person is named "+manifestData.people[0].name);
    }

    // Update is called once per frame
    void Update()
    {
        trainTime += Time.deltaTime;
        if (!character.leaves && convoDelay < delayMax){
            convoDelay += Time.deltaTime;
        }
        if(character.name == "Creep"&&character.proximity > closestProximity&&character.isActive){
            character.proximity -= Time.deltaTime;
        }
        if (character.affection < 0){
            character.leaves = true;
        }
        if(character.isActive){
            this.GetComponent<Transform>().localPosition = new Vector3 (fixedPos.x + (50-character.proximity), fixedPos.y, fixedPos.z);
        } 
    }
    public void clear(){

    }

    public void importJSON(int i){
        character = manifestData.people[i];
        openingLength = character.opening.Length;
        openSpot = 0;
        chatCount = 0;
    }
    public void enable(){
        character.isActive = true;
    }

    public void annoy(){
        character.affection -= genAffectMod*2;
    }

    public void respond(Symbols input){
        Symbols temp = Symbols.ex; 

        //deactivates character
        if (character.leaves){
            character.isActive = false;
        }

        //prevents deactivated characters from responding
        if (!character.isActive){
            return;
        }
        convoDelay = 0; //reset time since character last spoke
        chatCount ++;

        //says goodbye if train hits destination
        if (trainTime >= character.timeOut){
            if(character.affection > 60){
                speak(Symbols.circleX);
            }else if (character.affection > 20){
                speak(Symbols.ex);
            } else {
                //  Say Nothing
            }
            character.leaves = true;
            return;
        }

        //checks for special cases and responds
        for (int i = 0; i < character.cases.Length; i++){
            if (input == character.cases[i].input && chatCount >= character.cases[i].trigger){
                speak(character.cases[i].output);
                character.affection += character.cases[i].affect;
                return;
            }
        }

        //recites opening text if applicable
        //also punishes player for talking about complex topics too early
        if(openSpot < openingLength){
            if((int)input > 5){
                character.affection -= genAffectMod;
            }
            speak(character.opening[openSpot]); 
            openSpot++; 
        }

        //responses if player ends conversation
        else if((int)input%(int)temp==0){
            if (input == (Symbols)20){ //triangle x
                character.affection -= 100;
            } 
            if(character.affection > 60){
                speak(Symbols.circleX);
            }else if (character.affection > 20){
                speak(Symbols.ex);
            } else {
                //  Say Nothing
            }
            character.leaves = true;
        }

        //waiting for player response, this is mostly for security's sake
        else if (input == Symbols.empty&&character.waits){
            //do nothing
        }

        //general response
        else {
            speak(character.standard[Random.Range(0,character.standard.Length)]);
            character.affection += genAffectMod;
        }

    }

    private void speak (Symbols output){
        toSay.updateSymbolList(output);
    }

    public bool checkWait(){
        return character.waits;
    }

    public void pickFrame (State mov){
        this.GetComponent<SpriteRenderer>().flipX = false;
        if (mov == State.personLeave){
            this.GetComponent<SpriteRenderer>().flipX = true;
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>(character.spriteSheet[1]).GetComponent<SpriteRenderer>().sprite;
        }
        else if (mov == State.personEnter){
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>(character.spriteSheet[1]).GetComponent<SpriteRenderer>().sprite;
        }
        else{
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>(character.spriteSheet[0]).GetComponent<SpriteRenderer>().sprite;
        }
    }

    public int pullTimeOut(){
        return character.timeOut;
    }

    public int pullTotal(){
        return manifestData.people.Length;
    }

    public bool pullActive(){
        return character.isActive;
    }

    public string pullName(){
        return character.name;
    }

    public void setFixedPos(Vector3 insert){
        fixedPos = insert;
    }

}
