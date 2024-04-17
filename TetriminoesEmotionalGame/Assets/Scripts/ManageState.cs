using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageState : MonoBehaviour
{
    
    public GameObject camera;
    public GameObject character;
    private int currentChar = 0;
    private State game;
    private bool onSwitch = true;
    private Vector3 camOut;
    private Vector3 camIn;
    [SerializeField] private float timer = 0;
    private Symbols savedSymbol = Symbols.empty;
    private bool responding = false;
    private bool ending = false;
    private bool startedTalk = false;
    private int safetyTime = 15; //amount of seconds after fucking up a conversation before a character change is forced

    private Vector3 startPoint;
    private Vector3 endPoint;
    //section time limits
    private float startTime = 5;
    private float enterTime = 5;
    private float leaveTime = 5;
    private float endTime = 5;


    // Start is called before the first frame update
    void Start()
    {
        game = State.start;
    }

    // Update is called once per frame
    void Update()
    {
        switch(game){
            case State.start: //Opening cinematic---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                timer += Time.deltaTime;
                if(timer >= startTime){
                    Debug.Log("You've sat down");
                    game = State.personEnter;
                }
            break;

            case State.personEnter: //when a new person is entering the train------------------------------------------------------------------------------------------------------------------------------------------
                if(onSwitch){
                    character.GetComponent<PersonHandler>().importJSON(currentChar);
                    timer = 0;
                    onSwitch = false;
                    startedTalk = false;
                    character.GetComponent<PersonHandler>().pickFrame(game);
                    Debug.Log("Someone is coming");
                }
                timer += Time.deltaTime;

                //moves new character
                
                //ends entering scene
                if(timer >= enterTime){
                    game = State.sitting;
                    onSwitch = true;
                }
            break;

            case State.sitting: //when the train is moving but a conversation hasn't started---------------------------------------------------------------------------------------------------------------------------
                if(onSwitch){
                    //set camera to default
                    onSwitch = false;
                    character.GetComponent<PersonHandler>().pickFrame(game);
                    Debug.Log(character.GetComponent<PersonHandler>().pullName()+" sat next to you");
                }
                timer += Time.deltaTime;

                //safety catch  prevents players from waiting TOO long
                if(startedTalk&&timer<character.GetComponent<PersonHandler>().pullTimeOut()-safetyTime){
                    timer=character.GetComponent<PersonHandler>().pullTimeOut()-safetyTime;
                }

                if (timer >= character.GetComponent<PersonHandler>().pullTimeOut()){
                    end();
                }
                else if (Input.GetKeyDown(KeyCode.Space)){
                    if(!startedTalk){
                        game = State.talking;
                        character.GetComponent<PersonHandler>().enable();
                    }
                    handleInput();
                }

            break;

            case State.talking: //when the player is in conversation---------------------------------------------------------------------------------------------------------------------------------------------------
                 if(onSwitch){
                    //set camera to zoom in
                    onSwitch = false;
                    Debug.Log(character.GetComponent<PersonHandler>().pullName()+" started talking");
                 }
                 timer += Time.deltaTime;

                 if (!character.GetComponent<PersonHandler>().checkWait()&&
                 !responding&&
                 character.GetComponent<PersonHandler>().convoDelay >= character.GetComponent<PersonHandler>().delayMax){
                    StartCoroutine(responseDelay());
                 }

                if (timer >= character.GetComponent<PersonHandler>().pullTimeOut()){
                    StartCoroutine(responseDelay());
                    StartCoroutine(delayEnd());
                }else if(!character.GetComponent<PersonHandler>().pullActive()){
                    onSwitch = true;
                    game = State.sitting;
                    Debug.Log(character.GetComponent<PersonHandler>().pullName()+" stopped talking");
                }

                 //on player input-------------------------------------------------------------------//
                 if (Input.GetKeyDown(KeyCode.Space)){
                    handleInput();
                }
            break;

            case State.personLeave: //when a person is exiting the train-----------------------------------------------------------------------------------------------------------------------------------------------
                if(onSwitch){
                    timer = 0;
                    onSwitch = false;
                    savedSymbol = Symbols.empty;
                    character.GetComponent<PersonHandler>().pickFrame(game);
                    currentChar++;
                    Debug.Log(character.GetComponent<PersonHandler>().pullName()+" is leaving");
                }
                timer += Time.deltaTime;

                //move character

                if(timer >= leaveTime){
                    onSwitch = true;
                    if(currentChar < character.GetComponent<PersonHandler>().pullTotal()){
                        game = State.personEnter;
                    }
                    else{
                        game = State.end;
                    }
                }
            break;

            case State.end: //closing cinematic-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                if(onSwitch){
                    onSwitch = false;
                    Debug.Log("You've arrived at your station");
                }
                timer += Time.deltaTime;
                if(timer >= endTime){
                    Application.Quit();
                }
            break;

            default://---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            break;
        }
    }

    IEnumerator responseDelay(){
        responding = true;
        yield return new WaitForSeconds(character.GetComponent<PersonHandler>().delayMax);
        character.GetComponent<PersonHandler>().respond(savedSymbol);
        savedSymbol = Symbols.empty;
        responding = false;
    }


    private void end(){
        if((int)game>=4&&(int)game<=5){
            game = State.personLeave;
            onSwitch = true;
        }
    }

    IEnumerator delayEnd(){
        if (!ending){
            ending = true;
            yield return new WaitForSeconds(character.GetComponent<PersonHandler>().delayMax*3/2);
            end();
            ending = false;
        }

    }

    private void handleInput(){
        startedTalk = true;
        //code to change savedSymbol
        if (character.GetComponent<PersonHandler>().convoDelay < character.GetComponent<PersonHandler>().delayMax
        &&character.GetComponent<PersonHandler>().checkWait()){
            character.GetComponent<PersonHandler>().annoy();
        }
        if (!responding){
            StartCoroutine(responseDelay());
        }
    }
}
