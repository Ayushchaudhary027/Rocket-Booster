using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] AudioClip mainEngine;
    [SerializeField] float giveThrust = 1000f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftSideThrustParticles;
    [SerializeField] ParticleSystem rightSideThrustParticles;

    AudioSource audioSource;
    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            body.transform.SetParent(null);
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }
    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.transform.SetParent(null);
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            body.transform.SetParent(null);
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void StartThrusting()
    {
        body.AddRelativeForce(Vector3.up * giveThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationSpeed);
        if (!rightSideThrustParticles.isPlaying)
        {
            rightSideThrustParticles.Play();
        }
    }
    private void RotateRight()
    {
        ApplyRotation(-rotationSpeed);
        if (!leftSideThrustParticles.isPlaying)
        {
            leftSideThrustParticles.Play();
        }
    }
    private void StopRotating()
    {
        rightSideThrustParticles.Stop();
        leftSideThrustParticles.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        body.freezeRotation = true; //freezing rotaion of the rigidbody to take control manually
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        body.freezeRotation = false; //unfreezing rotaion of the rigidbody so physics system can take control
    }
}
