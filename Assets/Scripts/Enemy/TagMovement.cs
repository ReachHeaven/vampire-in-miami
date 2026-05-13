using System;
using UnityEngine;

public enum MovementKind
{
    Chase,
    Zigzag,
    Dash,
}

[Serializable]
public class TagMovement : EntityComponentDefinition
{
    public MovementKind Kind = MovementKind.Chase;

    [Header("Zigzag")]
    public float ZigzagAmplitude = 0.8f;
    public float ZigzagFrequency = 4f;

    [Header("Dash")]
    public float DashIdleSpeedMul = 0.25f;
    public float DashWindupSeconds = 0.35f;
    public float DashChargeSpeedMul = 3.5f;
    public float DashChargeSeconds = 0.5f;
    public float DashCooldownSeconds = 1.2f;
}
