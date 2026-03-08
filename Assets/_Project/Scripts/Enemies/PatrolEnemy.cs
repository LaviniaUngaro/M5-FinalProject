using System.Collections;
using UnityEngine;

public class PatrolEnemy : EnemiesFSM
{
    [Header("Patrol Attributes")]
    [SerializeField] private float _timeAtWaypoint = 4;
    [SerializeField] private Transform[] _patrolWaypoints;

    protected override void OnEnterPatrol()
    {
        Debug.Log("Enter Patrol");
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        int i = 0;
        while (true)
        {
            Agent.SetDestination(_patrolWaypoints[i].position);
            
            WaitUntil waitUntil = new WaitUntil(() => !Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance);
            yield return waitUntil; //aspetto che ritorni al waypoint
            
            WaitForSeconds waitForSeconds = new WaitForSeconds (_timeAtWaypoint);
            yield return waitForSeconds; // aspetto tot secondi

            i++;
            if (i >= _patrolWaypoints.Length)
            {
                i = 0;
            }
        }
    }

    protected override void OnExitPatrol()
    {
        StopCoroutine(PatrolRoutine());
    }
}
