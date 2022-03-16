using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    public Slider HealthSlider;
    public Slider EnergySlider;

    public void SetHealth(float Health)
    {
        HealthSlider.value = Health;
    }

    public void SetEnergy(float Energy)
    {
        EnergySlider.value = Energy;
    }
}
