using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExpressiveDataGenerators
{
    internal static class GeneratorAnalizerHelper
    {
        public static Dictionary<MethodCallExpression, string> FindAllProperUsePlaceHolderMethod(Expression itemGeneratorExpression)
        {
            var dict = new Dictionary<MethodCallExpression, string>();
            FindAllProperUsePlaceHolderMethod(itemGeneratorExpression, "/", dict);
            return dict;
        }

        public static void ValidateItemGeneratorExpression(Expression itemGeneratorExpression)
        {
            if ((
                    itemGeneratorExpression is MemberInitExpression ||
                    (itemGeneratorExpression is NewExpression && CheckIfAnonymousType(itemGeneratorExpression.Type))
                ) == false)
                throw new ArgumentException("Złe ...");
        }

        private static bool CheckIfAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        private static void FindAllProperUsePlaceHolderMethod(
            Expression expression,
            string prefix,
            Dictionary<MethodCallExpression, string> dict)
        {
            var initExpression = expression as MemberInitExpression;
            if (initExpression != null)
            {
                FindAllProperUsePlaceHolderMethod(initExpression, prefix, dict);
                return;
            }

            var newExpression = expression as NewExpression;
            if (newExpression != null && CheckIfAnonymousType(newExpression.Type))
            {
                FindAllProperUsePlaceHolderMethod(newExpression, prefix, dict);
            }
        }

        private static void FindAllProperUsePlaceHolderMethod(
            MemberInitExpression memberInitExpression,
            string prefix,
            Dictionary<MethodCallExpression, string> dict)
        {
            foreach (MemberAssignment memberAssignment in memberInitExpression.Bindings)
            {
                if (memberAssignment.Expression.NodeType == ExpressionType.Call)
                    dict.Add(
                        (MethodCallExpression)memberAssignment.Expression,
                        prefix + "/" + memberAssignment.Member.Name);
                else
                    FindAllProperUsePlaceHolderMethod(
                        memberAssignment.Expression,
                        prefix + "/" + memberAssignment.Member.Name,
                        dict);
            }
        }

        private static void FindAllProperUsePlaceHolderMethod(
            NewExpression newExpression,
            string prefix,
            Dictionary<MethodCallExpression, string> dict)
        {
            var memberAssigments = newExpression.Constructor.GetParameters()
                                                .Select(i => new { Name = i.Name, Expression = newExpression.Arguments[i.Position] });

            foreach (var memberAssignment in memberAssigments)
            {
                if (memberAssignment.Expression.NodeType == ExpressionType.Call)
                    dict.Add(
                        (MethodCallExpression)memberAssignment.Expression,
                        prefix + "/" + memberAssignment.Name);
                else
                    FindAllProperUsePlaceHolderMethod(
                        memberAssignment.Expression,
                        prefix + "/" + memberAssignment.Name,
                        dict);
            }
        }
    }
}