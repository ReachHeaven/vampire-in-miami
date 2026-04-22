using System;

[Serializable]
public class TagWeapon : EntityComponentDefinition
{
    public int Damage;
    public float BulletSpeed;
    public float Cooldown;
    public int Range;
    public CMSEntityPfb bullet;
}