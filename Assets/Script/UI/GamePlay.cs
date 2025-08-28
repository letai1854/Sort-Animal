using UnityEngine;

public class GamePlay : BaseScreen
{
    public override void Hide()
    {
        base.Hide();
        SoundManager.Instance.PlayHome();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Show(object data)
    {
        base.Show(data);
    }
}
