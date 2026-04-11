using System;
using System.Collections.Generic;

namespace Foundation.CMS
{
    public static class CMS
    {
        private static CMSTable<CMSEntity> _all = new();
        private static bool _isInitialized;

        public static void Init()
        {
            if (!_isInitialized)
            {
                AutoAdd();
                _isInitialized = true;
            }
        }

        private static void AutoAdd()
        {
            var subs = ReflectionUtil.FindAllSubclasses<CMSEntity>();
            foreach (var subclass in subs)
            {
                if (!subclass.IsAbstract)
                    _all.Add(Activator.CreateInstance(subclass) as CMSEntity);
            }
            
        }

        public static void AddManually(CMSEntity entity)
        {
            _all.Add(entity);
        }
        
        public static T Get<T>(string definitionId = null) where T : CMSEntity
        {
            if (definitionId == null)
                definitionId = E.Id<T>();
            var findById = _all.FindById(definitionId) as T;

            if (findById == null)
            {
                throw new InvalidOperationException($"CMS: entity '{definitionId}' not found in registry");
            }

            return findById;
        }
        

        public static List<T> GetAll<T>() where T : CMSEntity
        {
            var allSearch = new List<T>();

            foreach (var a in _all.GetAll())
                if (a is T)
                    allSearch.Add(a as T);

            return allSearch;
        }

        public static List<(CMSEntity e, T tag)> GetAllData<T>() where T : EntityComponentDefinition, new()
        {
            var allSearch = new List<(CMSEntity, T)>();

            foreach (var a in _all.GetAll())
                if (a.Is<T>(out var t))
                    allSearch.Add((a, t));

            return allSearch;
        }
    }
}