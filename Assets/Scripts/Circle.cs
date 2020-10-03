using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Circle : MonoBehaviour
{
    public int vertexCount = 40;
    public float lineWidth = 0.2f;
    public float angle = 0f;
    public float xRadius;
    public float yRadius;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetColor(Color.green);
        SetupCircle();
    }

    private void SetupCircle()
    {
        lineRenderer.widthMultiplier = lineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = angle;

        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 pos = new Vector3(xRadius * Mathf.Cos(theta), yRadius * Mathf.Sin(theta), 0f);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    public void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    float deltaTheta = (2f * Mathf.PI) / vertexCount;
    //    float theta = angle;

    //    Vector3 oldPos = Vector3.zero;
    //    for (int i = 0; i < vertexCount + 1; i++)
    //    {
    //        Vector3 pos = new Vector3(transform.lossyScale.x * xRadius * Mathf.Cos(theta), transform.lossyScale.y * yRadius * Mathf.Sin(theta), 0f);
    //        Gizmos.DrawLine(oldPos, transform.position + pos);
    //        oldPos = transform.position + pos;

    //        theta += deltaTheta;
    //    }
    //}
#endif

}
