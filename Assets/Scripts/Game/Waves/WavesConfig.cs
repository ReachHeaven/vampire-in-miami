using System;
using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;

namespace Game.Waves
{
    public class WavesConfig : ScriptableObject
    {
        [SerializeField] private List<WaveEntry> entries = new();
        public IReadOnlyList<WaveEntry> Entries => entries;
        
        [Serializable]
        public class WaveEntry
        {
            [SerializeField] private EnemyAdapter enemyConfig;
            public EnemyAdapter EnemyConfig => enemyConfig;
            [SerializeField] private int count = 1;
            public int Count => count;
        }
    }
}