using UnityEngine;

public class DeadlyObstacle : MonoBehaviour
{
    /// <summary>
    /// Eliminates the player upon collision
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        // check whether the collided object is the player
        if (player != null)
        {
            // eliminate the player
            player.Kill();
        }
    }

    /// <summary>
    /// Eliminates the player upon collision
    /// </summary>
    public void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        //check whether the collided object is the player
        if (player != null)
        {
            // eliminate the player
            player.Kill();
        }
    }


}
