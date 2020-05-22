using InsBrokers.Portal.Resource;

namespace InsBrokers.Portal
{
    public class Modal
    {
        public bool IsSuccessful { get; set; } = true;

        public string Message { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public bool ResetForm { get; set; } = true;

        public bool RefreshList { get; set; } = true;

        #region Auto Submit Props
        public bool AutoSubmit { get; set; } = true;

        public string AutoSubmitUrl { get; set; }

        public string AutoSubmitBtnText { get; set; } = Strings.Submit;

        public string AutoSubmitBtnIcon { get; set; } = "zmdi zmdi-floppy";
        #endregion
    }
}