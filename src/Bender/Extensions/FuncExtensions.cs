using System;
using Bender.Collections;

namespace Bender.Extensions
{
    public static class FuncExtensions
    {
        // Or

        public static Func<bool> Or(this Func<bool> func1, Func<bool> func2)
        {
            return func1 != null && func2 != null ? () => func1() || func2() : func1 ?? func2;
        }

        public static Func<T1, bool> Or<T1>(this Func<T1, bool> func1, Func<T1, bool> func2)
        {
            return func1 != null && func2 != null ? p1 => func1(p1) || func2(p1) : func1 ?? func2;
        }

        public static Func<T1, T2, bool> Or<T1, T2>(this Func<T1, T2, bool> func1, Func<T1, T2, bool> func2)
        {
            return func1 != null && func2 != null ? (p1, p2) => func1(p1, p2) || func2(p1, p2) : func1 ?? func2;
        }

        public static Func<T1, T2, T3, bool> Or<T1, T2, T3>(this Func<T1, T2, T3, bool> func1, Func<T1, T2, T3, bool> func2)
        {
            return func1 != null && func2 != null ? (p1, p2, p3) => func1(p1, p2, p3) || func2(p1, p2, p3) : func1 ?? func2;
        }

        public static Func<T1, T2, T3, T4, bool> Or<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> func1,
            Func<T1, T2, T3, T4, bool> func2)
        {
            return func1 != null && func2 != null ? (p1, p2, p3, p4) => func1(p1, p2, p3, p4) || func2(p1, p2, p3, p4) : func1 ?? func2;
        }

        // And

        public static Func<bool> And(this Func<bool> func1, Func<bool> func2)
        {
            return func1 != null && func2 != null ? () => func1() && func2() : func1 ?? func2;
        }

        public static Func<T1, bool> And<T1>(this Func<T1, bool> func1, Func<T1, bool> func2)
        {
            return func1 != null && func2 != null ? p1 => func1(p1) && func2(p1) : func1 ?? func2;
        }

        public static Func<T1, T2, bool> And<T1, T2>(this Func<T1, T2, bool> func1, Func<T1, T2, bool> func2)
        {
            return func1 != null && func2 != null ? (p1, p2) => func1(p1, p2) && func2(p1, p2) : func1 ?? func2;
        }

        public static Func<T1, T2, T3, bool> And<T1, T2, T3>(this Func<T1, T2, T3, bool> func1, Func<T1, T2, T3, bool> func2)
        {
            return func1 != null && func2 != null ? (p1, p2, p3) => func1(p1, p2, p3) && func2(p1, p2, p3) : func1 ?? func2;
        }

