using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayInReload = 1f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip rocketCrashed;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashedParticles;

    AudioSource audioSource;
    Rigidbody body;

    bool isTransitioning = false;
    bool collisionDisbale = false;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheatCodes();
    }

    private void CheatCodes()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            collisionDisbale = !collisionDisbale; //toggle collision
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisbale) { return; }

        switch (collision.gameObject.tag)
        {
            case "Helper":
                Debug.Log("in helper");
                body.transform.SetParent(collision.transform);
                break;

            case "Friendly":    //launch pad
                break;

            case "Finish":  //landing pad
                StartSuccessSequence();
                break;

            default:    //Obstacles
                StartCrashSequence();
                break;
        }
        Debug.Log("out helper"); 
        
    }

    public void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delayInReload);
    }

    public void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(rocketCrashed);
        crashedParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayInReload);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
