using System.Collections.Generic;
using UnityEngine;

public class RadarChartController : MonoBehaviour
{
  //public RadarChartConstants.RaderChartType _style = RadarChartConstants.RaderChartType.Normal;
  public List<GameObject> _valueList = new List<GameObject>();
  
  public RadarChartBaseController _baseController;
  public GameObject _valueListRoot;
  public GameObject _valuePrefab;

  public bool _drawBaseInnerGrid = true;
  public bool _showBaseFrame = true;
  public bool _showBaseAxis = true;
  public bool _showValueData = true;
  public bool _randomValueDisplayColor = true;

  [Range(3, 10)]
  public int _verticesCount = 5;

  [Range(1, 5)]
  public int _valueCount = 1;

  // Radius of the chart 
  [Range(0.5f, 5.0f)]
  public float _radius = 0.5f;

  [Range(0.01f, 1f)]
  public float _innerGridInterval = 0.2f;

  [Range(0.001f, 0.03f)]
  public float _lineWidth = 0.02f;

  [Range(0f, 1f)]
  public float _maxDisplayValue = 1f;

  bool m_shouldUpdate = false;

  public void UpdateSystem()
  {
    _baseController.UpdateGraph();
    SetupValueList();
    UpdateValueList();
  }
  
  // Use this for initialization
  void OnEnable()
  {
    _baseController.UpdateGraph();
    UpdateValueList();
  }

  void Update()
  {
    if (m_shouldUpdate)
    {
      m_shouldUpdate = false;
      _baseController.UpdateGraph();
      SetupValueList();
      UpdateValueList();
    }
  }

  public void UpdateValueList(int indexToUpdate)
  {
    RadarChartValueController valueController = _valueList[indexToUpdate].GetComponentInChildren<RadarChartValueController>();
      if (valueController)
        valueController.UpdateGraphic();
  }
  
  public void UpdateValueList()
  {
    foreach (GameObject obj in _valueList)
    {
      RadarChartValueController valueController = obj.GetComponentInChildren<RadarChartValueController>();
      if (valueController)
        valueController.UpdateGraphic();
    }
  }

  public void SetupValueList()
  {
    if (_valueListRoot == null)
      return;
    int total = _valueCount - _valueList.Count;
    if (total > 0)
    {
      for (int i = 0; i < total; ++i)
      {
        GameObject instObj = Instantiate(_valuePrefab, _valueListRoot.transform);
        instObj.name = "Value " + (_valueList.Count + 1);
        _valueList.Add(instObj);
      }
    }
    else if (total < 0)
    {
      for (int i = 0; i > total; i--)
      {
        Destroy(_valueList[_valueList.Count - 1]);
        _valueList.RemoveAt(_valueList.Count - 1);
      }
    }
  }

  void OnValidate()
  {
    // Value changed from editor 
    m_shouldUpdate = true;
  }
}