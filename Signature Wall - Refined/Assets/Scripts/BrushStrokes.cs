using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class BrushStrokes : MonoBehaviour
{
    public GameObject brushPrefab;
    LineRenderer currentLine;

    List<GameObject> brushStrokes = new List<GameObject>();
    public Button clearButton, saveButton;
    public GameObject canvas;

    void Start()
    {
        LoadPreviousSigns();
    }

    void LoadPreviousSigns()
    {
        string folderPath = GameManager.Instance.SignStoreDir;

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            return;
        }

        string[] files = Directory.GetFiles(folderPath, "*.png");

        if (files.Length == 0)
        {
            Debug.Log("No previous signs found.");
            return;
        }

        foreach (string file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);

            // Display the image on Screen 2 (or wherever you normally show)
            GameManager.Instance.ShowSign(texture);

            Debug.Log("Loaded previous sign: " + file);
        }
    }


    public void OnMouseDown()
    {
        // Debug.Log("Mouse Down");
        GameObject brush = Instantiate(brushPrefab);
        brushStrokes.Add(brush);
        currentLine = brush.GetComponent<LineRenderer>();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentLine.SetPosition(0, mousePos);
        currentLine.SetPosition(1, mousePos);
    }

    public void OnMouseDrag()
    {
        // Debug.Log("Mouse Drag");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AddPoint(mousePos);
        // currentLine.positionCount++;
        // currentLine.SetPosition(currentLine.positionCount - 1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void OnMouseUp()
    {
        // Debug.Log("Mouse Up");
        currentLine = null;
    }

    void AddPoint(Vector2 point)
    {
        currentLine.positionCount++;
        int positionIndex = currentLine.positionCount - 1;
        currentLine.SetPosition(positionIndex, point);
    }

    public void ClearStrokes()
    {

        clearButton.interactable = false;
        foreach (GameObject brush in brushStrokes)
        {
            Destroy(brush);
        }
        brushStrokes.Clear();
        clearButton.interactable = true;
    }

    public void SaveLinesAsPNG()
    {
        StartCoroutine(CaptureScreenshot());
    }

    IEnumerator CaptureScreenshot()
    {
        saveButton.interactable = false;
        canvas.SetActive(false);
        string filePath = GameManager.Instance.SignStoreDir + $"/{System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.png";
        yield return new WaitForEndOfFrame();
        var mainCamera = Camera.main;
        int width = (int)(Screen.width*GameManager.Instance.signQuality);
        int height = (int)(Screen.height*GameManager.Instance.signQuality);
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();
        RenderTexture.active = renderTexture;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
        // ScreenCapture.CaptureScreenshot(filePath);
        // Debug.Log("Capturing screenshot...");
        StartCoroutine(TransparencyCapture(filePath));
    }

    IEnumerator TransparencyCapture(string filePath)
    {
        GameManager.Instance.ShowHideLoader(true);
        // Debug.Log("Processing image in the background...");

        // Wait until the screenshot is saved (non-blocking)
        yield return new WaitUntil(() => File.Exists(filePath));

        // Debug.Log("Screenshot saved!");

        // Read the image bytes (still non-blocking)
        byte[] bytes = File.ReadAllBytes(filePath);
        // Debug.Log("Image bytes read!");
        yield return null;

        // Process the texture in the main thread
        Texture2D sign = new Texture2D(2, 2);
        sign.LoadImage(bytes);
        // Debug.Log("Texture loaded!");
        yield return null;
        // Process the pixels (remove white, smooth edges, etc.)
        Color[] pixels = sign.GetPixels();
        int batchSize = 10000; // Process 1000 pixels per frame to reduce yielding overhead
        Debug.Log(pixels.Length);
        for (int i = 0; i < pixels.Length; i += batchSize)
        {
            int end = Mathf.Min(i + batchSize, pixels.Length); // Ensure we don't go out of bounds

            for (int j = i; j < end; j++)
            {
                if (pixels[j] == Color.white)
                {
                    pixels[j] = new Color(1, 1, 1, 0); // Make white transparent
                }
                else
                {
                    pixels[j] = new Color(1, 1, 1, 1); // Make non-white opaque white
                }
            }

            // Yield control back after processing the batch, allowing other tasks to run
            yield return new WaitForSeconds(0.001f); // Small delay, adjust as needed
        }

        // Set the pixels on the texture
        sign.SetPixels(pixels);
        // Debug.Log("Pixels processed!");

        // Apply the changes on the main thread (no need to queue to main thread since it's already in a coroutine)
        sign.Apply();

        // Now encode the texture to PNG on the main thread
        byte[] pngBytes = sign.EncodeToPNG();

        // Save the PNG to file (this is done asynchronously but does not block the main thread)
        File.WriteAllBytes(filePath, pngBytes);

        // Update the UI elements on the main thread
        GameManager.Instance.ShowSign(sign);
        ClearStrokes();
        canvas.SetActive(true);
        saveButton.interactable = true;
        // Debug.Log($"Image saved to {filePath}");
        // Debug.LogError($"Image saved to {filePath}");
        GameManager.Instance.ShowHideLoader(false);
    }
}
