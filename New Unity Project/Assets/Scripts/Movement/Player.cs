using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Sprite portrait;

    public Vector2 gridPosition = Vector2.zero;

    public float moveSpeed = 10.0f;

    public bool moving = false;
    public bool attacking = false;

    public string playerName = "George";
    public int HP = 25;
    public int currentHP = 25;

    public float attackChance = 0.75f;
    public float defenseReduction = 0.15f;
    public int damageBase = 5;
    public int moveDistance = 5;
    public int attackRange = 1;
    public int damageRollSides = 6; //d6

    public int actionPoints = 2;

    public int playerNumber;

    


    public List<Vector3> positionQueue = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TurnUpdate()
    {
        if (actionPoints <= 0)
        {
            actionPoints = 2;
            moving = false;
            attacking = false;
            GameManager.instance.players[GameManager.instance.currentPlayerIndex].transform.GetComponent<Renderer>().material.color = Color.white;
            GameManager.instance.nextTurn();
        }
    }
    public virtual void TurnOnGUI()
    {

    }
}
