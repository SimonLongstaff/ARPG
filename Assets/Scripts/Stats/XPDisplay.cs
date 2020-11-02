using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.Resources
{
    public class XPDisplay : MonoBehaviour
    {
        Experience experience;


        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        // Update is called once per frame
        void Update()
        {

            GetComponent<Text>().text = String.Format("{0:0}", experience.GetXP());
        }
    }
}