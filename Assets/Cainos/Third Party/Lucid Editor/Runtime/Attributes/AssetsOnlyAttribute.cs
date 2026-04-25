using System;

namespace Cainos.LucidEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class AssetsOnlyAttribute : Attribute
    {
        public AssetsOnlyAttribute()
        {
        }
    }
}
