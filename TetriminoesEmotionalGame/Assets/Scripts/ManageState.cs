using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageState : MonoBehaviour
{
    
    public GameObject camera;
    public GameObject player;
    public GameObject character;
    public ThoughtBubble thoughts;
    public GameObject npcSpeech;
    public AudioSource pA;
    public SpriteRenderer background;
    public Image blackScreen;
    [SerializeField] private int currentChar = 0;
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
    private bool isPause = false;

    private Vector3 startPoint;
    private Vector3 endPointChar;
    //private Vector3 endPointPlayer;
    private Vector3 endPointFinal;
    //section time limits
    private float startTime = 2;
    private float enterTime = 5;
    private float leaveTime = 5;
    private float endTime = 2;

    //sounds
    public AudioClip entering;
    public AudioClip leaving;
    //station names
    public AudioClip Turnstyle;
    public AudioClip Mission;
    public AudioClip Odyssey;
    public AudioClip Crossroads;
    public AudioClip Winston;
    public AudioClip Leisure;
    public AudioClip Terminal;
    private AudioClip[] stations;
    //backgrounds
    public Sprite[] backgrounds;
    public Sprite backgroundTwo;
    public Sprite backgroundOne;


    // Start is called before the first frame update
    void Start()
    {
        startPoint = new Vector3(-400.0f,70.0f,1.0f);//FLAG change as appropriate
        endPointChar = new Vector3(-97.0f,70.0f,1.0f);
        //endPointPlayer = new Vector3(-951.0f,-583.0f,1.0f);
        endPointFinal = new Vector3(200.0f,70.0f,1.0f);
        character.GetComponent<Transform>().localPosition = startPoint;
        //player.GetComponent<Transform>().localPosition = endPointPlayer;
        game = State.start;
        character.GetComponent<PersonHandler>().setFixedPos(endPointChar);
        stations = new AudioClip[7];
        stations[0]= Turnstyle;
        stations[1]= Mission;
        stations[2]= Odyssey;
        stations[3]= Crossroads;
        stations[4]= Winston;
        stations[5]= Leisure;
        stations[6]= Terminal;
        backgrounds = new Sprite[2];
        backgrounds[0] = backgroundTwo;
        backgrounds[1] = backgroundOne;
    }

    // Update is called once per frame
    void Update()
    {
        switch(game){
            case State.start: //Opening cinematic---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                if(onSwitch){
                    onSwitch = false;
                    npcSpeech.SetActive(false);
                    StartCoroutine(fadeEnds(true));
                }
                timer += Time.deltaTime;
                if(timer >= startTime){
                    onSwitch = true;
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
                    //moves new character
                    StartCoroutine(lerp(startPoint,endPointChar,enterTime, false));
                    playOutro();
                    Debug.Log("Someone is coming");
                }
                Pausable();
                if(!isPause){
                    timer += Time.deltaTime;
                }
                
                //ends entering scene
                if(timer >= enterTime){
                    game = State.sitting;
                    onSwitch = true;
                    timer = 0;
                }
            break;

            case State.sitting: //when the train is moving but a conversation hasn't started---------------------------------------------------------------------------------------------------------------------------
                if(onSwitch){
                    //set camera to default
                    onSwitch = false;
                    character.GetComponent<PersonHandler>().pickFrame(game);
                    Debug.Log(character.GetComponent<PersonHandler>().pullName()+" sat next to you");
                    savedSymbol = Symbols.empty;
                }
                timer += Time.deltaTime;
                character.GetComponent<PersonHandler>().setProximity();

                //safety catch  prevents players from waiting TOO long
                if(startedTalk){
                    npcSpeech.SetActive(false);
                    if(timer<character.GetComponent<PersonHandler>().pullTimeOut()-safetyTime){
                        timer=character.GetComponent<PersonHandler>().pullTimeOut()-safetyTime;
                    }
                }

                if (timer >= character.GetComponent<PersonHandler>().pullTimeOut()){
                    end();
                }
                else if (Input.GetKeyDown(KeyCode.Space)||!character.GetComponent<PersonHandler>().checkWait()){
                    if(!startedTalk){
                        game = State.talking;
                        character.GetComponent<PersonHandler>().enable();
                        onSwitch = true;
                    }
                    handleInput();
                }

            break;

            case State.talking: //when the player is in conversation---------------------------------------------------------------------------------------------------------------------------------------------------
                 if(onSwitch){
                    //set camera to zoom in
                    onSwitch = false;
                    npcSpeech.SetActive(true);
                    Debug.Log(character.GetComponent<PersonHandler>().pullName()+" started talking");
                    character.GetComponent<PersonHandler>().pickFrame(game);
                 }
                 timer += Time.deltaTime;

                 if (!character.GetComponent<PersonHandler>().checkWait()&&
                 !responding/*&&
                 character.GetComponent<PersonHandler>().convoDelay >= character.GetComponent<PersonHandler>().delayMax*/){
                    StartCoroutine(responseDelay());
                 }

                if (timer >= character.GetComponent<PersonHandler>().pullTimeOut()){
                    StartCoroutine(responseDelay());
                    StartCoroutine(delayEnd());
                }else if(!character.GetComponent<PersonHandler>().pullActive()){
                    onSwitch = true;
                    game = State.sitting;
                    Debug.Log(character.GetComponent<PersonHandler>().pullName()+" stopped talking");
                    npcSpeech.SetActive(false);
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
                    StartCoroutine(playIntro(currentChar));
                    currentChar++;
                    StartCoroutine(lerp(character.GetComponent<Transform>().localPosition,endPointFinal,leaveTime, true));
                    Debug.Log(character.GetComponent<PersonHandler>().pullName()+" is leaving");
                    if(currentChar >=3 && currentChar <=5){
                        StartCoroutine(fade(currentChar%2));
                    }
                }
                Pausable();
                if(!isPause){
                    timer += Time.deltaTime;
                }

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
                    timer = 0;
                    //StartCoroutine(lerp(player,endPointPlayer,endPointFinal,endTime, true));
                    Debug.Log("You've arrived at your station");
                    StartCoroutine(fadeEnds(false));
                }
                timer += Time.deltaTime;
                if(timer >= endTime){
                    Application.Quit();
                    Debug.Log("application quit");
                }
            break;

            default://---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            break;
        }
    }

    IEnumerator responseDelay(){
        responding = true;
        yield return new WaitForSeconds(character.GetComponent<PersonHandler>().delayMax);
        character.GetComponent<PersonHandler>().respond(savedSymbol,timer);
        savedSymbol = Symbols.empty;
        responding = false;
    }


    private void end(){
        if((int)game>=4&&(int)game<=5){
            game = State.personLeave;
            npcSpeech.SetActive(false);
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
        savedSymbol = thoughts.returnSpeech();

        if (character.GetComponent<PersonHandler>().convoDelay < character.GetComponent<PersonHandler>().delayMax
        &&character.GetComponent<PersonHandler>().checkWait()){
            character.GetComponent<PersonHandler>().annoy();
        }
        if (!responding){
            StartCoroutine(responseDelay());
        }
    }

    IEnumerator lerp(Vector3 start, Vector3 end, float duration, bool easeIn)
    {
        float time = 0.0f;
        Vector3 d = start;

        while (time < duration)
        {
            float t = time / duration;

            //t = -(Mathf.Cos(Mathf.PI * t) - 1) / 2; //swap to to either ease in or out depending on bool //FLAG
            if(easeIn){
                //ease in
                t = 1 - Mathf.Cos((t * Mathf.PI) / 2);
            }else{
                //ease out
                t = Mathf.Sin((t * Mathf.PI) / 2);
            }

            d = Vector3.Lerp(start, new Vector3 (end.x + (5-character.GetComponent<PersonHandler>().pullProximity()/5),end.y,end.z), t);

            character.GetComponent<Transform>().localPosition = d;

            if(!isPause){
                time += Time.deltaTime;
            }

            yield return null;
        }
        character.GetComponent<Transform>().localPosition = d;
    }

    IEnumerator playIntro(int station){
        playSound(entering);
        yield return new WaitForSeconds(entering.length);
        playSound(stations[station]);
    }
    private void playOutro(){
        playSound(leaving);
    }

    private void playSound(AudioClip clip)
    {
        pA.clip = clip;
        pA.Play();   
    }

    IEnumerator fade(int bg){
        yield return new WaitForSeconds(leaveTime/3);
        float time = 0.0f;
        Color start = blackScreen.color;
        while(time<leaveTime/3){
            time += Time.deltaTime;
            blackScreen.color = start;
            start.a = 3*time/leaveTime;
            yield return null;
        }
        start.a = 1;
        blackScreen.color = start;
        yield return new WaitForSeconds(leaveTime/3);
        background.sprite = backgrounds[bg];
        time = leaveTime*2/3;
        while (time<leaveTime){
            time += Time.deltaTime;
            blackScreen.color = start;
            start.a = 3*(leaveTime-time)/leaveTime;
            yield return null;
        }
        start.a = 0;
        blackScreen.color = start;
    }

    IEnumerator fadeEnds(bool intro){
        Color start = blackScreen.color;
        if (intro){
            yield return new WaitForSeconds(startTime/2);
            float time = 0.0f;
            while(time<startTime/2){
                time += Time.deltaTime;
                blackScreen.color = start;
                start.a = (startTime-2*time)/startTime;
                yield return null;
            }
        }
        else{
            yield return new WaitForSeconds(endTime/2);
            float time = 0.0f;
            while(time<endTime/2){
                time += Time.deltaTime;
                blackScreen.color = start;
                start.a = 2*time/endTime;
                yield return null;
            }
        }
    }

    private void Pausable(){
        if (Input.GetKeyDown(KeyCode.Backspace)){
            isPause = !isPause;
            Debug.Log("pause hit");
        }
    }
}
