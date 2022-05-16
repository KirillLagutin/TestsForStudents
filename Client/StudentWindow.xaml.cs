using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DB;
using Models.lib;

namespace Client;
public partial class StudentWindow : Window
{
    public StudentWindow()
    {
        InitializeComponent();
        Loaded += (sender, args) =>
        {
            MainWindow_Loaded(sender, args);
        };
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var db = new RequestDb();
        await db.GetSubjectAsync();

        var subject = db._subjects.Select(subj => subj.Name).ToList();

        ListBoxTestList.ItemsSource = subject;
    }

    private async void ListBoxTestList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ForAll.theme1 = ListBoxTestList.SelectedItem.ToString();

        var db = new RequestDb();
        await db.GetSubjectAsync();
        foreach (var subj in db._subjects)
        {
            if (subj.Name == ForAll.theme1)
            {
                ForAll.time = subj.Allotted_time;
                ForAll.subjectID = subj.Id;
            }
        }
    }

    private void ButtonSelectTest_Click(object sender, RoutedEventArgs e)
    {
        new StudentWindow1().Show();
        Close();
    }

    private void MenuTop_DragAndDrop(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void Minimize_Window(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Close_Window(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
static class ForAll
{
    public static string theme1 { get; set; }
    public static int time { get; set; }
    public static int subjectID { get; set; }
}