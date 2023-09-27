using DemoData.Models;
using Riok.Mapperly.Abstractions;

namespace MyConvert;

[Mapper]
public static partial class MyMapper
{
    public static partial UserSO UserToUserSO(User user);
}