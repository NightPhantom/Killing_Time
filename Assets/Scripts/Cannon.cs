using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private float rotation = 0f;

    [SerializeField]
    ProjectileManager projectileManager;

    [SerializeField]
    private float fireRate = 0.1f;

    /// <summary>
    /// The time at which the last shot was fired
    /// </summary>
    /// <remarks>Use this along with the fireRate to limit the frequency of shots fired when the user holds down the key</remarks>
    private float lastShot;

    // Use this for initialization
    void Start()
    {
        if (projectileManager == null)
        {
            throw new MissingReferenceException("Missing the projectile manager.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Shoot();
    }

    /// <summary>
    /// Update the cannon's rotation based on user input
    /// </summary>
    private void Rotate()
    {
        // Read the keyboard/joystick left and right input to change the cannon's rotation
        rotation += Input.GetAxis("Horizontal") * speed;
        // Clamp the rotation to restrict the cannon to the top 180 degrees
        rotation = Mathf.Clamp(rotation, -90, 90);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -rotation);
    }

    /// <summary>
    /// Fire the cannon based on user input
    /// </summary>
    private void Shoot()
    {
        // Look for the "Space" key in user input
        float shoot = Input.GetAxis("Shoot");

        // If the user has pressed the "Shoot" ("Space") key, and sufficient time has passed since the last shot was fired, then we can trigger a new shot
        if (shoot > 0 && Time.time > fireRate + lastShot)
        {
            projectileManager.Shoot(transform.position, transform.rotation);
            lastShot = Time.time;
        }
    }
}
