# Indie Game Developer Workspace

## Who I Am
Solo indie developer building a portfolio of small games to ship, validate with market, and grow.
Backend/DevOps background. One shipped game (Evolve 2048). Learning Unity + C# + game design.
Based in Serbia (Niš). Language: Russian. Respond in Russian unless writing code or docs.

## Strategy
- Year 1: ship 12 small games, study metrics, find market fit
- Year 2: deeper projects with reputation and skill
- Long term: make something meaningful, validated by market first

## Current Project
Check .claude/CURRENT.md for game-specific details, GDD, and status.

## Shipped Games
1. Evolve 2048 — geometric puzzle, Tron aesthetic, itch.io

## Architecture (Universal — all projects)

### Principles
1. Foundation/ depends on nothing. Game code depends on Foundation.
2. Systems communicate via ScriptableObject Events, not direct references.
3. Core logic classes are pure C# when possible — no UnityEngine dependency.
4. Manager classes bridge pure logic and Unity.
5. No singletons. ServiceLocator when 3+ services exist.
6. Scenes minimal. Dynamic objects from prefabs.
7. Data-logic separation: ScriptableObject = data, MonoBehaviour = behavior, Event = channel.
8. Don't over-engineer. Add patterns when pain appears, not in advance.
9. Rule of three: same code in 3 places → extract to service/utility.
10. Each component = one responsibility. No god-classes (Player.cs with 500 lines).

### Folder Convention
```
Assets/
  Foundation/           — reusable across ALL games (copy between projects)
    Events/             — GameEvent, GameEventListener, typed variants
    Services/           — ServiceLocator, interfaces (IAudioService, IPoolService, etc.)
    Patterns/           — StateMachine, IState, ObjectPool
    Utils/              — helpers, extensions
  Game/                 — game-specific code
    Core/               — bootstrap, managers, input, configs
    Player/             — player components (Movement, Health, Weapon)
    Enemies/            — enemy components + configs
    Weapons/            — bullet, weapon configs
    Waves/              — wave spawning system
    Services/           — service implementations (AudioService, PoolService, etc.)
    UI/                 — views, screens, animations
    Data/               — ScriptableObject data classes
    Editor/             — editor-only tools
  Art/
    Sprites/
    Audio/
    Fonts/
  Prefabs/
  ScriptableObjects/
    Configs/
    Events/
    Waves/
  Scenes/
```

### Event Pattern
```
[Producer] → event.Raise() → [ScriptableObject asset] → [GameEventListener on Consumer]
```
Nobody knows each other. Everyone knows the event asset.
Use events when: result of action interests someone outside current class.
Use direct calls when: internal to one system (e.g., Rigidbody2D.MovePosition).

### Service Pattern (when project needs 3+ services)
```
[Bootstrap] → ServiceLocator.Register<IService>(impl)
[Anywhere]  → ServiceLocator.Get<IService>().DoThing()
```
Foundation holds interfaces. Game holds implementations.
Services appear organically: same code in 3 places → extract to service.
Typical services: IAudioService, IPoolService, ISaveService, ICameraService.

### State Pattern
```
StateMachine → AddState(new MenuState())
             → SwitchTo<PlayingState>()
Each state: Enter() → Update() per frame → Exit() on switch
```
Use when: complex states with logic in Update/Enter/Exit (pause stops time, gameplay enables input).
Don't use for simple panel show/hide — events are enough.

### Component Pattern (action games)
```
GameObject "Player"
├── PlayerMovement.cs      ← input + physics
├── PlayerHealth.cs        ← HP, TakeDamage(), Die()
├── PlayerWeapon.cs        ← shooting, cooldown
├── Rigidbody2D            ← Unity physics
├── Collider2D             ← collisions
└── SpriteRenderer         ← visual
```
One responsibility per component. Config in SO, not hardcoded.

## Code Style

### Naming
| Element | Convention | Example |
|---------|-----------|---------|
| Private field | _camelCase | _moveSpeed |
| Public property | PascalCase | MoveSpeed |
| Constant | PascalCase | MaxHealth |
| Interface | IPascalCase | IAudioService |
| Method | PascalCase | TakeDamage() |
| Parameter/local | camelCase | damageAmount |

### Rules
- NEVER public fields — [SerializeField] private + property if needed
- readonly when possible
- One class per file, filename = class name
- Allman braces (new line)
- var only when type obvious from right side
- XML docs on all public members
- Usings sorted, System.* first
- New Input System (not legacy Input class)

### Unity-Specific
- No Find/FindObjectOfType in runtime
- No Camera.main in loops (cache it)
- No string tag comparisons
- Prefer TryGetComponent over GetComponent
- Null-check UnityEngine.Object with == null (not ?.)
- Unsubscribe events in OnDisable/OnDestroy
- Cache WaitForSeconds in coroutines
- Listeners must live on active GameObjects (inactive = unsubscribed)
- Coroutines cannot start on inactive GameObjects
- Rigidbody2D movement in FixedUpdate via MovePosition (not transform.position)

### Performance
- No allocations in Update (no new, no LINQ, no string concat)
- Initialize collections with capacity when size known
- Object pooling for frequent spawn/destroy (enemies, bullets)
- Profile on target device, not Editor
- Mobile: QualitySettings.vSyncCount = 0, targetFrameRate = 60
- PC/WebGL: leave default (VSync handles it)
- Use #if UNITY_ANDROID || UNITY_IOS for platform-specific code

### Art Pipeline
- Vector/wireframe for geometric styles, pixel art for retro styles
- Photopea (browser) for manual drawing
- Editor scripts for procedural generation
- Sprite settings: Filter Mode Bilinear (vector) or Point (pixel), Compression None
- Pixels Per Unit matches sprite size

### Audio
- Two AudioSources: Music (loop, quiet) + SFX (one-shot, loud)
- sfxr.me for generating retro SFX
- freesound.org / itch.io for music and samples
- Fade music volume on state transitions

### CI/CD
- GameCI on GitHub Actions
- Parallel WebGL + Android builds
- Keystore stored as base64 in GitHub Secrets
- WebGL → itch.io, Android AAB → Google Play

## Lessons Learned (from Evolve 2048)
- StateMachine overkill for simple panel show/hide — events + SetActive enough
- Listeners on inactive GameObjects = silent failure
- Image.color is a tint multiplier (white = normal, black = invisible)
- StopCoroutine(nameof) doesn't work with StartCoroutine(Method())
- Ship to itch.io first, Google Play later
- AdMob rejects without published app — apply after store listing
- Don't over-engineer first, refactor when pain appears
- Geometric shapes easier to generate via code than draw manually
- One universal animation component (BreathAnimation) > many specialized ones

## Developer Context
This developer has shipped one game and is building their second.
They know: events, SO architecture, coroutines, UI, input system, basic animations, CI/CD.
They're learning: physics, Rigidbody2D, collisions, AI movement, ObjectPool, ServiceLocator.
Prioritize teaching over doing. Explain WHY before HOW.
Connect new concepts to what they already built in Evolve 2048.
