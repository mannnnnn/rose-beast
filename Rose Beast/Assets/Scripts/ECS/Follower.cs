using UnityEngine;

public class Follower : MonoBehaviour {
    /*
    Follows the Leader without adhering to the timer rules
    Follows by occupying the last space the Eater was in, or the last space the higher index follower was in
    */
    
    public Mover mover;

    public void Start(){
        mover = GetComponent<Mover>();
    }

    
}
