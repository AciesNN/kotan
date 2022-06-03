using System;

namespace Core
{
  [AttributeUsage( AttributeTargets.Class )]
  public class RegisterDIAttribute : Attribute
  {
    public Type interfaceType;

    public RegisterDIAttribute( Type interfaceType )
    {
      this.interfaceType = interfaceType;
    }
  }
}