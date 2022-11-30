using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// draw the radar chart background line
public class RadarChartBaseInfo : MonoBehaviour
{
  public RadarChartBaseController _baseController;
  
    // Root for labels
  public RectTransform _root;
  public RectTransform _label;
  public Vector2 _offset = new Vector2(0, 15.0f);
  public List<string> _data;

  List<RectTransform> m_labelList = new List<RectTransform>();

  bool m_shouldUpdateLabel = false;
  
  public void UpdateLabels()
  {
    for (int i = 0; i < _data.Count; ++i)
    {
      UpdateText(i, _data[i]);
    } 
  }
  
  public void UpdateText(int index, string newText)
  {
    _data[index] = newText;
    Transform labelTransform = m_labelList[index];
    Text label = labelTransform.GetComponent<Text>();
    label.text = newText;
  }
  
  void OnEnable()
  {
    SetupLabels();
    UpdateLabels();
  }

  void OnDisable()
  {
    ClearLabels();
    _data.Clear();
  }

  void Update()
  {
    if (m_shouldUpdateLabel)
    {
      m_shouldUpdateLabel = false;
      SetupLabels();
      UpdateLabels();
    }
  }
  
  public void ClearLabels()
  {
    for (int i = _root.childCount - 1; i >= 0; --i)
    {
      Destroy(_root.GetChild(i).gameObject);
    }
    m_labelList.Clear();
  }
  
  public void SetupLabels()
  {
    if (_baseController == null)
      return;
    if (_label == null)
      return;
      
    List<UIVertex> _vertexList = _baseController._base.GetVerticesPosition();

    int dataDiff = _vertexList.Count - _data.Count;

    // Match the vertices total
    //Debug.Log(_data.Count + " diff: " + dataDiff + " vertex count: " + _vertexList.Count);
    if (dataDiff > 0)
    {
      for (int i = 0; i < dataDiff; ++i)
      {
        _data.Add("" + _data.Count);
      }
    }
    else if (dataDiff < 0)
    {
      for (int i = 0; i > dataDiff; --i)
      {
        _data.RemoveAt(_data.Count - 1);
      }
    }

    ClearLabels();
    // To match the obj total
    for (int i = 0; i < _vertexList.Count; ++i)
    {
      RectTransform txt = Instantiate(_label, _root);
      txt.gameObject.SetActive(true);
      Vector2 p = _vertexList[i].position;
      p -= _offset;
      txt.anchoredPosition = p;
      m_labelList.Add(txt);
    } 
  }

  void OnValidate()
  {
    m_shouldUpdateLabel = true;
  }
}