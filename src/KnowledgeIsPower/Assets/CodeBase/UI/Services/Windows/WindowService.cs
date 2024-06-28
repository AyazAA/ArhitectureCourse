using CodeBase.UI.Services.Factory;
using Zenject;

namespace CodeBase.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        [Inject]
        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.Shop:
                    _uiFactory.CreateShop();
                    break;
            }
        }
    }
}