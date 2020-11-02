using UnityEngine;
using System.Collections;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {

        float HP = -1f;
        bool isDead = false;

        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if (HP < 0)
            {
                HP = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        private void RegenerateHealth()
        {
            HP = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + "took damage" + damage);

            HP = Mathf.Max(HP - damage, 0);
            print(HP);
            if (HP == 0) 
            {
                Death();
                GainExpPoints(instigator);
            }
        }

        private void GainExpPoints(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.gainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperinceReward));
        }

        public float getPercentage()
        {
            return 100 * (HP / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public float getHP()
        {
            return HP;
        }

        public float getMaxHP()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void Death()
        {
            if (isDead) { return; }

            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;


        }

        public object CaptureState()
        {
            return HP;
        }

        public void RestoreState(object state)
        {
            HP = (float)state;
            if (HP == 0)
            {
                Death();
            }
        }




    }
}