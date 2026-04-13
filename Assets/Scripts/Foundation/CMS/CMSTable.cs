using System.Collections.Generic;

namespace Foundation.CMS
{
    public class CMSTable<T> where T : CMSEntity, new() 
    {
        private List<T> _list = new List<T>();
        private Dictionary<string, T> _dict = new Dictionary<string, T>();

        public void Add(T component)
        {
            if (component.id == null)
            {
                component.id = E.Id(component.GetType());
            }
            
            _list.Add(component);
            _dict.Add(component.id, component);
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