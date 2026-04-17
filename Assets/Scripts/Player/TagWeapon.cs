using System;

[Serializable]
public class TagWeapon : EntityComponentDefinition
{
    public int Damage;
    public float BulletSpeed;
    public int Cooldown;
    public int Range;
    public CMSEntityPfb bullet;
}