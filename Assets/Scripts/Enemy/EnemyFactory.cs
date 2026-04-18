using Base;
using UnityEngine;

public class EnemyFactory : ViewFactoryBase<EnemyView>
{
    protected override string PrefabPath => "CMS/View/EnemyView";

    public EnemyView Create(CMSEntity model)
    {
        var state = new EnemyState(model);
        var view = Object.Instantiate(Prefab);
        view.Init(state);
        return view;
    }
}