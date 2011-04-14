using System;
using System.Reflection.Emit;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Vishcious.ArcGIS.SLContrib
{
    /// <summary>
    /// DynamicTypeFactory class
    /// </summary>
    public class TypeFactory
    {

        /// <summary>
        /// Creates the type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="properties">The property names and its corresponding types.</param>
        /// <returns></returns>
        public static Type CreateType( string name, IDictionary<string, Type> properties )
        {
            // Define dynamic assembly
            var assemblyName = new AssemblyName( "DynamicAssembly" );

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly( assemblyName, AssemblyBuilderAccess.Run );
            var module = assemblyBuilder.DefineDynamicModule( "DynamicModule" );

            // Define type
            var typeBuilder = module.DefineType(
                name,
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout,
                typeof( AttributesBase ) );

            var baseConstructor = typeof( AttributesBase ).GetConstructor( new[] { typeof( IAttributes ) } );

            // Define constructor
            var constructor = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new[] { typeof( IAttributes ) } );

            var ilGen = constructor.GetILGenerator();
            ilGen.Emit( OpCodes.Ldarg_0 );
            ilGen.Emit( OpCodes.Ldarg_1 );
            ilGen.Emit( OpCodes.Call, baseConstructor );
            ilGen.Emit( OpCodes.Ret );

            // Prepare to define properties
            var getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            var getMethodInfo = typeof( AttributesBase ).GetMethod( "GetValue", BindingFlags.Public | BindingFlags.Instance );
            var setMethodInfo = typeof( AttributesBase ).GetMethod( "SetValue", BindingFlags.Public | BindingFlags.Instance );

            foreach( var property in properties )
            {
                // Define property get method
                var getMethod = typeBuilder.DefineMethod( "get_" + property, getSetAttr, property.Value, Type.EmptyTypes );
                ilGen = getMethod.GetILGenerator();
                ilGen.Emit( OpCodes.Ldarg_0 );
                ilGen.Emit( OpCodes.Ldstr, property.Key );
                ilGen.Emit( OpCodes.Callvirt, getMethodInfo );
                ilGen.Emit( OpCodes.Ret );

                // Define property set method
                var setMethod = typeBuilder.DefineMethod( "set_" + property, getSetAttr, null, new[] { property.Value } );
                ilGen = setMethod.GetILGenerator();
                ilGen.Emit( OpCodes.Ldarg_0 );
                ilGen.Emit( OpCodes.Ldstr, property.Key );
                ilGen.Emit( OpCodes.Ldarg_1 );
                ilGen.Emit( OpCodes.Callvirt, setMethodInfo );
                ilGen.Emit( OpCodes.Ret );

                // Define property
                var propertyBuilder = typeBuilder.DefineProperty( property.Key, PropertyAttributes.HasDefault, property.Value, Type.EmptyTypes );

                propertyBuilder.SetGetMethod( getMethod );
                propertyBuilder.SetSetMethod( setMethod );
            }

            // Materialize type
            return typeBuilder.CreateType();
        }

        /// <summary>
        /// Creates the type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public static Type CreateType( string name, IEnumerable<string> properties )
        {
            Dictionary<string, Type> list = new Dictionary<string, Type>();

            foreach( string entry in properties )
            {
                list.Add( entry, typeof( object ) );
            }

            return CreateType(name, list);
        }
    }
}