using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarChartValueInfo : MonoBehaviour
{
  public RadarChartValueController _valueController;
  public RadarChartValueDisplay _displayController;
  
    // Root for labels
  public RectTransform _root;
  public RectTransform _label;
  public Vector2 _offset = new Vector2(0, 15.0f);
  public List<float> _data;
  [Range(0f, 1f)]
  public float _defaultValue = 0.5f;
  
  List<RectTransform> m_labelList = new List<RectTransform>();
  bool m_shouldUpdate = true;

  public void UpdateLabels()
  {
    if (_label == null)
      return;
      
    if (_valueController == null)
      return;
    if (_valueController._chartController == null)
      return;
      
    List<UIVertex> _vertexList = _valueController._chartController._baseController._base.GetVerticesPosition();

    while(_data.Count < _vertexList.Count)
    {
      //string info = "0.5";
      _data.Add(_defaultValue);
      // Add ui
      RectTransform txt = Instantiate(_label, _root);
      txt.gameObject.SetActive(true);
      m_labelList.Add(txt);
    }
    
    while(_data.Count > _vertexList.Count)
    {
      _data.RemoveAt(_data.Count - 1);
      // Remove ui
      Destroy(m_labelList[m_labelList.Count-1].gameObject);
      m_labelList.RemoveAt(m_labelList.Count - 1);
    }
    
    // Update positions & values
    for (int i = 0; i < _vertexList.Count; i++)
    {
      RectTransform txt = m_labelList[i];
      Vector2 p = _vertexList[i].position;
      p -= _offset;
      txt.anchoredPosition = p;
      UpdateValue(i, _data[i]);
    }
  }
  
  public Transform GetLabel(int index)
  {
    return m_labelList[index];
  }
  
  public void UpdateValue(int index, float newValue)
  {
    //Debug.Log("Update new value: " + index + " newValue: " + newValue);
    _data[index] = newValue;
    Transform labelTransform = GetLabel(index);
    Text label = labelTransform.GetComponent<Text>();
    if (_valueController._chartController._showValueData)
      label.text = "" + newValue;
    else
      label.text = "";
  }
  
  void ResetLabels()
  {
    ClearLabels();
    UpdateLabels();
  }
  
  void OnEnable()
  {
    ResetLabels();
  }

  void OnDisable()
  {
    ClearLabels();
  }
  
  void ClearLabels()
  {
    for (int i = _root.childCount - 1; i >= 0; --i)
    {
      Destroy(_root.GetChild(i).gameObject);
    }
    _data.Clear();
    m_labelList.Clear();
  }

  void Update()
  {
    if (m_shouldUpdate)
    {
      m_shouldUpdate = false;
      if (_displayController)
      {
        _displayController.SetParameters();
      }
    }
  }
  
  void OnValidate()
  {
    // Value changed from editor 
    m_shouldUpdate = true;
  }
}