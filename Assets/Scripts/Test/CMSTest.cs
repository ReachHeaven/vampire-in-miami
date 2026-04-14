using System;
using Foundation.CMS;
using Tags;
using UnityEngine;

namespace Test
{
    public class CMSTest : CMSEntity
    {
        public CMSTest()
        {
            var health = Define<TagHealth>();
            health.Max = 200;
            health.Current = 100;
        }

        public static void RunCmsTests()
        {
            Debug.Log("=== CMS TEST START ===");

            try
            {
                var player = CMS.Get("Player");
                Debug.Log($"✓ Get('Player') → id={player.id}, components={player.components.Count}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"✗ Get('Player') failed: {ex.Message}");
            }

            // Test 2: Get non-existent
            try
            {
                CMS.Get("NonExistent");
                Debug.LogError("✗ Get('NonExistent') should have thrown");
            }
            catch (InvalidOperationException)
            {
                Debug.Log("✓ Get('NonExistent') correctly threw exception");
            }

            // Test 3: Query<T1> — single tag
            int healthCount = 0;
            foreach (var health in CMS.Query<TagHealth>())
            {
                healthCount++;
                Debug.Log($"  TagHealth: Max={health.Max}, Current={health.Current}");
            }

            Debug.Log($"✓ Query<TagHealth> → {healthCount} entities");

            // Test 4: Query<T1, T2> — two tags, filters out turret
            int livingCount = 0;
            foreach (var (health, speed) in CMS.Query<TagHealth, TagSpeed>())
            {
                livingCount++;
                Debug.Log($"  Living: hp={health.Current}/{health.Max}, speed={speed.Value}");
            }

            Debug.Log($"✓ Query<TagHealth, TagSpeed> → {livingCount} entities (turret excluded)");

            // Test 5: Query<T1> for damage — only turret should match
            int damagingCount = 0;
            foreach (var damage in CMS.Query<TagDamage>())
            {
                damagingCount++;
                Debug.Log($"  TagDamage: Value={damage.Value}");
            }

            Debug.Log($"✓ Query<TagDamage> → {damagingCount} entities");

            // Test 6: QueryWithEntity<T1> — sanity check entity id
            foreach (var (health, entity) in CMS.QueryWithEntity<TagHealth>())
            {
                Debug.Log($"  {entity.id} has health {health.Current}");
            }

            Debug.Log($"✓ QueryWithEntity<TagHealth> iterated");

            // Test 7: QueryWithEntity<T1, T2> — two tags + entity
            foreach (var (health, speed, entity) in CMS.QueryWithEntity<TagHealth, TagSpeed>())
            {
                Debug.Log($"  {entity.id}: hp={health.Current}, speed={speed.Value}");
            }

            Debug.Log($"✓ QueryWithEntity<TagHealth, TagSpeed> iterated");

            // Test 8: Optional tags via entity.Is<T>
            foreach (var (health, entity) in CMS.QueryWithEntity<TagHealth>())
            {
                var speedInfo = entity.Is<TagSpeed>(out var speed) ? $"{speed.Value}" : "none";
                var damageInfo = entity.Is<TagDamage>(out var damage) ? $"{damage.Value}" : "none";
                Debug.Log($"  {entity.id}: hp={health.Current}, speed={speedInfo}, damage={damageInfo}");
            }

            Debug.Log($"✓ Optional tag check via Is<T>");

            // Test 9: QuerySingle — first with tag
            var firstWithHealth = CMS.QuerySingle<TagHealth>();
            if (firstWithHealth != null)
                Debug.Log($"✓ QuerySingle<TagHealth> → {firstWithHealth.Max}");
            else
                Debug.LogError("✗ QuerySingle<TagHealth> returned null");

            // Test 10: QuerySingle for non-existent tag
            var missing = CMS.QuerySingle<TagPosition>(); // замени на любой тег которого нет в префабах
            Debug.Log($"✓ QuerySingle<TagWave> (no entities have it) → {(missing == null ? "null" : "unexpected")}");

            // Test 11: Player.Get<T> через CMS vs прямое чтение из entity
            var playerEntity = CMS.Get("Player");
            var hpFromEntity = playerEntity.Get<TagHealth>();
            Debug.Log($"✓ Player entity.Get<TagHealth> → {hpFromEntity.Current}/{hpFromEntity.Max}");

            // Test 12: Is<T> and Is<T>(out)
            var isLiving = playerEntity.Is<TagSpeed>();
            var hasSpeed = playerEntity.Is<TagSpeed>(out var playerSpeed);
            Debug.Log($"✓ Player.Is<TagSpeed> → {isLiving}, with out → {hasSpeed}, value={playerSpeed?.Value}");

            Debug.Log("=== CMS TEST END ===");
        }
    }
}