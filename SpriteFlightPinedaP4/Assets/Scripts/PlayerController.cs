using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;

    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;

    public float maxSpeed = 5f;
    public GameObject boosterFlame;
    

    Rigidbody2D rb;

    public UIDocument uiDocument;
    private Label scoreText;
    private Button restartButton;

    public GameObject explosionEffect;

    private object bounceEffectPrefab;

    private GameObject Instantiate(object bounceEffectPrefab, Vector2 contactPoint, Quaternion identity)
    {
        throw new NotImplementedException();
    }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;

    }

    // Update is called once per frame
    void Update()
    {
      
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
        Debug.Log("Score: " + score);


        if (Mouse.current.leftButton.isPressed) {
           
            // Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;


            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);


            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                boosterFlame.SetActive(true);
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                boosterFlame.SetActive(false);
            }

        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;

        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);

        // Destory the effect after 1 second
        Destroy(bounceEffect, 1f);

    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



}


        
    






