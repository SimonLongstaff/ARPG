using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        Health target;
        Weapon currentWeapon;
        [SerializeField] float attackSpeed = 1f;
       
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
       

        float timeSinceLastattack = Mathf.Infinity;

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }


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

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
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
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

        //Animation Event
        void Hit()
        {
            if (target == null){ return; }
           

          
            else
            {
                target.TakeDamage(gameObject, GetComponent<BaseStats>().GetStat(Stat.Damage));
            }
            
           
        }

        void Shoot()
        {
            currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, GetComponent<BaseStats>().GetStat(Stat.Damage));
        }

        public bool CanAttack(GameObject target)
        {
            if(target == null) { return false; }

            Health targetHealth = GetComponent<Health>();
            return targetHealth != null & !targetHealth.IsDead();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            String weaponname = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponname);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }
    }
}