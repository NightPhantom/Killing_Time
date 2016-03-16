using UnityEngine;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab;

    /// <summary>
    /// A list to hold all the projectiles that have been previously instantiated but are currently inactive.
    /// Whenever possible we want to re-use objects, rather than having to incur the cost of constant instantiation and garbage collection.
    /// </summary>
    List<IProjectile> projectilePool = new List<IProjectile>();

    // Use this for initialization
    void Start()
    {
        if (projectilePrefab == null)
        {
            throw new MissingComponentException("Missing the projectile prefab.");
        }
    }

    /// <summary>
    /// Either instantiate a new projectile, or re-use one from the pool, at the given position with the given rotation (presumably based on the firing object)
    /// </summary>
    /// <param name="position">The location where the projectile should start</param>
    /// <param name="rotation">The direction in which the projectile should be oriented</param>
    public void Shoot(Vector3 position, Quaternion rotation)
    {
        if (projectilePool.Count > 0)
        {
            // Recycle a projectile from the pool
            IProjectile projectile = projectilePool[0];
            projectilePool.RemoveAt(0);

            // Reset it's position and re-activate it
            projectile.Position = position;
            projectile.Rotation = rotation;
            projectile.Activate();
        }
        else
        {
            // No projectiles are available for re-use, so we will have to instantiate a new one
            IProjectile projectile = ((GameObject)Instantiate(projectilePrefab.gameObject, position, rotation)).GetComponent<IProjectile>();
            projectile.projectileManager = this;
        }
    }

    public void AddProjectileToPool(IProjectile projectile)
    {
        if (!projectilePool.Contains(projectile))
        {
            // Add the spent projectile so that we can re-use it later
            projectilePool.Add(projectile);
        }
    }
}
