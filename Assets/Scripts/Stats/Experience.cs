using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experincePoints = 0f;

        public event Action onExperinceGained;

        public object CaptureState()
        {
            return experincePoints;
        }

        public void gainExperience(float experinceGained)
        {
            experincePoints += experinceGained;
            onExperinceGained();
        }

        public void RestoreState(object state)
        {
            experincePoints = (float)state;

        }

        public float GetXP()
        {
            return experincePoints;
        }
    }
}