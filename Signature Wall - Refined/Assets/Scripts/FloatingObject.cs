using UnityEngine;
using DG.Tweening;

public class FloatingObject : MonoBehaviour
{

    void Start()
    {
        // pop up entry animation and on finish, pop out exit animation
        transform.DOScale(Vector3.zero, 1f).From().SetEase(Ease.OutBounce).OnComplete(() =>
        {
            float randomY= transform.position.y + Random.Range(-1, 1f);
            float randomX = transform.position.x + Random.Range(-2f, -2f);
            transform.DOLocalMove(new Vector3(randomX, randomY, 0), Random.Range(5, 15)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            transform.DOLocalRotate(new Vector3(0, 0, Random.Range(-90, 90)), Random.Range(10, 20))
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            // transform.DoScale(Vector3.one * GameManager.Instance.signMinScale, Random.Range(5, 15)).SetEase(Ease.InOutSine);
            transform.DOScale(Vector3.one * GameManager.Instance.signMinScale, 10).SetEase(Ease.InOutSine);
        });
    }

}
