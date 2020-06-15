using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 gridPosition = Vector2.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving)
        {
            transform.GetComponent<Renderer>().material.color = Color.blue;
        } else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking)
        {
            transform.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnMouseExit()
    {
        transform.GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving)
        {
            GameManager.instance.moveCurrentPlayer(this);
        }
        else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking)
        {
            GameManager.instance.attackWithCurrentPlayer(this);
        }
    }
}
