using UnityEngine;

public class ChartSideTitleController : MonoBehaviour
{
  public void OnValueChange()
  {
    TestUIController tuc = GetComponentInParent<TestUIController>();
    if(tuc)
    {
      tuc.OnAxisTitleUpdate();
    }
  }
}