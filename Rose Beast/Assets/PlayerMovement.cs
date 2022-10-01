using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Mover mover;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
    }

    void OnMove(InputValue input){
        moveInput = input.Get<Vector2>();
    }

    void OnFire(InputValue input){
        if(ChimeraController.Instance.debug){
            StartCoroutine(ChimeraController.Instance.TimesUp());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(moveInput.magnitude > 0f) {
            if(Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)){
                if(moveInput.x > 0){
                    mover.Move(Vector2.right);
                } else if (moveInput.x < 0){
                    mover.Move(Vector2.left);
                }
            } else{
                 if(moveInput.y > 0){
                    mover.Move(Vector2.up);
                } else if (moveInput.y < 0){
                    mover.Move(Vector2.down);
                }
            }
        }
    }
}
