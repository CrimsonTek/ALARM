using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ALARM.TerrariaStuff;

namespace ALARM
{
    /// <summary>
    /// A class for System.Reflection related functions as well as storing cache for types
    /// </summary>
    public static class ReflectionCache
    {
        /// <summary>Any protection</summary>
        public const BindingFlags AnyProtection = BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Gets a method with a selected attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute to get.</typeparam>
        /// <param name="type">The type within which to search for the method.</param>
        /// <param name="parameterTypes">The array of types the method must match. If no parameters are expected, pass null. By default, this allows parameters that are assignable from this array.</param>
        /// <param name="returnType">The type of the return value. If no return value is expected, pass null. By default, this allows the return value to be assignable to this value.</param>
        /// <param name="methodInfo">The method info which has this attribute.</param>
        /// <param name="attribute">The attribute on this method.</param>
        /// <param name="exceptionThrowing">Whether or not to force exception throwing. Pass null to use the <see cref="ALARMConfigServer"/>'s settings.</param>
        /// <param name="bindingAttr">The method binding attribute.</param>
        /// <param name="exactParamTypes">Whether the method parameter types should have to match the expected parameter types exactly.</param>
        /// <param name="exactReturnValue">Whether the return type should have the match the expected return type exactly.</param>
        /// <returns></returns>
        public static bool TryGetMethodWithAttribute<TAttribute>(
            Type type,
            Type[] parameterTypes,
            Type returnType,
            out MethodInfo methodInfo,
            out TAttribute attribute,
            bool exceptionThrowing = false,
            BindingFlags bindingAttr = AnyProtection | BindingFlags.Static,
            bool exactParamTypes = false,
            bool exactReturnValue = false)
            where TAttribute : Attribute
        {
            Type[] paramTypes = parameterTypes ?? Type.EmptyTypes;
            Type _return = returnType ?? typeof(void);
            foreach (MethodInfo method in type.GetMethods(bindingAttr))
            {
                TAttribute custom = method.GetCustomAttribute<TAttribute>();
                if (custom == null)
                {
                    continue;
                }

                ParameterInfo[] arrParam = method.GetParameters();

                if (arrParam.Length != paramTypes.Length)
                {
                    if (exceptionThrowing)
                    {
                        throw new TargetParameterCountException($"{method} has the attribute {typeof(TAttribute)}, but the usage is incorrect. It should have {paramTypes.Length} parameters.");
                    }
                    break;
                }

                for (int j = 0; j < arrParam.Length; j++)
                {
                    ParameterInfo param = arrParam[j];

                    if (exactParamTypes)
                    {
                        if (param.ParameterType != paramTypes[j])
                        {
                            if (exceptionThrowing)
                            {
                                throw new Exception($"{method} has the attribute {typeof(TAttribute)}, but the usage is incorrect. Parameters {j} cannot be assigned from expected parameter {paramTypes[j]}.");
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (!param.ParameterType.IsAssignableFrom(paramTypes[j]))
                        {
                            if (exceptionThrowing)
                            {
                                throw new Exception($"{method} has the attribute {typeof(TAttribute)}, but the usage is incorrect. Parameters {j} is not expected parameter {paramTypes[j]}.");
                            }
                            break;
                        }
                    }
                }

                if (exactReturnValue)
                {
                    if (_return != method.ReturnType)
                    {
                        if (exceptionThrowing)
                        {
                            throw new Exception($"{method} has the attribute {typeof(TAttribute)}, but the usage is incorrect. The return type is not expected type {method.ReturnType}.");
                        }
                    }
                }
                else
                {
                    if (!_return.IsAssignableFrom(method.ReturnType))
                    {
                        if (exceptionThrowing)
                        {
                            throw new Exception($"{method} has the attribute {typeof(TAttribute)}, but the usage is incorrect. The return type cannot be assigned to expected type {method.ReturnType}.");
                        }
                    }
                }

                methodInfo = method;
                attribute = custom;
                return true;
            }
            methodInfo = default;
            attribute = default;
            return false;
        }
    }
}
