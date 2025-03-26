
using System.Xml.Serialization;

namespace ConsoleApplication.XMLDemo
{
    [Serializable]
    public class IntegrationProcessInfo
    {
        public string GlobalTrackingID { get; set; }
        public string SystemOfOrigin { get; set; }
        public string IOPCorrelationID { get; set; }
        public int IOPSequenceNumber { get; set; }
        public DateTime TransactionDateTime  { get; set; }
    }
    
    public class SerializationClass
    {
        public List<IntegrationProcessInfo> SerializeObjtoXMLString()
        {
            IntegrationProcessInfo info = new IntegrationProcessInfo()
            {
                GlobalTrackingID = "36EAFD344444E6B0",
                SystemOfOrigin = "GCS",
                IOPCorrelationID = "62F55F0A790E02A8",
                IOPSequenceNumber = 1,
                TransactionDateTime = DateTime.Now
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(IntegrationProcessInfo));
            using (StringWriter _sw = new StringWriter())
            {
                 xmlSerializer.Serialize(_sw, info);
                string XMLContent = _sw.ToString();
                Console.WriteLine(XMLContent);

            }
             using(StreamWriter _SW = new StreamWriter(@"C:\C# & Azure\Training\ConsoleApplication\ConsoleApplication\XMLinputs\sample.xml"))
            {
                xmlSerializer.Serialize(_SW, info);
                Console.WriteLine("Process completed");
            }


            List<IntegrationProcessInfo> infos = new List<IntegrationProcessInfo>()
            {
               new IntegrationProcessInfo()
               {
                    GlobalTrackingID = "36EAFD344444E6B0",
                SystemOfOrigin = "GCS",
                IOPCorrelationID = "62F55F0A790E02A8",
                IOPSequenceNumber = 1,
                TransactionDateTime = DateTime.Now
               },
               new IntegrationProcessInfo()
               {
                GlobalTrackingID = "36EAFD344444E6B0",
                SystemOfOrigin = "GCS",
                IOPCorrelationID = "62F55F0A790E02A8",
                IOPSequenceNumber = 1,
                TransactionDateTime = DateTime.Now
               }
            };

            XmlSerializer XML = new XmlSerializer(typeof(List<IntegrationProcessInfo>));
            using(StreamWriter SW = new StreamWriter(@"C:\C# & Azure\Training\ConsoleApplication\ConsoleApplication\XMLinputs\sample1.xml"))
            {
                XML.Serialize(SW, infos);
 
            }

            List<IntegrationProcessInfo> List1;

            using (StreamReader SR = new StreamReader(@"C:\C# & Azure\Training\ConsoleApplication\ConsoleApplication\XMLinputs\sample1.xml"))
            {
                List1 =  (List<IntegrationProcessInfo>)XML.Deserialize(SR);
                foreach (var item in List1)
                {
                    Console.WriteLine(item.TransactionDateTime);
                }
            }

            return List1;
        }
    }
}
