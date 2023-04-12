using BulungurAcademyAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace BulungurAcademyAdmin.Broker;
                                                                          
class BrokerBase
{
#pragma warning disable
    public HttpClient client = new HttpClient();

    public async Task Post<TEntity>(string typeName, TEntity? entity)
    {
        try
        {
            var json = JsonSerializer.Serialize(entity);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Constants.baseUrl + typeName, requestContent);
        }
        catch (Exception ex)
        {
            MessageBox.Show("1. Internet aloqasini tekshirib ko'ring.\n\n2." + ex.Message, "Nimadir hato bo'lyapti!!");
        }
    }

    public async Task PostExamSubject(Exam? exam, Subject? subject)
    {
        try
        {
            var response = await client.PostAsync(
                Constants.baseUrl +
                $"Exam/examId:Guid/subjectId:Guid?examId={exam.Id}&subjectId={subject.Id}",
                null)
                .Result
                .Content
                .ReadAsStringAsync();
        }
        catch (NullReferenceException ex)
        {

            MessageBox.Show("1. Internet aloqasini tekshirib ko'ring.\n\n2." + ex.Message, "Nimadir hato bo'lyapti!!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("1. Internet aloqasini tekshirib ko'ring.\n\n2." + ex.Message, "Nimadir hato bo'lyapti!!");
        }
        
    }
    public async Task<List<TEntity>> Get<TEntity>(string typeName)
    {
        try
        {
            var json = await (await client.GetAsync(Constants.baseUrl + typeName))
                .Content
                .ReadAsStringAsync();

            var list = JsonSerializer.Deserialize<List<TEntity>>(json);
            return list ?? new List<TEntity>();
        }
        catch (Exception ex)
        {
            MessageBox.Show("1. Internet aloqasini tekshirib ko'ring.\n\n2." + ex.Message, "Nimadir hato bo'lyapti!!");
            return null;
        }
    }

    public async Task Put<TEntity>(string typeName, TEntity entity)
    {
        try
        {
            var json = JsonSerializer.Serialize(entity);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(Constants.baseUrl + typeName, requestContent);
        }
        catch (Exception ex)
        {
            MessageBox.Show("1. Internet aloqasini tekshirib ko'ring.\n\n2." + ex.Message, "Nimadir hato bo'lyapti!!");
        }
    }

    public async Task Delete(string typeName, Guid id)
    {
        try
        {
            var response = await client.DeleteAsync(Constants.baseUrl + typeName + "/" + id);
        }
        catch (Exception ex)
        {
            MessageBox.Show("1. Internet aloqasini tekshirib ko'ring.\n\n2." + ex.Message, "Nimadir hato bo'lyapti!!");
        }
    }


}
