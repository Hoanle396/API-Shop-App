using System.Runtime.Serialization;

namespace WebAPI.Enum
{
  public enum OrderStatus
  {
    [EnumMember(Value = "pending")]
    PENDING,

    [EnumMember(Value = "closed")]
    CLOSED,

    [EnumMember(Value = "success")]
    SUCCESS,

    [EnumMember(Value = "confirm")]
    CONFIRM

  }
}
