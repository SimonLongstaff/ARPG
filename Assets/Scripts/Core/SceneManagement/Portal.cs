using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            CastleTownEast, CastleTownWest, BigEast, BigWest
        }


        [SerializeField] int sceneToLoad = 0;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaittime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player"){
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition(){

            if(sceneToLoad < 0){
                Debug.Log("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            SavingWarpper saveWrapper = FindObjectOfType<SavingWarpper>();

            saveWrapper.Save();
            
                   
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            saveWrapper.Load();


            Portal otherPortal = GetOtherPortal();
           UpdatePlayer(otherPortal);


            yield return new WaitForSeconds(fadeWaittime);
            saveWrapper.Save();
            yield return fader.FadeIn(fadeInTime);


           print("Scene loaded");
           Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        }

        private Portal GetOtherPortal()
        {
           foreach(Portal portal in FindObjectsOfType<Portal>()){
               if (portal == this) continue;
               if (portal.destination != destination) continue;

               return portal;
           }

           return null;
        }
    }
}