using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ItemObserveApp.Common
{
    public class ExtendWebView : WebView
    {
        public static readonly BindableProperty NavigatingCommandProperty =
            BindableProperty.Create(nameof(NavigatingCommand), typeof(ICommand), typeof(ExtendWebView), null);

        public static readonly BindableProperty NavigatedCommandProperty =
            BindableProperty.Create(nameof(NavigatedCommand), typeof(ICommand), typeof(ExtendWebView), null);


        public static BindableProperty EvaluateJavascriptProperty =
            BindableProperty.Create(nameof(EvaluateJavascript), typeof(Func<string, Task<string>>), typeof(ExtendWebView), null, BindingMode.OneWayToSource);

        public ExtendWebView()
        {
            Navigating += (s, e) =>
            {
                if (NavigatingCommand?.CanExecute(e) ?? false)
                    NavigatingCommand.Execute(e);
            };

            Navigated += (s, e) =>
            {
                if (NavigatedCommand?.CanExecute(e) ?? false)
                    NavigatedCommand.Execute(e);
            };
        }

        public ICommand NavigatingCommand
        {
            get { return (ICommand)GetValue(NavigatingCommandProperty); }
            set { SetValue(NavigatingCommandProperty, value); }
        }

        public ICommand NavigatedCommand
        {
            get { return (ICommand)GetValue(NavigatedCommandProperty); }
            set { SetValue(NavigatedCommandProperty, value); }
        }

        public Func<string, Task<string>> EvaluateJavascript
        {
            get { return (Func<string, Task<string>>)GetValue(EvaluateJavascriptProperty); }
            set { SetValue(EvaluateJavascriptProperty, value); }
        }

    }
}
