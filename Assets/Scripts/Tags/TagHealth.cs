using System;
using Foundation.CMS;

namespace Tags
{
    [Serializable]
    public class TagHealth : EntityComponentDefinition
    {
        public int Max;
        public int Current;
    }
}