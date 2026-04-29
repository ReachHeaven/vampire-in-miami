using UnityEngine;

public class CameraDirection : MonoBehaviour
{
    private void Update()
    {
        if (G.Player == null) return;
        var p = G.Player.transform.position;
        var t = transform.position;
        transform.position = new Vector3(p.x, p.y, t.z);
    }
}