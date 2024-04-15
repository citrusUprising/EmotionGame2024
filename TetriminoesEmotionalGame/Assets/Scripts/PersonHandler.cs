using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class specialCases{
    public Symbols input;
    public Symbols output;
    public int affect;
}

[System.Serializable]
public class Person{
    public string name;
    public int affection; // caps at -1 and 100
    public Image[] spriteSheet;
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
    private Person character;

    //minutae
    private int genAffectMod = 10;
    private int openSpot = 0;
    private int openingLength;
    public float convoDelay = 0;
    public int delayMax = 3;
    private float trainTime = 0;

    private int closestProximity;


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
        if(character.name == "Stalker"&&character.proximity > closestProximity){
            character.proximity -= Time.deltaTime;
        }
        if (character.affection < 0){
            character.leaves = true;
        }
    }
    public void clear(){

    }

    public void importJSON(int i){
        character = manifestData.people[i];
        openingLength = character.opening.Length;
        openSpot = 0;
    }
    public void enable(){
        character.isActive = true;
    }

    public void annoy(){
        character.affection -= genAffectMod*2;
    }

    public void respond(Symbols input){
        Symbols temp = (Symbols)3; //FLAG  remember to change when enum pushes go through
        if (character.leaves){
            character.isActive = false;
        }
        if (!character.isActive){
            return;
        }
        convoDelay = 0;
        if (trainTime >= character.timeOut){
            if(character.affection > 60){
                //say Circle X
            }else if (character.affection > 20){
                //say X
            } else {
                //  Say Nothing
            }
            character.leaves = true;
            return;
        }
        for (int i = 0; i < character.cases.Length; i++){
            if (input == character.cases[i].input){
                //say character.cases[i].output;
                character.affection += character.cases[i].affect;
                return;
            }
        }
        if(openSpot < openingLength){
            //say opening[openSpot]
            openSpot++; 
        }
        else if((int)input%(int)temp==0){
            if (input == (Symbols)9){ //FLAG remeber to change to triangle x
                character.affection -= 100;
            } 
            if(character.affection > 60){
                //say Circle X
            }else if (character.affection > 20){
                //say X
            } else {
                //  Say Nothing
            }
            character.leaves = true;
        }
        else if (input == (Symbols)0&&character.waits){
            //do nothing
        }
        else {
            character.affection += genAffectMod;
        }

    }
}
