using UnityEngine;

namespace Core.Utils
{
  public static class Mathfe
  {
    public static int SignOrZero( this float val )
      => Mathf.Approximately( val, 0 ) ? 0 : ( val > 0 ? 1 : -1 );
  }
}
