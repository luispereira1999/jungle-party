using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class ProgressBarCircle : MonoBehaviour
{
    [Header("Title Setting")]
    public string Title;
    public Color TitleColor;
    public Font TitleFont;

    [Header("Bar Setting")]
    public Color BarColor;
    public Color BarBackGroundColor;
    public Color MaskColor;
    public Sprite BarBackGroundSprite;
    [Range(1f, 60f)]
    public int Alert = 60 - 15;
    public Color BarAlertColor;

    [Header("Sound Alert")]
    public AudioClip sound;
    public bool repeat = false;
    public float RepearRate = 1f;

    private Image bar, barBackground, Mask;
    private float nextPlay;
    private AudioSource audiosource;
    private Text txtTitle;

    private float barValue;
    public float BarValue
    {
        get { return barValue; }

        set
        {
            value = Mathf.Clamp(value, 0, 60);
            barValue = value;
            UpdateValue(barValue);
        }
    }

    void Awake()
    {
        txtTitle = transform.Find("Text").GetComponent<Text>();
        barBackground = transform.Find("BarBackgroundCircle").GetComponent<Image>();
        bar = transform.Find("BarCircle").GetComponent<Image>();
        audiosource = GetComponent<AudioSource>();
        Mask = transform.Find("Mask").GetComponent<Image>();
    }

    private void Start()
    {
        txtTitle.text = Title;
        txtTitle.color = TitleColor;
        txtTitle.font = TitleFont;

        bar.color = BarColor;
        Mask.color = MaskColor;
        barBackground.color = BarBackGroundColor;
        barBackground.sprite = BarBackGroundSprite;

        UpdateValue(barValue);
    }

    void UpdateValue(float val)
    {
        bar.fillAmount = -(val / 60) + 1f;

        txtTitle.text = Title + "00:00" + "\n" + "Ronda: 1";

        if (Alert >= val)
        {
            barBackground.color = BarAlertColor;
        }
        else
        {
            barBackground.color = BarBackGroundColor;
        }
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateValue(0);
            txtTitle.color = TitleColor;
            txtTitle.font = TitleFont;
            Mask.color = MaskColor;
            bar.color = BarColor;
            barBackground.color = BarBackGroundColor;
            barBackground.sprite = BarBackGroundSprite;
        }
        else
        {
            if (Alert >= barValue && Time.time > nextPlay)
            {
                nextPlay = Time.time + RepearRate;
                audiosource.PlayOneShot(sound);
            }
        }
    }
}