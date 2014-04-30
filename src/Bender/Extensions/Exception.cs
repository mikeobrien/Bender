using System;

namespace Bender.Extensions
{
    public static class Exception<TSource> where TSource : Exception
    {
        public static TReturn Map<TReturn, TTarget>(Func<TReturn> func, Func<TSource, TTarget> map) 
            where TTarget : Exception
        {
            try
            {
                return func();
            }
            catch (TSource exception)
            {
                throw map(exception);
            }
        }
    }
}
