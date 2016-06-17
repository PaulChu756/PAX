using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Paul Chu Helicopter Script
/// </summary>

public class Aircraft : MonoBehaviour
{
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        engineSound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Control Torque applied to the body of the copter based on player input.
        // Get Axis is a float value that is between -1 to 1.
        // ControlTorque = (-1 to 1, 1, -1 to 1) Don't change Y number, Helicopter will spin
        Vector3 torqueValue = Vector3.zero;
        //Vector3 controlTorque = new Vector3(-Input.GetAxis("Horizontal") * forwardRotorTorqueMult, 1.0f, -Input.GetAxis("Vertical") * sidewaysRotorTorqueMult);
        Vector3 controlTorque = new Vector3(-Input.GetAxis("Mouse X") * forwardRotorTorqueMult, 1.0f, Input.GetAxis("Mouse Y") * sidewaysRotorTorqueMult);

        // Main Rotor is active, apply net torque to the copter body as well the lift force to create spinning rotors.
        if (mainRotorActive == true) 
        {
            // notes control torque to net torque. Also applies force to rb = y. for lift force
            torqueValue += (controlTorque * maxRotorForce * rotorVel); 
            rb.AddRelativeForce(Vector3.up * maxRotorForce * rotorVel);
        }

        // When the helicopter is lifting off, the y axis Increase for it to level out so it won't freak out/crash.
        // if it's less than 70s ANGLES!
        if (Vector3.Angle(Vector3.up, transform.up) < 20) 
        {
            // applies force to the rb in Y direction.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime * rotorVel * 2);
        }

        // tail rotor active. apply tail rotor force to net torque, torque to copter body.
        if (tailRotorActive == true) 
        {
            // subtract rotor max force mult by throttle from net forque and apply copter's body.
            torqueValue -= (Vector3.up * maxTailRotorForce * tailRotorVel); 
        }
        rb.AddRelativeTorque(torqueValue);
    }
    
    void Update()
    {
        // Engine SOUND! WOOT!
        engineSound.pitch = rotorVel;

        // animate main rotor
        if (mainRotorActive == true) 
        {
            mainRotor.transform.rotation = transform.rotation * Quaternion.Euler(0, rotorRot, 0); // Rotors JUST RIGHT! 
        }

        // Animite tail rotor
        if (tailRotorActive == true) 
        {
            tailRotor.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, tailRotorRot); // Tail rotate goes in the correct direction
        }

        // having the max rotor vel * rotor vel * delta time to equal and increment rotorRot
        // having maxTailRotorVel * rotor Vel * delta time to equal and increment tailRotorRot
        rotorRot += maxRotorVel * rotorVel * Time.deltaTime; 
        tailRotorRot += maxTailRotorVel * rotorVel * Time.deltaTime;

        // counteract force of gravity / max force rotors (0.0f to 1.0f) = minimum throttle to keep copter stationary.
        float hoverRotorVel = (rb.mass * Mathf.Abs(Physics.gravity.y) / maxRotorForce);
        // counteract torque of main rotor, find torque by main rotor to use for tail rotor throttle.
        float hoverTailRotorVel = (maxRotorForce * rotorVel) / maxTailRotorForce;

        // if the player increase rotor throttle, increase throttle to main rotor

        //if (Input.GetAxis("up") != 0.01f)
        //{
        //    rotorVel += Input.GetAxis("up") * 0.001f;
        //}

        if (Input.GetAxis("Vertical") != 0.01f)
        {
            rotorVel += Input.GetAxis("Vertical") * 0.001f;
        }

        else // to slow it back to hover vel.
        {
            rotorVel = Mathf.Lerp(rotorVel, hoverRotorVel, Time.deltaTime * Time.deltaTime * 5);
        }

        // setting tail rot vel to min vel. Increase or decrease by player in
        tailRotorVel = hoverTailRotorVel - Input.GetAxis("Horizontal");
        //Debug.Log(Input.GetAxis("down"));

        // limit rotor vel to 0 and 1 to make sure for a greater force.
        if (rotorVel > 1.0f) 
        {
            rotorVel = 1.0f;
        }

        else if(rotorVel < 0.0f)
        {
            rotorVel = 0.0f;
        }
    }

    AudioSource engineSound;

    public GameObject mainRotor;
    public GameObject tailRotor;

    static Rigidbody rb;

    float maxRotorForce = 22241.1081f;
    float maxRotorVel = 7200.0f;
    public float rotorVel = 0.0f;
    float rotorRot = 0.0f;

    float maxTailRotorForce = 15000.0f;
    float maxTailRotorVel = 2200.0f;
    float tailRotorVel = 0.0f;
    float tailRotorRot = 0.0f;

    // control mouse sensitvity
    float forwardRotorTorqueMult = 0.2f;
    float sidewaysRotorTorqueMult = 0.2f;

    bool mainRotorActive = true;
    bool tailRotorActive = true;

}
