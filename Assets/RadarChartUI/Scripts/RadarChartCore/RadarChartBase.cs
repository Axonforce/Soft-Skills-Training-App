using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// draw the radar chart background line
public class RadarChartBase: Graphic
{
  public RadarChartController _chartController;

  public void SetParameters()
  {
    SetVerticesDirty();
  }

  public List<UIVertex> GetVerticesPosition()
  {
    float vol = _chartController._maxDisplayValue + 0.6f;
    List<UIVertex> _vertexList = new List<UIVertex>();

    _vertexList.Clear();

    var v = UIVertex.simpleVert;
    v.color = Color.red;

    // every vertex
    for (int i = 0; i < _chartController._verticesCount; i++)
    {
      float deg = (360f / _chartController._verticesCount) * 0.5f;
      float offset = (_chartController._lineWidth / Mathf.Cos(deg * Mathf.Deg2Rad)) / 2f;
      float rad = (90f - (360f / (float)_chartController._verticesCount) * i) * Mathf.Deg2Rad;

      float x = 0.5f + Mathf.Cos(rad) * (_chartController._radius * vol + offset);
      float y = 0.5f + Mathf.Sin(rad) * (_chartController._radius * vol + offset);

      Vector2 p = CreatePos(x, y);
      v.position = p;

      _vertexList.Add(v);
    }
    return _vertexList;
  }

  protected override void OnPopulateMesh(VertexHelper vh)
  {
    vh.Clear();

    var v = UIVertex.simpleVert;
    v.color = color;

    // Outer frame
    if(_chartController._showBaseFrame)
      DrawFrame(vh, _chartController._maxDisplayValue);

    // Axis
    if (_chartController._showBaseAxis)
      DrawAxis(vh, _chartController._maxDisplayValue);

    // Major Grid
    if (_chartController._drawBaseInnerGrid && _chartController._innerGridInterval < _chartController._maxDisplayValue)
    {
      int numOfGrid = (int)(_chartController._maxDisplayValue / _chartController._innerGridInterval);
      for (int i = 0; i < numOfGrid; i++)
      {
        DrawFrame(vh, i * _chartController._innerGridInterval);
      }
    }
  }

  // Coordination
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

  // Draw outer frame
  void DrawFrame(VertexHelper vh, float vol)
  {
    int currentVectCount = vh.currentVertCount;

    //var v = UIVertex.simpleVert;
    var v = UIVertex.simpleVert;
    v.color = color;

    // every vertice
    for (int i = 0; i < _chartController._verticesCount; i++)
    {
      float deg = (360f / _chartController._verticesCount) * 0.5f;
      float offset = (_chartController._lineWidth / Mathf.Cos(deg * Mathf.Deg2Rad)) / 2f;

      float rad = (90f - (360f / (float)_chartController._verticesCount) * i) * Mathf.Deg2Rad;

      float x1 = 0.5f + Mathf.Cos(rad) * (_chartController._radius * vol - offset);
      float y1 = 0.5f + Mathf.Sin(rad) * (_chartController._radius * vol - offset);
      float x2 = 0.5f + Mathf.Cos(rad) * (_chartController._radius * vol + offset);
      float y2 = 0.5f + Mathf.Sin(rad) * (_chartController._radius * vol + offset);

      Vector2 p1 = CreatePos(x1, y1);
      Vector2 p2 = CreatePos(x2, y2);

      v.position = p1;
      vh.AddVert(v);

      v.position = p2;
      vh.AddVert(v);

      vh.AddTriangle
      (
        (((i + 0) * 2) + 0) % (_chartController._verticesCount * 2) + currentVectCount,
        (((i + 0) * 2) + 1) % (_chartController._verticesCount * 2) + currentVectCount,
        (((i + 1) * 2) + 0) % (_chartController._verticesCount * 2) + currentVectCount
      );

      vh.AddTriangle
      (
        (((i + 1) * 2) + 0) % (_chartController._verticesCount * 2) + currentVectCount,
        (((i + 0) * 2) + 1) % (_chartController._verticesCount * 2) + currentVectCount,
        (((i + 1) * 2) + 1) % (_chartController._verticesCount * 2) + currentVectCount
      );
    }

  }

  // Draw axis
  void DrawAxis(VertexHelper vh, float vol)
  {
    int currentVertCount = vh.currentVertCount;

    var v = UIVertex.simpleVert;
    v.color = color;

    for (int i = 0; i < _chartController._verticesCount; i++)
    {
      float halfwidthDeg = 90 * _chartController._lineWidth / (Mathf.PI * _chartController._radius * vol);

      float rad1 = (90f - halfwidthDeg - (360f / (float)_chartController._verticesCount) * i) * Mathf.Deg2Rad;
      float rad2 = (90f + halfwidthDeg - (360f / (float)_chartController._verticesCount) * i) * Mathf.Deg2Rad;

      float x3 = 0.5f + Mathf.Cos(rad1) * _chartController._radius * vol;
      float y3 = 0.5f + Mathf.Sin(rad1) * _chartController._radius * vol;
      float x4 = 0.5f + Mathf.Cos(rad2) * _chartController._radius * vol;
      float y4 = 0.5f + Mathf.Sin(rad2) * _chartController._radius * vol;

      float x1 = 0.5f + (x3 - x4) / 2f;
      float y1 = 0.5f + (y3 - y4) / 2f;
      float x2 = 0.5f + (x4 - x3) / 2f;
      float y2 = 0.5f + (y4 - y3) / 2f;

      Vector2 p1 = CreatePos(x1, y1);
      Vector2 p2 = CreatePos(x2, y2);
      Vector2 p3 = CreatePos(x3, y3);
      Vector2 p4 = CreatePos(x4, y4);

      v.position = p1;
      vh.AddVert(v);

      v.position = p2;
      vh.AddVert(v);

      v.position = p3;
      vh.AddVert(v);

      v.position = p4;
      vh.AddVert(v);


      vh.AddTriangle
      (
        ((i * 4) + 0) + currentVertCount,
        ((i * 4) + 3) + currentVertCount,
        ((i * 4) + 2) + currentVertCount
      );


      vh.AddTriangle
      (
        ((i * 4) + 0) + currentVertCount,
        ((i * 4) + 1) + currentVertCount,
        ((i * 4) + 3) + currentVertCount
      );
    }
  }
}