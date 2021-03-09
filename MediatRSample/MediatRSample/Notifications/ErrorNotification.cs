using MediatR;

namespace MediatRSample.Notifications
{
    public class ErrorNotification : INotification
    {
        public string Error { get; set; }

        public string StackTraceError { get; set; }
    }
}