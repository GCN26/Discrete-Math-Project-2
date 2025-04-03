using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VertexManager : MonoBehaviour
{
    public List<VerticesScript> vertexList;
    public TextMeshProUGUI text;
    public string resultsN,resultsI,resultsO;
    public bool callforupdate;
    public TMP_Dropdown dropdown;

    public GameObject vertexGO;

    public enum displayResults
    {
        neutral,
        inR,
        outR
    }

    public displayResults resultChoice;

    public ToastFade toast;

    // Update is called once per frame
    void Update()
    {
        if(dropdown.value == 0)
        {
            resultChoice = displayResults.neutral;
        }
        else if(dropdown.value == 1)
        {
            resultChoice = displayResults.inR;
        }
        else if(dropdown.value == 2)
        {
            resultChoice = displayResults.outR;
        }
        switch (resultChoice)
        {
            case displayResults.neutral:
                text.text = resultsN;
                break;
            case displayResults.inR:
                text.text = resultsI;
                break;
            case displayResults.outR:
                text.text = resultsO;
                break;
        }
    }

    public void getTable()
    {
        resultsN = "";
        resultsI = "";
        resultsO = "";

        string row;
        for (int i = 0; i < vertexList.Count; i++)
        {
            row = "";
            for(int j = 0;j<vertexList.Count;j++)
            {
                row = row + vertexList[i].edgesNeutralCount[j].ToString();
                if(j != vertexList.Count - 1)
                {
                    row += ", ";
                }
            }
            resultsN += row + "\n";
        }

        for (int i = 0; i < vertexList.Count; i++)
        {
            row = "";
            for (int j = 0; j < vertexList.Count; j++)
            {
                row = row + vertexList[i].edgesInCount[j].ToString();
                if (j != vertexList.Count - 1)
                {
                    row += ", ";
                }
            }
            resultsI += row + "\n";
        }

        for (int i = 0; i < vertexList.Count; i++)
        {
            row = "";
            for (int j = 0; j < vertexList.Count; j++)
            {
                row = row + vertexList[i].edgesOutCount[j].ToString();
                if (j != vertexList.Count - 1)
                {
                    row += ", ";
                }
            }
            resultsO += row + "\n";
        }
    }

    public void updateLists(VerticesScript vert1)
    {
        if (vertexList.Count > 0)
        {
            for (int i = 0; i < vertexList.Count; i++)
            {
                if (vertexList[i] != vert1)
                {
                    vertexList[i].addVertexToLists(vert1);
                }
            }
        }
        else vert1.addVertexToLists(vert1);
    }

    public void createVert()
    {
        if (vertexList.Count < 16)
        {
            Instantiate(vertexGO);
        }
        else
        {
            toast.a = 1;
            toast.pressCount += 1;
        }

    }

    public void Reset()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
