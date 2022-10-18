using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    [Header("Panels, Dialogs & Elements")]
    private PageBase[] allPages;
    private DialogBase[] allDialogs;
    private UIElementBase[] allElements;

    private Stack<PageBase> openPagesStack = new Stack<PageBase>();

    //[ExecuteInEditMode()]
    public void Initialization()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);


            #region Get Pages, Dialogs and Elements

            allPages = GetComponentsInChildren<PageBase>(true);
            allDialogs = GetComponentsInChildren<DialogBase>(true);
            allElements = GetComponentsInChildren<UIElementBase>(true);

            #endregion

            #region Load all of the pages and dialogs

            foreach (var item in allPages)
            {
                item.SetValuesOnSceneLoad();
            }

            foreach (var item in allDialogs)
            {
                item.SetValuesOnSceneLoad();
            }

            #endregion

            #region Close all of the pages and dialogs

            foreach (var item in allPages)
            {
                ClosePage(item);
            }

            foreach (var item in allDialogs)
            {
                CloseDialog(item);
            }

            #endregion
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Page's functions

    public void OpenPage(PageBase page)
    {
        if (openPagesStack.Count > 0)
        {

            for (int i = openPagesStack.Count - 1; i >= 0; i--)
            {
                List<UIElementBase> items = openPagesStack.Peek().elements;
                foreach (var item in items)
                {
                    item.gameObject.SetActive(false);
                }
                openPagesStack.Peek().gameObject.SetActive(false);
                //openPagesStack.Pop();
            }
        }
        foreach (var item in allPages)
        {
            if (page == item)
            {
                item.gameObject.SetActive(true);
                item.SetValues();
                foreach (var i in item.elements)
                {
                    i.gameObject.SetActive(true);
                    i.SetValues();
                }
                openPagesStack.Push(item);
                continue;
            }
        }
    }

    public void ClosePage(PageBase page)
    {
        if (openPagesStack.Count > 0 && openPagesStack.Peek() == page)
            openPagesStack.Pop();
        foreach (var item in allPages)
        {
            if (page == item)
            {
                item.gameObject.SetActive(false);
                foreach (var i in item.elements)
                {
                    i.gameObject.SetActive(false);
                }
                continue;
            }
        }
    }

    public void ClosePage<T>() where T : PageBase
    {
        if (openPagesStack.Count > 0 && openPagesStack.Peek() is T)
            openPagesStack.Pop();
        foreach (var item in allPages)
        {
            if (item is T)
            {
                item.gameObject.SetActive(false);
                foreach (var i in item.elements)
                {
                    i.gameObject.SetActive(false);
                }
                continue;
            }
        }
    }

    public T GetPageOfType<T>() where T : PageBase
    {
        for (int i = 0; i < allPages.Length; i++)
        {
            if (allPages[i] is T t)
                return t;
        }
        return null;
    }

    #endregion

    #region Dialog's functions

    public void OpenDialog(DialogBase dialog, bool forceOpen = false)
    {
        //if (openPagesStack.Count > 0)
        //{
        //    for (int i = openPagesStack.Count - 1; i >= 0; i--)
        //    {
        //        //List<UI_Elements> items = openPagesStack.Peek().items;
        //        //foreach (var item in items)
        //        //{
        //        //    item.gameObject.SetActive(false);
        //        //}
        //        //openPagesStack.Peek().gameObject.SetActive(false);
        //        //openPagesStack.Pop();
        //    }
        //}

        foreach (var item in allDialogs)
        {
            if (dialog == item)
            {
                item.gameObject.SetActive(true);
                item.SetValues();
                foreach (var i in item.elements)
                {
                    i.gameObject.SetActive(true);
                    i.SetValues();
                }
                //openPagesStack.Push(item);
                continue;
            }
        }
    }

    public void CloseDialog(DialogBase dialog)
    {
        //if (openPagesStack.Count > 0 && openPagesStack.Peek() == dialog)
        //    openPagesStack.Pop();

        foreach (var d in allDialogs)
        {
            if (dialog == d)
            {
                d.gameObject.SetActive(false);
                break;
            }
        }
    }

    public T GetDialogOfType<T>() where T : DialogBase
    {
        for (int i = 0; i < allDialogs.Length; i++)
        {
            if (allDialogs[i] is T t)
            {
                return t;
            }
        }
        return null;
    }

    #endregion

    #region Button's functions

    public void OnBackPressed()
    {
        if (openPagesStack != null && openPagesStack.Count > 0)
        {
            if (openPagesStack.Peek() == GetPageOfType<LevelPage>())
            {
                Application.Quit();
            }
            else
            {
                List<UIElementBase> items = openPagesStack.Peek().elements;
                foreach (var item in items)
                {
                    item.gameObject.SetActive(false);
                }
                openPagesStack.Peek().gameObject.SetActive(false);
                openPagesStack.Pop();

                if (openPagesStack.Count < 1)
                {
                    OpenPage(GetPageOfType<LevelPage>());
                }
                else
                {
                    OpenPage(openPagesStack.Pop());
                }
            }
        }
    }

    #endregion

    #region Elements's functions

    public T GetElementOfType<T>() where T : UIElementBase
    {
        for (int i = 0; i < allPages.Length; i++)
        {
            if (allElements[i] is T t)
                return t;
        }
        return null;
    }

    #endregion

    #region other functions

    /// <summary>
    /// close all pages and dialogs
    /// </summary>
    public void CloseEverythings()
    {
        foreach (var item in allDialogs)
        {
            item.gameObject.SetActive(false);
            foreach (var i in item.elements)
            {
                i.gameObject.SetActive(false);
            }
        }

        foreach (var item in allPages)
        {
            item.gameObject.SetActive(false);
            foreach (var i in item.elements)
            {
                i.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// set again current page or dialog element value
    /// </summary>
    public void UpdateUI()
    {
        var tempPage = openPagesStack.Peek();
        tempPage.SetValues();
        foreach (var item in tempPage.elements)
        {
            item.SetValues();
        }
    }

    public string GetActivePanelTitle()
    {
        foreach (var page in allPages)
        {
            if (page.gameObject.activeSelf)
            {
                return page.Title;
            }
        }
        foreach (var dialog in allDialogs)
        {
            if (dialog.gameObject.activeSelf)
            {
                return dialog.Title;
            }
        }
        return string.Empty;
    }

    #endregion
}


