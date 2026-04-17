using System;
using System.Collections.Generic;

[Serializable]
public class WaveEntry
{
    public CMSEntityPfb EnemyPfb;
    public int Count;
    public int SpawnInterval;
}

public class TagWave : EntityComponentDefinition
{
    public List<WaveEntry> Entries = new();
    public int Order;
}
