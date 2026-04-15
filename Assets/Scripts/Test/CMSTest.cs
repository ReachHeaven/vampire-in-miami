using System;
using System.Linq;
using Base.Player;
using UnityEngine;

namespace Test
{
    public class CMSTest : CMSEntity
    {
        public CMSTest()
        {
            var player = CMS.GetAll<CMSEntity>().FirstOrDefault(ent => ent.Is<TagStats>());
            Debug.Log(player);
        }

    }
}