using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class VerticesScript : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public SpriteRenderer spriteRenderer;

    public bool selected;
    public bool hover;
    //Vertex and Int lists share an index
    public List<VerticesScript> edgesIn, edgesOut,edgesNeutral;
    public List<int> edgesInCount, edgesOutCount, edgesNeutralCount;

    public GameObject inPos, outPos, neutralPos;

    public VertexManager manager;
    public TextMeshProUGUI number;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < manager.vertexList.Count; i++)
        {
            edgesIn.Add(manager.vertexList[i]);
            edgesOut.Add(manager.vertexList[i]);
            edgesNeutral.Add(manager.vertexList[i]);
            edgesInCount.Add(0);
            edgesOutCount.Add(0);
            edgesNeutralCount.Add(0);
            if (manager.vertexList[i] == this)
            {
                number.text = (i+1).ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selected && !hover)
        {
            spriteRenderer.color = Color.red;
        }
        else if(!selected && hover)
        {
            spriteRenderer.color = Color.blue;
        }
        else if(!selected && !hover)
        {
            spriteRenderer.color = Color.white;
        }
    }

    private void OnMouseOver()
    {
    }
    /// <summary>
    /// InOutNeutral: 0 = In, 1 = Out, 2 = Neutral
    /// </summary>
    /// <param name="altVert"></param>
    /// <param name="InOutNeutral"></param>
    public void calcLines(VerticesScript altVert,int InOutNeutral)
    {
        switch (InOutNeutral)
        {
            case 0:
                if (edgesIn.Count > 0)
                {
                    for (int i = 0; i < edgesIn.Count; i++)
                    {
                        if (altVert == edgesIn[i])
                        {
                            edgesInCount[i] += 1;
                            break;
                        }
                        else if (altVert != edgesIn[i] && i == edgesIn.Count - 1)
                        {
                            edgesIn.Add(altVert);
                            edgesInCount.Add(0);
                        }
                    }
                }
                else
                {
                    edgesIn.Add(altVert);
                    edgesInCount.Add(1);
                }
                break;
            case 1:
                if (edgesOut.Count > 0)
                {
                    for (int i = 0; i < edgesOut.Count; i++)
                    {
                        if (altVert == edgesOut[i])
                        {
                            edgesOutCount[i] += 1;
                            break;
                        }
                        else if (altVert != edgesOut[i] && i == edgesOut.Count - 1)
                        {
                            edgesOut.Add(altVert);
                            edgesOutCount.Add(0);
                        }
                    }
                }
                else
                {
                    edgesOut.Add(altVert);
                    edgesOutCount.Add(1);
                }
                break;
            case 2:
                if (edgesNeutral.Count > 0)
                {
                    for (int i = 0; i < edgesNeutral.Count; i++)
                    {
                        if (altVert == edgesNeutral[i])
                        {
                            edgesNeutralCount[i] += 1;
                            break;
                        }
                        else if (altVert != edgesNeutral[i] && i == edgesNeutral.Count - 1)
                        {
                            edgesNeutral.Add(altVert);
                            edgesNeutralCount.Add(0);
                        }
                    }
                }
                else
                {
                    edgesNeutral.Add(altVert);
                    edgesNeutralCount.Add(1);
                }
                break;
        }
    }

}
