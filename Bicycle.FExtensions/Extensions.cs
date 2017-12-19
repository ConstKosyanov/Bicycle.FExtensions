using System;
using System.Linq;

namespace Bicycle.FExtensions
{
    public static class Extensions
    {
        public static bool In<T>(this T local, params T[] args) => args.Contains(local);
        public static T Do<T>(this T local, Action<T> action)
        {
            action(local);
            return local;
        }
        public static T Do<T,Tout>(this T local, Func<T,Tout> func)
        {
            func(local);
            return local;
        }
        public static TOut Make<TIn, TOut>(this TIn local, Func<TIn, TOut> func) => func(local);
        public static T To<T>(this object local) => (T)local;
        public static T As<T>(this object local) where T : class => local as T;
    }
}