using System;

[Serializable]
public class TagExperience : EntityComponentDefinition
{
    public int ExperienceToNextLevel = 100;
    public int MaxLevel = 20;
}