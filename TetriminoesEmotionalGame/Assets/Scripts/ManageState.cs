using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageState : MonoBehaviour
{
    
    public GameObject camera;
    public GameObject character;
    private int currentChar = 0;
    private State game;
    private bool onSwitch;
    private Vector3 camOut;
    private Vector3 camIn;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        game = State.start;
    }

    // Update is called once per frame
    void Update()
    {
        switch(game){
            case State.start: //Opening cinematic
            timer += Time.deltaTime;
            break;

            case State.personEnter: //when a new person is entering the train
            if(onSwitch){
                character.GetComponent<PersonHandler>().importJSON(currentChar);
                timer = 0;
                onSwitch = false;
            }
            timer += Time.deltaTime;
            break;

            case State.sitting: //when the train is moving but a conversation hasn't started
            if(onSwitch){
                timer = 0;
                onSwitch = false;
            }
            timer += Time.deltaTime;
            break;

            case State.talking: //when the player is in conversation
            if(onSwitch){
                character.GetComponent<PersonHandler>().enable();
                timer = 0;
                onSwitch = false;
            }
            timer += Time.deltaTime;
            break;

            case State.personLeave: //when a person is exiting the train
            if(onSwitch){
                timer = 0;
                onSwitch = false;
            }
            timer += Time.deltaTime;
            break;

            case State.end: //closing cinematic
            if(onSwitch){
                onSwitch = false;
            }
            timer += Time.deltaTime;
            break;

            default:
            break;
        }
    }
}
