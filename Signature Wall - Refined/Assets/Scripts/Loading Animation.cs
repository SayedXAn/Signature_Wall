using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingAnimation : MonoBehaviour
{
    public Image[] circles;
    public int ind = 0;
    void Start()
    {
        DOTween.Init();
        //ResetCircles();
        //StartCoroutine(TheUp());
    }

    public void StartAnimation()
    {
        ResetCircles();
        StartCoroutine(TheUp());
    }
    IEnumerator TheUp()
    {
        yield return new WaitForSeconds(0.1f);
        circles[ind].rectTransform.DOMoveY(circles[ind].rectTransform.position.y + 60f, 0.1f);
        ind++;
        if(ind < circles.Length)
        {
            StartCoroutine(TheUp());
        }
        else
        {
            ind = 0;
            StartCoroutine(TheDown());
        }
    }

    IEnumerator TheDown()
    {
        yield return new WaitForSeconds(0.1f);
        circles[ind].rectTransform.DOMoveY(circles[ind].rectTransform.position.y - 60f, 0.1f);
        ind++;
        if (ind < circles.Length)
        {
            StartCoroutine(TheDown());
        }
        else
        {
            ind = 0;
            StartCoroutine(TheUp());
        }
    }
    private void ResetCircles()
    {
        ind = 0;
        StopCoroutine(TheDown());
        StopCoroutine(TheUp());
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].rectTransform.position = new Vector3(circles[i].rectTransform.position.x, 540f, circles[i].rectTransform.position.z);
        }
    }
}
