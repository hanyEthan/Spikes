using System;

namespace XCore.Caching.Custom.Contracts
{
    public interface IXSerializable
    {
    }
    public interface IXIncludable : IXSerializable
    {
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class XDontSerialize : Attribute
    {
    }
}
