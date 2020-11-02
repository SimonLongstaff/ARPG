using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float weaponPercentageBonus = 0f;
        [SerializeField] float WeaponRange = 2f;
        [SerializeField] GameObject equppiedWeapon = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if(equppiedWeapon != null)
            {
                Transform handTransform;
                handTransform = GetHandTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equppiedWeapon, handTransform);
                weapon.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (weaponOverride != null){
                animator.runtimeAnimatorController = weaponOverride;
            }
            else if (overrideController != null)
                {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
        

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTORYING";
            Destroy(oldWeapon.gameObject);
        }

        public float GetPercentageBonus()
        {
            return weaponPercentageBonus;
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjeile(){
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float CalculatedDamage){
            
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand,leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(instigator,target, CalculatedDamage);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetRange()
        {
            return WeaponRange;
        }


    }
}