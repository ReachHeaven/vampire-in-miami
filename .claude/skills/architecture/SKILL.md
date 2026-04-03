---
name: architecture
description: Help with architecture and design decisions. Use when developer asks where to put code, how to structure a system, whether to use a pattern, or when planning a new game's structure.
---

# Architecture Decisions

Help make good structural decisions for indie game projects.

## Decision Framework

For every architecture question, evaluate:

1. **Project size** — is this a 2-week game or a 6-month game?
2. **Reuse** — will this be used in future games? (Foundation/ vs Game/)
3. **Complexity budget** — does the benefit justify the abstraction?
4. **Current patterns** — does the codebase already have a pattern for this?
5. **Pain test** — is there actual pain (duplication, bugs) or hypothetical future pain?

## Scale Guide

```
Micro game (2-4 weeks, like Evolve 2048):
  - GameManager does most coordination
  - Events for cross-system communication
  - No ServiceLocator, no StateMachine (unless 3+ complex states)
  - Direct references OK for same-object components

Small game (1-3 months, like survival game):
  - ServiceLocator for 3+ services (audio, pools, save)
  - Events everywhere for cross-system
  - StateMachine for game states with complex Enter/Exit logic
  - Component pattern: PlayerMovement, PlayerHealth, PlayerWeapon (not Player.cs god-class)
  - ObjectPool mandatory for spawning
  - SO configs for all tunable data

Medium game (3-6 months):
  - Full service architecture with interfaces
  - Consider assembly definitions
  - Save system with interface
  - Analytics service
  - Multiple scenes with loading
```

## Learned Decisions from Evolve 2048

- StateMachine was overkill for 3 simple screens → events + SetActive sufficient
- BoardView direct reference was refactored to BoardData SO + event → cleaner but more work
- One universal BreathAnimation component > separate scripts per object
- Graphic type instead of Image → works on both Image and TMP_Text
- ShapeGenerator (Editor script) > manual pixel drawing for geometric shapes
- Listeners on inactive GameObjects = silent failure → always put on active objects

## Response Format

```
## Decision: [question]

**For this project size:** [recommendation]

**Options:**
1. [Simple approach] — [when it's enough]
2. [Medium approach] — [when you need it]
3. [Full approach] — [when project justifies it]

**My recommendation:** [which one and why]

**Warning signs to upgrade:** [when simple becomes not enough]
```

## Anti-Patterns to Flag
- ServiceLocator in a 3-class project → overengineering
- Direct references in a 20-class project → underengineering
- God class GameManager with 500+ lines → time to split
- "We might need it later" → YAGNI, add when you need it
- ECS for 30 enemies → wrong scale
- StateMachine for show/hide panels → events are enough
