using Base;
using UnityEngine;

namespace Player
{
    public class WeaponState : ObjectState
    {
        public int Damage;
        public float BulletSpeed;
        public int Cooldown;
        public int Range;
        public GameObject BulletPfb;

        public WeaponState(CMSEntity model)
        {
            Model = model;
            var w = model.Get<TagWeapon>();
            Damage = w.Damage;
            BulletSpeed = w.BulletSpeed;
            Cooldown = w.Cooldown;
            Range = w.Range;
            BulletPfb = w.bullet ? w.bullet.gameObject : null;
        }
    }
}
