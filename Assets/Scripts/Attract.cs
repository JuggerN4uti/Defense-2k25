using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : MonoBehaviour
{
    GameObject Player;
    public float attractionSpeed, attractionAcceleration;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, attractionSpeed * Time.deltaTime);
        attractionSpeed += attractionAcceleration * Time.deltaTime;
    }
}
