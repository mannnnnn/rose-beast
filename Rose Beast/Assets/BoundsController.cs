using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsController : MonoBehaviour
{

    public List<WallBounds> bounds = new List<WallBounds>();
     public List<GameObject> areas = new List<GameObject>();
    public int unlockedBounds = 1;

    public void ExpandBounds()
    {
        unlockedBounds++;
        bounds[unlockedBounds-1].gameObject.SetActive(true);
        bounds[unlockedBounds-2].RemoveBounds();
        areas[unlockedBounds-1].gameObject.SetActive(true);
        StartCoroutine(MoveTimerUp());
        StartCoroutine(MoveRetryDown());

        if(ChimeraController.Instance.instructions.gameObject.activeSelf){
            ChimeraController.Instance.instructions.gameObject.SetActive(false);
        }
    }

    IEnumerator MoveTimerUp()
    {
       float movementDelay = 0.3f;
       Transform timerTrans = ChimeraController.Instance.TimerLabel.transform;
       Vector3 origPosition = timerTrans.transform.position;
       Vector3 nextPos = new Vector3(timerTrans.transform.position.x, timerTrans.transform.position.y+26, timerTrans.transform.position.z);

       float elapsedTime = 0;
        while(elapsedTime < movementDelay){
            ChimeraController.Instance.TimerLabel.transform.position = Vector2.Lerp(origPosition, nextPos, elapsedTime/movementDelay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

      ChimeraController.Instance.TimerLabel.transform.position = nextPos;
       yield return null;
    }

     IEnumerator MoveRetryDown()
    {
       float movementDelay = 0.3f;
       Transform retryDown = ChimeraController.Instance.RetryLabel.transform;
       Vector3 origPosition = retryDown.transform.position;
       Vector3 nextPos = new Vector3(retryDown.transform.position.x, retryDown.transform.position.y-26, retryDown.transform.position.z);

       float elapsedTime = 0;
        while(elapsedTime < movementDelay){
            ChimeraController.Instance.RetryLabel.transform.position = Vector2.Lerp(origPosition, nextPos, elapsedTime/movementDelay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

      ChimeraController.Instance.RetryLabel.transform.position = nextPos;
       yield return null;
    }
}
