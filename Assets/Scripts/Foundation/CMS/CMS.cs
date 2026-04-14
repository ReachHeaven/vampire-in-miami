using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation.CMS
{
    public static class CMS
    {
        private static CMSTable<CMSEntity> _all = new();
        private static bool _isInitialized;

        public static void Init()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            LoadPrefabs();
        }

        private static void LoadPrefabs()
        {
            var prefabs = Resources.LoadAll<CMSEntityPrefab>("CMS");
            foreach (var pfb in prefabs)
            {
                _all.Add(new CMSEntity
                {
                    id = pfb.name,
                    components = pfb.Components
                });
            }
        }

        public static CMSEntity Get(string definitionId)
        {
            var found = _all.FindById(definitionId);
            if (found == null)
                throw new InvalidOperationException($"CMS: entity '{definitionId}' not found");
            return found;
        }

        public static IEnumerable<T> Query<T>() where T : EntityComponentDefinition
        {
            foreach (var entity in _all.GetAll())
                if (entity.Is<T>(out var tag))
                    yield return tag;
        }

        public static IEnumerable<(T1, T2)> Query<T1, T2>()
            where T1 : EntityComponentDefinition
            where T2 : EntityComponentDefinition
        {
            foreach (var entity in _all.GetAll())
                if (entity.Is<T1>(out var a) && entity.Is<T2>(out var b))
                    yield return (a, b);
        }

        public static IEnumerable<(T1, T2, T3)> Query<T1, T2, T3>()
            where T1 : EntityComponentDefinition
            where T2 : EntityComponentDefinition
            where T3 : EntityComponentDefinition
        {
            foreach (var entity in _all.GetAll())
                if (entity.Is<T1>(out var a) && entity.Is<T2>(out var b) && entity.Is<T3>(out var c))
                    yield return (a, b, c);
        }


        public static IEnumerable<(T1, T2, T3, T4)> Query<T1, T2, T3, T4>()
            where T1 : EntityComponentDefinition
            where T2 : EntityComponentDefinition
            where T3 : EntityComponentDefinition
            where T4 : EntityComponentDefinition
        {
            foreach (var entity in _all.GetAll())
                if (entity.Is<T1>(out var a) && entity.Is<T2>(out var b) && entity.Is<T3>(out var c) &&
                    entity.Is<T4>(out var d))
                    yield return (a, b, c, d);
        }

        public static IEnumerable<(T1, CMSEntity)> QueryWithEntity<T1>()
            where T1 : EntityComponentDefinition
        {
            foreach (var e in _all.GetAll())
                if (e.Is<T1>(out var a))
                    yield return (a, e);
        }

        public static IEnumerable<(T1, T2, CMSEntity)> QueryWithEntity<T1, T2>()
            where T1 : EntityComponentDefinition
            where T2 : EntityComponentDefinition
        {
            foreach (var e in _all.GetAll())
                if (e.Is<T1>(out var a) && e.Is<T2>(out var b))
                    yield return (a, b, e);
        }

        public static IEnumerable<(T1, T2, T3, CMSEntity)> QueryWithEntity<T1, T2, T3>()
            where T1 : EntityComponentDefinition
            where T2 : EntityComponentDefinition
            where T3 : EntityComponentDefinition
        {
            foreach (var e in _all.GetAll())
                if (e.Is<T1>(out var a) && e.Is<T2>(out var b) && e.Is<T3>(out var c))
                    yield return (a, b, c, e);
        }

        public static IEnumerable<(T1, T2, T3, T4, CMSEntity)> QueryWithEntity<T1, T2, T3, T4>()
            where T1 : EntityComponentDefinition
            where T2 : EntityComponentDefinition
            where T3 : EntityComponentDefinition
            where T4 : EntityComponentDefinition
        {
            foreach (var e in _all.GetAll())
                if (e.Is<T1>(out var a) && e.Is<T2>(out var b) && e.Is<T3>(out var c) && e.Is<T4>(out var d))
                    yield return (a, b, c, d, e);
        }

        public static T QuerySingle<T>() where T : EntityComponentDefinition
        {
            foreach (var entity in _all.GetAll())
                if (entity.Is<T>(out var tag))
                    return tag;
            return null;
        }
    }
}