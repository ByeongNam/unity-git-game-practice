using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0,1)] float movementFactor;
    [SerializeField] float period = 2f;
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon){ return; } // NaN handle smallest float 이하로 나누기 방지
        float cycles = Time.time / period; //Continually growing over time

        const float tau = Mathf.PI * 2; // constant value 2파이
        float rawSinWave = Mathf.Sin(cycles * tau); // -1 ~ 1 의 값
        
        movementFactor = (rawSinWave + 1f)/2f; //movement factor 는 0부터 1 이므로 rawSinWave 에 1을 더하면 0~2 이것을 2로 나누면 0~1

        Vector3 offset = movementVector * movementFactor;
        transform.position = offset + startingPosition;
    }
}
