using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HorizontalSelector : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public List<string> options = new List<string>();
    public int currentIndex;



    public void NextOption()
    {
        currentIndex++;
        if (currentIndex >= options.Count) currentIndex = 0;
        UpdateDisplay();

        
    }

    public void PrevOption() 
    { 
        currentIndex--;
        if (currentIndex < 0) currentIndex = options.Count - 1;
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        displayText.text = options[currentIndex].ToString();

    }
}
