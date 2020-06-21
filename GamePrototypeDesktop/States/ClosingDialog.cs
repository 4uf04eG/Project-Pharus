using GamePrototypeDesktop.Graphics;
using GamePrototypeDesktop.Managers;
using GamePrototypeDesktop.UIComponents;
using GamePrototypeDesktop.UIComponents.Base;

namespace GamePrototypeDesktop.States
{
    public class ClosingDialog : GameState
    {
        public ClosingDialog(Camera camera) : base(camera)
        {
            var onCloseDialog = new Dialog()
            {
                Text = "Выйти из игры?",
            };
            onCloseDialog.OnOkClick += (sender, args) => GraphicsManager.Instance.Game.Exit();
            onCloseDialog.OnCancelClick += (sender, args) => StateManager.Instance.RemoveState();
            AddComponent(onCloseDialog);
        }
    }
}