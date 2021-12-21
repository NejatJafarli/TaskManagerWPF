using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp21.Command;

namespace WpfApp21.ViewModel
{
    public class AddViewModel : Window
    {

        public RelayCommand OkBtnCommand { get; set; }
        public RelayCommand CancelBtnCommand { get; set; }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AddViewModel));


        public AddViewModel()
        {

            OkBtnCommand = new RelayCommand(s =>
            {
                if (s is Window w)
                    if (!string.IsNullOrWhiteSpace(Text))
                    {
                        try
                        {
                            Process.Start(Text);
                            MessageBox.Show($"{Text} Process Started");
                            w.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"File Not Found ERROR Message: {ex.Message}");
                        }
                    }
                    else
                        MessageBox.Show("Text Is Empty");
            });

            CancelBtnCommand = new RelayCommand(s =>
            {
                if (s is Window w)
                    w.Close();
            });
        }
    }
}
