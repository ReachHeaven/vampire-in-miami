---
name: review
description: Review C# code against project architecture and code style. Use when asked to review, check, audit, or validate code quality.
---

# Code Review

Review changed or specified C# files against CLAUDE.md standards.

## Process

1. Read each file completely
2. Check architecture rules (Foundation/Game separation, event-driven communication, pure logic classes)
3. Check code style (naming, formatting, XML docs)
4. Check Unity rules (no Find, no Camera.main loops, proper null checks, listeners on active objects)
5. Check performance (no Update allocations, collection capacity)
6. Check common traps: coroutines on inactive objects, StopCoroutine(nameof) mismatch, GetComponent vs SerializeField

## Report Per File

```
## filename.cs

### Good
[What's done right — be specific]

### Issues
- [ERROR] line X: what + why + fix
- [WARNING] line X: what + why
- [SUGGESTION] line X: what + benefit

Clean file: ✅ filename.cs — clean
```

## Summary

```
Total: X errors, Y warnings, Z suggestions
Priority: [most critical fix]
Learn: [one concept worth studying]
```

## Severity Guide
- **ERROR**: breaks architecture, will cause runtime bugs, violates core principles
- **WARNING**: code style violations, performance risks, maintainability issues
- **SUGGESTION**: improvements that aren't wrong but could be better

## Common Issues to Flag
- Public fields instead of [SerializeField] private
- Missing null guard for value 0 / default in SO lookups
- Direct references where events should be used (and vice versa — events where direct is fine)
- Debug.Log left in production code
- Coroutine started on potentially inactive GameObject
- Image.color used as tint on sprites (white = normal, black = invisible)
- StopCoroutine(nameof) when started with StartCoroutine(Method())
