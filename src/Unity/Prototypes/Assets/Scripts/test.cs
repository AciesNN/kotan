using Core;
using UnityEngine;

public class test : MonoBehaviour
{
  private void Start()
  {
    //TestDI();
  }

  private static void TestDI()
  {
    var t = DI.Get<Core.Utils.ITimerFactory>().New( 1 );
    Debug.Log( $"? {t.Check()}" );
  }
}
