using UnityEngine;

public class ChartSideValueController : MonoBehaviour
{
  public void OnValueChange()
  {
    TestUIController tuc = GetComponentInParent<TestUIController>();
    if(tuc)
    {
      tuc.OnAxisValueUpdate();
    }
  } 
}