namespace Core.Input
{
  public interface IInput
  {
    int GetAxisRawSign( string name );

    bool GetButtonDown( string name );

    void Update();
  }
}
