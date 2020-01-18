using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Database_Gizmo.Structure
{
    /// <summary>
    /// An extension of the base <see cref="Window"/> class that contains several helper methods.
    /// </summary>
    public class ExtendedWindow: Window
    {
        /// <summary>
        /// Displays a simple error message box.
        /// </summary>
        /// <param name="e">The exception that was thrown when the error occured.</param>
        /// <param name="action">The name of the action that was being completed when the error occurred. e.g. 'Load Connections.'</param>
        public void ShowErrorMessage(Exception e, string action)
        {
            MessageBox.Show(
                $"An error occurred while attempting to {action}.{Environment.NewLine}The error message is:{Environment.NewLine}{e.Message}", $"Error: {action}", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays a simple error message box.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="caption">The caption for the error message box.</param>
        public void ShowErrorMessage(string message, string caption)
        {
            MessageBox.Show(
                $"{message}", $"{caption}", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
