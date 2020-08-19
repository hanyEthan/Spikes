using System;

namespace ADS.Common.Contracts
{
    public interface IXSerializable
    {
    }
    public interface IXIncludable : IXSerializable
    {
    }
    [AttributeUsage( AttributeTargets.Property )]
    public class XDontSerialize : Attribute
    {
    }
}