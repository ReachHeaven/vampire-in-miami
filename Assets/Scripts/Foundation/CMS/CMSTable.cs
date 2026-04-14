using System.Collections.Generic;

namespace Foundation.CMS
{
    public class CMSTable<T> where T : CMSEntity
    {
        private List<T> _list = new();
        private Dictionary<string, T> _dict = new();

        public void Add(T entity)
        {
            _list.Add(entity);
            _dict.Add(entity.id, entity);
        }

        public T FindById(string id)
        {
            return _dict.GetValueOrDefault(id);
        }

        public List<T> GetAll()
        {
            return _list;
        }
    }
}