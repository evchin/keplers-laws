using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitLines : MonoBehaviour
{
    [SerializeField] private GameObject        _sun;
    [SerializeField] private GameObject        _mercury;
    [SerializeField] private Material        _lineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DrawLine(_mercury.transform.position));
    }

    IEnumerator DrawLine(Vector3 pos)
    {
        LineRenderer line = (new GameObject("line")).AddComponent<LineRenderer>();
        line.material = _lineMaterial;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
        line.SetPosition(0, _sun.transform.position);
        line.SetPosition(1, pos);
        yield return new WaitForSeconds(0.5f);
        pos = _mercury.transform.position;
        StartCoroutine(DrawLine(pos));
    }
}
