using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetModifiers(Stat stat);

        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}