using Blazored.Toast.Services;
using Microsoft.JSInterop;

namespace EcoRoute.UI
{
    public static class ToastBridge
    {
        public static IToastService? ToastService { get; set; }

        [JSInvokable("ShowToastFromJs")]
        public static void ShowToast(string message)
        {
            ToastService?.ShowSuccess(message); 
        }
    }
}
