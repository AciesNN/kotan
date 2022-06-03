using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core
{
  public class NaiveDI
  {
    internal NaiveDI()
      => RegisterAll();

    Dictionary<Type, Type> interfaceToType = new Dictionary<Type, Type>();
    Dictionary<Type, object> interfaceToObject = new Dictionary<Type, object>();

    public void RegisterAll()
    {
      var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                  from type in assembly.GetTypes()
                  where type.IsDefined( typeof( RegisterDIAttribute ) ) && !type.IsAbstract
                  select type;
      foreach ( var type in types )
      {
        Register( type.GetCustomAttribute<RegisterDIAttribute>().interfaceType, type );
      }
    }

    public I Get<I>()
    {
      var interfaceType = typeof( I );

      if ( interfaceType == null || !interfaceToType.ContainsKey( interfaceType ) )
        throw new Exception( $"Type for {interfaceType} is unknown" );

      if ( !interfaceToObject.ContainsKey( interfaceType ) )
      {
        var type = interfaceToType[ interfaceType ];
        var newObj = Activator.CreateInstance( type );
        interfaceToObject.Add( interfaceType, newObj );
      }

      return ( I )interfaceToObject[ interfaceType ];
    }

    public void Register( Type interfaceType, Type simpleType )
    {
      if ( interfaceType == null || !interfaceType.IsInterface )
        throw new Exception( $"{nameof( interfaceType )} should be an interface, type: {interfaceType}" );

      if ( simpleType == null || simpleType.GetConstructor( Type.EmptyTypes ) == null )
        throw new Exception( $"{nameof( simpleType )} should have an empty constructor, type {simpleType}" );

      if ( !interfaceType.IsAssignableFrom( simpleType ) )
        throw new Exception( $"{simpleType} should implement an interface {interfaceType}" );

      if ( interfaceToType.ContainsKey( interfaceType ) )
        throw new Exception( $"{interfaceType} can't be registered for {simpleType}, already registered for {interfaceToType[ interfaceType ]}" );

      interfaceToType.Add( interfaceType, simpleType );
    }

    public void Register<I, T>()
      where I : class
      where T : new()
      => Register( typeof( I ), typeof( T ) );
  }
}