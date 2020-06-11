using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 moveDestination;
    public float moveSpeed = 10.0f;

    void Awake()
    {
        moveDestination = transform.position;
    }
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

    }
}
