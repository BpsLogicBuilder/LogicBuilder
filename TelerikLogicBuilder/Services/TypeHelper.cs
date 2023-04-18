using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class TypeHelper : ITypeHelper
    {
        private readonly IExceptionHelper _exceptionHelper;

        public TypeHelper(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public bool AreCompatibleForOperation(Type t1, Type t2, CodeBinaryOperatorType op)
        {
            t1 = IsNullable(t1) ? Nullable.GetUnderlyingType(t1)!/*Not null for nullables*/ : t1;
            t2 = IsNullable(t2) ? Nullable.GetUnderlyingType(t2)!/*Not null for nullables*/ : t2;

            if (t1.IsValueType
                && t2.IsValueType
                && op == CodeBinaryOperatorType.ValueEquality
                && (AssignableFrom(t1, t2) || AssignableFrom(t2, t1)))
                return true;

            if (op == CodeBinaryOperatorType.IdentityEquality || op == CodeBinaryOperatorType.IdentityInequality)
                return AssignableFrom(t1, t2) || AssignableFrom(t2, t1);

            if (op == CodeBinaryOperatorType.BooleanAnd || op == CodeBinaryOperatorType.BooleanOr)
                return t1 == typeof(bool) && t2 == typeof(bool);

            if (op == CodeBinaryOperatorType.BitwiseAnd || op == CodeBinaryOperatorType.BitwiseOr)
            {
                return (t1 == typeof(bool) && t2 == typeof(bool))
                    || (AssigneableToLong(t1) && AssigneableToLong(t2))
                    || (AssigneableToULong(t1) && AssigneableToULong(t2));

                bool AssigneableToLong(Type t) =>
                t == typeof(long)
                || NumbersDictionary[typeof(long)].Contains(t)
                || ImplicitConversionExistsFrom(typeof(long), t);

                bool AssigneableToULong(Type t) =>
                    t == typeof(ulong)
                    || NumbersDictionary[typeof(ulong)].Contains(t)
                    || ImplicitConversionExistsFrom(typeof(ulong), t);
            }

            if (op == CodeBinaryOperatorType.Add//compiler converts to string.Concat()
                && (t1 == typeof(string) || ImplicitConversionExistsFrom(typeof(string), t1))
                && (t2 == typeof(string) || ImplicitConversionExistsFrom(typeof(string), t2)))
            {
                return true;
            }

            return AreCompatibleForOperation(t1, t2, OperatorsesDictionary[op]);
        }

        public bool AssignableFrom(Type to, Type from)
        {
            if (to.IsAssignableFrom(from))
                return true;

            if (!(!IsNullable(to) && IsNullable(from)))
            {//Anything but To is NOT nullable and From IS nullable
                to = IsNullable(to) ? Nullable.GetUnderlyingType(to)!/*Not null for nullables*/ : to;
                from = IsNullable(from) ? Nullable.GetUnderlyingType(from)!/*Not null for nullables*/ : from;

                if (NumbersDictionary.ContainsKey(to) && NumbersDictionary[to].Contains(from))
                    return true;
            }

            bool ReturnTypeValid(Type returnType) => returnType == to || (NumbersDictionary.ContainsKey(to) && NumbersDictionary[to].Contains(returnType));
            bool ParameterValid(Type parameterType) => (parameterType == from) || (NumbersDictionary.ContainsKey(parameterType) && NumbersDictionary[parameterType].Contains(from));
            bool MatchImplicitOperator(MethodInfo m) => m.Name == "op_Implicit"
                                                        && ReturnTypeValid(m.ReturnType)
                                                        && ParameterValid(m.GetParameters().Single().ParameterType);

            return from.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(MatchImplicitOperator)
                    || to.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(MatchImplicitOperator);
        }

        public string GetIndexReferenceDefault(Type indexType, Type memberType)
        {
            if(!MiscellaneousConstants.Literals.Contains(indexType))
                throw _exceptionHelper.CriticalException("{1FF953EA-FB18-4B56-9E50-9DA54F705572}");

            if (indexType == typeof(string))
                return memberType.ToString();

            if (indexType == typeof(bool))
                return Activator.CreateInstance(indexType)!.ToString()!;

            if (indexType == typeof(char))
                return '0'.ToString(CultureInfo.CurrentCulture);

            return Activator.CreateInstance(indexType)!.ToString()!;
        }

        public string GetTypeDescription(Type type)
        {
            if (type.IsGenericType)
            {
                return string.Format
                (
                    CultureInfo.InvariantCulture,
                    MiscellaneousConstants.GENERICTYPEFORMAT,
                    type.Name,
                    string.Join
                    (
                        MiscellaneousConstants.COMMA.ToString(),
                        type.GetGenericArguments().Select(a => GetTypeDescription(a))
                    )
                );
            }

            return type.Name;
        }

        public Type GetUndelyingTypeForValidList(Type type)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(List<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IList<>))
                || type.GetGenericTypeDefinition().Equals(typeof(Collection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(ICollection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>))))
                return type.GetGenericArguments()[0];
            else if (type.IsArray)
                return type.GetElementType() ?? throw _exceptionHelper.CriticalException("{28046255-8B32-484D-A2B1-AC40603CBCF0}");
            else
                throw _exceptionHelper.CriticalException("{5ED4C326-21DA-4EB7-937A-2ED05DB47A0E}");
        }

        public bool IsLiteralType(Type type)
        {
            if (IsNullable(type))
                type = Nullable.GetUnderlyingType(type)!;/*Not null for nullables.*/

            return MiscellaneousConstants.Literals.Contains(type);
        }

        public bool IsNullable(Type type) 
            => type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));

        public bool IsValidConnectorList(Type type)
            => IsValidList(type) 
                && ToId(GetUndelyingTypeForValidList(type)) == ConnectorWrapperClasses.CONNECTORCLASS;

        public bool IsValidIndex(Type type) 
            => MiscellaneousConstants.Literals.Contains(type);

        public bool IsValidList(Type type) 
            => (type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(List<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IList<>))
                || type.GetGenericTypeDefinition().Equals(typeof(Collection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(ICollection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>))))
                || (type.IsArray && type.GetArrayRank() == 1);

        public bool IsValidLiteralReturnType(Type type)
        {
            if (type == typeof(void))
                return true;

            return IsLiteralType(type);
        }

        public string ToId(Type type)
        {
            string? typeName = ToId();

            if (typeName != null)
                return typeName;

            Type? fromAssemblySearch = type.Assembly
                .GetTypes()
                .FirstOrDefault(t => t?.Name == type.Name);//Search the assembly when type is not null
                                                           //but AssemblyQualifiedName and FullName are null
                                                           //(AssemblyQualifiedName and FullName may be null if loaded from a different context)

            if (fromAssemblySearch == null
                || fromAssemblySearch.AssemblyQualifiedName == null
                || fromAssemblySearch.FullName == null)
                return string.Empty;

            type = fromAssemblySearch;

            return ToId()!;

            string? ToId()
                => type.IsGenericType && !type.IsGenericTypeDefinition
                           ? type.AssemblyQualifiedName
                           : type.FullName;
        }

        public bool TryParse(string toParse, Type type, out object? result)
        {
            if (!IsLiteralType(type))
                throw _exceptionHelper.CriticalException("{A1026252-A00D-41A2-B501-D0B313E5383F}");

            if (type == null)
            {
                result = null;
                return false;
            }

            if (type == typeof(string))
            {
                result = toParse;
                return true;
            }

            if (IsNullable(type))
                type = Nullable.GetUnderlyingType(type)!;/*Not null for nullables.*/

            MethodInfo? method = type.GetMethods().SingleOrDefault(IsTryParseMethod);

            if (method == null)
            {
                result = null;
                return false;
            }

            object?[] args = new object?[] { toParse, null };
            bool success = (bool)method.Invoke(null, args)!;//SomeLiteralType.TryParse does not return null
            result = success ? args[1] : null;

            return success;

            bool IsTryParseMethod(MethodInfo method)
            {
                if (method.Name != "TryParse") return false;
                ParameterInfo[] parameters = method.GetParameters();
                return parameters.Length == 2
                    && parameters[0].ParameterType == typeof(string)
                    && parameters[1].IsOut
                    && parameters[1].ParameterType.GetElementType() == type;
            }
        }

        private bool AreCompatibleForOperation(Type t1, Type t2, string op)
        {
            t1 = IsNullable(t1) ? Nullable.GetUnderlyingType(t1)!/*Not null for nullables*/ : t1;
            t2 = IsNullable(t2) ? Nullable.GetUnderlyingType(t2)!/*Not null for nullables*/ : t2;

            if (NumbersDictionary.ContainsKey(t1) && t1 == t2)
                return true;//return true for standard conversions

            if ((NumbersDictionary.ContainsKey(t1) && NumbersDictionary[t1].Contains(t2))
                || (NumbersDictionary.ContainsKey(t2) && NumbersDictionary[t2].Contains(t1)))
                return true;//return true for standard conversions

            foreach (Type key in NumbersDictionary.Keys)
            {
                if ((key == t2 && ImplicitConversionExistsFrom(t2, t1))
                    || (key == t1 && ImplicitConversionExistsFrom(t1, t2)))
                    return true;

                if ((NumbersDictionary[key].Contains(t2) && ImplicitConversionExistsFrom(key, t1))
                    || (NumbersDictionary[key].Contains(t1) && ImplicitConversionExistsFrom(key, t2)))
                    return true;
            }

            return t2.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(MatchOperator)//operator overload must exist
                    || t1.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(MatchOperator);//on either t1 or t2

            bool ParameterMatch(Type p, Type type) => (p == type)//type is t1 or t2. p is the current parmeter a or b (public static bool operator <(IntDigit a, IntDigit b))
                        || (NumbersDictionary.ContainsKey(p) && NumbersDictionary[p].Contains(type))//standard conversions
                        || (NumbersDictionary.ContainsKey(type) && NumbersDictionary[type].Contains(p))//standard conversions
                        || ImplicitConversionExistsFrom(p, type);//implicit operator exists

            bool OverLoadExists(MethodInfo m, Type type)//e.g. op == op_GreaterThan
                => ParameterMatch(m.GetParameters()[0].ParameterType, type)//Do poth parameters 
                && ParameterMatch(m.GetParameters()[1].ParameterType, type);//match the op method (op_GreaterThan) 
                                                                            //on type (type=t1 or type=t2).

            bool MatchOperator(MethodInfo m) => m.Name == op//e.g. op == op_GreaterThan
                    && (OverLoadExists(m, t2) && OverLoadExists(m, t1));//check both t1 and t2 match the operator overload
        }

        private bool ImplicitConversionExistsFrom(Type to, Type from)
        {
            bool ReturnTypeValid(Type returnType) => //op_Implicit return type
                (returnType == to)
                || (NumbersDictionary.ContainsKey(to) && NumbersDictionary[to].Contains(returnType))
                || (NumbersDictionary.ContainsKey(returnType) && NumbersDictionary[returnType].Contains(to));

            bool ParameterValid(Type parameterType) => //op_Implicit argument
                (parameterType == from)
                || (NumbersDictionary.ContainsKey(parameterType) && NumbersDictionary[parameterType].Contains(from))
                || (NumbersDictionary.ContainsKey(from) && NumbersDictionary[from].Contains(parameterType));

            bool MatchImplicitOperator(MethodInfo m) => m.Name == "op_Implicit"
                                    && ReturnTypeValid(m.ReturnType)
                                    && ParameterValid(m.GetParameters().Single().ParameterType);

            return from.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(MatchImplicitOperator)
                    || to.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(MatchImplicitOperator);
        }

        private static readonly Dictionary<Type, HashSet<Type>> NumbersDictionary = new()
        {
            { typeof(decimal), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) } },
            { typeof(double), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float) } },
            { typeof(float), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong) } },
            { typeof(ulong), new HashSet<Type> { typeof(byte), typeof(char), typeof(ushort), typeof(uint) } },
            { typeof(long), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint) } },
            { typeof(uint), new HashSet<Type> { typeof(byte), typeof(char), typeof(ushort) } },
            { typeof(int), new HashSet<Type> { typeof(byte), typeof(sbyte), typeof(char), typeof(short), typeof(ushort) } },
            { typeof(ushort), new HashSet<Type> { typeof(byte), typeof(char) } },
            { typeof(short), new HashSet<Type> { typeof(byte), typeof(sbyte) } },
            { typeof(byte), new HashSet<Type> { } },//Needed for standard conversions to work
            { typeof(sbyte), new HashSet<Type> { } }//Needed for standard conversions to work
        };

        private static readonly Dictionary<CodeBinaryOperatorType, string> OperatorsesDictionary = new()
        {
            { CodeBinaryOperatorType.Add, "op_Addition" },
            { CodeBinaryOperatorType.Subtract, "op_Subtraction" },
            { CodeBinaryOperatorType.Multiply, "op_Multiply" },
            { CodeBinaryOperatorType.Divide, "op_Division" },
            { CodeBinaryOperatorType.Modulus, "op_Modulus" },
            { CodeBinaryOperatorType.Assign, "op_Assign" },
            { CodeBinaryOperatorType.IdentityInequality, "op_Inequality" },
            { CodeBinaryOperatorType.IdentityEquality, "op_Equality" },
            { CodeBinaryOperatorType.ValueEquality, "op_Equality" },
            { CodeBinaryOperatorType.BitwiseOr, "op_BitwiseOr" },
            { CodeBinaryOperatorType.BitwiseAnd, "op_BitwiseAnd" },
            { CodeBinaryOperatorType.BooleanOr, "op_LogicalOr" },
            { CodeBinaryOperatorType.BooleanAnd, "op_LogicalAnd" },
            { CodeBinaryOperatorType.LessThan, "op_LessThan" },
            { CodeBinaryOperatorType.LessThanOrEqual, "op_LessThanOrEqual" },
            { CodeBinaryOperatorType.GreaterThan, "op_GreaterThan" },
            { CodeBinaryOperatorType.GreaterThanOrEqual, "op_GreaterThanOrEqual" }
        };
    }
}
