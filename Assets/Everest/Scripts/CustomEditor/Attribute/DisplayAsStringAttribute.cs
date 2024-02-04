using System;

namespace Everest.CustomEditor {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DisplayAsStringAttribute : Attribute {
    }
}