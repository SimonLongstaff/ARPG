using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Health target;
        [SerializeField] float WeaponRange = 2f;
        [SerializeField] float attackSpeed = 1f;
        [SerializeField] float WeaponDamage = 10f;

        float timeSinceLastattack = Mathf.Infinity;

        private void Update()
        {
            timeSinceLastattack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) { return; }

           if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();                
                AttackBehaviour();
                
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastattack >= attackSpeed)
            {
                //Triggers Hit() 
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastattack = 0;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < WeaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("cancelAttack");
            target = null;
        }

        //Animation Event
        void Hit()
        {
            if (target == null){ return; }
            Health targetHealth = target.GetComponent<Health>();
            targetHealth.TakeDamage(WeaponDamage);
           
        }

        public bool CanAttack(GameObject target)
        {
            if(target == null) { return false; }

            Health targetHealth = GetComponent<Health>();
            return targetHealth != null & !targetHealth.IsDead();
        }

      

    }
}