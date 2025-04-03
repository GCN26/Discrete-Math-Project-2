using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LineDirection : MonoBehaviour
{
    public LineRenderer line;
    public GameObject lineDirectionObj;

    public VerticesScript vert1, vert2;

    public int linesBetween;
    void Start()
    {
    }
    private void Update()
    {
        if (lineDirectionObj != null)
        {
            Vector3 midpoint = (line.GetPosition(1) + line.GetPosition(0))/2;

            Vector3 direction = (line.GetPosition(1) - line.GetPosition(0)).normalized;

            lineDirectionObj.transform.position = midpoint;

            lineDirectionObj.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }
}
