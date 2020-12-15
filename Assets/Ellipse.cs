using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(LineRenderer))]
public class Ellipse : MonoBehaviour 
{
    public Vector2 radius = new Vector2(1f, 1f);
    public float width = 1f;
    public float rotationAngle = 45;
    public int resolution = 500;
    public float fociScale = 0.1f;

    public GameObject focus1;
    public GameObject focus2;
    public GameObject earth;
    public float lineSpeed = 0.2f;
    public Material lineMaterial;
    public float orbitSpeed = 0.05f;

    private ArrayList positions = new ArrayList();
    private Vector3[] foci = new Vector3[2];
    private LineRenderer self_lineRenderer;
    
    private void Start() 
    {
        Time.timeScale = 1.0f;
        StartCoroutine(DrawLinesForOneRevolution(0));
    }

    void OnValidate()
    {
        UpdateEllipse();
        FillFoci();
        InstantiateFoci();
    }

    private IEnumerator DrawLinesForOneRevolution(int i)
    {
        DrawLine(focus1, (Vector3) positions[i]);
        DrawLine(focus2, (Vector3) positions[i]);
        yield return new WaitForSeconds(orbitSpeed);
        if (i < positions.Count)
        {
            if (i == positions.Count - 1)
                StartCoroutine(DrawLinesForOneRevolution(0));
            else StartCoroutine(DrawLinesForOneRevolution(i + 1));
        }
    }
    
    public void UpdateEllipse()
    {
        if ( self_lineRenderer == null)
            self_lineRenderer = GetComponent<LineRenderer>();
            
        self_lineRenderer.positionCount = resolution+3;
        
        self_lineRenderer.startWidth = width;
        self_lineRenderer.endWidth = width;
        
        AddPointToLineRenderer(0f, 0);
        for (int i = 1; i <= resolution + 1; i++) 
        {
            AddPointToLineRenderer((float)i / (float)(resolution) * 2.0f * Mathf.PI, i);
        }
        AddPointToLineRenderer(0f, resolution + 2);
    }
    
    void AddPointToLineRenderer(float angle, int index)
    {
        Quaternion pointQuaternion = Quaternion.AngleAxis (rotationAngle, Vector3.forward);
        Vector3 pointPosition;
        
        pointPosition = new Vector3(radius.x * Mathf.Cos (angle), radius.y * Mathf.Sin (angle), 0.0f);
        pointPosition = pointQuaternion * pointPosition;
        
        self_lineRenderer.SetPosition(index, transform.position + pointPosition);
        positions.Add(transform.position + pointPosition);
    }

    void FillFoci()
    {
        float x = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(radius.x, 2) - Mathf.Pow(radius.y, 2)));
        foci[0] = new Vector3(transform.position.x - x, transform.position.y, transform.position.z);
        foci[1] = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
    }

    void InstantiateFoci()
    {
        focus1.transform.position = foci[0];
        focus1.transform.localScale = new Vector3(fociScale, fociScale, fociScale);
        focus2.transform.position = foci[1];
        focus2.transform.localScale = new Vector3(fociScale, fociScale, fociScale);
    }

    void DrawLine(GameObject start, Vector3 end)
    {
        // LineRenderer lr = start.GetComponent<LineRenderer>();
        // lr.material = lineMaterial;
        // lr.startWidth = width;
        // lr.endWidth = width;
        // lr.SetPosition(0, start.transform.position);
        // lr.SetPosition(1, end);
        earth.transform.position = end;
    }
}