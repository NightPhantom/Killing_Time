using UnityEngine;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    GameObject targetPrefab;

    [SerializeField]
    int speed = 10;

    /// <summary>
    /// Holds the targets currently on the screen
    /// </summary>
    private List<ITarget> targets = new List<ITarget>();

    /// <summary>
    /// When a target reaches this size it should stop splitting on hit and simply be destroyed
    /// </summary>
    private float minimunTargetSize = 0.0625f;

    // Use this for initialization
    void Start()
    {
        if (targetPrefab == null)
        {
            throw new MissingComponentException("Missing the target prefab.");
        }

        // Initialize the level
        Reset();
    }

    /// <summary>
    /// Initialize the level with 2 full size targets
    /// </summary>
    private void Reset()
    {
        List<Vector3> startPositions = new List<Vector3> { new Vector3(3, 0, 0), new Vector3(-3, 0, 0) };

        for (int i = 0; i < startPositions.Count; i++)
        {
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            CreateNewTarget(targetPrefab.gameObject, startPositions[i], randomDirection * speed);
        }
    }

    /// <summary>
    /// Called when a projectile collides with a target
    /// </summary>
    /// <param name="target">The target that is being destroyed</param>
    public void TargetHit(GameObject target)
    {
        // If the target is still large enough to be split, then create 2 new targets half it's size
        if (target.transform.localScale.x > minimunTargetSize)
        {
            // Get the original target's direction
            Vector2 targetVelocity = target.GetComponent<Rigidbody2D>().velocity;

            // Direction of first new target is 90 degrees counter clockwise from the original target's direction
            Vector2 newDirectionA = new Vector2(-targetVelocity.y, targetVelocity.x);

            // Direction of first new target is 90 degrees clockwise from the original target's direction
            Vector2 newDirectionB = new Vector2(targetVelocity.y, -targetVelocity.x);

            // Create the new targets
            GameObject newTargetA = CreateNewTarget(target, target.transform.position, newDirectionA);
            GameObject newTargetB = CreateNewTarget(target, target.transform.position, newDirectionB);

            // Reduce the sizes by half
            newTargetA.transform.localScale /= 2;
            newTargetB.transform.localScale /= 2;
        }

        // Destroy the original target
        targets.Remove(target.GetComponent<ITarget>());
        Destroy(target);
        if (targets.Count < 1)
        {// If we destroyed the last remaining target, then reset the scene
            Reset();
        }
    }

    /// <summary>
    /// Instantiate a new Target object.
    /// </summary>
    /// <param name="template">The GameObject/prefab from which we will base the new target</param>
    /// <param name="startingPosition">The initial position of the new target</param>
    /// <param name="initialDirection">The direction in which the new target should move</param>
    private GameObject CreateNewTarget(GameObject template, Vector2 startingPosition, Vector2 initialDirection)
    {
        // Instantiate a new target based on the given arguments
        GameObject newTarget = (GameObject)Instantiate(template);
        newTarget.transform.position = startingPosition;
        newTarget.GetComponent<Rigidbody2D>().velocity = initialDirection;

        // Assign the target manager for the call back when the target is hit
        ITarget targetScript = newTarget.GetComponent<ITarget>();
        targetScript.targetManager = this;
        targets.Add(targetScript);

        return newTarget;
    }
}
