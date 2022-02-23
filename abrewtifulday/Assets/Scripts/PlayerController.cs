using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    float speed = 3f;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(-horizontal, 0f, -vertical).normalized;
        UpdateAnimations();
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
    void UpdateAnimations()
    {
        if (Input.GetKey(KeyCode.W)) {
            anim.SetBool("isWalking", true);
        } else if (Input.GetKey(KeyCode.A)) {
            anim.SetBool("isWalking", true);
        } else if (Input.GetKey(KeyCode.D)) {
            anim.SetBool("isWalking", true);
        } else if (Input.GetKey(KeyCode.S)) {
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }
    }
}