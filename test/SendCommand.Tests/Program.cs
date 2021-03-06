﻿using ECommon.Components;
using ECommon.Configurations;
using ENode.Commanding;
using ENode.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SendCommand.Tests
{
    public class CreateNoteCommand : Command<string>
    {
        public string Title { get; set; }
    }

    public class Program
    {
        private static IEnumerable<ICommand> CreateCommands(int commandCount)
        {
            var commands = new List<ICommand>();
            for (var i = 1; i <= commandCount; i++)
            {
                commands.Add(new CreateNoteCommand
                {
                    AggregateRootId = i.ToString(),
                    Title = "Sample Note"
                });
            }
            return commands;
        }

        private static void InitializeENodeFramework()
        {
            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly()
            };
            ECommon.Configurations.Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .InitializeBusinessAssemblies(assemblies)
                .UseEQueue()
                .StartEQueue();

            Console.WriteLine("ENode started...");
        }

        private static void Main(string[] args)
        {
            InitializeENodeFramework();
            var sendAsyncCount = 100000;
            var sendSyncount = 1000;
            SendCommandAsync(sendAsyncCount);
            // SendCommandSync(sendSyncount);
            Console.ReadLine();
        }

        private static void SendCommandAsync(int commandCount)
        {
            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
            Console.WriteLine($"workderThreads: {workerThreads}, completionPortThreads: {completionPortThreads}");

            var commands = CreateCommands(commandCount);
            var watch = Stopwatch.StartNew();
            var sequence = 0;
            var printSize = commandCount / 10;
            var commandService = ObjectContainer.Resolve<ICommandService>();

            var asyncAction = new Action<ICommand>(async command =>
            {
                await commandService.SendAsync(command);
                var current = Interlocked.Increment(ref sequence);
                if (current % printSize == 0)
                {
                    Console.WriteLine("----Sent {0} commands async, time spent: {1}ms", current, watch.ElapsedMilliseconds);
                }
            });

            var preheatingTask = new Task(() =>
           {
               commandService.SendAsync(commands.FirstOrDefault()).Wait();
               Console.WriteLine("complete preheating!");
           });
            preheatingTask.Start();
            preheatingTask.Wait();

            var taskList = new List<Task>();
            Console.WriteLine("--Start to send commands asynchronously, total count: {0}.", commandCount);

            Parallel.ForEach(commands, (command) =>
            {
                asyncAction(command);
            });

            Console.WriteLine("--Commands send async completed, throughput: {0}/s", commandCount * 1000 / watch.ElapsedMilliseconds);
        }

        private static void SendCommandSync(int commandCount)
        {
            var commands = CreateCommands(commandCount);
            var watch = Stopwatch.StartNew();
            var sentCount = 0;
            var printSize = commandCount / 10;
            var commandService = ObjectContainer.Resolve<ICommandService>();
            Console.WriteLine("");
            Console.WriteLine("--Start to send commands synchronously, total count: {0}.", commandCount);
            foreach (var command in commands)
            {
                commandService.Send(command);
                sentCount++;
                if (sentCount % printSize == 0)
                {
                    Console.WriteLine("----Sent {0} commands, time spent: {1}ms", sentCount, watch.ElapsedMilliseconds);
                }
            }
            Console.WriteLine("--Commands send completed, throughput: {0}/s", commandCount * 1000 / watch.ElapsedMilliseconds);
        }
    }
}