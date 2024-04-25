namespace Kafka.Consumer.Console
{
    public class ParametersModel
    {
        public string BootstrapServer { get; set; }
        public string GroupId { get; set; }
        public string Topic { get; set; }

        public ParametersModel()
        {
            BootstrapServer = "localhost:9092";
            GroupId = "test-group";
            Topic = "topic1";
        }
    }
}
