using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public CamController camController;
    public LayerMask targetMask;
    public float radius;
    public float angle;
    Light spotlight;
    bool detected = false;
    private void Start()
    {
        spotlight = GetComponentInChildren<Light>();
        radius = spotlight.range;
        angle = spotlight.innerSpotAngle;
    }
    private void FieldOfViewCheck()
    {
        //this is what actually looks for the player
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        //If anything is in our array it has picked up our player
        if (rangeChecks.Length != 0)
        {
            //the only thing in the targetmask is the player, so we use the first index
            Transform target = rangeChecks[0].transform;
            //establishes direction to enemy rotation to player location
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //gets the angle between the forward direction and the normalized vector to the target and compares it to half the angle we established in the beginning.
            //the angle is halved because half of the angle is to the left and half is to the right
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //starts raycast from center of enemy, toward the player, from the distance to the player, only checking objects in the obstructionMask
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget))
                {
                    
                }
                else
                    detected = false;
            }
            else
                //this could cause problems later
                detected = false;
        }
        //if the player is detected and leaves the detection range then they are no longer being detected
        else if (detected)
            detected = false;

        //chases the player if the detection is maxed out.
        //if (chase)
        //{
        //    agent.destination = playerRef.transform.position;
        //}
    }
}
