using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weapon = null;
    [SerializeField] float respawnTime = 5f;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player"){

            other.GetComponent<Fighter>().EquipWeapon(weapon);

            StartCoroutine(HideForSeconds(respawnTime));
        

        }
    }

    private IEnumerator HideForSeconds(float seconds)
    {
        PickupVisability(false);
        yield return new WaitForSeconds(seconds);
        PickupVisability(true);
    }

    private void PickupVisability(bool shouldShow)
    {
        gameObject.GetComponent<Collider>().enabled = shouldShow;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldShow);
        }
    }
}
