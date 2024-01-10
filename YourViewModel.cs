using Lab2;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

public class YourViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    TodoTask selectedTask;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<TodoTask> TodoList { get; set; }
    public ICommand EditTaskCommand { get; private set; }
    public ICommand NewTaskCommand { get; private set; }


    public TodoTask SelectedTask
    {
        get { return selectedTask; }
        set
        {
            if (selectedTask != value)
            {
                selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }
    }

    public YourViewModel()
    {
        TodoList = new ObservableCollection<TodoTask>();
        EditTaskCommand = new RelayCommand(EditTask);
        NewTaskCommand = new RelayCommand(CreateNewTask);
    }

    private void EditTask()
    {
        if (SelectedTask != null)
        {
            var editWindow = new TaskDialogWindow();
            editWindow.DataContext = SelectedTask.Clone(); // Òóò íóæíî  êëîíèðîâàíèå äëÿ èçáåæàíèÿ èçìåíåíèé â îðèãèíàëå
            var result = editWindow.ShowDialog();

            if (result == true)
            {
                // Òóò îáíîâëÿåì TodoList è ñîîòâåòñòâóþùèé îáúåêò TodoTask
                int index = TodoList.IndexOf(SelectedTask);
                TodoList[index] = (TodoTask)editWindow.DataContext;
                OnPropertyChanged(nameof(TodoList));
            }
        }
    }

    private void CreateNewTask()
    {
        var newTaskWindow = new TaskDialogWindow();
        var result = newTaskWindow.ShowDialog();

        if (result == true)
        {
            TodoList.Add((TodoTask)newTaskWindow.DataContext);
            OnPropertyChanged(nameof(TodoList));
        }
    }
}
