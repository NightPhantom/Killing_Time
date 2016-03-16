using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour, IProjectile
{
    [SerializeField]
    private float speed = 1f;

    public ProjectileManager projectileManager { get; set; }

    /// <summary>
    /// Flag used to indicate whether or not a bullet is active. When active it should move, when inactive it should be hidden away until we need a new bullet
    /// </summary>
    private bool IsActive = true;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public Quaternion Rotation
    {
        get
        {
            return transform.rotation;
        }
        set
        {
            transform.rotation = value;
        }
    }

    /// <summary>
    /// When a bullet is not active, we keep it in this position to hide it away until we need it again
    /// </summary>
    private readonly Vector3 disabledPosition = new Vector3(0, -6, 0);

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {// When active, the bullet should move constantly "up"
            GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.up) * speed;
        }
        else
        {// When inactive, the bullet should stop moving, until we recycle it again
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // If the bullet collides with a target, then we should deactivate it to return it to the pool
        ITarget target = collider.gameObject.GetComponent<ITarget>();
        if (target != null)
        {
            Deactivate();
        }
    }

    void OnBecameInvisible()
    {
        // If the bullet reaches the end of the screen, then we should deactivate it to return it to the pool
        Deactivate();
    }

    /// <summary>
    /// Called when a bullet "fired"
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Called when a bullet goes out of scope or hits a target
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        Position = disabledPosition;
        projectileManager.AddProjectileToPool(this);
    }
}
