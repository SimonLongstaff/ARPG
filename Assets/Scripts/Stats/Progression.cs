using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "ARPG/Progression", order = 0)]
public class Progression : ScriptableObject {

    [SerializeField] ProgressionCharacterClass[] characterClasses = null;

    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

    public float getStat(Stat stat, CharacterClass characterClass, int level)
    {
        BuildLookUp();

        float[] levels = lookupTable[characterClass][stat];
        if(levels.Length < level)
        {
            return 0;
        }

        return levels[level-1];
    }

    public int GetLevels(Stat stat, CharacterClass characterClass)
    {
        BuildLookUp();
        float[] levels = lookupTable[characterClass][stat];
        return levels.Length;
    }

    private void BuildLookUp()
    {
        if (lookupTable != null) return;

        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

        foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
        {
            var statLookupTable = new Dictionary<Stat, float[]>();

            foreach(ProgressionStat stat in progressionCharacterClass.stats)
            {
                statLookupTable[stat.stat] = stat.levels;
            }

            lookupTable[progressionCharacterClass.characterClass] = statLookupTable;
        }
    }

    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
    }

    [System.Serializable]
    class ProgressionStat
    {
        public Stat stat;
        public float[] levels;
    }

}