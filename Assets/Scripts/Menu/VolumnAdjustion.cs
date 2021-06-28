using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumnAdjustion : MonoBehaviour
{

    public AudioMixer mixer;
    public Image bar;
    private float curPercent = 1; // Percent of current settings
    // Bar control method controls option bar using two buttons
    public void BarControl(bool up)
    {
        // Each time the button is pressed, the value is adjusted by 10%
        curPercent += (up) ? 0.1f : -0.1f;
        if (curPercent > 1)
        {
            curPercent = 1;
        }
        else if (curPercent <= 0)
        {
            curPercent = 0.0001f; // Can't set value to 0 as it cause bugs
        }

        // Adjust real value
        mixer.SetFloat("MusicVolumn", Mathf.Log10(curPercent) * 20);

        // Also adjust the bar display amount on screen
        bar.rectTransform.sizeDelta = new Vector2(520 * curPercent, 75);
        Debug.Log(bar.rectTransform.rect.width);
    }
}