        public static Func<T1, T2, T3, T4, bool> And<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> func1,
            Func<T1, T2, T3, T4, bool> func2)
        {
            return func1 != null && func2 != null ? (p1, p2, p3, p4) => func1(p1, p2, p3, p4) && func2(p1, p2, p3, p4) : func1 ?? func2;
        }

        // AndNot

        public static Func<bool> AndNot(this Func<bool> func1, Func<bool> func2)
        {
            return func1 != null ? (Func<bool>)(() => func1() && !func2()) : () => !func2();
        }

        public static Func<T1, bool> AndNot<T1>(this Func<T1, bool> func1, Func<T1, bool> func2)
        {
            return func1 != null ? (Func<T1, bool>)(p1 => func1(p1) && !func2(p1)) : (p1) => !func2(p1);
        }

        public static Func<T1, T2, bool> AndNot<T1, T2>(this Func<T1, T2, bool> func1, Func<T1, T2, bool> func2)
        {
            return func1 != null ? (Func<T1, T2, bool>)((p1, p2) => func1(p1, p2) && !func2(p1, p2)) : (p1, p2) => !func2(p1, p2);
        }

        public static Func<T1, T2, T3, bool> AndNot<T1, T2, T3>(this Func<T1, T2, T3, bool> func1, Func<T1, T2, T3, bool> func2)
        {
            return func1 != null ? (Func<T1, T2, T3, bool>)((p1, p2, p3) => func1(p1, p2, p3) && !func2(p1, p2, p3)) : (p1, p2, p3) => !func2(p1, p2, p3);
        }

        public static Func<T1, T2, T3, T4, bool> AndNot<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> func1,
            Func<T1, T2, T3, T4, bool> func2)
        {
            return func1 != null ? (Func<T1, T2, T3, T4, bool>)((p1, p2, p3, p4) => func1(p1, p2, p3, p4) && !func2(p1, p2, p3, p4)) : (p1, p2, p3, p4) => !func2(p1, p2, p3, p4);
        }

        // WhenNot

        public static bool WhenNot(this Func<bool> func)
        {
            return func != null && !func();
        }

        public static bool WhenNot<T>(this Func<T, bool> func, T arg)
        {
            return func != null && !func(arg);
        }

        public static bool WhenNot<T1, T2>(this Func<T1, T2, bool> func, T1 arg1, T2 arg2)
        {
            return func != null && !func(arg1, arg2);
        }

        public static bool WhenNot<T1, T2, T3>(this Func<T1, T2, T3, bool> func, T1 arg1, T2 arg2, T3 arg3)
        {
            return func != null && !func(arg1, arg2, arg3);
        }

        public static bool WhenNot<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return func != null && !func(arg1, arg2, arg3, arg4);
        }

        // When

        public static Action When(this Action @do, Func<bool> when)
        {
            return () => { if (when()) @do(); };
        }

        public static Action<T1> When<T1>(this Action<T1> @do, Func<T1, bool> when)
        {
            return p1 => { if (when(p1)) @do(p1); };
        }

        public static Action<T1, T2> When<T1, T2>(this Action<T1, T2> @do,
            Func<T1, T2, bool> when)
        {
            return (p1, p2) => { if (when(p1, p2)) @do(p1, p2); };
        }

        public static Action<T1, T2, T3> When<T1, T2, T3>(this Action<T1, T2, T3> @do,
            Func<T1, T2, T3, bool> when)
        {
            return (p1, p2, p3) => { if (when(p1, p2, p3)) @do(p1, p2, p3); };
        }

        public static Action<T1, T2, T3, T4> When<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @do,
            Func<T1, T2, T3, T4, bool> when)
        {
            return (p1, p2, p3, p4) => { if (when(p1, p2, p3, p4)) @do(p1, p2, p3, p4); };
        }

        // WhenElse func

        public static Func<TResult> WhenElse<TResult>(this Func<TResult> @do, Func<bool> when, Func<TResult> @else) where TResult : class
        {
            return () => when() ? @do() : @else();
        }

        public static Func<T1, TResult> WhenElse<T1, TResult>(this Func<T1, TResult> @do, Func<T1, bool> when, 
            Func<T1, TResult> @else) where TResult : class
        {
            return p1 => when(p1) ? @do(p1) : @else(p1);
        }

        public static Func<T1, T2, TResult> WhenElse<T1, T2, TResult>(this Func<T1, T2, TResult> @do, 
            Func<T1, T2, bool> when, Func<T1, T2, TResult> @else) where TResult : class
        {
            return (p1, p2) => when(p1, p2) ? @do(p1, p2) : @else(p1, p2);
        }

        public static Func<T1, T2, T3, TResult> WhenElse<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @do, 
            Func<T1, T2, T3, bool> when, Func<T1, T2, T3, TResult> @else) where TResult : class
        {
            return (p1, p2, p3) => when(p1, p2, p3) ? @do(p1, p2, p3) : @else(p1, p2, p3);
        }

        public static Func<T1, T2, T3, T4, TResult> WhenElse<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @do,
            Func<T1, T2, T3, T4, bool> when, Func<T1, T2, T3, T4, TResult> @else) where TResult : class
        {
            return (p1, p2, p3, p4) => when(p1, p2, p3, p4) ? @do(p1, p2, p3, p4) : @else(p1, p2, p3, p4);
        }

         // WhenElse action

        public static Action WhenElse(this Action @do, Func<bool> when, Action @else)
        {
            return () => { if(when()) @do(); else @else(); };
        }

        public static Action<T1> WhenElse<T1>(this Action<T1> @do, Func<T1, bool> when,
            Action<T1> @else)
        {
            return p1 => { if (when(p1)) @do(p1); else @else(p1); };
        }

        public static Action<T1, T2> WhenElse<T1, T2>(this Action<T1, T2> @do,
            Func<T1, T2, bool> when, Action<T1, T2> @else)
        {
            return (p1, p2) => { if (when(p1, p2)) @do(p1, p2); else @else(p1, p2); };
        }

        public static Action<T1, T2, T3> WhenElse<T1, T2, T3>(this Action<T1, T2, T3> @do,
            Func<T1, T2, T3, bool> when, Action<T1, T2, T3> @else)
        {
            return (p1, p2, p3) => { if (when(p1, p2, p3)) @do(p1, p2, p3); else @else(p1, p2, p3); };
        }

        public static Action<T1, T2, T3, T4> WhenElse<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @do,
            Func<T1, T2, T3, T4, bool> when, Action<T1, T2, T3, T4> @else)
        {
            return (p1, p2, p3, p4) => { if (when(p1, p2, p3, p4)) @do(p1, p2, p3, p4); else @else(p1, p2, p3, p4); };
        }

        // Pipe

        public static Func<TResult> Pipe<TResult>(this Func<TResult> func1, Func<TResult, TResult> func2)
        {
            return () => func2(func1());
        }

        public static Func<T1, TResult> Pipe<T1, TResult>(this Func<T1, TResult> func1, Func<TResult, T1, TResult> func2)
        {
            return p1 => func2(func1(p1), p1);
        }

        public static Func<T1, T2, TResult> Pipe<T1, T2, TResult>(this Func<T1, T2, TResult> func1, 
            Func<TResult, T1, T2, TResult> func2)
        {
            return (p1, p2) => func2(func1(p1, p2), p1, p2);
        }

        public static Func<T1, T2, T3, TResult> Pipe<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func1, 
            Func<TResult, T1, T2, T3, TResult> func2)
        {
            return (p1, p2, p3) => func2(func1(p1, p2, p3), p1, p2, p3);
        }

        public static Func<T1, T2, T3, T4, TResult> Pipe<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func1, 
            Func<TResult, T1, T2, T3, T4, TResult> func2)
        {
            return (p1, p2, p3, p4) => func2(func1(p1, p2, p3, p4), p1, p2, p3, p4);
        }

        // Pipe conditionally

        public static Func<TResult> PipeWhen<TResult>(this Func<TResult> func1,
            Func<TResult, TResult> func2, Func<TResult, bool> predicate)
        {
            return () => func1().Map(r => predicate(r) ? func2(r) : r);
        }

        public static Func<T1, TResult> PipeWhen<T1, TResult>(this Func<T1, TResult> func1,
            Func<TResult, T1, TResult> func2, Func<TResult, T1, bool> predicate)
        {
            return p1 => func1(p1).Map(r => predicate(r, p1) ? func2(r, p1) : r);
        }

        public static Func<T1, T2, TResult> PipeWhen<T1, T2, TResult>(this Func<T1, T2, TResult> func1,
            Func<TResult, T1, T2, TResult> func2, Func<TResult, T1, T2, bool> predicate)
        {
            return (p1, p2) => func1(p1, p2).Map(r => predicate(r, p1, p2) ? func2(r, p1, p2) : r);
        }

        public static Func<T1, T2, T3, TResult> PipeWhen<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func1,
            Func<TResult, T1, T2, T3, TResult> func2, Func<TResult, T1, T2, T3, bool> predicate)
        {
            return (p1, p2, p3) => func1(p1, p2, p3).Map(r => predicate(r, p1, p2, p3) ? func2(r, p1, p2, p3) : r);
        }

        public static Func<T1, T2, T3, T4, TResult> PipeWhen<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func1,
            Func<TResult, T1, T2, T3, T4, TResult> func2, Func<TResult, T1, T2, T3, T4, bool> predicate)
        {
            return (p1, p2, p3, p4) => func1(p1, p2, p3, p4).Map(r => predicate(r, p1, p2, p3, p4) ? func2(r, p1, p2, p3, p4) : r);
        }

        // ThenDo

        public static Action ThenDo(this Action action1, Action action2)
        {
            return action1 == null ? action2 : () => { action1(); action2(); };
        }

        public static Action<T1> ThenDo<T1>(this Action<T1> action1, Action<T1> action2)
        {
            return action1 == null ? action2 : p1 => { action1(p1); action2(p1); };
        }

        public static Action<T1, T2> ThenDo<T1, T2>(this Action<T1, T2> action1, Action<T1, T2> action2)
        {
            return action1 == null ? action2 : (p1, p2) => { action1(p1, p2); action2(p1, p2); };
        }

        public static Action<T1, T2, T3> ThenDo<T1, T2, T3>(this Action<T1, T2, T3> action1,
            Action<T1, T2, T3> action2)
        {
            return action1 == null ? action2 : (p1, p2, p3) => { action1(p1, p2, p3); action2(p1, p2, p3); };
        }

        public static Action<T1, T2, T3, T4> ThenDo<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action1,
            Action<T1, T2, T3, T4> action2)
        {
            return action1 == null ? action2 : (p1, p2, p3, p4) => { action1(p1, p2, p3, p4); action2(p1, p2, p3, p4); };
        }

        // ThenDoWhen

        public static Action ThenDoWhen(this Action action1,
            Action action2, Func<bool> predicate)
        {
            return action1.ThenDo(() => { if (predicate()) action2(); });
        }

        public static Action<T1> ThenDoWhen<T1>(this Action<T1> action1,
            Action<T1> action2, Func<T1, bool> predicate)
        {
            return action1.ThenDo(p1 => { if (predicate(p1)) action2(p1); });
        }

        public static Action<T1, T2> ThenDoWhen<T1, T2>(this Action<T1, T2> action1,
            Action<T1, T2> action2, Func<T1, T2, bool> predicate)
        {
            return action1.ThenDo((p1, p2) => { if (predicate(p1, p2)) action2(p1, p2); });
        }

        public static Action<T1, T2, T3> ThenDoWhen<T1, T2, T3>(this Action<T1, T2, T3> action1,
            Action<T1, T2, T3> action2, Func<T1, T2, T3, bool> predicate)
        {
            return action1.ThenDo((p1, p2, p3) => { if (predicate(p1, p2, p3)) action2(p1, p2, p3); });
        }

        public static Action<T1, T2, T3, T4> ThenDoWhen<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action1,
            Action<T1, T2, T3, T4> action2, Func<T1, T2, T3, T4, bool> predicate)
        {
            return action1.ThenDo((p1, p2, p3, p4) => { if (predicate(p1, p2, p3, p4)) action2(p1, p2, p3, p4); });
        }
    }
}
