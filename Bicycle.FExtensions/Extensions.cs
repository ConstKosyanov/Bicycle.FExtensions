using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bicycle.FExtensions
{
    public static class Extensions
    {
        ///<summary>Checking is the instance of <typeparamref name="T"/> in the <paramref name="array"/></summary>
        ///<example><c>ConsoleColor.Black.In(ConsoleColor.Black,ConsoleColor.White)</c></example>
        ///<exception cref="ArgumentNullException">If <paramref name="array"/> is null</exception>
        ///<returns>true if the array contains the value</returns>
        public static bool In<T>(this T local, params T[] array) => array.Contains(local);

        ///<summary>Executes <paramref name="action"/> on a calling object, and returns the object</summary>
        ///<example><c>1.Do(x => x + 1).Do(x => x - 1)</c> will return 1</example>
        ///<exception cref="ArgumentNullException">If <paramref name="action"/> is null</exception>
        ///<returns>The calling object after the action</returns>
        public static T Do<T>(this T local, Action<T> action)
        {
            action(local);
            return local;
        }

        ///<summary>Executes <paramref name="func"/> on a calling object, and returns the object, ignoring return value</summary>
        ///<example><c>"exam".Do(x => Console.ReadKey()).Do(x => x + "ple")</c> will return "example" after pressing the key</example>
        ///<remarks>Use this method in case when you don't care about return value value of <paramref name="func"/></remarks>
        ///<exception cref="ArgumentNullException">If <paramref name="action"/> is null</exception>
        ///<returns>The calling object after the function performance</returns>
        public static T Do<T, Tout>(this T local, Func<T, Tout> func)
        {
            func(local);
            return local;
        }

        ///<summary>Executes <paramref name="func"/> on a calling object, and returns result of the performance</summary>
        ///<example><c>"1".Make(int.Parse)</c> will return 1 as int</example>
        ///<exception cref="ArgumentNullException">If <paramref name="action"/> is null</exception>
        ///<returns>The result of <paramref name="func"/></returns>
        public static TOut Make<TIn, TOut>(this TIn local, Func<TIn, TOut> func) => func(local);

        ///<summary>Performs explicit conversion to <typeparamref name="T"/></summary>
        ///<exception cref="InvalidCastException"></exception>
        public static T To<T>(this object local) => (T)local;

        ///<summary>Performs conversion to <typeparamref name="T"/> via <c>as</c> operator</summary>
        public static T As<T>(this object local) where T : class => local as T;

        ///<summary>Calls SingleOrDefault extension method, pass the result as <c>out</c> paramether <paramref name="result"/>, if the result is null returns false</summary>
        ///<exception cref="ArgumentNullException"/>
        ///<exception cref="InvalidOperationException"/>
        public static bool TryGetSingle<T>(this IEnumerable<T> local, Func<T, bool> predicate, out T result)
        {
            result = local.SingleOrDefault(predicate);
            return !Equals(result, default(T));
        }

        ///<summary></summary>
        public static IEnumerable<T> WhereWithMin<T, TResult>(this IEnumerable<T> local, Func<T, TResult> selector) => local.Min(selector)
            .Make(x => local.Where(y => selector(y).Equals(x)));

        ///<summary></summary>
        public static T SingleWithMin<T, TResult>(this IEnumerable<T> local, Func<T, TResult> selector) => local.Min(selector)
            .Make(x => local.Single(y => selector(y).Equals(x)));

        ///<summary></summary>
        public static IEnumerable<T> WhereWithMax<T, TResult>(this IEnumerable<T> local, Func<T, TResult> selector) => local.Max(selector)
            .Make(x => local.Where(y => selector(y).Equals(x)));

        ///<summary></summary>
        public static T SingleWithMax<T, TResult>(this IEnumerable<T> local, Func<T, TResult> selector) => local.Max(selector)
            .Make(x => local.Single(y => selector(y).Equals(x)));

        public static IEnumerable<T> SimpleJoin<T, TKey>(this IEnumerable<T> local, IEnumerable<TKey> keys, Func<T, TKey> keySelector) => local.Join(keys, keySelector, x => x, (x, y) => x);

        public static string Left(this string local, int length) => !string.IsNullOrEmpty(local)
            ? local.Substring(0, length > local.Length ? length : local.Length)
            : string.Empty;

        public static TResult If<TSource, TResult>(this TSource local, Func<TSource, bool> Condition, Func<TSource, TResult> thanGet, Func<TSource, TResult> elseGet) => Condition(local)
            ? thanGet(local)
            : elseGet(local);

        public static TResult If<TSource, TResult>(this TSource local, Func<TSource, bool> Condition, TResult thanValue, Func<TSource, TResult> elseGet) => Condition(local)
            ? thanValue
            : elseGet(local);

        public static TResult If<TSource, TResult>(this TSource local, Func<TSource, bool> Condition, Func<TSource, TResult> thanGet, TResult elseValue) => Condition(local)
            ? thanGet(local)
            : elseValue;

        public static TResult If<TSource, TResult>(this TSource local, Func<TSource, bool> Condition, TResult thanValue, TResult elseValue) => Condition(local)
            ? thanValue
            : elseValue;

        public static TResult IfNotNull<TSource, TResult>(this TSource local, Func<TSource, TResult> thanGet, TResult elseValue) => local != null
            ? thanGet(local)
            : elseValue;
    }
}