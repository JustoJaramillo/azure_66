using System;
using System.Threading.Tasks;
using azure_66.functions.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace azure_66.functions.Functions
{
    public static class ScheduledFunction
    {
        [FunctionName("ScheduledFunction")]
        public static async Task Run(
            [TimerTrigger("0 */2 * * * *")] TimerInfo myTimer,
            [Table("todo", Connection = "AzureWebJobsStorage")] CloudTable todoTable,
            ILogger log)
        {
            log.LogInformation($"Deleting completed todo function executed at: {DateTime.UtcNow}");

            string filter = TableQuery.GenerateFilterConditionForBool("IsCompleted",QueryComparisons.Equal,true);
            TableQuery<TodoEntity> query = new TableQuery<TodoEntity>().Where(filter);
            TableQuerySegment<TodoEntity> completedTodos = await todoTable.ExecuteQuerySegmentedAsync(query, null);
            int deleted = 0;
            foreach (TodoEntity completedTodo in completedTodos)
            {
                await todoTable.ExecuteAsync(TableOperation.Delete(completedTodo));
                deleted++;
            }
            log.LogInformation($"Deleted: {deleted} items at: {DateTime.UtcNow} ");
        }
    }
}
