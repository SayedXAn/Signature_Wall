using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject signPrefab, signatureHolder, settingsPanel, loaderVideo;
    [SerializeField] Camera secondCamera;
    [SerializeField] VideoClip[] videoClips;
    //[SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Color[] textColors;
    List<GameObject> signs = new List<GameObject>();

    public int currentStyleIndex = 0;

    public float signQuality = 0.05f;
    public float signMinScale = 0.25f;
    public float signMaxScale = 1f;

    public string SignStoreDir
    {
        get
        {
            return Application.dataPath + "/Signs/";
        }
    }

    [SerializeField] Slider qualitySlider;
    [SerializeField] TMP_InputField signMinScaleInput, signMaxScaleInput;

    public void ShowSign(Texture2D signTex)
    {
        //random position inside second Camera view
        Vector3 position = secondCamera.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 10));
        GameObject sign = Instantiate(signPrefab, position, Quaternion.identity, signatureHolder.transform);
        sign.GetComponent<SpriteRenderer>().sprite = Sprite.Create(signTex, new Rect(0, 0, signTex.width, signTex.height), new Vector2(0.5f, 0.5f));
        sign.GetComponent<SpriteRenderer>().color = textColors[currentStyleIndex];
        sign.transform.localScale = new Vector3(signMaxScale, signMaxScale, signMaxScale);
        signs.Add(sign);
    }

    public void ClearSigns()
    {
        foreach (var sign in signs)
        {
            Destroy(sign);
        }
        signs.Clear();
    }

    public void SwitchWallStyle(int styleIndex)
    {
        currentStyleIndex = styleIndex;
        //videoPlayer.Stop();
        ClearSigns();
        //videoPlayer.clip = videoClips[styleIndex];
        //videoPlayer.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SwitchWallStyle(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SwitchWallStyle(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SwitchWallStyle(2);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearSigns();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            settingsPanel.SetActive(true);
        }

    }

    public void SaveSettings()
    {
        signQuality = qualitySlider.value;
        signMinScale = float.Parse(signMinScaleInput.text);
        signMaxScale = float.Parse(signMaxScaleInput.text);
        settingsPanel.SetActive(false);
        PlayerPrefs.SetFloat("signQuality", signQuality);
        PlayerPrefs.SetFloat("signMinScale", signMinScale);
        PlayerPrefs.SetFloat("signMaxScale", signMaxScale);
    }

    void GetSettings()
    {
        signQuality = PlayerPrefs.GetFloat("signQuality", 1f);
        signMinScale = PlayerPrefs.GetFloat("signMinScale", 0.05f);
        signMaxScale = PlayerPrefs.GetFloat("signMaxScale", 0.25f);
        qualitySlider.value = signQuality;
        signMinScaleInput.text = signMinScale.ToString();
        signMaxScaleInput.text = signMaxScale.ToString();
    }

    public void ShowHideLoader(bool show)
    {
        loaderVideo.SetActive(show);
        if(show)
        {
            loaderVideo.GetComponent<LoadingAnimation>().StartAnimation();
        }
    }

    void Start()
    {
        GetSettings();
        SwitchWallStyle(0);
        if (!System.IO.Directory.Exists(SignStoreDir))
        {
            System.IO.Directory.CreateDirectory(SignStoreDir);
        }
    }



    // singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
