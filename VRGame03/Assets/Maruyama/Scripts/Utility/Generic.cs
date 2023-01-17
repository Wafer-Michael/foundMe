using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace maru
{
    public class Generic
    {
        public static T Construct<T, P1>(P1 p1)
        {
            return (T)typeof(T).GetConstructor(new System.Type[] { typeof(P1) }).Invoke(new object[] { p1 });
        }

        public static T Construct<T, P1, P2>(P1 p1, P2 p2)
        {
            return (T)typeof(T).GetConstructor(new System.Type[] { typeof(P1), typeof(P2) }).Invoke(new object[] { p1, p2 });
        }
    }
}


