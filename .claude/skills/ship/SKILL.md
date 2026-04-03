---
name: ship
description: Guide through releasing a game. Use when developer asks about building, publishing, store submission, analytics setup, CI/CD, or launch preparation.
---

# Ship It

Guide through the release process for indie games.

## Pre-Release Checklist

### Code
- [ ] Remove all Debug.Log (or wrap in #if UNITY_EDITOR)
- [ ] Remove test scripts (BoardTest, DebugFillAll keybind)
- [ ] #if UNITY_EDITOR around debug-only code
- [ ] Platform-conditional ads: #if UNITY_ANDROID || UNITY_IOS
- [ ] Platform-conditional frame rate: mobile only

### Build
- [ ] Test on target device (not just Editor)
- [ ] Build size acceptable (< 100MB for mobile casual)
- [ ] Resolution/aspect ratio tested on multiple sizes
- [ ] Touch input works on real device
- [ ] Safe area doesn't clip UI

### Quality
- [ ] Full playthrough without crashes
- [ ] Game Over → Restart works cleanly
- [ ] Menu → Play → Game Over → Menu → Play works
- [ ] Score saves between sessions
- [ ] Sound plays correctly (music + SFX)
- [ ] Animations don't break on rapid input
- [ ] No placeholder art/text visible

## Platform Guides

### itch.io (do first, always)
- WebGL build → zip contents (not folder) → upload
- Kind: HTML, SharedArrayBuffer enabled
- Viewport: 540×960 for mobile portrait games
- Free, instant, no review
- Theme page to match game aesthetic

### Google Play
- $25 one-time developer registration
- AAB build (not APK), IL2CPP, ARMv7+ARM64
- Keystore: CREATE ONCE, SAVE FOREVER, BACKUP
- Review: 1-7 days (2-3 typical)
- Required: icon 512×512, screenshots 1080×1920, feature graphic 1024×500, privacy policy URL, content rating questionnaire

### CI/CD (GameCI)
- .github/workflows/build.yml
- Parallel WebGL + Android builds
- Secrets: UNITY_LICENSE, UNITY_EMAIL, UNITY_PASSWORD, keystore
- Artifacts downloadable from Actions tab
- Push to main → builds automatically

## Store Listing Tips
- First 2 lines of description = hook (visible without expanding)
- GIF > screenshots for itch.io
- Category + tags matter for discoverability
- Devlog post on itch.io = free marketing

## Rules
- Always ship to itch.io first (instant, no review)
- Don't let perfectionism block shipping
- "Ship it broken, patch it fast" > "Ship it perfect, ship it never"
- First game = pipeline learning, not revenue
