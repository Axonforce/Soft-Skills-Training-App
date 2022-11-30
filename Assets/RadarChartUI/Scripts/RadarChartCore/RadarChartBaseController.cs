using System.Collections.Generic;
using UnityEngine;

public class RadarChartBaseController : MonoBehaviour
{
  public RadarChartBase _base;
  public RadarChartBaseInfo _baseInfo;

  public void UpdateGraph()
  {
    _base.SetParameters();
    _baseInfo.SetupLabels();
    _baseInfo.UpdateLabels();
  }
  
  void OnEnable()
  {
    UpdateGraph();
  }
}
