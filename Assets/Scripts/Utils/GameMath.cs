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
        camera??= Camera.main;
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }
    
    public static Vector2 GetSpawnPoint(Camera camera, float spawnPadding = 1f)
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