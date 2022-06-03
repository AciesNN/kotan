using Core;
using Core.Utils;
using UnityEngine;

public class hit : MonoBehaviour
{
  [SerializeField] float time = 0.5f;

  Timer t;
  private void OnEnable()
  {
    t = DI.Get<ITimerFactory>().New( time );
    t.On();
  }

  void Update()
  {
    if( t?.Check() ?? false )
    {
      t = null;
      gameObject.SetActive(false);
    }
  }
}
