using System;

[Serializable]
public class TagExperience : EntityComponentDefinition
{
    public int Level = 0;
    public int Experience = 0;
    public int ExperienceToNextLevel = 100;
}