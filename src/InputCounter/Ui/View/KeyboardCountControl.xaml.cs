using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InputCounter.Ui.ViewModel;

namespace InputCounter.Ui.View;

/// <summary>
/// Interaction logic for KeyboardCountControl.xaml
/// </summary>
public partial class KeyboardCountControl : UserControl
{
    /// <summary>
    /// Creates a new instance of the <see cref="KeyboardCountControl"/>
    /// </summary>
    public KeyboardCountControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Init the view model
    /// </summary>
    /// <returns>The awaitable task</returns>
    public async Task InitControlAsync()
    {
        if (DataContext is KeyboardCountControlViewModel viewModel)
            await viewModel.LoadDataAsync();
    }
}