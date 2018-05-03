using Prism.Mvvm;

namespace SBICT.Modules.Chat
{
    public class Chat : BindableBase
    {
        private string _name;

        #region Properties

        /// <summary>
        /// Name of this chat
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        #endregion
    }
}