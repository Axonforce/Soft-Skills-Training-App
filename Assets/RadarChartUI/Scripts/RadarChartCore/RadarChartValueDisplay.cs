using UnityEngine;
using UnityEngine.UI;

public class RadarChartValueDisplay : Graphic
{
  public RadarChartValueController _valueController;
  
  int m_verticesCount = 5;

  public void SetParameters()
  {
    if (_valueController == null)
      return;
    if (_valueController._chartController == null)
      return;
    m_verticesCount = _valueController._chartController._verticesCount;
    SetVerticesDirty();
  }

  protected override void OnPopulateMesh(VertexHelper vh)
  {
    if (_valueController == null)
      return;
    if (_valueController._chartController == null)
      return;
    if (_valueController._valueInfo == null)
      return;
 
    vh.Clear();
    var v = UIVertex.simpleVert;
    v.color = color;

    Vector2 center = CreatePos(0.5f, 0.5f);
    v.position = center;
    vh.AddVert(v);

    //every vertex
    if (_valueController._valueInfo._data.Count == m_verticesCount)
    {
      for (int i = 1; i <= m_verticesCount; i++)
      {
        float rad = (90f - (360f / (float)m_verticesCount) * (i - 1)) * Mathf.Deg2Rad;
        float x = 0.5f + Mathf.Cos(rad) * _valueController._chartController._radius * _valueController._valueInfo._data[i - 1];
        float y = 0.5f + Mathf.Sin(rad) * _valueController._chartController._radius * _valueController._valueInfo._data[i - 1];

        Vector2 p = CreatePos(x, y);
        v.position = p;
        vh.AddVert(v);

        vh.AddTriangle(0, i, i == m_verticesCount ? 1 : i + 1);
        //Debug.Log(i + " pos: " + p);
      }
    }

  }

  Vector2 CreatePos(float x, float y)
  {
    Vector2 p = Vector2.zero;
    p.x -= rectTransform.pivot.x;
    p.y -= rectTransform.pivot.y;
    p.x += x;
    p.y += y;
    p.x *= rectTransform.rect.width;
    p.y *= rectTransform.rect.height;
    return p;
  }

  protected override void OnEnable()
  {
    base.OnEnable();

    if (_valueController == null)
      return;
    if (_valueController._chartController == null)
      return;
    if (_valueController._chartController._randomValueDisplayColor)
    {
      color = new Color
      (
        Random.Range(0f, 1f), 
        Random.Range(0f, 1f), 
        Random.Range(0f, 1f),
        // So the color won't be too transparent to display
        Random.Range(0.5f, 1f)
      );
    }
    
  }
}