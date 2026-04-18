using UnityEngine;

public static class GameMath
{
    public static string Color(this string str, string c)
    {
        string coloredString = "<color=" + c + ">";
        coloredString += str;
        coloredString += "</color>";
        return coloredString;
    }
    
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

    public static Vector2 DirectionToMouse2D(Vector2 from, Camera camera)
    {
        Vector2 mousePosition = MousePosition2D(camera);
        return (from - mousePosition).normalized;
    }

    public static Vector2 DirectionToMouse2DRaw(Vector2 from, Camera camera)
    {
        Vector2 mousePosition = MousePosition2D(camera);
        return from - mousePosition;
    }

    public static Vector3 MousePosition2D(Camera camera = null)
    {
        camera ??= Camera.main;
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public static Vector2 GetSpawnPoint(Camera camera, float spawnPadding = 0.5f)
    {
        float halfH = camera.orthographicSize + spawnPadding;
        float halfW = halfH * camera.aspect;
        Vector2 cp = camera.transform.position;
        int side = Random.Range(0, 4);
        return side switch
        {
            0 => new Vector2(Random.Range(cp.x - halfW, cp.x + halfW), cp.y + halfH),
            1 => new Vector2(Random.Range(cp.x - halfW, cp.x + halfW), cp.y - halfH),
            2 => new Vector2(cp.x - halfW, Random.Range(cp.y - halfH, cp.y + halfH)),
            _ => new Vector2(cp.x + halfW, Random.Range(cp.y - halfH, cp.y + halfH))
        };
    }
}