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
    public float maxValue;
    public Color BarColor;
    public Color BarBackGroundColor;
    public Color MaskColor;
    public Sprite BarBackGroundSprite;
    //[Range(1f, 60f)]
    [SerializeField] private int _alertTime = 15;
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
            value = Mathf.Clamp(value, 0, maxValue);
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

    void Start()
    {
        txtTitle.text = Title;
        txtTitle.color = TitleColor;
        txtTitle.font = TitleFont;

        bar.color = BarColor;
        Mask.color = MaskColor;
        barBackground.color = BarBackGroundColor;
        barBackground.sprite = BarBackGroundSprite;

        UpdateValue(BarValue);
    }

    public void UpdateValue(float val)
    {
        bar.fillAmount = -(val / maxValue) + 1f;

        txtTitle.text = Title + val + "\n" + "Ronda: 1";

        if (_alertTime >= val)
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
            if (_alertTime >= barValue && Time.time > nextPlay)
            {
                nextPlay = Time.time + RepearRate;
                audiosource.PlayOneShot(sound);
            }
        }
    }
}