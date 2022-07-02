using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private EntityBase self;

    private void OnEnable()
    {
        self.healthChangedEvent += OnHealthChanged;
    }

    private void Awake()
    {
        slider.value = self.health / self.maxHealth;
    }

    private void OnHealthChanged(float newHealth)
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        slider.value = self.health / self.maxHealth;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(0, -gameObject.transform.parent.rotation.eulerAngles.y, 0);
    }

    private void OnDisable()
    {
        self.healthChangedEvent -= OnHealthChanged;
    }
}
