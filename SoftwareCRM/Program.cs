using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoftwareCRM.API;

namespace SoftwareCRM
{
    class Program
    {
        private static Api api;
        static void Main(string[] args)
        {
            api = new Api();

            var users = CreateUsers();
            var negotiations = CreateNegotiations(users);
            var tasks = CreateTasks(negotiations);

            UpdateNegotiations(negotiations);
            CloseTasks(tasks);
            Deals(negotiations);
            Records(users);

            Console.ReadLine();
        }

        private static int CreateUsers()
        {
            try
            {
                dynamic body = new JObject();
                body.Name = "East Side Gallery";
                body.Neighborhood = "Friedrichshain-Kreuzberg";
                body.ZipCode = 10243;
                body.CompanyId = null;
                body.StreetAddressNumber = 3-100;
                body.TypeId = 1;
                body.Register = 04473728000191; //como chegou na ploo: 44.737.280/0019-1
                body.Phones = JArray.FromObject(new[] 
                { JObject.Parse(@"{PhoneNumber: '(30) 2517159', TypeId: 1, CountryId: 49}") });

                string createClients = api.CreateClient(body);
                Console.WriteLine("New user created successfully!");

                dynamic obj = JsonConvert.DeserializeObject(createClients);
                return obj.value[0].Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error creating new user: \n\n" + e);
            }

            return 0;
        }

        private static int CreateNegotiations(int users = 0)
        {
            try
            {
                dynamic body = new JObject();
                body.Title = "Negotiation";
                body.ContactId = users;
                body.Amount = 0;
                body.StageId = 163205;

                string createDeals = api.CreateDeal(body);
                Console.WriteLine("Negotiation successfully created!");

                dynamic obj = JsonConvert.DeserializeObject(createDeals);
                return obj.value[0].Id;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error creating trade: \n\n" + e);
            }

            return 0;
        }

        private static int CreateTasks(int negotiations = 0)
        {
            try
            {
                dynamic body = new JObject();
                body.Title = "Task";
                body.Description = "Description task";
                body.DateTime = DateTime.Now.ToString("s");
                body.EndTime = DateTime.Now.ToString("s");
                body.ContactId = 0;
                body.DealId = negotiations;

                string createTasks = api.CreateTask(body);
                Console.WriteLine("Task created successfully!");

                dynamic obj = JsonConvert.DeserializeObject(createTasks);
                return obj.value[0].Id;

            }
            catch(Exception e)
            {
                Console.WriteLine("Error creating task: \n\n" + e);
            }

            return 0;
        }

        private static void UpdateNegotiations(int negotiations)
        {
            try
            {
                dynamic body = new JObject();
                body.Amount = 15000;

                api.UpdateDeal(body, negotiations);
                Console.WriteLine("Trading updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating negotiation: \n\n" + e);
            }
        }
        
        private static void CloseTasks(int tasks)
        {
            try
            {
                api.FinishTask(tasks);
                Console.WriteLine("Task successfully closed!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when closing task: \n\n" + e);
            }
        }
        
        private static void Deals(int negotiations)
        {
            try
            {
                api.WinDeal(negotiations);
                Console.WriteLine("Deal closed successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when closing a deal: \n\n" + e);
            }
        }

        private static void Records(int users)
        {
            try
            {
                dynamic body = new JObject();
                body.ContactId = users;
                body.Content = "Deal closed successfully!";
                body.Date = DateTime.Now.ToString("s");

                api.InteractionRecord(body);

                Console.WriteLine("Successfully updated record!");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error updating record: \n\n" + e);
            }
        }
    }
}
