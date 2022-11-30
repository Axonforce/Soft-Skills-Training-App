using UnityEngine;

public class RadarChartValueController : MonoBehaviour
{
  public RadarChartController _chartController;
  public RadarChartValueDisplay _value;
  public RadarChartValueInfo _valueInfo;
  
  public void UpdateGraphic()
  {
    _valueInfo.UpdateLabels();
    _value.SetParameters();
  }

  void OnEnable()
  {
    if (_chartController == null)
    {
      _chartController = GetComponentInParent<RadarChartController>();
    }
  }
}