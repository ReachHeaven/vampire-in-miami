# Current Project: Untitled Survival Game

## Overview
Top-down arena survival game. Vampire Survivors meets Hotline Miami.
Move, shoot, survive waves of enemies. Start minimal, grow features organically.

## Status: Week 0 (Setup)
- [ ] New Unity project (2D URP)
- [ ] Copy Foundation/ from Evolve 2048
- [ ] Git repo + CI/CD setup
- [ ] Player moves (white square + Rigidbody2D)

## Roadmap

### Week 1: Moving Square
- Player GameObject: SpriteRenderer + Rigidbody2D + Collider2D
- PlayerMovement.cs: WASD via new Input System, MovePosition in FixedUpdate
- PlayerConfig.asset (SO): speed
- Camera follows player
- Walls (static Collider2D)

### Week 2: Square Shoots
- Click → bullet flies toward cursor
- Bullet.cs: moves forward, dies on collision or after lifetime
- WeaponConfig.asset (SO): damage, bullet speed, cooldown
- PlayerWeapon.cs: cooldown, spawn bullet

### Week 3: Enemy Appears
- Red square walks toward player
- EnemyMovement.cs: Vector2.MoveTowards player
- EnemyHealth.cs: TakeDamage(), Die()
- EnemyConfig.asset (SO): speed, hp, damage
- Bullet hits enemy → damage → death
- Enemy hits player → player takes damage
- PlayerHealth.cs: HP, TakeDamage(), Die()

### Week 4: Waves
- WaveConfig.asset (SO): enemy types, count, spawn delay
- WaveSpawner.cs: spawns enemies by config, coroutine with delays
- GameManager: score, wave tracking, game over on player death
- HUD: HP bar, wave number, score, kill count

### Week 5: Second Enemy + ObjectPool
- Fast enemy (Runner): high speed, low HP
- ObjectPool: reuse enemies and bullets instead of Instantiate/Destroy
- ServiceLocator: register IPoolService, IAudioService
- SFX: shoot, enemy death, player hit, wave start

### Week 6: Polish + Release
- Sprites instead of squares
- Screen shake on hit (CameraService)
- Particles on death
- Menu, Game Over, Restart
- itch.io + Google Play

## Architecture Plan

### Event Flow (expected)
```
InputHandler → PlayerMovement.Move()
InputHandler → PlayerWeapon.Shoot()
PlayerWeapon → Instantiate(Bullet) → Bullet flies
Bullet + Enemy → OnTriggerEnter2D → EnemyHealth.TakeDamage()
EnemyHealth.Die() → OnEnemyDied.Raise()
GameManager listens OnEnemyDied → score++, check wave end
Enemy + Player → OnTriggerEnter2D → PlayerHealth.TakeDamage()
PlayerHealth.Die() → OnPlayerDied.Raise() → GameOver
WaveSpawner listens OnWaveStart → spawn enemies by config
```

### Services (add when needed, not upfront)
- IAudioService — when sound needed from 3+ places
- IPoolService — when enemies/bullets spawn/die frequently
- ISaveService — when saving progress between sessions
- ICameraService — when screen shake needed from multiple sources

### Key Configs (ScriptableObjects)
- PlayerConfig: speed, maxHp, invincibility frames
- WeaponConfig: damage, bulletSpeed, cooldown, bulletLifetime
- EnemyConfig: speed, maxHp, damage, sprite (one per enemy TYPE)
- WaveConfig: list of (enemyConfig, count, spawnDelay)

### Physics Setup
- Player: Rigidbody2D (Dynamic, Gravity 0, Freeze Rotation Z), BoxCollider2D
- Enemy: Rigidbody2D (Dynamic, Gravity 0, Freeze Rotation Z), CircleCollider2D
- Bullet: Rigidbody2D (Dynamic, Gravity 0), CircleCollider2D (IsTrigger)
- Walls: BoxCollider2D (static, no Rigidbody)
- Collision Detection: Continuous on bullets (fast moving)

## Parking Lot
- Weapon pickups / weapon switching
- Player abilities (dash, shield, slow-mo)
- Boss enemies
- Procedural arena generation
- Leaderboard
- Progression between runs (roguelike meta)

## Art Direction
- TBD — start with colored squares, decide style after prototype works
- Options: pixel art (Hotline Miami), vector/minimal, flat cartoon
