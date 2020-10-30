
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {

        [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;
        Health health;

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        // Update is called once per frame
        void Update()
        {
            updateAnimation();
            navMeshAgent.enabled = !health.IsDead();
        }

        public void StartMoveAction(Vector3 destination)
        {

            MoveTo(destination);
            GetComponent<ActionScheduler>().StartAction(this);
        }

        private void updateAnimation()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("Blend", speed);

        }

        public void MoveTo(Vector3 destination)
        {

            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }



    }
}
