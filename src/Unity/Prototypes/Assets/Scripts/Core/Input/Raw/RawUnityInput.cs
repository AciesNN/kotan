using Core.Utils;

namespace Core.Input
{
  //wrapper upon an old Unity Input system can return if GET_AXIS_RAW_SIGN and GET_BUTTON_DOWN
  public class RawUnityInput : IInput
  {
    public static readonly RawUnityInput Default = new RawUnityInput();

    //GetAxesRawSign
    public int GetAxisRawSign( string name )
      => UnityEngine.Input.GetAxisRaw( name ).SignOrZero();

    //GetButtonDown
    public bool GetButtonDown( string name )
      => UnityEngine.Input.GetButtonDown( name );

    public void Update() { }
  }
}