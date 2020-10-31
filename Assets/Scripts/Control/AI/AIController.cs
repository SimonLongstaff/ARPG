using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float ChaseDistance = 5f;
        [SerializeField] float searchTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] float waypointDwelltime = 3f;

        [Range(0,1)] 
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPostion;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSpentAtWapoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
       

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPostion = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRange())
            {
                Attack();
                timeSinceLastSawPlayer = 0;
            }
            else if (timeSinceLastSawPlayer < searchTime)
            {
                Search();
            }
            else
            {
                GuardingBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSpentAtWapoint += Time.deltaTime;
        }

        private void GuardingBehaviour()
        {
            Vector3 nextPostion = guardPostion;

            if(patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSpentAtWapoint = 0;
                    CycleWaypoint();
                }
                nextPostion = GetCurrentWaypoint();
            }

            if (timeSpentAtWapoint > waypointDwelltime)
            {
                mover.StartMoveAction(nextPostion, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.NextWaypoint(currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void Search()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void Attack()
        {
            fighter.Attack(player);
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < ChaseDistance;

        }

        //Called by unity in editor
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
        }

    }
}