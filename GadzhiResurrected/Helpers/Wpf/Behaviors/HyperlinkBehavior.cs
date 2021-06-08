using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace GadzhiResurrected.Helpers.Wpf.Behaviors
{
    /// <summary>
    /// Методы расширения для ссылок
    /// </summary>
    public static class HyperlinkBehavior
    {
        /// <summary>
        /// Получить свойство
        /// </summary>
        public static bool GetIsExternal(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsExternalProperty);
        }

        /// <summary>
        /// Записать свойство
        /// </summary>
        public static void SetIsExternal(DependencyObject obj, bool value)
        {
            obj.SetValue(IsExternalProperty, value);
        }

        /// <summary>
        /// Установить свойство
        /// </summary>
        public static readonly DependencyProperty IsExternalProperty =
            DependencyProperty.RegisterAttached("IsExternal", typeof(bool), typeof(HyperlinkBehavior), 
                                                new UIPropertyMetadata(false, OnIsExternalChanged));

        /// <summary>
        /// Изменение свойства
        /// </summary>
        private static void OnIsExternalChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (!(sender is Hyperlink hyperlink)) return;

            if ((bool)args.NewValue)
                hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
            else
                hyperlink.RequestNavigate -= Hyperlink_RequestNavigate;
        }

        /// <summary>
        /// Перейти по ссылке
        /// </summary>
        private static void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (e.Uri.IsFile && File.Exists(e.Uri.LocalPath))
                Process.Start(new ProcessStartInfo(e.Uri.LocalPath));
            else
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));

            e.Handled = true;
        }
    }
}