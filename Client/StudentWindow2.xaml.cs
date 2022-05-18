using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DB;
using Models.lib;

namespace Client;

public partial class StudentWindow2 : Window
{
    int MaxTimerCounter = ForAll.time * 60;//время прохождения теста в секундах
        const int MinTimerCounter = 0;

        
        DispatcherTimer _dispTimer;
        int _dispTimerCounter;
        public StudentWindow2()
        {
            InitializeComponent();
            _dispTimerCounter = MaxTimerCounter;
            _dispTimer = new DispatcherTimer();
            _dispTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _dispTimer.Tick += new EventHandler(dispTimer_Tick);
            _dispTimer.Start();
        }

        //
        private async void GetQuestions()
        {
            //получение questionid answerid по известному subjectid
            var db = new RequestDb();
            await db.GetTestAsync();
            List<Test> currentTest = new List<Test>();
            for (int i = 0; i < db._tests.Count; i++)
            {
                if (db._tests[i].SubjectId == ForAll.subjectID)
                {
                    currentTest.Add(new Test
                    {
                        Id = db._tests[i].Id,
                        QuestionId = db._tests[i].QuestionId,
                        AnsverId = db._tests[i].AnsverId
                    });
                }
            }
        }
        
        void dispTimer_Tick(object sender, EventArgs e)
        {
            //сначала выводится значение, потом инкрементируется
            //меняем цвет таймера на красный когда остается мало времени
            if (_dispTimerCounter <= (0.998 * MaxTimerCounter))//вместо 0.998 ввести значение в долях когда время станет красным
            {
                InfoTextBlock.Foreground = Brushes.Red;
            }

            if ((_dispTimerCounter % 60) == 0)
            {
                InfoTextBlock.Text = $"{(_dispTimerCounter / 60).ToString()}:{(_dispTimerCounter % 60).ToString()}0";
            }
            else
            {
                InfoTextBlock.Text = $"{(_dispTimerCounter / 60).ToString()}:{(_dispTimerCounter % 60).ToString()}";
            }
            _dispTimerCounter--;

            if (_dispTimerCounter < MinTimerCounter)
            {
                _dispTimer.Stop();
                new StudentWindow3().Show();
                Close();
            }
        }

        private void ButtonEndTest_Click(object sender, RoutedEventArgs e)
        {
            _dispTimer.Stop();
            new StudentWindow3().Show();
            Close();
        }
}