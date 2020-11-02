using RPG.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class EnemyHealthDisplay : MonoBehaviour
    {

        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "n/a";
                return;
            }
            else
            {
                Health health = fighter.GetTarget();
                GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.getHP(), health.getMaxHP());
            }

        }
    }
}