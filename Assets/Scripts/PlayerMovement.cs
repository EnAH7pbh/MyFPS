using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController player;
    public float speed = 12f;
    public float gravity = -10f;
    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistant = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    public float jumpHeight = 3f;
    // Update is called once per frame
    void Update () {
        isGrounded = Physics.CheckSphere (groundCheck.position, groundDistant, groundMask);
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        if (Input.GetButtonDown ("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt (jumpHeight * -2f * gravity);
        }
        if (Input.GetButtonDown ("SquatDown")) {
            this.transform.localScale = new Vector3 (1, 0.5f, 1);
        } else if (Input.GetButtonUp ("SquatDown")) {
            this.transform.localScale = new Vector3 (1, 1, 1);
        }
        if (Input.GetKeyDown (KeyCode.LeftShift)) {
            speed = speed * 2;
        }else if (Input.GetKeyUp(KeyCode.LeftShift)){
            speed = 12;
        }
        float x = Input.GetAxis ("Horizontal");
        float z = Input.GetAxis ("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        player.Move (move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        player.Move (velocity * Time.deltaTime);
    }
}