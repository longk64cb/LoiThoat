using System;
using UnityEngine;

namespace Everest.CustomEditor {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DisplayAsDateTimeAttribute : PropertyAttribute {
        public string format = "yyyy-MM-dd HH:mm:ss,fff";
    }
}
