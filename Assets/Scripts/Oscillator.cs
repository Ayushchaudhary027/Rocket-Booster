using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField][Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;


    public void Start()
    {
        startingPosition = transform.position;
    }


    public void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;  //continually growing over time
        const float tau = Mathf.PI * 2; //constant value 
        float rawSinWave = Mathf.Sin(cycles * tau); //going from -1 to +1
        movementFactor = (rawSinWave + 1f) / 2f;    //recalculated to go from 0 to 1
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
