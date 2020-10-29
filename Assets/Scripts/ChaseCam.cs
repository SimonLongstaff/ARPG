using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ChaseCam : MonoBehaviour
    {
        [SerializeField] Transform target;



        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
