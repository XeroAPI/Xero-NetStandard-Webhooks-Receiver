using System.Runtime.Serialization;

namespace XeroWebhooks.DTO
{

    public enum EventType{
        [EnumMember(Value = "Create")]
        CREATE = 1,
        [EnumMember(Value = "Update")]
        UPDATE = 2
    }

}
