using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{
    public State state;

    public enum State {
        Normal, AfterDamage
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(90, 0 ,0));
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
        if(actionPoints == 0)
        {
            actionPoints = 2;
            moving = false;
            attacking = false;
            transform.GetComponent<Renderer>().material.color = Color.white;
            GameManager.instance.nextTurn();
        }
    }

    public override void TurnUpdate()
    {
        if (Vector3.Distance(moveDestination, transform.position) > 0.1f)
        {
            transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
            if (Vector3.Distance(moveDestination, transform.position) <= 0.1f)
            {
                transform.position = moveDestination;
                actionPoints--;
            }
        }
        transform.GetComponent<Renderer>().material.color = Color.green;
    }
    public override void TurnOnGUI()
    {
        float buttonHeight = 50;
        float buttonWidth = 150;

        Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);
        if (GUI.Button(buttonRect, "Move")){
            if (!moving)
            {
                moving = true;
                attacking = false;
            }
            else
            {
                moving = false;
                attacking = false;
            }
        }

        buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
        if (GUI.Button(buttonRect, "Attack")) {
            if (!attacking)
            {
                moving = false;
                attacking = true;
            }
            else {
                moving = false;
                attacking = false;
            }
            state = State.AfterDamage;
        }

        buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);
        if (GUI.Button(buttonRect, "End Turn")){
            actionPoints = 2;
            moving = false;
            attacking = false;
            transform.GetComponent<Renderer>().material.color = Color.white;
            GameManager.instance.nextTurn();
        }
        base.TurnUpdate();
    }
}
