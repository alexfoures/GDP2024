using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCanvas : MonoBehaviour
{
    [SerializeField] GameObject display;
    [SerializeField] Image[] images;
    [SerializeField] TMP_Text[] text;
    [SerializeField] private ImagesDisplay[] sprites;
    private int step = 0;

    public string[] nextStep()
    {
        images[0].sprite = sprites[step].sprite1;
        images[1].sprite = sprites[step].sprite2;
        display.SetActive(true);
        string[] values = new string[] { sprites[step].id1.ToString(), sprites[step].id2.ToString() };
        step++;
        return values;

    }

    public void stop()
    {
        UpdateText(0, 0);
        UpdateText(0, 1);
        UpdateText(0, 2);
        display.SetActive(false);
    }

    public void UpdateText(int value, int id)
    {
        text[id].text = value.ToString();
    }

}
[System.Serializable]
public class ImagesDisplay
{
    public Sprite sprite1;
    public Sprite sprite2;
    public int id1;
    public int id2;
}

