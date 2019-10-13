using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private float maxStamina = 100;
    public float currentStamina;
    public Slider sliderStamina;
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gameIsPause)
        {
            HealStamina();
        }
    }

    private void HealStamina()
    {
        currentStamina += 0.05f;
        sliderStamina.value = currentStamina;

        if (currentStamina >= 100)
        {
            currentStamina = 100;
        }
    }

    public void DrainStamina(float amount)
    {
        currentStamina -= amount;
        sliderStamina.value = currentStamina;
    }
}
