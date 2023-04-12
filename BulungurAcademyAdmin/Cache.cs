using BulungurAcademyAdmin.Broker;
using BulungurAcademyAdmin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BulungurAcademyAdmin;

class Cache
{
#pragma warning disable

    public async static Task<List<Subject>> RefreshSubjects()
    {
        return subjects = await broker.Get<Subject>("Subject");
    }
    public async static Task<List<ExamApplicant>> RefreshExamApplicants()
    {
        return examApplicants = await broker.Get<ExamApplicant>("ExamApplicants");
    }
    public async static Task<List<Exam>> RefreshExams()
    {
        return exams = await broker.Get<Exam>("Exam");
    }
    public async static Task<List<User>> RefreshUsers()
    {
        return users = await broker.Get<User>("User");
    }

    private static BrokerBase broker = new UsersBroker();

    private static List<User> users;

    private static List<Exam> exams;

    private static List<Subject> subjects;

    private static List<ExamApplicant> examApplicants;

    public static List<User> Users
    {
        get { return users; }
        set { users = value; }
    }

    public static List<Exam> Exams
    {
        get { return exams; }
        set { exams = value; }
    }

    public static List<Subject> Subjects
    {
        get { return subjects; }
        set { subjects = value; }
    }

    public static List<ExamApplicant> ExamApplicants
    {
        get { return examApplicants; }
        set { examApplicants = value; }
    }

}
