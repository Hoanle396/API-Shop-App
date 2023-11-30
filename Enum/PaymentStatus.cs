using System.Runtime.Serialization;

namespace WebAPI.Enum
{
  public enum PaymentStatus
  {
    [EnumMember(Value = "pending")]
    PENDING,

    [EnumMember(Value = "closed")]
    CLOSED,

    [EnumMember(Value = "confirm")]
    CONFIRM,

    [EnumMember(Value = "success")]
    SUCCESS,
  }
}
