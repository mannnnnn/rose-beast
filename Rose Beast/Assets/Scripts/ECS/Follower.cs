using UnityEngine;
using UnityEngine.Tilemaps;


[DisallowMultipleComponent]
public class Follower : Trait
{
    /*
    Follows the Leader without adhering to the timer rules
    Follows by occupying the last space the Eater was in, or the last space the higher index follower was in
    */
    
    public Mover mover;
    public Leader leader;

    public void Start(){
        mover = GetComponent<Mover>();
        if(leader == null){
            FindNearbyLeader();
        }
    }

    public void FindNearbyLeader(){
        if(leader == null){
           Leader closestLeader = null;
           float closestLeaderDistance = -1;

           foreach(Leader foundLeader in FindObjectsOfType<Leader>()){
                
                float foundLeaderDistance = (transform.position - foundLeader.transform.position).magnitude;
                
                if(closestLeader == null || closestLeaderDistance > foundLeaderDistance){
                    closestLeader = foundLeader;
                    closestLeaderDistance = (transform.position - closestLeader.transform.position).magnitude;
                }
           }

           if(closestLeader != null){
                leader = closestLeader;
                if(!leader.followers.Contains(this)){
                    leader.followers.Add(this);
                }
           }
        }
    }

    
}
