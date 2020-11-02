
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] Transform target;
        [SerializeField] float MaxSpeed = 10f;


        NavMeshAgent navMeshAgent;
        Health health;

        // Start is called before the first frame update

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
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

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {

            MoveTo(destination, speedFraction);
            GetComponent<ActionScheduler>().StartAction(this);
        }

        private void updateAnimation()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("Blend", speed);

        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {

            navMeshAgent.destination = destination;
            navMeshAgent.speed = MaxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 postion = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = postion.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
