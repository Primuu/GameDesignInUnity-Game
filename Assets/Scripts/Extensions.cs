using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");

    // Extension for Rigidbody2D to detect collisions in a specific direction
    public static bool Raycast(this Rigidbody2D rb, Vector2 direction)
    {
        if (rb.isKinematic) {
            return false;
        }

        float radius = 0.25f; // Radius used in collision detection
        float distance = 0.375f; // Distance at which collision is checked

        // Method for detecting objects on a specific path (in this case, in the form of a circle)
        RaycastHit2D hit = Physics2D.CircleCast(rb.position, radius, direction.normalized, distance, layerMask);
        return hit.collider != null && hit.rigidbody != rb; 
    }

    // Extension for Transform to measure of how much two vectors are "pointing" in the same direction
    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        // Difference between the positions of two objects
        Vector2 direction = other.position - transform.position;
        // Dot product is a measure of how much two vectors are "pointing" in the same direction:
        // * 1 means that the vectors point exactly in the same direction 
        // * 0 means that they are perpendicular
        // * -1 means that they point in exactly opposite directions
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }

}
