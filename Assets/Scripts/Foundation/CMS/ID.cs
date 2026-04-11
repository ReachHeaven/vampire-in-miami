namespace Foundation.CMS
{
    public static class ID<T>
    {
        private static string _cache;

        public static string Get()
        {
            if (_cache == null)
                _cache = typeof(T).FullName;
            return _cache;
        }
    }
}