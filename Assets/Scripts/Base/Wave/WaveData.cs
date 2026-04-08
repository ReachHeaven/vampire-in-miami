using System;
using System.Collections.Generic;
using Base.Enemy;
using UnityEngine;

namespace Base.Wave
{
    public class WaveData : ScriptableObject
    {
        public List<WaveEnemy> waveEnemies;
    }

    [Serializable]
    public class WaveEnemy
    {
        public EnemyData Enemy;
        public int count;
        public int spawnInterval;
    }
}