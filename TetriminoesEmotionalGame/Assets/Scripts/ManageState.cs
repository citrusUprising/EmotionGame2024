using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageState : MonoBehaviour
{
    public enum State
    {
        start = 0,
        end = 1,
        personEnter = 2,
        personLeave = 3,
        sitting = 4,
        talking = 5
    };
    public enum Symbols
    {
        square = 0,
        circle = 1,
        triangle = 2,
        x = 3,
        squareX = 4,
        squareTri = 5,
        squareCirc = 6,
        circleX = 7,
        circleTri = 8,
        triangleX = 9

    };
    public GameObject camera;
    private State game;

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
