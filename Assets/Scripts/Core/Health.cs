using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {

        [SerializeField] float HP = 100f;
        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            HP = Mathf.Max(HP - damage, 0);
            print(HP);
            if (HP == 0) { Death(); }
        }

        void Death()
        {
            if (isDead) { return; }

            GetComponent<Animator>().SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;


        }



    }
}