using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;

namespace ImageResizer.Controllers
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoElement>> GetTodosAsync();
    }
    public class TodoService : ITodoService
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private const string TableName = "TodoTable";

        public TodoService(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<IEnumerable<TodoElement>> GetTodosAsync()
        {
            var table = Table.LoadTable(_dynamoDbClient, TableName);
            var search = table.Scan(new ScanOperationConfig());
            var documentList = await search.GetNextSetAsync();

            var todos = new List<TodoElement>();
            foreach (var document in documentList)
            {
                todos.Add(new TodoElement
                {
                    Title = document["Title"],
                    Description = document["Description"],
                    IsDone = (bool)document["IsDone"]
                });
            }

            return todos;
        }
    }
}
