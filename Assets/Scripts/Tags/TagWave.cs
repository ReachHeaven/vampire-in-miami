using System;
using System.Collections.Generic;

[Serializable]
public class TagWaveEnemy : EntityComponentDefinition
{
    public CMSEntityPfb EnemyPfb;
    public int Count;
    public int SpawnInterval;
}

[Serializable]
public class TagWave : EntityComponentDefinition
{
    public List<TagWaveEnemy> Entries = new();
    public int Order;
}