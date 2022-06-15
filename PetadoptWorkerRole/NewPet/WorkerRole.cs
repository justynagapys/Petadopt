using Azure.Storage.Queues;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace NewPet
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("NewPet is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("NewPet has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("NewPet is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("NewPet has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                //Download from queue
                var clientAccessKey = "DefaultEndpointsProtocol=https;AccountName=storagepetadopt;AccountKey=ZYgb1jpti/Nj3FA2kj1xsoWhRa4GPRIHxj92uTv1Fqo+gl2u7FVP6CiSNJzxevVi/DNmiZTAY5WH+ASt8OL1gQ==;EndpointSuffix=core.windows.net";
                var client = new QueueServiceClient(clientAccessKey);
                var newpetsQueue = client.GetQueueClient("newpet");

                var message = await newpetsQueue.ReceiveMessageAsync(); //downoald for 30 seconds without delete
                var messageContent = message.Value.Body;

                /*//Send email
                string to = "gabi1014@op.pl";
                string from = "gabi1014@op.pl";
                MailMessage mail = new MailMessage(from, to);
                mail.Subject = "Another pet in our shelter!";
                mail.Body = "New pet is looking for a home!";

                SmtpClient mailClient = new SmtpClient("smtp.poczta.onet.pl")
                {
                    Port = 465,
                    Credentials = new NetworkCredential(from, "haslo"),
                    EnableSsl = true
                };

                mailClient.Send(mail);*/

                //Delete
                newpetsQueue.DeleteMessageAsync(message.Value.MessageId, message.Value.PopReceipt);

                await Task.Delay(10000);
            }
        }
    }
}
