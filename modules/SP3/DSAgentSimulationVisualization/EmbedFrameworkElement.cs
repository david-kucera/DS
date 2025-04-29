using Avalonia.Controls;
using Avalonia.Platform;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using System.Windows;
using System;

namespace DSAgentSimulationVisualization
{
    public class EmbedFrameworkElement : NativeControlHost
    {
        private readonly FrameworkElement _frameworkElement;

        public EmbedFrameworkElement(FrameworkElement frameworkElement)
        {
            _frameworkElement = frameworkElement;
        }

        protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Disconnect the FrameworkElement from any previous parent
                if (_frameworkElement.Parent is System.Windows.Controls.Panel panel)
                {
                    panel.Children.Remove(_frameworkElement);
                }
                else if (_frameworkElement.Parent is System.Windows.Controls.ContentControl contentControl)
                {
                    contentControl.Content = null;
                }
                else if (_frameworkElement.Parent is System.Windows.Controls.Decorator decorator)
                {
                    decorator.Child = null;
                }
                else if (_frameworkElement.Parent is System.Windows.Controls.ItemsControl itemsControl)
                {
                    itemsControl.Items.Remove(_frameworkElement);
                }
                else if (_frameworkElement.Parent is System.Windows.Controls.Primitives.Popup popup)
                {
                    popup.Child = null;
                }
                else if (_frameworkElement.Parent != null)
                {
                    throw new InvalidOperationException("Unsupported parent type: " + _frameworkElement.Parent.GetType());
                }

                // Create a new ElementHost
                ElementHost elementHost = new ElementHost
                {
                    Child = _frameworkElement
                };

                return new PlatformHandle(elementHost.Handle, "Hndl");
            }
            return base.CreateNativeControlCore(parent);
        }


        protected override void DestroyNativeControlCore(IPlatformHandle control)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // destroy the win32 window
                WinApi.DestroyWindow(control.Handle);

                return;
            }

            base.DestroyNativeControlCore(control);
        }
    }
}
