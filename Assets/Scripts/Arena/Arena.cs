using UnityEngine;


public class Arena : MonoBehaviour
{
    [HideInInspector] public new BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        G.Arena = this;
    }
}