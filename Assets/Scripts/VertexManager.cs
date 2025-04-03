using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VertexManager : MonoBehaviour
{
    public List<VerticesScript> vertexList;
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getTable()
    {
        string total = "";
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
            total += row + "\n";
        }
        text.text = total;
    }
}
