using System.Collections.Generic;
using System.Linq;
using Buffs;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMain : MonoBehaviour
{
    [SerializeField] private MenuView _menu;
    [SerializeField] private IntroView _intro;
    [SerializeField] private DialogView _openingDialog;
    private RewardView _rewardView;
    private List<IAction> _allBuffs;

    private void Awake()
    {
        G.GameMain = this;
        G.Hud = FindFirstObjectByType<HudView>();
        _rewardView = FindFirstObjectByType<RewardView>(FindObjectsInactive.Include);
    }

    private async void Start()
    {
        G.Hud.SetHealth(
            G.Player.State.MaxHealth,
            G.Player.State.Health);

        _allBuffs = CMS.GetAllData<TagBuffs>()
            .SelectMany(x => x.tag.Buffs)
            .ToList();

        _rewardView.gameObject.SetActive(false);

        bool hasMenu = _menu != null;
        bool hasDialog = _openingDialog != null;
        if (_intro != null)
            await _intro.Play(
                chainNext: hasMenu || hasDialog,
                onBeforeFade: hasMenu ? (System.Action)(() => _menu.Show()) : null);
        if (hasMenu) await _menu.Play(chainNext: hasDialog);
        if (hasDialog) await _openingDialog.Play();

        G.Waves.RunAll().Forget();
    }

    private void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb != null && kb.zKey.wasPressedThisFrame)
            G.Waves.KillAllEnemies();
    }

    public void OnLevelUp()
    {
        var choices = _allBuffs.OrderBy(_ => Random.value).Take(3).ToList();
        _rewardView.Show(choices);
    }

    public void OnAllWavesCleared()
    {
        G.Hud.SetMessage("All waves cleared");
        Debug.Log("[GameMain] All waves cleared!");
    }
}
