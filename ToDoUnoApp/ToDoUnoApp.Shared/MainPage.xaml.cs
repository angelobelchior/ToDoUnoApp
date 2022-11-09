using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Windows.UI.Text;

namespace ToDoUnoApp;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        DataContext = new ViewModel();
    }
}

public class ViewModel : NotifyPropertyChanged
{
    public Command AddToDoItemCommand { get; set; }
    public ObservableCollection<TodoItem> Items { get; set; }

    private string _title;
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    private bool _isDone;
    public bool IsDone
    {
        get => _isDone;
        set => Set(ref _isDone, value);
    }

    public ViewModel()
    {
        Items = new ObservableCollection<TodoItem>();
        AddToDoItemCommand = new Command(AddTodoItem);
    }

    private void AddTodoItem()
    {
        if (string.IsNullOrEmpty(Title)) return;

        Items.Add(new TodoItem(Title, IsDone));
        Title = string.Empty;
        IsDone = false;
    }
}

public class TodoItem : NotifyPropertyChanged
{
    public Guid Id { get; set; }
    
    private string _title;
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    private bool _isDone;
    public bool IsDone
    {
        get => _isDone;
        set => Set(ref _isDone, value);
    }
    public TodoItem(string title, bool isDone)
    {
        Id = Guid.NewGuid();
        Title = title;
        IsDone = isDone;
    }
}

public class NotifyPropertyChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

public class Command : ICommand
{
    private readonly Action _action;
    public Command(Action action) => _action = action;
    public event EventHandler CanExecuteChanged;
    public bool CanExecute(object parameter) => true;
    public void Execute(object parameter) => _action();
}

public class Command<T> : ICommand
{
    private readonly Action<T> _action;
    public Command(Action<T> action) => _action = action;
    public event EventHandler CanExecuteChanged;
    public bool CanExecute(object parameter) => true;
    public void Execute(object parameter) => _action((T)parameter);
}

public class IsDoneConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isDone && isDone)
            return TextDecorations.Strikethrough;
        else
            return TextDecorations.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value;
    }
}