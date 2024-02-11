using UnityEngine;

public class HealTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player object
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Check if the playerHealth is not null
            if (playerHealth != null)
            {
                // Call the Heal method on the playerHealth
                playerHealth.Heal();
            }
            else
            {
                Debug.LogError("PlayerHealth component not found on the player object.");
            }
        }
    }
}
