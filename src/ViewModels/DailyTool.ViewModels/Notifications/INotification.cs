namespace DailyTool.ViewModels.Notifications
{
    public interface INotification
    {
        NotificationType NotificationType { get; set; }

        /// <remarks>
        /// Keeps notifications alive as long as this property is true.
        /// </remarks>
        bool IsRunning { get; set; }

        string Text { get; set; }
    }
}
