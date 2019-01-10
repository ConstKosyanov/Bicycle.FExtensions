using System;
using System.Collections.Generic;
using System.Linq;

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

        [Obsolete("Use " + nameof(Filter) + " instead")]
        public static IEnumerable<T> SimpleJoin<T, TKey>(this IEnumerable<T> local, IEnumerable<TKey> keys, Func<T, TKey> keySelector) => local.Filter(keys, keySelector);

        /// <summary>Allows to filter the collection by another collection of values and key extractor</summary>
        public static IEnumerable<T> Filter<T, TKey>(this IEnumerable<T> local, IEnumerable<TKey> keys, Func<T, TKey> keySelector) => local.Join(keys, keySelector, x => x, (x, y) => x);

        public static string Left(this string local, int length) => length < local?.Length
            ? local.Substring(0, length)
            : local;

        public static TResult If<TSource, TResult>(this TSource local, Func<TSource, bool> condition, Func<TSource, TResult> thanGet, Func<TSource, TResult> elseGet) => condition(local)
            ? thanGet(local)
            : elseGet(local);

        public static TResult If<TSource, TResult>(this TSource local, Func<TSource, bool> condition, TResult thanValue, Func<TSource, TResult> elseGet) => condition(local)
            ? thanValue
            : elseGet(local);

        public static TResult If<TSource, TResult>(this TSource local, Func<TSource, bool> condition, Func<TSource, TResult> thanGet, TResult elseValue) => condition(local)
            ? thanGet(local)
            : elseValue;

        public static TResult If<TSource, TResult>(this TSource local, Func<TSource, bool> condition, TResult thanValue, TResult elseValue) => condition(local)
            ? thanValue
            : elseValue;

        ///<summary>Executes function and returns its result if object is not null, esle returns elseValue</summary>
        public static TResult IfNotNull<TSource, TResult>(this TSource local, Func<TSource, TResult> thanGet, TResult elseValue) => local != null
            ? thanGet(local)
            : elseValue;

        ///<summary>Executes function and returns its result if object is not null, esle returns default value</summary>
        public static TResult IfNotNull<TSource, TResult>(this TSource local, Func<TSource, TResult> thanGet) => local.IfNotNull(thanGet, default);

        ///<summary>Executes function and returns its result if condition is true, esle returns original object</summary>
        public static TSource DoIf<TSource>(this TSource local, Func<TSource, bool> condition, Func<TSource, TSource> action) => condition(local)
            ? action(local)
            : local;

        ///<summary>Executes function and if condition is true and returns original object</summary>
        public static TSource DoIf<TSource>(this TSource local, Func<TSource, bool> condigion, Action<TSource> action)
        {
            if(condigion(local))
                action(local);
            return local;
        }

        [Obsolete("This method will be removed in next vesions, please use DefaultIfEmpty")]
        public static IEnumerable<T> SetIfEmpty<T>(this IEnumerable<T> local, IEnumerable<T> defaultCollection) => DefaultIfEmpty(local, defaultCollection);

        public static IEnumerable<T> DefaultIfEmpty<T>(this IEnumerable<T> local, IEnumerable<T> defaultCollection) => local.Any()
            ? local
            : defaultCollection;

        /// <summary>Returns value of key from dictionary, if key dosn't exist return defaultValue</summary>
        public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> local, TKey key, TValue defaultValue) => local.TryGetValue(key, out var result)
            ? result
            : defaultValue;

        /// <summary>Selects Tout from Tin using <paramref name="func"/>, and returns collection of distinct Tout. Equivalent of "some.Select(selector).Distinct()"</summary>
        public static IEnumerable<TOut> Distinct<Tin, TOut>(this IEnumerable<Tin> local, Func<Tin, TOut> func) => local
            ?.Select(func)
            .Distinct();
    }
}