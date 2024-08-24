using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraFlash : MonoBehaviour
{
    [SerializeField] GameObject cameraFlash;
    private SpriteRenderer flashSR;
    private Camera snapShotCam;

    [SerializeField] RawImage snapShotTexture;
    private RenderTexture renderTexture;
    private Texture2D image;

    [SerializeField] Color flashColor;
    Color snapColor = new Color(255, 255, 255, 0);

    private void Start()
    {
        LeanTween.reset();
        snapShotCam = GetComponent<Camera>();
        flashSR = cameraFlash.GetComponent<SpriteRenderer>();

        // Create the RenderTexture once and reuse it
        renderTexture = new RenderTexture(snapShotCam.pixelWidth, snapShotCam.pixelHeight, 24);
        image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    }

    private void OnDestroy()
    {
        // Clean up the RenderTexture when the object is destroyed
        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }

        if (image != null)
        {
            Destroy(image);
        }
    }

    public void FlashCamera()
    {
        cameraFlash.SetActive(true);
        //Set flash sprite renderer back to transparent
        flashSR.color = flashColor;

        // Fade out the flash and disable it after
        LeanTween.alpha(cameraFlash, 0, 0.4f).setEaseOutCirc().setOnComplete(() => cameraFlash.SetActive(false));

        // Delay the screenshot until after the frame ends
        //TakeScreenShot();
    }

    private void TakeScreenShot()
    {
        snapShotCam.targetTexture = renderTexture;

        snapShotCam.Render();

        // Read the pixels from the RenderTexture into the Texture2D
        RenderTexture.active = renderTexture;
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = null;
        snapShotCam.targetTexture = null;

        snapShotTexture.texture = image;
        SnapShotSequence();
    }

    void SnapShotSequence()
    {
        snapShotTexture.color = snapColor;

        snapShotTexture.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        LeanTween.alpha(snapShotTexture.rectTransform, 1, 0.8f);
        LeanTween.move(snapShotTexture.rectTransform, new Vector2(-650, -350), 1.5f).setEaseInCubic();

        LeanTween.scale(snapShotTexture.rectTransform, Vector3.one * 0.2f, 1.5f).setEaseInCubic();
    }
}