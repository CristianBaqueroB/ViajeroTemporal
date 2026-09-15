using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        // Actualiza la barra de vida si existe un UIManager en la escena actual
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActualizarBarraVida(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActualizarBarraVida(currentHealth, maxHealth);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.MostrarPantallaDerrota();
        }
    }

    public void SincronizarVidaConUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActualizarBarraVida(currentHealth, maxHealth);
        }
    }

    public void ResetearVida()
    {
        currentHealth = maxHealth;
        isDead = false;
        SincronizarVidaConUI();
    }
}