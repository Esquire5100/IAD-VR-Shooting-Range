using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;

    private enemyController enemyController;


    public void Awake()
    {
        enemyController = GetComponentInParent<enemyController>();
        healthBarFill = GetComponentInChildren<Image>();
        healthBarFill.fillAmount = 1f; // Initialize health bar to full
    }

    private void Update()
    {
        DrainHealthBar();
    }

    private void DrainHealthBar()
    {
        float ratio = enemyController.currentHealth / enemyController.maxHealth;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(healthBarFill.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        sequence.Play();
    }
}
