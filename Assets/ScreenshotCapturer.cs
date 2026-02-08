using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCapturer : MonoBehaviour
{
    public Camera cam;
    public int width;
    public int height;

    public Texture2D CaptureFromCamera()
    {
        //
        RenderTexture rt = new RenderTexture(width, height, 24);
        RenderTexture prev = RenderTexture.active;

        cam.targetTexture = rt;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        cam.Render();

        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = prev;

        Object.Destroy(rt);
        return tex;
    }
}
