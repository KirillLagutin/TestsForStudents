using System;
using System.Windows;

namespace Client;

public partial class StudentWindow3 : Window
{
    public StudentWindow3()
    {
        InitializeComponent();
    }
    
    private void ButtonBackToSubjects_Click(object sender, RoutedEventArgs e)
    {
        new StudentWindow().Show();
        Close();
    }

    private void ButtonExit_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}