using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveOrLoadManager : MonoBehaviour
{
    private const string ESFile = "profile.txt";
    private const string TagSign = "?tag=";

    private const string LEVEL = "level";

    public int CurrentLevel
    {
        get
        {
            var val = GetInt(LEVEL);
            if(val == -1)
            {
                val = 0;
            }
            return val;
        }
        set
        {
            SetInt(LEVEL, value);
        }
    }

    #region Tools

    #region Remove

    public void RemoveTag(string tag)
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                PlayerPrefs.DeleteKey(ESFile + TagSign + tag);
            }
            catch
            {
            }
#else
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                ES3.DeleteKey(ESFile + TagSign + tag);
            }
            catch
            {
            }
#endif
    }

    #endregion

    #region GET

    public string GetString(string tag)
    {
#if !UNITY_WEBGL
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                return ES3.Load<string>(ESFile + TagSign + tag);
            }
            catch
            {
                return null;
            }
        else
            return null;
#else
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                return PlayerPrefs.GetString(ESFile + TagSign + tag);
            }
            catch
            {
                return null;
            }
        else
            return null;
#endif
    }

    public string GetString(string tag, string defaultValue)
    {
#if !UNITY_WEBGL
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                return ES3.Load<string>(ESFile + TagSign + tag);
            }
            catch
            {
                return defaultValue;
            }
        else
            return defaultValue;
#else
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                return PlayerPrefs.GetString(ESFile + TagSign + tag);
            }
            catch
            {
                return defaultValue;
            }
        else
            return defaultValue;
#endif
    }

    public int GetInt(string tag)
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                return PlayerPrefs.GetInt(ESFile + TagSign + tag);
            }
            catch
            {
                return -1;
            }
        else
            return -1;
#else
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                return ES3.Load<int>(ESFile + TagSign + tag);
            }
            catch
            {
                return -1;
            }
        else
            return -1;
#endif
    }

    public int GetInt(string tag, int defaultValue)
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                return PlayerPrefs.GetInt(ESFile + TagSign + tag);
            }
            catch
            {
                return defaultValue;
            }
        else
            return defaultValue;
#else
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                return ES3.Load<int>(ESFile + TagSign + tag);
            }
            catch
            {
                return defaultValue;
            }
        else
            return defaultValue;
#endif
    }

    public float GetFloat(string tag)
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                return PlayerPrefs.GetFloat(ESFile + TagSign + tag);
            }
            catch
            {
                return -1;
            }
        else
            return -1;
#else
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                return ES3.Load<float>(ESFile + TagSign + tag);
            }
            catch
            {
                return -1;
            }
        else
            return -1;
#endif
    }

    public float GetFloat(string tag, float defaultValue)
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                return PlayerPrefs.GetFloat(ESFile + TagSign + tag);
            }
            catch
            {
                return defaultValue;
            }
        else
            return defaultValue;
#else
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                return ES3.Load<float>(ESFile + TagSign + tag);
            }
            catch
            {
                return defaultValue;
            }
        else
            return defaultValue;
#endif
    }

    public bool GetBool(string tag)
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                if (PlayerPrefs.GetInt(ESFile + TagSign + tag) == 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        else
            return false;
#else
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                return ES3.Load<bool>(ESFile + TagSign + tag);
            }
            catch
            {
                return false;
            }
        else
            return false;
#endif
    }

    public bool GetBool(string tag, bool defaultValue)
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey(ESFile + TagSign + tag))
            try
            {
                if (PlayerPrefs.GetInt(ESFile + TagSign + tag) == 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return defaultValue;
            }
        else
            return defaultValue;
#else
        if (ES3.KeyExists(ESFile + TagSign + tag))
            try
            {
                return ES3.Load<bool>(ESFile + TagSign + tag);
            }
            catch
            {
                return defaultValue;
            }
        else
            return defaultValue;
#endif
    }

    #endregion

    #region SET

    public void SetString(string tag, string value)
    {
#if UNITY_WEBGL
        PlayerPrefs.SetString(ESFile + TagSign + tag, value);
        PlayerPrefs.Save();
#else
        ES3.Save<string>(ESFile + TagSign + tag, value);
#endif
    }

    public void SetBool(string tag, bool value)
    {
#if UNITY_WEBGL
        if (value)
            PlayerPrefs.SetInt(ESFile + TagSign + tag, 1);
        else
            PlayerPrefs.SetInt(ESFile + TagSign + tag, 0);

        PlayerPrefs.Save();
#else
        ES3.Save<bool>(ESFile + TagSign + tag, value);
#endif
    }

    public void SetInt(string tag, int value)
    {
#if UNITY_WEBGL
        PlayerPrefs.SetInt(ESFile + TagSign + tag, value);
        PlayerPrefs.Save();
#else
        ES3.Save<int>(ESFile + TagSign + tag, value);
#endif
    }

    public void SetFloat(string tag, float value)
    {
#if UNITY_WEBGL
        PlayerPrefs.SetFloat(ESFile + TagSign + tag, value);
        PlayerPrefs.Save();
#else
        ES3.Save<float>(ESFile + TagSign + tag, value);
#endif
    }

    #endregion

    #endregion
}
