using UnityEngine;

public class Arena : MonoBehaviour
{
    [HideInInspector] public new BoxCollider2D collider;

    private void Awake()
    {
        G.Arena = this;
        collider = GetComponent<BoxCollider2D>();
    }
}