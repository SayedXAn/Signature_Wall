using UnityEngine;
using UnityEngine.SceneManagement;

public class BG_Resizer : MonoBehaviour
{
    // Update is called once per frame
    private void Start()
    {
        if (PlayerPrefs.GetFloat("x") != 0f)
        {
            transform.localScale = new Vector3(PlayerPrefs.GetFloat("x"), transform.localScale.y, transform.localScale.z);
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.01f, transform.localScale.y, transform.localScale.z);
            PlayerPrefs.SetFloat("x", transform.localScale.x);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y, transform.localScale.z);
            PlayerPrefs.SetFloat("x", transform.localScale.x);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Refined");
        }
    }
}
