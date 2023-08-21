using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ContentPageChanger : MonoBehaviour
{
    List<GameObject> content = new();
    [SerializeReference] TextMeshProUGUI pageLabel;
    [SerializeField] int entriesPerPage = 3;
    int currentPage = 1;

    int pageCount
    {
        get
        {
            return (content.Count % entriesPerPage > 0 ? (int)(content.Count / entriesPerPage) + 1 : (int)content.Count / entriesPerPage);
        }
    }

    void Start()
    {
        ClearChildrenOnStart();
    }

    void ClearChildrenOnStart()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // Get the i-th child transform
            Transform childTransform = transform.GetChild(i);
            //Destroy(childTransform.gameObject);
        }
    }

    public void Clear()
    {
        int childCount = transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }


    public void Refresh()
    {

        GetAllChildren();
        VieWCurrentPage();
    }

    public void AddChild(GameObject b)
    {
        if (content.Contains(b))
        {
            throw new System.Exception(name +" - ContentPageChanger.AddChild() - Managed list already contains object =>" + b.name);
        }
        content.Add(b);
    }

    void GetAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // Get the i-th child transform
            Transform childTransform = transform.GetChild(i);
            Debug.Log("Adding child " + childTransform.name + " to ContentPageChanger");
            // Do something with the child transform
            AddChild(childTransform.gameObject);
        }
    }

    public void ChangePage(bool forward)
    {
        Debug.Log("Clicked page change btn.");
        if (forward)
        {
            currentPage = Mathf.Clamp(currentPage - 1, 1, pageCount);
        }
        else currentPage = Mathf.Clamp(currentPage + 1, 1, pageCount);
        VieWCurrentPage();
    }
    public void VieWCurrentPage()
    {
        foreach (var item in content)
        {
            item.SetActive(false);
        }

        List<GameObject> objectsToActivate = content.Skip((currentPage - 1) * entriesPerPage).Take(entriesPerPage).ToList();

        foreach (var item in objectsToActivate)
        {
            item.SetActive(true);
        }
        pageLabel.text = currentPage.ToString() + "/" + pageCount.ToString();
    }
}
