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
        transform.GetComponent<Renderer>().material.color = Color.blue;
    }

    private void OnMouseExit()
    {
        transform.GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnMouseDown()
    {
        GameManager.instance.moveCurrentPlayer(this);
    }
}
