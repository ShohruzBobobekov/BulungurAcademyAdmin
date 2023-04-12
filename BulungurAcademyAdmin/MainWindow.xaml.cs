using BulungurAcademyAdmin.Broker;
using BulungurAcademyAdmin.DataTranferObjects.Exams;
using BulungurAcademyAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace BulungurAcademyAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //#pragma warning disable 
        private BrokerBase broker = new BrokerBase();
        private object tabControlSender;
        private SelectionChangedEventArgs tabControlEvent;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Download();
            tabControlSender = sender;
            tabControlEvent = e;
            if (UsersTab.IsSelected)
            {
                if (Cache.Users == null)
                    return;

                UsersTabInitialize();
            }
            else if (SubjectsTab.IsSelected)
            {
                if (Cache.Subjects == null)
                    return;

                for (int i = 0; i < Cache.Subjects.Count; i++)
                {
                    Cache.Subjects[i].Index = i + 1;
                }
                SubjectsListView.ItemsSource = Cache.Subjects;
            }
            else if (ExamsTab.IsSelected)
            {
                if (Cache.Exams == null)
                    return;
                for (int i = 0; i < Cache.Exams.Count; i++)
                {
                    Cache.Exams[i].Index = i + 1;
                }
                SelectExamComboBoxForAddSubject.ItemsSource = Cache.Exams;
                SelectSubjectComboBoxAddToExam.ItemsSource = Cache.Subjects;
                ExamsListView.ItemsSource = Cache.Exams;
            }
            else if (ExamApplicantsTab.IsSelected)
            {
                if (Cache.Exams == null)
                    return;
                if (Cache.ExamApplicants == null)
                    return;
                ExamApplicantsTabInitilize();
                FilterExamApplicants();
            }
        }
        private async ValueTask Download()
        {
            if (Cache.Users == null)
                await Cache.RefreshUsers();
            if (Cache.Subjects == null)
                await Cache.RefreshSubjects();
            if (Cache.Exams == null)
                await Cache.RefreshExams();
            if (Cache.ExamApplicants == null)
                await Cache.RefreshExamApplicants();
        }
        private void ExamApplicantsTabInitilize()
        {
            EA_ExamComboBox.ItemsSource = Cache.Exams.Select(e => e.ExamName);
            EA_AttendanceComboBox.ItemsSource = new string[] { "Hammasi", "Kelgan", "Kelmagan" };
            EA_PayedComboBox.ItemsSource = new string[] { "Hammasi", "To'lagan", "To'lamagan" };
            var subjectNames = new List<string>();
            subjectNames.Add("Hammasi");
            subjectNames.AddRange(Cache.Subjects.Select(s => s.Name).ToList());
            EA_FirstSubjectComboBox.ItemsSource = subjectNames;
            EA_SecondSubjectComboBox.ItemsSource = subjectNames;

        }
        private void FilterExamApplicants()
        {
            //          Exam
            var list = Cache.ExamApplicants
                .Where(ea => ea.Exam.ExamName
                == EA_ExamComboBox.Text);

            //          Attendance
            if (EA_AttendanceComboBox.Text == "Kelgan")
                list = list.Where(ea => ea.IsArrived is true);
            else if (EA_AttendanceComboBox.Text == "Kelmagan")
                list = list.Where(ea => ea.IsArrived is false);

            //          Payed
            if (EA_PayedComboBox.Text == "To'lagan")
                list = list.Where(ea => ea.IsPayed is true);
            else if (EA_PayedComboBox.Text == "To'lamagan")
                list = list.Where(ea => ea.IsPayed is false);

            //          FirstSubject
            if (EA_FirstSubjectComboBox.Text != "Hammasi")
                list = list.Where(ea => ea.FirstSubject.Name == EA_FirstSubjectComboBox.Text);

            //         
            if (EA_SecondSubjectComboBox.Text != "Hammasi")
                list = list.Where(ea => ea.SecondSubject.Name == EA_SecondSubjectComboBox.Text);

            ShowExamApplicantsList(list.ToList());
        }
        private void ShowExamApplicantsList(List<ExamApplicant> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index = i + 1;
            }
            ExamApplicantsListView.ItemsSource = list;
        }

        #region Users

        private void UsersTabInitialize()
        {
            for (int i = 0; i < Cache.Users.Count; i++)
            {
                Cache.Users[i].Index = i + 1;
            }

            UsersListView.ItemsSource = Cache.Users;
            UsersGroup.ItemsSource = new string[] { "IsActive", "FirstName" };
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchBox.Text.ToLower();
            int index = 1;
            List<User> searchUsers = new List<User>();
            foreach (var item in Cache.Users)
            {
                string itemText = item.ToString().ToLower();

                if (itemText.Contains(searchQuery))
                {
                    item.Index = index;
                    index++;
                    searchUsers.Add(item);
                }
            }
            UsersListView.ItemsSource = searchUsers;
        }

        private void UsersGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsersListView.Items.GroupDescriptions.Clear();
            //Task.Delay(1000).Wait();
            var group = UsersGroup.SelectedItem.ToString();
            if (group == "None")
            {
                return;
            }

            UsersListView.Items.GroupDescriptions.Add(new PropertyGroupDescription(group));
        }

        #region Users Toggle button
        private async void ActivateUser(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = (ToggleButton)sender;
            var user = (User)btn.DataContext;
            user.Status = Entities.Enum.UserStatus.Active;
            await broker.Put<User>("User", user);
        }

        private async void DeactivateUser(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = (ToggleButton)sender;
            var user = (User)btn.DataContext;
            user.Status = Entities.Enum.UserStatus.Inactive;
            await broker.Put<User>("User", user);
        }
        #endregion

        private async void DeleteUser(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var user = (User)btn.DataContext;
            await broker.Delete("User", user.Id);
            await Cache.RefreshUsers();
            TabControl_SelectionChanged(tabControlSender, tabControlEvent);
        }
        #endregion

        #region Subjects
        private async void DeleteSubject(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var subject = (Subject)btn.DataContext;
            await broker.Delete("Subject", subject.Id);
            await Cache.RefreshSubjects();
            TabControl_SelectionChanged(tabControlSender, tabControlEvent);
        }
        #endregion

        #region ExamApplicant
        private async void ExamApplicantPayed(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            var EAobject = (ExamApplicant)tb.DataContext;
            EAobject.IsPayed = true;
            await broker.Put<ExamApplicant>("ExamApplicants", EAobject);
        }

        private async void ExamApplicantNotPayed(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            var EAobject = (ExamApplicant)tb.DataContext;
            EAobject.IsPayed = false;
            await broker.Put<ExamApplicant>("ExamApplicants", EAobject);
        }

        private async void ExamApplicantArrived(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            var EAobject = (ExamApplicant)tb.DataContext;
            EAobject.IsArrived = true;
            await broker.Put<ExamApplicant>("ExamApplicants", EAobject);
        }

        private async void ExamApplicantNotArrived(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            var EAobject = (ExamApplicant)tb.DataContext;
            EAobject.IsArrived = false;
            await broker.Put<ExamApplicant>("ExamApplicants", EAobject);
        }

        private async void Refresh_button_Clicked(object sender, RoutedEventArgs e)
        {
            await Cache.RefreshExamApplicants();
            ExamApplicantsTabInitilize();
            FilterExamApplicants();
        }

        #endregion

        private async void Save_Subject_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectNameTextBox.Text is null || SubjectNameTextBox.Text.Length <= 1)
                return;
            var subject = new Subject(SubjectNameTextBox.Text);
            await broker.Post("Subject", subject);
            await Cache.RefreshSubjects();
            SubjectNameTextBox.Text = "";
            TabControl_SelectionChanged(tabControlSender, tabControlEvent);
        }

        private async void Save_Exam_Click(object sender, RoutedEventArgs e)
        {
            if (ExamNameTextBox.Text is null || ExamNameTextBox.Text.Length <= 1)
                return;
            var dateTime = new DateTime(
                year: ExamDatePicker.SelectedDate.Value.Year,
                month: ExamDatePicker.SelectedDate.Value.Month,
                day: ExamDatePicker.SelectedDate.Value.Day,
                hour: ExamTimePicker.SelectedTime.Value.Hour,
                minute: ExamTimePicker.SelectedTime.Value.Minute,
                second: ExamTimePicker.SelectedTime.Value.Second);

            var exam = new ExamForCreationDto(ExamNameTextBox.Text, dateTime);
            await broker.Post("Exam", exam);
            ExamNameTextBox.Text = "";
            ExamDatePicker.SelectedDate = default;
            ExamTimePicker.SelectedTime = default;
            await Cache.RefreshExams();
            TabControl_SelectionChanged(tabControlSender, tabControlEvent);
        }

        private async void Save_ExamSubject_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Exam exam = (Exam)SelectExamComboBoxForAddSubject.SelectedItem;
            Subject subject = (Subject)SelectSubjectComboBoxAddToExam.SelectedItem;
            await broker.PostExamSubject(exam, subject);
            await Cache.RefreshExams();
            TabControl_SelectionChanged(tabControlSender, tabControlEvent);
            SelectExamComboBoxForAddSubject.SelectedItem = exam;
        }

        private async void Delete_Exam(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Exam exam = (Exam)btn.DataContext;
            await broker.Delete("Exam", exam.Id);
            await Cache.RefreshExams();
            TabControl_SelectionChanged(tabControlSender, tabControlEvent);
            SelectExamComboBoxForAddSubject.SelectedItem = exam;
        }

        private async void Refresh_Users_Click(object sender, RoutedEventArgs e)
        {
            await Cache.RefreshUsers();
        }
    }
}
