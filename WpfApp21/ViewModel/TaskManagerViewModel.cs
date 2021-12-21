using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApp21.Command;
using WpfApp21.Views;

namespace WpfApp21.ViewModel
{
    public class TaskManagerViewModel : Window
    {
        public RelayCommand ExitAppCommand { get; set; }
        public RelayCommand LoadedWindowCommand { get; set; }
        public RelayCommand KillTaskCommand { get; set; }
        public RelayCommand AddNewTaskCommand { get; set; }



        public bool KillBtnEnabled
        {
            get { return (bool)GetValue(KillBtnEnabledProperty); }
            set { SetValue(KillBtnEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KillBtnEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KillBtnEnabledProperty =
            DependencyProperty.Register("KillBtnEnabled", typeof(bool), typeof(TaskManagerViewModel));



        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(TaskManagerViewModel));



        public ObservableCollection<Process> Proces { get; set; } = new ObservableCollection<Process>(Process.GetProcesses().ToList().OrderBy(x => x.ProcessName).ToList());
        public TaskManagerViewModel()
        {
            ExitAppCommand = new RelayCommand(s =>
            {
                  Application.Current.Shutdown();
            });

            AddNewTaskCommand = new RelayCommand(s =>
            {
                var temp = new AddView();
                temp.ShowDialog();
            });

            LoadedWindowCommand = new RelayCommand(s =>
            {
                SelectedIndex = -1;
                KillBtnEnabled = false;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            });

            KillTaskCommand = new RelayCommand(s =>
            {
                if (SelectedIndex != -1)
                    Proces[SelectedIndex].Kill();
                else
                    KillBtnEnabled=false;
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (SelectedIndex == -1)
            {
                KillBtnEnabled = false;
                Proces.Clear();
                var temp2 = Process.GetProcesses().ToList().OrderBy(x => x.ProcessName).ToList();
                foreach (var item in temp2)
                    Proces.Add(item);
            }
            else
            {
                KillBtnEnabled = true;
                var temp = Proces[SelectedIndex].Id;
                Proces.Clear();
                var temp2 = Process.GetProcesses().ToList().OrderBy(x => x.ProcessName).ToList();
                foreach (var process in temp2)
                    Proces.Add(process);
                SelectedIndex = Proces.ToList().FindIndex(x => x.Id == temp);
            }
        }
    }
}
