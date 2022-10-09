using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchDraw : MonoBehaviour
{
    private GameObject drawObject;
    private LineRenderer line;
    private Coroutine drawing;
    private List<Vector3> points = new List<Vector3>();
    
    public ObjectPool pooledObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            FinishLine();
        }
    }
    
    void StartLine()
    {
        if (drawing != null)
        {
            StopCoroutine(drawing);
        }
        pooledObject.DisablesObjects();
        drawing = StartCoroutine(DrawLine());
    }

    void FinishLine()
    {
        Destroy(drawObject);
        StopCoroutine(drawing);

        for (int i = 0; i < points.Count; i++)
        {
            var primitiveObj = pooledObject.GetPooledObject();
            primitiveObj.SetActive(true);
            primitiveObj.transform.position = points[i];
        }
        points.Clear();
    }

    public IEnumerator DrawLine()
    {
        drawObject = Instantiate(Resources.Load("Line") as GameObject, new Vector3(0,0,0), Quaternion.identity);
        line = drawObject.GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = 0;
        
        while (true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
            position.z = 0;
            line.positionCount++;
            line.SetPosition(line.positionCount-1, position);
            points.Add(position);
            yield return null;
        }
    }
}
