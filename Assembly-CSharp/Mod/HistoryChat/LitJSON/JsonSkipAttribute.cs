using System;

namespace LitJSON;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class JsonSkipAttribute : Attribute
{
}
