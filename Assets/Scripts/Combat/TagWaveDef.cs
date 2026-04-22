using System;
using System.Collections.Generic;

[Serializable]
public class WaveEntry
{
    public CMSEntityPfb EnemyPfb;
    public int Count;
    public int SpawnInterval;
}

[Serializable]
public class WaveDefinition
{
    public int Order;
    public List<WaveEntry> Entries = new();
}

[Serializable]
public class TagAllWaves : EntityComponentDefinition
{
    public List<WaveDefinition> Waves = new();
}