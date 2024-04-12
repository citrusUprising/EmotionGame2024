using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageState : MonoBehaviour
{
    
    public GameObject camera;
    private State game;
    private bool onSwitch;
    private Vector3 camOut;
    private Vector3 camIn;

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
            break;

            case State.personEnter: //when a new person is entering the train
            break;

            case State.sitting: //when the train is moving but a conversation hasn't started
            break;

            case State.talking: //when the player is in conversation
            break;

            case State.personLeave: //when a person is exiting the train
            break;

            case State.end: //closing cinematic
            break;

            default:
            break;
        }
    }
}
