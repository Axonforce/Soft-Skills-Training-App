using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TestUIController : MonoBehaviour
{
  public RadarChartController _chartController;
  public Dropdown _valueListDropDown;
  public Toggle _drawBaseInnerGrid;
  public Toggle _showBaseFrame;
  public Toggle _showBaseAxis;
  public Toggle _showValueData;
  public Slider _radius;
  public Slider _innerGridInterval;
  public Slider _lineWidth;
  public Slider _displayValue;
  
  public GameObject _axisBaseCell;
  public GameObject _axisValueCell;
  public GameObject _baseCellRoot;
  public GameObject _valueCellRoot;
  

  bool m_canUpdateValue = false;
  int m_sideTotal = 6;
  int m_valueTotal = 1;
  
  public void OnTogglesValueUpdate()
  {
    _chartController._drawBaseInnerGrid = _drawBaseInnerGrid.isOn;
    _chartController._showBaseFrame = _showBaseFrame.isOn;
    _chartController._showBaseAxis = _showBaseAxis.isOn;
    _chartController._showValueData = _showValueData.isOn;
    _chartController.UpdateSystem();
  }
  
  public void OnRadiusSliderValueUpdate()
  {
    _chartController._radius = _radius.value;
    _chartController.UpdateSystem();
  }
  
  public void OnInnerGridIntervalSliderValueUdpate()
  {
    _chartController._innerGridInterval = _innerGridInterval.value;
    _chartController.UpdateSystem();
  }
  
  public void OnLineWidthSliderValueUpdate()
  { 
    _chartController._lineWidth = _lineWidth.value;
    _chartController.UpdateSystem();
  }
  
  public void OnMaxDisplaySliderValueUpdate()
  { 
    _chartController._maxDisplayValue = _displayValue.value;
    _chartController.UpdateSystem();
  }
    
  public void OnAxisTitleUpdate()
  {
    for (int i = 0; i < _baseCellRoot.transform.childCount; ++i)
    {
      Text t = _baseCellRoot.transform.GetChild(i).GetComponentInChildren<Text>();
      _chartController._baseController._baseInfo.UpdateText(i, t.text);
    } 
  }
  
  public void OnAxisValueUpdate()
  {
    if (!m_canUpdateValue)
      return;

    RadarChartValueInfo info = _chartController._valueList[_valueListDropDown.value].GetComponentInChildren<RadarChartValueInfo>();
    
    for (int i = 0; i < _valueCellRoot.transform.childCount; ++i)
    {
      Slider s = _valueCellRoot.transform.GetChild(i).GetComponentInChildren<Slider>();
      // Assuming both lists have same size
      info._data[i] = s.value;
      info.UpdateValue(i, s.value);
    }

    info._valueController._value.SetParameters();
    
  }
  
  public void OnValueRemove()
  {
    m_valueTotal--;
    m_valueTotal = Mathf.Clamp(m_valueTotal, 1, 1000);
    UpdateValueList();
  }
  
  public void OnValueAdd()
  {
    m_valueTotal++;
    UpdateValueList();
  }
  
  public void OnValueDropDownSelect()
  {
    // Get the value data from the selected 
    UpdateValueAxisDesc();
  }
  
  public void OnSideRemove()
  {
    m_sideTotal--;
    m_sideTotal = Mathf.Clamp(m_sideTotal, 3, 1000);
    UpdateSideValue();
  }
  
  public void OnSideAdd()
  {
    m_sideTotal++;
    UpdateSideValue();
  }
  
  void UpdateSideValue()
  {
    _chartController._verticesCount = m_sideTotal;
    _chartController.UpdateSystem();
    Invoke("UpdateBaseAxisDesc", 0.5f);
    Invoke("UpdateValueAxisDesc", 0.5f);
  }
  
  void UpdateValueList()
  {
    _chartController._valueCount = m_valueTotal;
    _chartController.SetupValueList();
    _chartController.UpdateValueList();

    _valueListDropDown.ClearOptions();
    List<string> options = new List<string>();
    for (int i = 0; i < _chartController._valueList.Count; ++i)
    {
      options.Add(_chartController._valueList[i].name);
    }
    _valueListDropDown.AddOptions(options);
    UpdateValueAxisDesc();
  }
  
  void OnEnable()
  {
    UpdateValueList();
    UpdateBaseAxisDesc();
    UpdateValueAxisDesc();
    UpdateProperties();
  }
  
  void UpdateBaseAxisDesc()
  {
    DestroyChildren(_baseCellRoot.transform);
    // Add titles to root
    //Debug.Log("size: " + _radar._axisLabels.Count);
    foreach(string axisTitle in _chartController._baseController._baseInfo._data)
    {
      GameObject obj = Instantiate(_axisBaseCell, _baseCellRoot.transform);
      InputField input = obj.GetComponent<InputField>();
      input.text = axisTitle;
    }
    
  }
  
  void UpdateValueAxisDesc()
  {
    m_canUpdateValue = false;
    // Clean up first
    DestroyChildren(_valueCellRoot.transform);
    // Get the selected data 
    RadarChartValueInfo info = _chartController._valueList[_valueListDropDown.value].GetComponentInChildren<RadarChartValueInfo>();

    for (int i = 0; i < info._data.Count; ++i)
    {
      float value = info._data[i];
      GameObject obj = Instantiate(_axisValueCell, _valueCellRoot.transform);
      Slider s = obj.GetComponent<Slider>();
      s.value = value;
      info.UpdateValue(i, s.value);
    }
    
    m_canUpdateValue = true;
  }
  
  void UpdateProperties()
  {
    _drawBaseInnerGrid.isOn = _chartController._drawBaseInnerGrid;
    _showBaseFrame.isOn = _chartController._showBaseFrame;
    _showBaseAxis.isOn = _chartController._showBaseAxis;
    _showValueData.isOn = _chartController._showValueData;
    _radius.value = _chartController._radius;
    _innerGridInterval.value = _chartController._innerGridInterval;
    _lineWidth.value = _chartController._lineWidth;
    _displayValue.value = _chartController._maxDisplayValue;
  }
  
  void DestroyChildren(Transform t)
  {
    for (int i = t.childCount - 1; i >= 0; --i)
    {
      //Debug.Log("Destroy: " + t.GetChild(i).gameObject);
      Destroy(t.GetChild(i).gameObject);
    }

    t.DetachChildren();
  }
}