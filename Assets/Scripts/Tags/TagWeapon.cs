using System;

[Serializable]
public class TagWeapon : EntityComponentDefinition
{
    public int Damage;
    public int Speed;
    public int Range;
    public CMSEntity bullet;
}