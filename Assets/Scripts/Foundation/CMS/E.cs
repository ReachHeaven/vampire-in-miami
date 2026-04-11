    using System;

    namespace Foundation.CMS
    {
        public static class E
        {
            public static string Id(Type type) => type.FullName;

            public static string Id<T>() => ID<T>.Get();
        }
    }