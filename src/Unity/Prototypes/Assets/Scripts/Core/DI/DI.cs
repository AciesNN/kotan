namespace Core
{
  public static class DI
  {
    static NaiveDI instance;
    public static I Get<I>()
      => ( instance ??= new NaiveDI() ).Get<I>();
  }
}
