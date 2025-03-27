using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using ConsoleApplication.delegates;
using ConsoleApplication.EventHubs;
using ConsoleApplication.List;
using ConsoleApplication.Oops;
using ConsoleApplication.StorageAccountHandler;
using ConsoleApplication.XMLDemo;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

internal class Program
{
    public class Bird
    {
        public virtual void Fly()
        {
            Console.WriteLine("I can fly!");
        }
    }

    // Derived class 1
    public class Sparrow : Bird
    {
        /// <summary>
        /// The Fly
        /// </summary>
        public override void Fly()
        {
            Console.WriteLine("I am a sparrow, and I can fly!");
        }
    }

    // Derived class 2

    public class Ostrich : Bird
    {
        // Ostriches can't fly, so this violates the LSP if we throw an exception!>
        public override void Fly()
        {
            Console.WriteLine("Ostriches can't fly!");
        }
    }


    private static async Task Main(string[] args)
    {

        // Encapsulation
        IBankAcBankAccountcount _bankAccount = new BankAccount();

        Console.WriteLine("Current Balance");

        Console.WriteLine(_bankAccount.Balance);

        Console.WriteLine("Current Balance After Deposit");

        _bankAccount.Deposit(20.25);

        Console.WriteLine(_bankAccount.Balance);

        Console.WriteLine("Current Balance After Withdraw");

        _bankAccount.Widdraw(7.25);

        Console.WriteLine(_bankAccount.Balance);

        Console.WriteLine("-:-!-");

        // Inheritance

        SavingsAccount _savingsAccount = new SavingsAccount();

        Console.WriteLine("Unpdated Balance");

        _savingsAccount.applyInterest();

        // polymorphism

        // Mthod over loading difference in parameter, type count, mixed parameter
        // operator overloading 
        //Method overriding : virtual, override, base, new

        CurrentAccount _currentAccount = new CurrentAccount();

        Console.WriteLine(_currentAccount.Widdraw(19));

        List<Bird> _birds = new List<Bird>()
        {
            new Sparrow(), new Ostrich()
        };

        foreach (Bird bird in _birds)
        {
            bird.Fly();
        }

        string[] strings = ["krishna", "hello"];


        myList _myList = new myList(strings);

        List<string> Names = _myList.listMethod();

        List<string> Names1 = new List<string>(strings);

        foreach (var item in Names1)
        {
            Console.WriteLine(item);
        }

        SerializationClass _serializationClass = new SerializationClass();

        List<IntegrationProcessInfo> Data = _serializationClass.SerializeObjtoXMLString();

        EventHubProducerHandler eventHubProducerHandler = new EventHubProducerHandler();

        string eventhubConnectionString = "Endpoint=sb://z-001-webapp-eh.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=K4MC/0RDWsYketAwRG/ABcDzSlSZfaEJj+AEhLOdYJA=";
        string eventhubNamespace = "z-001-webapp-eh.servicebus.windows.net";
        string eventHubNam = "integrationinfo";

        string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=z003storageaccountdv;AccountKey=/5gMo2UKtnPcK8wgeurw7KkUzY8O/WgOc1ut6U6cSiSgcViDsChSHNX1Etrjul1GRYx7qj70DMYt+AStjSbzSQ==;EndpointSuffix=core.windows.net";
        string blobContainerName = "resume";

        EventData eventdata = new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Data)))
        {
            MessageId = Guid.NewGuid().ToString(),
            ContentType = "json/plain"
        };


        //await eventHubProducerHandler.sendAsync(eventdata, eventhubConnectionString, eventhubNamespace, eventHubNam);


        string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

        //await eventHubProducerHandler.ReceiveEventsAsync(consumerGroup, eventhubConnectionString, eventhubNamespace, eventHubNam, storageConnectionString, blobContainerName);

        //delegates _delegates = new delegates();

        //_delegates.delegatesMain();

        string StorageAccountName = "z003storageaccountdv";
        string Accesskey = "smChzGfKp8WsdBgETbZX3wi6pAsT4NC0yDcrK1xD8qTPrZyr6s8iQ3+VrWh1MGEriOY3ZiLHfTqL+AStBuAoRw==";
        string Container = "mdm";
        string Blob = "New_Text_Document.txt";
        


        StorageAccountHandler accountHandler = new StorageAccountHandler(StorageAccountName, Accesskey, 100, 5);

        //await accountHandler.DeleteBlobAsync(Blob, Container);

        await accountHandler.ReadBlobAsync(Blob, Container);

    }
}
