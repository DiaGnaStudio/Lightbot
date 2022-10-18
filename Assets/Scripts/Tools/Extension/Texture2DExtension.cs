#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class Texture2DExtension
{
    public static Sprite SaveImage(this Texture2D texture, string path)
    {
        string SpltePath(string path)
        {
            string resault = string.Empty;
            string[] parts = path.Split('/');
            for (int i = 0; i < parts.Length - 1; i++)
            {
                resault += parts[i];
            }
            return resault;
        }

        var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(path);
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = SpltePath(uniqueFileName);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(uniqueFileName, bytes);

        AssetDatabase.Refresh();
        Debug.Log("Image saved in: " + uniqueFileName);

        return AssetDatabase.LoadAssetAtPath<Sprite>(uniqueFileName);

    }

    public static Texture2D DeCompress(this Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}

#endif