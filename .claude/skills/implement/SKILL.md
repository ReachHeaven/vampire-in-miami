---
name: implement
description: Guide through implementing a new feature step by step. Use when developer wants to add a system, feature, screen, or component. Breaks work into steps, teaches each concept.
---

# Guided Implementation

Walk developer through building a feature step-by-step.

## Process

1. **Clarify** — what does the feature do in one sentence?
2. **Scope check** — is this MVP or can it wait? Check CURRENT.md.
3. **Plan** — break into 2-4 steps, show the plan, get approval
4. **Architecture** — where does this live? Foundation/ or Game/? Which patterns apply? Which existing systems does it touch?
5. **Per step:**
   - Explain concept
   - Name file + namespace
   - Developer writes first
   - Review together
6. **Wire up** — connect via events or Inspector
7. **Test** — suggest verification approach

## Rules
- Max 1 new file per step
- Always: Foundation/ or Game/?
- Always: event or direct reference?
- Always: pure C# possible or needs MonoBehaviour?
- Follow existing patterns in codebase (BreathAnimation pattern for animations, TileConfig pattern for SO configs)
- Never write more than 30 lines without developer input
- If feature is too big for one session, define clear stopping point
- Reference how similar problems were solved in Evolve 2048
