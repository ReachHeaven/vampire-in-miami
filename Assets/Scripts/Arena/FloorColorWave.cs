using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorColorWave : MonoBehaviour
{
    [Header("Wave Settings")]
    public float waveSpeed = 0.5f;
    public Color baseColor = new Color(0.2f, 0.15f, 0.3f);
    public Color waveColor = new Color(0.4f, 0.1f, 0.5f);

    private SpriteRenderer _sr;
    private Tilemap _tilemap;

    private void Awake()
    {
        // Пытаемся найти один из компонентов
        _sr = GetComponent<SpriteRenderer>();
        _tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        // Рассчитываем фазу волны (0..1)
        float wave = Mathf.Sin(Time.time * waveSpeed) * 0.5f + 0.5f;
        Color lerpedColor = Color.Lerp(baseColor, waveColor, wave);

        // Применяем цвет в зависимости от того, что нашли
        if (_sr != null)
        {
            _sr.color = lerpedColor;
        }
        else if (_tilemap != null)
        {
            _tilemap.color = lerpedColor;
        }
    }
}