using UnityEngine;

public class CollideTest : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _otherCollider;

    private BoxCollider2D _collider;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _renderer.color =
            CollisionCheck.IsCollide2D(_collider.bounds, _otherCollider.bounds)
            ? Color.red
            : Color.green;
    }
}
