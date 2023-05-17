using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Mover mover;
    private Vector2 moveInput;
    public bool CanMove = true; 

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
    }

    void OnMove(InputValue input){
        moveInput = input.Get<Vector2>();

        if(!CanMove && !ChimeraController.Instance.gameRunning){
             ChimeraController.Instance.StartOver();
        }
    }

    void OnFire(InputValue input){
        if(ChimeraController.Instance.debug){
            StartCoroutine(ChimeraController.Instance.TimesUp());
        }

        if(!CanMove && !ChimeraController.Instance.gameRunning){
             ChimeraController.Instance.StartOver();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!CanMove) return;
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
