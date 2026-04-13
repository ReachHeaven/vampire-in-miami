using Foundation.CMS;
using Tags;

namespace Test
{
    public class CMSTest : CMSEntity
    {
        public CMSTest()
        {
            var health = Define<TagHealth>();
            health.Max = 200;
            health.Current = 100;
        } 
    }
}