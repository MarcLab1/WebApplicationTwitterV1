using Newtonsoft.Json;

namespace WebApplicationTwitterV1
{
    public class TwitterUser
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        public string Id { get; set; }
        public string NumFriends { get; set; }
        public string NumFollowers { get; set; }

    }
}
