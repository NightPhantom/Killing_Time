using UnityEngine;
using System.Collections;

public interface IProjectile
{
    ProjectileManager projectileManager { get; set; }

    void Activate();

    void Deactivate();

    Vector3 Position { get; set; }

    Quaternion Rotation { get; set; }
}
