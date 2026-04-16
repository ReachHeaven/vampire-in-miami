using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightSystem : MonoBehaviour, IService
{
    private Camera _camera;
    private Dictionary<CMSEntity, float> _lastShotTime = new();

    public void Init()
    {
        _camera = Camera.main;
    }

    public void Shoot(CMSEntity attacker, MonoBehaviour source)
    {
        if (!attacker.Is<TagWeapon>(out var weapon)) return;
        if (!weapon.bullet) return;
        if (!CheckCooldown(attacker, weapon.Cooldown)) return;

        Vector2 shooterPos = source.transform.position;
        var nearestEnemy = G.WaveSystem.FindNearestEnemy(shooterPos, weapon.Range);
        Vector2 targetPos = nearestEnemy != null
            ? (Vector2)nearestEnemy.transform.position
            : GetMouseWorldPosition();
        Vector2 direction = (targetPos - shooterPos).normalized;

        Debug.Log($"1. bulletPrefab id: {weapon.bullet.GetId()}");
    
        var bulletEntity = weapon.bullet.AsEntity();
        Debug.Log($"2. bulletEntity: {bulletEntity != null}");
    
        var bulletStats = bulletEntity?.Get<TagBullet>();
        Debug.Log($"3. bulletStats: {bulletStats != null}");
    
        var bulletGO = Instantiate(weapon.bullet.gameObject, shooterPos, Quaternion.identity);
        var bullet = bulletGO.GetComponent<Bullet>();
        Debug.Log($"4. bullet MB: {bullet != null}");
        
        bullet.Init(direction, weapon.Damage + bulletStats.Damage, bulletStats.Speed);
    }

    private bool CheckCooldown(CMSEntity attacker, float cooldown)
    {
        float now = Time.time;
        if (_lastShotTime.TryGetValue(attacker, out float lastTime))
        {
            if (now - lastTime < cooldown) return false;
        }
        _lastShotTime[attacker] = now;
        return true;
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        return _camera.ScreenToWorldPoint(mouseScreen);
    }
}