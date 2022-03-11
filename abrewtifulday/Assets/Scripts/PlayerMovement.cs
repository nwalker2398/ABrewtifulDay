using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    void Update()
    {
        Vector3 movement = GetPlayerInput();
        Vector3 scaleMovement = movement * movementSpeed * Time.deltaTime;
        transform.Translate(scaleMovement);

        if (Input.GetKeyDown(KeyCode.Space)) {
            ScoreSystem.incrementScore(1);
        }
        transform.position = new Vector3(transform.position.x, 0.047f, transform.position.z);
    }

    Vector3 GetPlayerInput()
    {
        Vector3 movement = new Vector3();

        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");

        return movement;
    }
}
