using UnityEngine;

/// <summary>
/// Math and gameplay utilities for 2D Unity games.
/// Everything that's often missing from stock Mathf / Vector2.
/// </summary>
public static class MathUtil
{
    // ============================================================
    //  CONSTANTS
    // ============================================================

    public const float TAU = 6.2831853f;       // 2π — full rotation in radians
    public const float GOLDEN_RATIO = 1.6180339f;
    public const float SQRT2 = 1.4142136f;


    // ============================================================
    //  ANGLES & VECTORS
    // ============================================================

    /// <summary>Angle of a vector in degrees. (1,0) = 0°, (0,1) = 90°. Range [-180..180].</summary>
    public static float VectorToAngle(Vector2 v)
        => Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;

    /// <summary>Unit vector from an angle in degrees.</summary>
    public static Vector2 AngleToVector(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    /// <summary>Rotate a vector by an angle in degrees (counter-clockwise).</summary>
    public static Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad);
        float s = Mathf.Sin(rad);
        return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
    }

    /// <summary>Perpendicular counter-clockwise (90° left).</summary>
    public static Vector2 PerpCCW(Vector2 v) => new Vector2(-v.y, v.x);

    /// <summary>Perpendicular clockwise (90° right).</summary>
    public static Vector2 PerpCW(Vector2 v) => new Vector2(v.y, -v.x);

    /// <summary>Direction from 'from' to 'to', normalized.</summary>
    public static Vector2 DirTo(Vector2 from, Vector2 to) => (to - from).normalized;

    /// <summary>
    /// Whether the target is on the left or right of forward.
    /// &gt; 0 — left, &lt; 0 — right, == 0 — collinear.
    /// </summary>
    public static float SideOf(Vector2 forward, Vector2 dirToTarget)
        => forward.x * dirToTarget.y - forward.y * dirToTarget.x;

    /// <summary>Angle [0..180] between two vectors.</summary>
    public static float AngleBetween(Vector2 a, Vector2 b) => Vector2.Angle(a, b);


    // ============================================================
    //  ANGLE WRAPPING
    // ============================================================

    /// <summary>Wrap an angle into the [-180..180] range.</summary>
    public static float WrapAngle180(float deg)
    {
        deg %= 360f;
        if (deg > 180f) deg -= 360f;
        else if (deg < -180f) deg += 360f;
        return deg;
    }

    /// <summary>Wrap an angle into the [0..360) range.</summary>
    public static float WrapAngle360(float deg)
    {
        deg %= 360f;
        if (deg < 0f) deg += 360f;
        return deg;
    }

    /// <summary>Shortest signed angle difference, in [-180..180].</summary>
    public static float DeltaAngle(float from, float to) => Mathf.DeltaAngle(from, to);


    // ============================================================
    //  FRAME-RATE INDEPENDENT SMOOTHING
    // ============================================================
    //  Use instead of the "lazy" Lerp(a, b, 0.1f) in Update,
    //  so the speed doesn't depend on framerate.
    //  decay: 1 — very smooth, 5 — medium, 15 — fast.
    // ============================================================

    /// <summary>Lerp factor that is independent of framerate.</summary>
    public static float DampFactor(float decay, float dt)
        => 1f - Mathf.Exp(-decay * dt);

    public static float Damp(float current, float target, float decay, float dt)
        => Mathf.Lerp(current, target, DampFactor(decay, dt));

    public static Vector2 Damp(Vector2 current, Vector2 target, float decay, float dt)
        => Vector2.Lerp(current, target, DampFactor(decay, dt));

    public static Vector3 Damp(Vector3 current, Vector3 target, float decay, float dt)
        => Vector3.Lerp(current, target, DampFactor(decay, dt));

    /// <summary>Smooth angle interpolation that handles the 359→0 wrap-around.</summary>
    public static float DampAngle(float current, float target, float decay, float dt)
        => Mathf.LerpAngle(current, target, DampFactor(decay, dt));


    // ============================================================
    //  REMAP — convert a value from one range into another
    // ============================================================
    //  Example: HP 0..100 to bar fill 0..1
    //  fillAmount = MathUtil.Remap(hp, 0, 100, 0, 1);
    // ============================================================

    public static float Remap(float v, float inMin, float inMax, float outMin, float outMax)
        => outMin + (v - inMin) * (outMax - outMin) / (inMax - inMin);

    /// <summary>Remap with the result clamped to [outMin..outMax].</summary>
    public static float RemapClamped(float v, float inMin, float inMax, float outMin, float outMax)
    {
        float t = Mathf.Clamp01((v - inMin) / (inMax - inMin));
        return outMin + t * (outMax - outMin);
    }


    // ============================================================
    //  SNAP — round to a grid
    // ============================================================

    /// <summary>Round a value to the nearest step (1.7 with step=0.5 → 1.5).</summary>
    public static float Snap(float v, float step) => Mathf.Round(v / step) * step;

    public static Vector2 Snap(Vector2 v, float step)
        => new Vector2(Snap(v.x, step), Snap(v.y, step));

    /// <summary>Pixel-perfect alignment for pixel art (ppu = the sprite's pixels per unit).</summary>
    public static Vector2 PixelSnap(Vector2 v, float pixelsPerUnit)
    {
        float pixel = 1f / pixelsPerUnit;
        return new Vector2(
            Mathf.Round(v.x / pixel) * pixel,
            Mathf.Round(v.y / pixel) * pixel
        );
    }


    // ============================================================
    //  DISTANCES & GEOMETRY
    // ============================================================

    /// <summary>Squared distance (no sqrt). Use for comparisons.</summary>
    public static float SqrDistance(Vector2 a, Vector2 b)
    {
        float dx = a.x - b.x, dy = a.y - b.y;
        return dx * dx + dy * dy;
    }

    /// <summary>Fast "is within radius" check, no sqrt.</summary>
    public static bool InRange(Vector2 a, Vector2 b, float radius)
        => SqrDistance(a, b) <= radius * radius;

    /// <summary>Manhattan distance |dx|+|dy|. For grid-based games.</summary>
    public static float ManhattanDistance(Vector2 a, Vector2 b)
        => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    /// <summary>Chebyshev distance max(|dx|,|dy|). King's move in chess.</summary>
    public static float ChebyshevDistance(Vector2 a, Vector2 b)
        => Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));

    /// <summary>Closest point on segment a—b to point p.</summary>
    public static Vector2 ClosestPointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float lenSqr = ab.sqrMagnitude;
        if (lenSqr < 1e-6f) return a;
        float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / lenSqr);
        return a + ab * t;
    }

    /// <summary>Distance from a point to a segment.</summary>
    public static float DistanceToSegment(Vector2 p, Vector2 a, Vector2 b)
        => Vector2.Distance(p, ClosestPointOnSegment(p, a, b));


    // ============================================================
    //  RANDOMNESS
    // ============================================================

    /// <summary>Random point inside a circle of radius r.</summary>
    public static Vector2 RandomInCircle(float radius)
        => Random.insideUnitCircle * radius;

    /// <summary>Random point strictly on the circle.</summary>
    public static Vector2 RandomOnCircle(float radius)
    {
        float a = Random.value * TAU;
        return new Vector2(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius);
    }

    /// <summary>Random point in a ring [innerRadius..outerRadius] (uniform by area).</summary>
    public static Vector2 RandomInRing(float innerRadius, float outerRadius)
    {
        // sqrt is needed for uniform area distribution, otherwise points cluster near the center
        float r = Mathf.Sqrt(Random.Range(innerRadius * innerRadius, outerRadius * outerRadius));
        float a = Random.value * TAU;
        return new Vector2(Mathf.Cos(a) * r, Mathf.Sin(a) * r);
    }

    /// <summary>Random vector inside a cone around a direction (for bullet/particle spread).</summary>
    public static Vector2 RandomInCone(Vector2 direction, float halfAngleDeg)
        => RotateVector(direction, Random.Range(-halfAngleDeg, halfAngleDeg));

    /// <summary>+1 or -1 with equal probability.</summary>
    public static int RandomSign() => Random.value < 0.5f ? -1 : 1;

    /// <summary>true with the given probability [0..1].</summary>
    public static bool Chance(float probability) => Random.value < probability;

    /// <summary>Weighted index selection (e.g., loot rarity roll).</summary>
    public static int WeightedRandom(float[] weights)
    {
        float total = 0f;
        for (int i = 0; i < weights.Length; i++) total += weights[i];
        float pick = Random.value * total;
        float cum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cum += weights[i];
            if (pick <= cum) return i;
        }
        return weights.Length - 1;
    }


    // ============================================================
    //  AIMING / SHOOTING
    // ============================================================

    /// <summary>
    /// Lead point for shooting at a moving target.
    /// Returns the position to aim at so the bullet meets the target.
    /// If there's no solution (target outruns the projectile) — returns target.
    /// </summary>
    public static Vector2 LeadTarget(Vector2 shooter, Vector2 target, Vector2 targetVelocity, float projectileSpeed)
    {
        Vector2 toTarget = target - shooter;
        float a = targetVelocity.sqrMagnitude - projectileSpeed * projectileSpeed;
        float b = 2f * Vector2.Dot(toTarget, targetVelocity);
        float c = toTarget.sqrMagnitude;

        float t;
        if (Mathf.Abs(a) < 1e-4f)
        {
            // Equal speeds — linear equation
            if (Mathf.Abs(b) < 1e-4f) return target;
            t = -c / b;
        }
        else
        {
            float disc = b * b - 4f * a * c;
            if (disc < 0f) return target; // can't reach
            float sq = Mathf.Sqrt(disc);
            float t1 = (-b - sq) / (2f * a);
            float t2 = (-b + sq) / (2f * a);
            // Take the smallest positive time
            if (t1 > 0f && t2 > 0f) t = Mathf.Min(t1, t2);
            else t = Mathf.Max(t1, t2);
        }

        return t > 0f ? target + targetVelocity * t : target;
    }


    // ============================================================
    //  2D SPRITE LOOK ROTATION
    // ============================================================

    /// <summary>Rotation quaternion for a sprite whose "front" faces right (+X).</summary>
    public static Quaternion LookAt2D(Vector2 from, Vector2 to)
    {
        Vector2 dir = to - from;
        return Quaternion.Euler(0f, 0f, VectorToAngle(dir));
    }

    /// <summary>
    /// Rotation quaternion for a sprite whose "front" faces up (+Y).
    /// Common for top-down games where the character is drawn facing up.
    /// </summary>
    public static Quaternion LookAt2DUp(Vector2 from, Vector2 to)
    {
        Vector2 dir = to - from;
        return Quaternion.Euler(0f, 0f, VectorToAngle(dir) - 90f);
    }


    // ============================================================
    //  EASING — popular animation curves
    // ============================================================
    //  All take t in [0..1] and return a modified t in [0..1].
    //  Usage: float eased = MathUtil.EaseOutQuad(t);
    //         value = Mathf.Lerp(from, to, eased);
    // ============================================================

    public static float EaseInQuad(float t) => t * t;
    public static float EaseOutQuad(float t) => 1f - (1f - t) * (1f - t);

    public static float EaseInOutQuad(float t)
        => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;

    public static float EaseInCubic(float t) => t * t * t;
    public static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);

    /// <summary>Overshoots past 1 and bounces back — nice for UI pop-in.</summary>
    public static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    /// <summary>Bouncing finish — for falling objects.</summary>
    public static float EaseOutBounce(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;
        if (t < 1f / d1) return n1 * t * t;
        if (t < 2f / d1) { t -= 1.5f / d1; return n1 * t * t + 0.75f; }
        if (t < 2.5f / d1) { t -= 2.25f / d1; return n1 * t * t + 0.9375f; }
        t -= 2.625f / d1;
        return n1 * t * t + 0.984375f;
    }


    // ============================================================
    //  LOOPING OSCILLATIONS
    // ============================================================

    /// <summary>Sine wave between min and max with the given speed (rad/s).</summary>
    public static float Pulse(float min, float max, float speed)
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        return Mathf.Lerp(min, max, t);
    }

    /// <summary>Pulse with a custom phase (for several objects out of sync).</summary>
    public static float Pulse(float min, float max, float speed, float phase)
    {
        float t = (Mathf.Sin(Time.time * speed + phase) + 1f) * 0.5f;
        return Mathf.Lerp(min, max, t);
    }

    /// <summary>Triangle wave 0→1→0→1 (PingPong normalized).</summary>
    public static float Triangle01(float speed)
        => Mathf.PingPong(Time.time * speed, 1f);


    // ============================================================
    //  MISC
    // ============================================================

    /// <summary>Sign of a number that never returns 0 (v == 0 → +1).</summary>
    public static int SignNonZero(float v) => v < 0f ? -1 : 1;

    /// <summary>Vector2 comparison with epsilon.</summary>
    public static bool Approximately(Vector2 a, Vector2 b, float epsilon = 0.0001f)
        => (a - b).sqrMagnitude < epsilon * epsilon;

    /// <summary>Clamp a vector by max length (e.g., to cap velocity).</summary>
    public static Vector2 ClampMagnitude(Vector2 v, float maxLength)
    {
        if (v.sqrMagnitude > maxLength * maxLength) return v.normalized * maxLength;
        return v;
    }

    /// <summary>Returns t in [0..1], where 0 = min, 1 = max. Inverse of Lerp.</summary>
    public static float InverseLerp(float min, float max, float value)
        => Mathf.Clamp01((value - min) / (max - min));


    // ============================================================
    //  MOVEMENT
    // ============================================================

    /// <summary>
    /// Move position one step in 'direction' but only along axes where the next position
    /// stays inside 'bounds'. Each axis is checked independently, so the body can slide
    /// along walls instead of stopping completely. Call from FixedUpdate.
    /// </summary>
    /// <param name="radius">Distance from the body's center to its edge along the axis.</param>
    public static Vector2 ClampToArena(Vector2 pos, Vector2 direction, float speed, Collider2D bounds, float radius = 0.5f)
    {
        float step = speed * Time.fixedDeltaTime;

        if (direction.x != 0f)
        {
            Vector2 nextX = pos + new Vector2(direction.x * step, 0f);
            Vector2 edgeX = nextX + new Vector2(Mathf.Sign(direction.x) * radius, 0f);
            if (bounds.OverlapPoint(edgeX))
                pos = nextX;
        }

        if (direction.y != 0f)
        {
            Vector2 nextY = pos + new Vector2(0f, direction.y * step);
            Vector2 edgeY = nextY + new Vector2(0f, Mathf.Sign(direction.y) * radius);
            if (bounds.OverlapPoint(edgeY))
                pos = nextY;
        }

        return pos;
    }


    // ============================================================
    //  INPUT / MOUSE
    // ============================================================

    /// <summary>Mouse position in world space (defaults to Camera.main).</summary>
    public static Vector3 MousePosition2D(Camera camera = null)
    {
        camera ??= Camera.main;
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// Normalized vector pointing from the mouse to 'from' (i.e. away from the cursor).
    /// Useful for knockback away from a click point or aim-away effects.
    /// </summary>
    public static Vector2 DirectionToMouse2D(Vector2 from, Camera camera)
    {
        Vector2 mousePosition = MousePosition2D(camera);
        return (from - mousePosition).normalized;
    }

    /// <summary>Same as DirectionToMouse2D but not normalized — preserves the distance to the cursor.</summary>
    public static Vector2 DirectionToMouse2DRaw(Vector2 from, Camera camera)
    {
        Vector2 mousePosition = MousePosition2D(camera);
        return from - mousePosition;
    }


    // ============================================================
    //  CAMERA / SPAWNING
    // ============================================================

    /// <summary>
    /// Random point on one of the four edges just outside the camera view.
    /// Used to spawn enemies off-screen so they walk into view.
    /// </summary>
    /// <param name="spawnPadding">How far past the screen edge to spawn (world units).</param>
    public static Vector2 GetSpawnPoint(Camera camera, float spawnPadding = 0.5f)
    {
        float halfH = camera.orthographicSize + spawnPadding;
        float halfW = halfH * camera.aspect;
        Vector2 cp = camera.transform.position;
        int side = Random.Range(0, 4);
        return side switch
        {
            0 => new Vector2(Random.Range(cp.x - halfW, cp.x + halfW), cp.y + halfH), // top
            1 => new Vector2(Random.Range(cp.x - halfW, cp.x + halfW), cp.y - halfH), // bottom
            2 => new Vector2(cp.x - halfW, Random.Range(cp.y - halfH, cp.y + halfH)), // left
            _ => new Vector2(cp.x + halfW, Random.Range(cp.y - halfH, cp.y + halfH))  // right
        };
    }


    // ============================================================
    //  STRING / RICH TEXT EXTENSIONS
    // ============================================================

    /// <summary>
    /// Wraps the string in a Unity rich-text color tag.
    /// Usage: "Hello".Color("red") → "&lt;color=red&gt;Hello&lt;/color&gt;".
    /// Accepts named colors ("red") or hex ("#FF0000").
    /// </summary>
    public static string Color(this string str, string c) => $"<color={c}>{str}</color>";
}
