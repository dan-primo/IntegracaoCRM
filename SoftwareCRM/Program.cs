﻿using System;
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

            var user = CreateUser();
            var negotiation = CreateNegotiation(user);
            var task = CreateTask(negotiation);

            UpdateNegotiation(negotiation);
            CloseTask(task);
            Deal(negotiation);
            Record(user);

            Console.ReadLine();
        }

        private static int CreateUser()
        {
            try
            {
                dynamic body = new JObject();
                body.Name = "East Side Gallery";
                body.Neighborhood = "Friedrichshain-Kreuzberg";
                body.ZipCode = 10243;
                body.CompanyId = null;
                body.StreetAddressNumber = 3-100;
                body.TypeId = 0;
                body.Phones = JArray.FromObject(new[] 
                { JObject.Parse(@"{PhoneNumber: '(30) 2517159', TypeId: 0, CountryId: 49}") });

                string createClient = api.CreateClient(body);
                Console.WriteLine("New user created successfully!");

                dynamic obj = JsonConvert.DeserializeObject(createClient);
                return obj.value[0].Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error creating new user: \n\n" + e);
            }

            return 0;
        }

        private static int CreateNegotiation(int user = 0)
        {
            try
            {
                dynamic body = new JObject();
                body.Title = "Negotiation";
                body.ContactId = user;
                body.Amount = 0;
                body.StageId = 0;

                string createDeal = api.CreateDeal(body);
                Console.WriteLine("Negotiation successfully created!");

                dynamic obj = JsonConvert.DeserializeObject(createDeal);
                return obj.value[0].Id;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error creating trade: \n\n" + e);
            }

            return 0;
        }

        private static int CreateTask(int negotiation = 0)
        {
            try
            {
                dynamic body = new JObject();
                body.Title = "Task";
                body.Description = "Description task";
                body.DateTime = DateTime.Now.ToString("s");
                body.EndTime = DateTime.Now.ToString("s");
                body.ContactId = 0;
                body.DealId = negotiation;

                string createTask = api.CreateTask(body);
                Console.WriteLine("Task created successfully!");

                dynamic obj = JsonConvert.DeserializeObject(createTask);
                return obj.value[0].Id;

            }
            catch(Exception e)
            {
                Console.WriteLine("Error creating task: \n\n" + e);
            }

            return 0;
        }

        private static void UpdateNegotiation(int negotiation)
        {
            try
            {
                dynamic body = new JObject();
                body.Amount = 15000;

                api.UpdateDeal(body, negotiation);
                Console.WriteLine("Trading updated successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating negotiation: \n\n" + e);
            }
        }
        
        private static void CloseTask(int task)
        {
            try
            {
                api.FinishTask(task);
                Console.WriteLine("Task successfully closed!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when closing task: \n\n" + e);
            }
        }
        
        private static void Deal(int negotiation)
        {
            try
            {
                api.WinDeal(negotiation);
                Console.WriteLine("Deal closed successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when closing a deal: \n\n" + e);
            }
        }

        private static void Record(int user)
        {
            try
            {
                dynamic body = new JObject();
                body.ContactId = user;
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
