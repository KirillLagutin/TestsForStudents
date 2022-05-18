using System.Windows;

namespace Client;

public partial class StudentWindow1 : Window
{
    int _maxTimerCounter = ForAll.time * 60;//время прохождения теста
    public StudentWindow1()
    {
        InitializeComponent();
        
        TextBlockSubjectName.Text = ForAll.theme1;
        
        if ((_maxTimerCounter % 60) == 0)
        {
            InfoTextBlock.Text = $"{(_maxTimerCounter / 60).ToString()}:{(_maxTimerCounter % 60).ToString()}0";
        }
        else
        {
            InfoTextBlock.Text = $"{(_maxTimerCounter / 60).ToString()}:{(_maxTimerCounter % 60).ToString()}";
        }
    }
    
    private void ButtonSelectTest_Click(object sender, RoutedEventArgs e)
    {
        new StudentWindow2().Show();
        Close();
    }
}